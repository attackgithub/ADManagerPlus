using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace ADManager
{
    class GetUsers
    {


        private List<UserProperties> userDataList;
        private List<UserProperties> foundUserList { get; set; }
        private bool isContain { get; set; }
        public string foundMessage { get; set; }
        public string getUserMessage { get; set; }
    

       

        public GetUsers()
        {
         
           
        }
       
        // AD üzerinde Kullanıcı arama yapılan methodumuz.
       public DataTable SearchUser(string searchedPerson)
        {
            string _searchedPerson = searchedPerson;
            var user = new User();
            IEnumerable<UserProperties> foundUserList = user.SearchUser(_searchedPerson);
            return DataTableAktar(foundUserList);
        }
      

        public DataTable GetAllADUsers()
        {
            var user = new User();
            IEnumerable<UserProperties> adUserList = user.GetAllUsers();
            return DataTableAktar(adUserList);


        }

        public DataTable DataTableAktar(IEnumerable<UserProperties> userList)
        {
            List<UserProperties> _userList = userList.ToList();
            var fillDataTable = new DataTableFill();
            return fillDataTable.FillDataTable(_userList);
        }

       

    }
}
