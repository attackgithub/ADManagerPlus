using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace ADManager
{
    class Authentication
    {


        private string userName { get; set; }
        private string password { get; set; }

        private readonly string path = System.Configuration.ConfigurationManager.AppSettings["path"];

        public string authErr { get; set; }

        public Authentication(string userName, string password)
        {
            this.userName = userName;
            this.password = password;

        }

        public bool IsAuthenticated()
        {

            bool authenticated = false;

            if (userName.Trim().Length != 0 && password.Trim().Length != 0)
            {

                try
                {
                    using (DirectoryEntry ldapEntry = new DirectoryEntry(path, userName, password))
                    {
                        object nativeObject = ldapEntry.NativeObject;
                        Giris._userName = userName;
                        Giris._userPassword = password;
                        authenticated = true;
                    }
                }

                catch (DirectoryServicesCOMException dirExc)
                {
                    //not authenticated
                    authErr = dirExc.Message;
                }

                catch (Exception ex)
                {
                    //not authenticated for other reason
                    authErr = ex.Message;

                }
            }

            else

                authErr = "Kullanıcı Adı ya da parola boş olamaz";

            return authenticated;

        }
    }
}
