using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ADManager
{
    class AddAdmin
    {
        private string samName { get; set; }
        private string groupName { get; set; }
        public AddAdmin(string samName, string groupName)
        {
            this.samName = samName;
            this.groupName = groupName;
        }

        public string AddUserAdmin()
        {
            var user = new User();
            return user.AddUserToAdmin(samName, groupName);


        }

    }
}
