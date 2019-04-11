using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADManager
{
    class UserMemGroups
    {
        private string samAccountName { get; set; }
       

        public UserMemGroups(string samAccountName)
        {
            this.samAccountName = samAccountName; 
        }

        public List<string> GetUserMemberGroups()
        {
            var user = new User();
            List<string> userGroup = user.GetAutharizationGroup(samAccountName);
            return userGroup;

        }

    }
}
