using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace ADManager
{
    public class User
    {

        // Set the Canonical
        private const string canName = "CN";

        private string userName { get; set; }

        private string userPassword { get; set; }

        // Get domain info from app config file. 
        private readonly string domain = System.Configuration.ConfigurationManager.AppSettings["domainServer"];
        private PrincipalContext principalContext { get; set; }

        // Getting ldap path from appconfig file. 
        private string Path
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["path"];
            }

        }

        public bool isError = false;

        public string errorMessage = string.Empty;

        public User()
        {
            this.userName = Giris._userName;
            this.userPassword = Giris._userPassword;

        }





        private string UserAccountControl(string code)
        {

            switch (code)

            {
                case "512":

                    return "Normal hesap";

                case "544":

                    return "Aktif - Parola Gerektirmiyor";


                case "546":

                    return "Pasif-Parola Gerektirmiyor";

                case "514":

                    return "Pasif";

            }
            return "Bilinmeyen";
        }

        /// <summary>
        ///Get active directory all users  data 
        /// </summary>
        /// <returns> Userdata list </returns>

        public List<UserProperties> GetAllUsers()
        {

            List<UserProperties> usersList = new List<UserProperties>();

            try
            {
               
                using (principalContext = new PrincipalContext(ContextType.Domain, domain, userName, userPassword))
                using (UserPrincipal user = new UserPrincipal(principalContext))
                {
                    user.Name = "*";
                    using (PrincipalSearcher pS = new PrincipalSearcher())
                    {
                        pS.QueryFilter = user;

                        foreach (var result in pS.FindAll())
                        {

                            DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

                            usersList.Add(GetUserProperties(de));




                        }


                    }

                }
            }

            catch (DirectoryServicesCOMException dic)
            {
                isError = true;
                errorMessage = dic.Message;

            }

            return usersList;

        }



        public IEnumerable<UserProperties> SearchUser(string arananKisi)
        {
            List<UserProperties> userList = new List<UserProperties>();
            

            try
            {
                using (principalContext = new PrincipalContext(ContextType.Domain, domain, userName, userPassword))
                using (UserPrincipal user = new UserPrincipal(principalContext))
                {
                    user.Name = $"{arananKisi}*";
                    using (PrincipalSearcher pS = new PrincipalSearcher(user))
                {

                            foreach (var ps in pS.FindAll().Where(x=> x.SamAccountName.Contains(arananKisi)))
                            {
                                DirectoryEntry de = (DirectoryEntry)ps.GetUnderlyingObject();
                                userList.Add(GetUserProperties(de));


                            }
                    }
                }
                
            }

            catch (PrincipalExistsException ex)
            {
                errorMessage = ex.Message;

            }

            catch (PrincipalException ex)
            {
                errorMessage = ex.Message;

            }
            return userList;
        }


       
        public UserProperties GetUserProperties(DirectoryEntry de)
        {
           
            DirectorySearcher sc = new DirectorySearcher(de);
            SearchResult results = sc.FindOne();
            var userProperties = new UserProperties();
            userProperties.cannonicalName = de.Properties["cn"].Value.ToString();
            userProperties.samAccountName = de.Properties["samaccountname"][0].ToString();
            userProperties.userAccountControlCode = de.Properties["useraccountcontrol"][0].ToString();
            userProperties.userAccountControl = UserAccountControl(de.Properties["useraccountcontrol"][0].ToString());
            userProperties.whenCreated = Convert.ToDateTime(de.Properties["whenCreated"].Value).ToLocalTime().ToString();
            userProperties.pwdLastSet = DateTime.FromFileTime((long)results.Properties["pwdLastSet"][0]).ToShortDateString();
            userProperties.lastLogon = DateTime.FromFileTime((long)results.Properties["lastLogon"][0]).ToLocalTime().ToString();
            return userProperties;

        }

        public bool CreateUserAccount(UserFormInputs userFormInput)
        {

            bool kayitDurum = false;
            errorMessage = string.Empty;

            try
            {
                using (principalContext = new PrincipalContext(ContextType.Domain, domain, userName, userPassword))
                using (UserPrincipal userPrincipal = new UserPrincipal(principalContext, userFormInput.userName, userFormInput.userPass, true))
                {
                    userPrincipal.SamAccountName = userFormInput.userName;
                    userPrincipal.Name = userFormInput.name + " " + userFormInput.surname;
                    userPrincipal.Surname = userFormInput.surname;
                    userPrincipal.DisplayName = userFormInput.name + " " + userFormInput.surname;
                    userPrincipal.Enabled = true;
                    userPrincipal.Save();

                    //Kullanıcı ilk oturum açısında parola değiştirsin .
                    userPrincipal.ExpirePasswordNow();

                    kayitDurum = true;

                }


            }

            catch (PrincipalExistsException ex)
            {
                errorMessage = ex.Message;

            }

            catch (PrincipalException ex)
            {
                errorMessage = ex.Message;

            }
            return kayitDurum;


        }


        public string DeleteUser(string samAccountName)
        {
            string _samAccountName = samAccountName;
            string errorState = "";
            try
            {
                if (principalContext == null)
                {
                    principalContext = new PrincipalContext(ContextType.Domain, domain, Giris._userName, Giris._userPassword);

                }

                using (var user = UserPrincipal.FindByIdentity(principalContext, _samAccountName))
                {

                    if (_samAccountName != null)
                    {

                        user.Delete();
                        errorState = "Kullanıcı başarıyla silindi";
                    }


                }
            }
            catch (Exception ex)
            {

                errorState = ex.Message;
            }


            return errorState;

        }
        /// <summary>
        ///  Get Autharization Groups of Selected User.
        ///  Returns only Security Groups
        /// </summary>
        /// <param name="samAccountName"></param>
        /// <returns> User Autharization Group Lists </returns>
        public List<string> GetAutharizationGroup(string samAccountName)
        {

            string _samAccountName = samAccountName;
            List<string> groupList = new List<string>();

            using (principalContext = new PrincipalContext(ContextType.Domain, domain, userName, userPassword))
            using (UserPrincipal userPrin = UserPrincipal.FindByIdentity(principalContext, _samAccountName))
            {
                if (userPrin != null)
                {
                    var groups = userPrin.GetAuthorizationGroups();

                    foreach (Principal gp in groups)
                    {
                        groupList.Add(gp.ToString());
                    }

                }

                return groupList;
            }


        }

        public string DisableUser(string samAccountName)
        {
            string disableState = string.Empty;
            string _samAccountName = samAccountName;


            try
            {
                if (principalContext == null)

                    principalContext = new PrincipalContext(ContextType.Domain, domain, userName, userPassword);

                using (var user = UserPrincipal.FindByIdentity(principalContext, _samAccountName))
                {
                    if (user.Enabled == true)
                    {
                        user.Enabled = false;
                        user.Save();
                        disableState = "Kullanıcı başarıyla Pasif edildi";
                    }

                    else
                    {
                        disableState = "Kullanıcı zaten pasif";

                    }


                }

            }

            catch (Exception ex)
            {
                disableState = ex.Message;

            }

            return disableState;
        }

        public string AddUserToAdmin(string samAccountName, string groupName)
        {

            try
            {
                if (principalContext == null)

                {
                    principalContext = new PrincipalContext(ContextType.Domain, domain, userName, userPassword);
                }

                GroupPrincipal group = GroupPrincipal.FindByIdentity(principalContext, groupName);
                group.Members.Add(principalContext, IdentityType.UserPrincipalName, samAccountName);
                group.Save();
            }
            catch (DirectoryServicesCOMException ex)
            {
                return ex.Message;
            }

            catch (Exception exc)
            {
                return exc.Message;
            }

                return "Başarıyla Eklendi";


        }

    }

    public class UserProperties
    {
        public string cannonicalName { get; set; }

        public string samAccountName { get; set; }

        public string userAccountControlCode { get; set; }

        public string userAccountControl { get; set; }

        public string lastLogon { get; set; }

        public string whenCreated { get; set; }

        public string pwdLastSet { get; set; }


    }

}

