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
    public partial class ComputerInfoForm : Form
    {

        private string ipAddress { get; set; }
        public ComputerInfoForm(string ipAddress)
        {
            this.ipAddress = ipAddress;
            InitializeComponent();
        }

     

        private void GetirBtn_Click(object sender, EventArgs e)
        {
            CpuLbl.Text = string.Empty;
            ComputerInfo computerInfo = new ComputerInfo(ipAddress);
            List<string> cpuList = computerInfo.GetCpuInfo();
            if (computerInfo.errState)

                MessageBox.Show(computerInfo.infoErr);

            else
            {
                foreach (var a in cpuList)

                    CpuLbl.Text += a;
            }
           
        }
    }
}
