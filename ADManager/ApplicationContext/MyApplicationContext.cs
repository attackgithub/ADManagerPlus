﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ADManager
{
    class MyApplicationContext:ApplicationContext
    {
        public MyApplicationContext(Form mainForm)
            :base (mainForm)
        {

        }

        // Makes Active form as Main Form. 
        protected override void OnMainFormClosed(object sender, EventArgs e)
        {
            if (Form.ActiveForm != null)
            {
                this.MainForm = Form.ActiveForm;
            }
            else
            {
                base.OnMainFormClosed(sender, e);
            }
            }


    }
}
