using System;
using System.Windows.Forms;
using JapaneseDateClass.Class;
using JapaneseDateClass.Test;
using System.Collections.Generic;

namespace JapaneseDateClass
{


    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 日付を和暦に変換します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            JapaneseDate jpd = new JapaneseDate(dateTimePicker.Value);
            lblDateRetDate.Text = jpd.Date;
        }

        /// <summary>
        /// 日付を和暦に変換します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStrConv_Click(object sender, EventArgs e)
        {
            JapaneseDate jpd = new JapaneseDate(DateTime.Now);

            if (jpd.SetData(txtDateString.Text) == JapaneseDate.DateStatus.Success)
            {
                lblDateRetString.Text = jpd.Date;
            }
            else
            {
                lblDateRetString.Text = "変換できない文字列";
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJapaniseDateLimitLine_Click(object sender, EventArgs e)
        {




        }
    }
}
