using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace ADManager
{
    class ComputerInfo
    {
        private string ipAddress { get; set; }
        private string computerDomain { get; set; }

        public string infoErr { get; set; }
        public bool errState;
        public ComputerInfo(string ipAddress)
        {
            this.ipAddress = ipAddress;

        }



        public List<string> GetCpuInfo()
        {
          
            computerDomain = System.Configuration.ConfigurationManager.AppSettings["computerDomain"];
            ConnectionOptions oConn = new ConnectionOptions();
            oConn.Username =computerDomain+"\\"+Giris._userName;
            oConn.Password = Giris._userPassword;
            List<string> cpuList = new List<string>();

            try
            {
                var oScope = new ManagementScope($"\\\\{ipAddress}\\root\\CIMV2", oConn);
                oScope.Options.EnablePrivileges = true;
               
                oScope.Connect();


                var query = new ObjectQuery(" Select * from Win32_Processor");
                var  ObjSearcher = new ManagementObjectSearcher(oScope, query);

                foreach (var obj in ObjSearcher.Get())
                {
                    cpuList.Add(obj["Name"].ToString());

                }
            }
           

            catch (System.Runtime.InteropServices.COMException comEx)
            {
                errState = true;
                infoErr = comEx.Message;
            }

            catch (Exception err)

            {
                errState = true;
                infoErr = err.Message;
            }

            return cpuList;


        }

    }
}
