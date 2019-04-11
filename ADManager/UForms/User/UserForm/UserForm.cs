
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADManager
{
    public partial class UserForm : Form
    {

        private string userName { get; set; }
        private string logonName { get; set; }
        private string logonPass { get; set; }
        private string deletedUserSamName { get; set; }
        private DataTable searchedUserDT { get; set; }
        private bool isCached { get; set; }
        public DataTable allUserDT { get; set;}


       
       
        private GetUsers getUsers { get; set; }
       
        public static string searchedName {get;set;}




      

     
        public UserForm()
        {
            logonName = Giris._userName;
            logonPass = Giris._userPassword;
            allUserDT = new DataTable();
            isCached = false;
        
           
          
            InitializeComponent();
        }

        private void CreateUser_Load(object sender, EventArgs e)
        {
            SetDataGridSettings();
        }

        public void SetDataGridSettings()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

        }

        /// <summary>
        ///  Get TextBox text values from UserForm 
        /// </summary>


        private void button1_Click(object sender, EventArgs e)
        {
            if (NameTxt.Text == string.Empty || SurnameTxt.Text == string.Empty || PasswordTxt.Text == string.Empty)

                MessageBox.Show("Lütfen tüm alanları doldurunuz");

            else 

              SaveUserData();
           
             

        }

      
        private void SaveUserData()
        {
            var addUser = new AddUser(NameTxt.Text, SurnameTxt.Text, UserNameTxt.Text, PasswordTxt.Text);
            string response = addUser.SaveUser();
            MessageBox.Show(response);
         
        }

        private void AraBtn_Click(object sender, EventArgs e)
        {
            FillGridview(sender);
        }

        private void GetAllBtn_Click(object sender, EventArgs e)
        {
            FillGridview(sender);
        }

        // Kullanıcı IU ekranındaki Gridview'i AD Veritababındaki kullanıcılar ile dolduruyoruz.
        public void FillGridview(object sender)
        {
            searchedName = SearchTxt.Text;
          
            try
            {
                if (((Button)sender).Name == "GetAllBtn")
                {

                    if (!isCached)
                    {
                        dataGridView1.Columns.Clear();
                        dataGridView1.DataSource = GetAllUser();
                    }
                  
                }

                else if (((Button)sender).Name == "AraBtn")

                {

                    dataGridView1.Columns.Clear();
                    getUsers = new GetUsers();
                    searchedUserDT = getUsers.SearchUser(searchedName);

                    if (searchedUserDT.Rows.Count == 0)
                    {
                        MessageBox.Show("Kayıt Bulunamadı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    dataGridView1.DataSource = searchedUserDT;
                    isCached = false;
                    allUserDT = new DataTable();
                   
                }

                
                for (int a = 1; a <= 7; a++)

                {
                    dataGridView1.Columns[a].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

    


            }

            catch (ArgumentOutOfRangeException Exc)
            {
                MessageBox.Show(Exc.Message);
            }

            catch (Exception ex)
            {
                MessageBox.Show(getUsers.getUserMessage);

            }
        

        }


        public DataTable GetAllUser()
        {
            if (allUserDT.Rows.Count == 0)
            {
                getUsers = new GetUsers();
                allUserDT = getUsers.GetAllADUsers();
                isCached = true;

            }
            MessageBox.Show("aa");
            MessageBox.Show(allUserDT.Rows[2][2].ToString());
            return allUserDT;
           


        }

      

        // Fill usernameTxt text as like (name.surname) automatically. 
        private void SurnameTxt_KeyUp(object sender, KeyEventArgs e)
        {

            if (NameTxt.Text != string.Empty)

                UserNameTxt.Text = this.userName = NameTxt.Text + "." + SurnameTxt.Text;


        }

        // Delete user. 
        public void DeleteUser(string samName)
        {
         
            string _samName = samName;
            var deleteUser = new DeleteUser(_samName);
            string deleteUserResponse = deleteUser.KullaniciSil();
            MessageBox.Show(deleteUserResponse);

        }


        /// <summary>
        /// Mouse event on datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

            // Getting row number that curson on 
            //TR: Üzerinde bulunduğumuz satırın numarasını alıyoruz.
            int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                // create a context menu 
                ContextMenu cm = new ContextMenu();

                //check if curson on a row 
                if (currentMouseOverRow < 0)
                {

                   
                    MessageBox.Show("Lütfen bir satır seçiniz");
                }

                // Adding menu. 
               
                cm.MenuItems.Add(new MenuItem("Kullanıcı Üyeliklerini Göster", GetUserGroup));
                cm.MenuItems.Add(new MenuItem("Kullanıcıyı Pasife Al",DisableUser));
                cm.MenuItems.Add(new MenuItem("Admin Yetkisi Ver", AdminYetkisiVer));
                cm.MenuItems.Add(new MenuItem("Kullanıcı Sil", datagridview1_kullaniciSil));
                cm.Show(dataGridView1, new Point(e.X, e.Y));


            }


        }


        // Gets the Authorization Group list of Selected User.
        // Seçilen Kullanıcının Üyelik Grup Listesinin getirir. 
        private void GetUserGroup(object sender, EventArgs e)
        {

            List<string> userGroupList;
            var usersMemFrm = new UsersMemFrm();

           
        

            try
            {
                string samName = dataGridView1.SelectedCells[1].Value.ToString();
                UserMemGroups userMemGroups = new UserMemGroups(samName);
                userGroupList = userMemGroups.GetUserMemberGroups();
                usersMemFrm.PrintGroups(userGroupList);
                usersMemFrm.Show();

              
            }
            catch (ArgumentOutOfRangeException)
            {

                MessageBox.Show("Lütfen tüm satırı seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }


        private void DisableUser(object sender, EventArgs e)
        {
            try
            {
                string samAccountName = dataGridView1.SelectedCells[1].Value.ToString();
                var disableUser = new DisableUser(samAccountName);
                string disableState = disableUser.KullaniciPasifYap();
                MessageBox.Show(disableState);
            }

            catch (ArgumentOutOfRangeException)
            {

                MessageBox.Show("Lütfen tüm satırı seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void AdminYetkisiVer(object sender , EventArgs e)
        {
            try
            {
                string samName = dataGridView1.SelectedCells[2].Value.ToString();
                var adminYetkiForm = new AdminUye(samName);
                adminYetkiForm.Show();

            }
            catch (ArgumentOutOfRangeException)
            {

                MessageBox.Show("Lütfen tüm satırı seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

         
            
        }
        /// <summary>
        /// Occurs when datagridview Right Click item "Kullanıcı Sil" clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagridview1_kullaniciSil(object sender, EventArgs e)
        {

            try
            {
                // Gridview'de seçilen satırın samAccountName değerini seçer.
                deletedUserSamName = dataGridView1.SelectedCells[2].Value.ToString();

                var deleteUserDialog = MessageBox.Show(deletedUserSamName + " Kullanıcısını silmek istiyor musunuz", "Uyarı", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (deleteUserDialog == DialogResult.Yes)

                {
                    var deleteUser = new DeleteUser(deletedUserSamName);
                    string state = deleteUser.KullaniciSil();
                    MessageBox.Show(state);
                }

            }



            catch (ArgumentOutOfRangeException)
            {

                MessageBox.Show("Lütfen tüm satırı seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void FormCloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Minimize Form on Panel Right Click. 
        // TR: Formun en üstünde sağ tık yaparak formu simge durumuna alıyoruz
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu cm = new ContextMenu();
                cm.MenuItems.Add(new MenuItem("Simge durumuna al ", MinimizeForm));
                cm.Show(panel1, new Point(e.X, e.Y));
            }
        }

        private void MinimizeForm(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void ExcelBtn_Click(object sender, EventArgs e)
        {
            DataGridToExcel();
        }

        private void DataGridToExcel()
        {
            if (dataGridView1.DataSource != null)
            {
                DataTable userData = (DataTable)dataGridView1.DataSource;
                Report userReport = new Report(userData);
              MessageBox.Show(userReport.ReportGridviewToExcel());

            }


        }

        private void ClearTxtBtn_Click_1(object sender, EventArgs e)
        {
           
            foreach (Control c in groupBox1.Controls)
            {
                if (c is TextBox)
                    c.Text = "";
            }
        }

       
    }


    /// <summary>
    /// Get The Values on UserForm 
    /// </summary>
    public class UserFormData
    {
        public string userName { get; set; }

        public string userPassword { get; set; }

        public string name { get; set; }

        public string surName { get; set; }

        public bool isUserAktif { get; set; }





    }
}
