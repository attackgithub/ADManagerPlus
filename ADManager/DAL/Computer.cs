using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Net;
using System.Management;

namespace ADManager
{
   public  class Computer
    {

       

        private readonly string domainServer = System.Configuration.ConfigurationManager.AppSettings["domainServer"];

        private readonly string computerDomain = System.Configuration.ConfigurationManager.AppSettings["computerDomain"];

        private string ErrorMessage;
     
      

        /// <summary>
        /// Getting Ip Adresses of Computers.
        /// </summary>
        /// <param name="computerName">Name of Computer to get ip address.</param>
        /// <returns>ip adress of computer</returns>
       public string GetComputerIpAddress(string computerName)
        {


            string ipAdress = string.Empty;
            string computerDomainName = computerName + "." + computerDomain;
            try
            {
                if (computerName != null)
                    ipAdress = Dns.Resolve(computerDomainName).AddressList[0].ToString();
            }

            catch (Exception ex)
            {
                ipAdress = "İp Adresi Çözümlenemedi";

            }
            return ipAdress;

        }

        /// <summary>
        /// Searching for all computers in domain. 
        /// </summary>
        /// <returns> List of Computers.</returns>
        public List<ComputersProperties> SearchComputer()
        {
            
            List<ComputersProperties> computerProList = new List<ComputersProperties>();
            try
            {
                using (var principialCtx = new PrincipalContext(ContextType.Domain, domainServer))
                using (var computerPricipial = new ComputerPrincipal(principialCtx))
                using (var priSearcher = new PrincipalSearcher(computerPricipial))
                {
                    computerPricipial.Name = "*";
                    priSearcher.QueryFilter = computerPricipial;
                    PrincipalSearchResult<Principal> computerList = priSearcher.FindAll();

                    foreach (var p in computerList)
                    {
                        ComputerPrincipal pc = (ComputerPrincipal)p;
                        string ipAdress = GetComputerIpAddress(p.Name);
                        var computerPro = new ComputersProperties(pc.Name, pc.Sid.ToString(), ipAdress, pc.LastPasswordSet.ToString(),pc.LastBadPasswordAttempt.ToString());
                        computerProList.Add(computerPro);
                      
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }

            return computerProList;


        }


       


    }


    public class ComputersProperties
    {

        public string computerName { get; set; }
        public string computerSID { get; set; }
        public string ipAdress { get; set; }
        public string lastPassSet { get; set; }
        public string lastBadAttempt { get; set; }

        public ComputersProperties(string computerName, string computerSID, string ipAdress, string lastPassSet, string lastBadAttempt)
        {
            this.computerName = computerName;
            this.computerSID = computerSID;
            this.ipAdress = ipAdress;
            this.lastPassSet = lastPassSet;
            this.lastBadAttempt = lastBadAttempt;

        }

     

    }



}
