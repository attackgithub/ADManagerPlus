using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Collections;

namespace ADManager
{



    public partial class Home : Form
    {

        // Parameters  to make form movable on mouse  press.
        private int TogMove, MvalX, MvalY;

        // Property name of Cannonical Name 
        public const string canName = "CN";

        private string logonName { get; set; }

        private string logonPass { get; set; }
        private DataTable dt { get; set; }


        public Home()
        {

            logonName = Giris._userName;
            logonPass = Giris._userPassword;
            InitializeComponent();

        }


        private void Home_Load(object sender, EventArgs e)
        {
            SetDataGridSettings();
            SetLabelSettings();

        }


        /// <summary>
        /// Set DataGridview Design.
        /// DataGridview tasarımını yaptığımız methodumuz.
        /// </summary>
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
        /// Set Label Text.
        /// </summary>
        public void SetLabelSettings()
        {
            label2.Text = DateTime.Today.ToLongDateString() + " " + System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(System.DateTime.Now.DayOfWeek);
            label3.Text = "Bağlantı Kuruldu : " + System.Configuration.ConfigurationManager.AppSettings["path"];
        }

        
        private void KullaniciBilgiGetirBtn_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.DataSource != null)
                this.dataGridView1.DataSource = null;


            dataGridView1.Columns.Clear();

            FillGridview();
            label5.Text = $"{dataGridView1.Rows.Count} adet kayıt listelendi";
            label5.Visible = true;
        }

        // Domain Sunucudaki tüm kullanıcıları alıp, UI Form üzerindeki Gridview'de gösteriyoruz. 
        private void FillGridview()
        {
            try
            {
                if (dt == null)
                {
                    var users = new GetUsers();
                    dt = users.GetAllADUsers();
                }
              
                dataGridView1.DataSource = dt;
 
                for (int a = 1; a <= 6; a++)
                   
                    dataGridView1.Columns[a].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }



        //  HOME IU formunun mouse ile taşınmasını sağlayan methodlar. 
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MvalX = e.X;
            MvalY = e.Y;
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)

                this.SetDesktopLocation(MousePosition.X - MvalX, MousePosition.Y - MvalY);


        }


        private void KullaniciEkleBtn_Click(object sender, EventArgs e)
        {
           
            OpenForm(sender);

        }

        private void CihazYonetimBtn_Click(object sender, EventArgs e)
        {
           
            OpenForm(sender);
        }

        private void OpenForm(object sender)
        {
          
            bool isOpen = false;
            string formName = string.Empty;

            switch (((Button)sender).Name)
            {
                case "KullaniciEkleBtn":
                    formName = "UserForm";
                    break;

                case "CihazYonetimBtn":
                    formName = "ComputerForm";
                    break;

            }
                
           for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                {
                    if (Application.OpenForms[i].Name == formName)
                    {
                        MessageBox.Show("Bu Form Zaten açık", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isOpen = true;
                    }

                }

            if (!isOpen)
            {

                this.WindowState = FormWindowState.Minimized;

                switch(formName)
                {
                    case "UserForm":

                        ShowForm <UserForm>();
                        break;

                    case "ComputerForm":

                        ShowForm<ComputerForm>();
                        break;
 
                }
                  
            }


        }

        private void ShowForm<T>() where T : Form
        {

            var form = (Form)Activator.CreateInstance(typeof(T));
            form.Show();
           
        }


        //Close All Forms And Exit From Application. - TR: Home Form kapandığında tüm formlar da kapansın.
        private void FormCloseBtn_Click(object sender, EventArgs e)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                Application.OpenForms[i].Close();
        }

        private void RaporlamaBtn_Click(object sender, EventArgs e)
        {
            var user = new User();
            user.SearchUser("far");
          //  IEnumerable<UserProperties> kullaList = user.SearchUser("far");
          //  foreach (var a in kullaList)
           // {
             //   MessageBox.Show(a.lastLogon);

            //}
           // user.GetAllUsers();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ExcelBtn_Click(object sender, EventArgs e)
        {
            DataGridToExcel();
        }

        // Kullanıcı Listesinin Excel formatında raporlanmasını sağlar.
        private void DataGridToExcel()
        {
            if (dataGridView1.DataSource != null)
            {
                DataTable usersData = (DataTable)dataGridView1.DataSource;
                Report userReport = new Report(usersData);
                MessageBox.Show(userReport.ReportGridviewToExcel());

            }

        }

     

    }
}
