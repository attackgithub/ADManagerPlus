using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ADManager
{
    class DataTableFill
    {

       

        private List<UserProperties> userData { get; set; }

        public DataTable dt { get; set; }

        public DataTable computerDt { get; set;}

        private bool isError { get; set; }

        public string errorMessage { get; set; }

        private string logonName { get; set;}

        private string logonPass { get; set; }


        public DataTableFill()
        {
            logonName = Giris._userName;
            logonPass = Giris._userPassword;

        }

        /// <summary>
        /// Fill DataGridView on  User Form. 
        /// Kullanıcı Formlarındaki datagridviewleri bu class yordamıyla dolduruyoruz.
        /// </summary>
        /// <param name="usersDataList"></param>
        /// <returns> Domain Users  as List </returns>
        public DataTable FillDataTable(List<UserProperties> usersDataList)
        {
           
            try
            {
                var user = new User();

                userData = usersDataList;

                isError = user.isError;

                if (!user.isError)
                {

                dt = new DataTable();
                dt.Columns.Add("Kayıt No", typeof(int));
                dt.Columns.Add("İsim", typeof(string));
                dt.Columns.Add("Kullanıcı Adı", typeof(string));
                dt.Columns.Add("Kullanıcı Aktif Kodu", typeof(string));
                dt.Columns.Add("Kullanıcı Durumu", typeof(string));
                dt.Columns.Add("Son Bağlantı", typeof(string));
                dt.Columns.Add("Katılma Tarihi", typeof(string));
                dt.Columns.Add("Parola Değiştirme", typeof(string));

                    for (int i = 0, j = 1; i < userData.Count && j <= userData.Count; i ++, j++)
                    {

                        dt.NewRow();

                        UserProperties users = userData[i];
                        dt.Rows.Add(j, users.cannonicalName, users.samAccountName, users.userAccountControlCode, users.userAccountControl, users.lastLogon, users.whenCreated, users.pwdLastSet);
                    
                      
                    }

                }

                else
                {

                    errorMessage = "Hata Oluştu";
                    dt = null;

                }

            }

            catch (Exception ex)
            {
                errorMessage = ex.Message;
                dt = null;

            }


            return dt;
        }

        /// <summary>
        /// Fill DataGridView on  Computer Form. 
        /// Bilgisayar Formlarındaki datagridviewleri bu class yordamıyla dolduruyoruz.
        /// </summary>
        /// <param name="computerList"></param>
        /// <returns> Domain Computers  as List </returns>
        public DataTable FillDataTableWithComputers(List<ComputersProperties> computerList)
        {
            
            List<ComputersProperties> _computerList = computerList;

            try
            {

                computerDt = new DataTable();
                computerDt.Columns.Add("Kayıt No", typeof(int));
                computerDt.Columns.Add("Pc Adı", typeof(string));
                computerDt.Columns.Add("Sid", typeof(string));
                computerDt.Columns.Add("İp Adresi", typeof(string));
                computerDt.Columns.Add("Son Parola Oluşturma", typeof(string));
                computerDt.Columns.Add("Son Başarısız Parola Giriş", typeof(string));
               

                for (int i = 0, j = 1; i < _computerList.Count && j <= _computerList.Count; i ++, j++)
                {

                   computerDt.NewRow();
                   ComputersProperties computerPro = _computerList[i];
                   computerDt.Rows.Add(j,computerPro.computerName, computerPro.computerSID, computerPro.ipAdress, computerPro.lastPassSet, computerPro.lastBadAttempt);
                }

            }
            catch (Exception ex)
            {

                computerDt= null;

            }


            return computerDt;


        }


        }


    }

