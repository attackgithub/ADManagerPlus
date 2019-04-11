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
    public partial class ComputerForm : Form
    {





        private ComputerBL computerBl;

        private string computerName { get; set; }
        // Form moving parameters.
        private int tagMove, MouseX, MouseY;

        // Getting Datagridview Rows data from DataTableFill class
    
        public ComputerForm()
        {
            
            InitializeComponent();
        }

        // "Tüm Kayıtları Sorgula" button clicked.


        private void ListAll_Click(object sender, EventArgs e)
        {

            FillGridView(sender);
            label2.Text = $"{dataGridView1.Rows.Count} adet kayıt listelendi";
        }

        private void ComputerForm_Load(object sender, EventArgs e)
        {
            SetDataGridSettings();
        }


        /// <summary>
        /// Get All Computers From Domain.
        /// </summary>
     

        private void FillGridView(object sender)
        {

            computerBl = new ComputerBL();
            DataTable computersData = new DataTable();
            dataGridView1.Columns.Clear();

            // Creating DataTableFill class instance

            try
            {
                if (((Button)sender).Name == "AraBtn")
                {
                    computersData = computerBl.FillGridView("AraBtn", SearchTxt.Text);

                }

                else if (((Button)sender).Name == "ListAll")
                {
                    computersData = computerBl.FillGridView("ListAll", SearchTxt.Text);
                }


                dataGridView1.DataSource = computersData;

                for (int a = 1; a <= 5; a++)
                {
                    dataGridView1.Columns[a].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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

        private void FormCloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AraBtn_Click(object sender, EventArgs e)
        {
            FillGridView(sender);
            label2.Text = $"{dataGridView1.Rows.Count} adet kayıt listelendi";
        }


        // Form Movement Methods. 
        // TR: Formu hareket ettirmek için kullanılan methodlar.
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            tagMove = 1;
            MouseX  = e.X;
            MouseY = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (tagMove == 1)
                this.SetDesktopLocation((MousePosition.X - MouseX), (MousePosition.Y - MouseY));
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            tagMove = 0;
        }

     
        public void DataGridToExcel()
        {
            if (dataGridView1.DataSource != null)
            {
                DataTable computerDataTable = (DataTable)dataGridView1.DataSource;
                Report report = new Report(computerDataTable);
               MessageBox.Show(report.ReportGridviewToExcel());

            }

            else

                MessageBox.Show("Lütfen Liste Oluşturunuz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ExcelBtn_Click(object sender, EventArgs e)
        {
            DataGridToExcel();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            int currentMouseRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu cm = new ContextMenu();
                if (currentMouseRow < 0)

                {
                    MessageBox.Show("Lütfen bir satır seçiniz ");
                }

                cm.MenuItems.Add("Yeniden Başlat",RebootComputer);
                cm.MenuItems.Add("Kapat", ShutDownComputer);
                cm.MenuItems.Add("Bilgisayar Bilgileri", GetComputerInfo);
                cm.Show(dataGridView1,new Point(e.X,e.Y));
                }
        }

        private void RebootComputer(object sender, EventArgs e)
        {
            try
            {
                computerName = dataGridView1.SelectedCells[1].Value.ToString();
                computerBl = new ComputerBL();
                computerBl.RebootComputer(computerName);
            }

            catch (ArgumentOutOfRangeException )
            {
                MessageBox.Show("Lütfen tüm satırı seçiniz","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

           
        }

        private void ShutDownComputer(object sender, EventArgs e)
        {
            try
            {
                computerName = dataGridView1.SelectedCells[1].Value.ToString();
                computerBl = new ComputerBL();
                computerBl.ShutDownComputer(computerName);
            }

            catch (ArgumentOutOfRangeException )
            {
                MessageBox.Show("Lütfen tüm satırı seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void KaydetBtn_Click(object sender, EventArgs e)
        {

        }

        private void GetComputerInfo(object sender, EventArgs e)
        {
            try
            {
                string ipAddress = dataGridView1.SelectedCells[1].Value.ToString();
                ComputerInfoForm compForm = new ComputerInfoForm(ipAddress);
                compForm.Show();
            }
            catch (ArgumentOutOfRangeException )
            {
                MessageBox.Show("Lütfen tüm satırı seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }   
        }
       

        private void SearchTxt_MouseDown(object sender, MouseEventArgs e)
        {
            SearchTxt.Clear();
        }

        

    }
}
