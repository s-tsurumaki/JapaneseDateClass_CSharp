using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapaneseDateClass
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            JapaneseDateClass.Class.JapaneseDate jpd = new Class.JapaneseDate(dateTimePicker.Value);
            lblDateRetDate.Text = jpd.Date;
        }

        private void btnStrConv_Click(object sender, EventArgs e)
        {
            JapaneseDateClass.Class.JapaneseDate jpd = new Class.JapaneseDate(DateTime.Now);

            if (jpd.SetData(txtDateString.Text) == Class.JapaneseDate.DateStatus.Success)
            {
                lblDateRetString.Text = jpd.Date;
            }
            else
            {
                lblDateRetString.Text = "変換できない文字列";
            }
            
        }
    }
}
