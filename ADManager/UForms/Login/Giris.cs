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
using System.Net;


namespace ADManager
{
    public partial class Giris : Form
    {
       
        // This is Login Form Class.Checks if username and password valid on domain server. 
       
        
        // Parameters to make form movable
        int TogMove, Mvalx, Mvaly;
        public static string _userName { get; set; }

        public static string _userPassword { get; set; }

        public Giris()
        {
            InitializeComponent();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Giris_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void Giris_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)

                this.SetDesktopLocation(MousePosition.X - Mvalx, MousePosition.Y - Mvaly);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        ///  "Giriş" button pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GirisBtn_Click(object sender, EventArgs e)
        {
           
            Authentication auth = new Authentication(TxtUser.Text, TxtPass.Text);
            bool isAuth = auth.IsAuthenticated();

            if (isAuth)
            {

                Home hm = new Home();
                hm.Show();
                this.Close();
            }
            else
                MessageBox.Show(auth.authErr);

      

        }

     

        private void Giris_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            Mvalx = e.X;
            Mvaly = e.Y;
        }

        /// <summary>
        /// Authentication controller class.
        /// </summary>
        /// <param name="username"> username from  login screen </param>
        /// <param name="password">  password from login screen </param>
        /// <returns></returns>
    }
   



}

