using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ADManager
{
    class ComputerBL
    {

      

        private string _arananPc { get; set; }

        private string _btnName { get; set; }

        public string errMes { get; set; }

       
        public List<ComputersProperties> computerList { get; set; }

       

        public void GetAllComputers()
        {
            var computer = new Computer();
            computerList = computer.SearchComputer();
           


        }

        public List<ComputersProperties> SearchSingleComputer(string arananPc)
        {

            List<ComputersProperties> foundComList = new List<ComputersProperties>();
            _arananPc = arananPc;
            bool isContain = false;

            try
            {

                if (computerList == null)
                {
                    GetAllComputers();
                }

                for (int a = 0; a < computerList.Count; a ++)
                {
                    var computerPro = computerList[a];
                    isContain = computerPro.computerName.Contains(_arananPc) || computerPro.ipAdress.Contains(_arananPc);

                    if (isContain)
                    {

                        foundComList.Add(computerPro);
                      

                    }

                }    

            }

            catch (Exception ex)
            {
                throw ex;

            }

            return foundComList;


        }

        public DataTable FillGridView(string btnName, string arananPc)
        {

            _arananPc = arananPc;
            _btnName = btnName;
            List<ComputersProperties> secilenPcList= new List<ComputersProperties>();
            var dataTableFill = new DataTableFill();

            if (computerList == null)
            {
                GetAllComputers();
            }

            // Creating DataTableFill class instance
           

            if ( _btnName== "AraBtn")
            {
                secilenPcList = SearchSingleComputer(_arananPc);

            }

            else if (_btnName == "ListAll")
            {
                secilenPcList = computerList;
            }

           DataTable dataTableComputers = dataTableFill.FillDataTableWithComputers(secilenPcList);
            return dataTableComputers;

            


        }

        public void ShutDownComputer(string computerName)
        {
            string _computerName = computerName;
            System.Diagnostics.ProcessStartInfo shutDownPs = new System.Diagnostics.ProcessStartInfo("shutdown");
            shutDownPs.Arguments = $"/m \\\\{_computerName} /s /t 0 ";
            System.Diagnostics.Process.Start(shutDownPs);


        }

        public void RebootComputer(string computerName)
        {
            string _computerName = computerName;
            System.Diagnostics.ProcessStartInfo rebootPs = new System.Diagnostics.ProcessStartInfo("shutdown");
            rebootPs.Arguments = $"/m \\\\{_computerName} /r /t 0";
            System.Diagnostics.Process.Start(rebootPs);
        }



    }
}
