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
    public partial class AdminUye : Form
    {

        private string samAccountName { get; set; }
      

        public  AdminUye(string samAccountName)
        {
            this.samAccountName = samAccountName;
            InitializeComponent();
        }

        private void TekSecBtn_Click(object sender, EventArgs e)
        {
            TekAktar();
        }

        public void TekAktar()
        {
            listBox3.Items.Add(listBox2.SelectedItem.ToString());


        }

        public void HepsiniAktar()
        {
            foreach(var group in listBox2.Items)

                listBox3.Items.Add(group);
        }

        private void HepsiSecBtn_Click(object sender, EventArgs e)
        {
            HepsiniAktar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        private void KydtBtn_Click(object sender, EventArgs e)
        {
            AddUsertToAdminGroup();
        }

        public void AddUsertToAdminGroup()
        {
            if (listBox3.Items.Count != 0)
            {
                string groupName = listBox3.Items[0].ToString();
                var addUserToAdmin = new AddAdmin(samAccountName,groupName);
                MessageBox.Show(addUserToAdmin.AddUserAdmin());

            }
                


        }

        private void AdminUye_Load(object sender, EventArgs e)
        {

        }
    }
}
