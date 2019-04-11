using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace ADManager
{
    class AddUser
    {

        private string name { get; set; }
        private string surName { get; set; }
        private string message { get; set; }
        private string userName { get; set; }
        private string userPassword { get; set; }
        private bool kayitDurum { get; set; }
       


       


        public AddUser(string name, string surName, string userName, string userPassword)
        {
            this.name = name;
            this.surName = surName;
            this.userName = userName;
            this.userPassword = userPassword;
       
        

        }

        // Method to Add User on DC Server .
        public string SaveUser()
        {

            try
            {
             
                var userFormInput = new UserFormInputs(name, surName, userName, userPassword, true);
                var user = new User();
                kayitDurum = user.CreateUserAccount(userFormInput);

                if (kayitDurum)

                    message = "Kayıt Başarıyla Eklendi";

                else

                    message = user.errorMessage;
            }

            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

      



    }
}
