using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ADManager
{
    class DeleteUser
    {

        private string samAccountName { get; set;}
       

        public DeleteUser(string samAccountName)
        {
            this.samAccountName = samAccountName;
         
        }

        public string KullaniciSil()
        {
            var user = new User();
            string deleteUserResponse = user.DeleteUser(samAccountName);
            return deleteUserResponse;

        }
    }
}
