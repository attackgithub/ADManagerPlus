using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ADManager
{
    class DisableUser
    {
        private string samAccountName { get; set; }

      
        public DisableUser(string samAccountName)
        {
            this.samAccountName = samAccountName;


        }


        public string KullaniciPasifYap()
        {

            var User = new User();

           return User.DisableUser(samAccountName);




        }
    }
}
