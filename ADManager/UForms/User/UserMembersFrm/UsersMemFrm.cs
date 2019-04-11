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
    public partial class UsersMemFrm : Form
    {
        public UsersMemFrm()
        {
            InitializeComponent();
        }


        public void PrintGroups(List<string> groups)
        {
            foreach (var grp in groups)
            {
               GroupList.Items.Add(grp);
            }


        }

     
    }
}
