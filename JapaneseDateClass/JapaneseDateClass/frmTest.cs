using System;
using System.Windows.Forms;
using JapaneseDateClass.Class;
using JapaneseDateClass.Test;
using System.Collections.Generic;

namespace JapaneseDateClass
{


    /// <summary>
    /// 
    /// </summary>
    public partial class frmTest : Form
    {

        /// <summary>
        /// 
        /// </summary>
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

        private void button2_Click(object sender, EventArgs e)
        {
            Test.TestUnitItem cxt = new TestUnitItem();

            Console.WriteLine("ChrckString:X99.99.99");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.ToCharAlphabet_Dot;
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w,item.Answer, jpd.Date);
            }

            Console.WriteLine("ChrckString:X99/99/99");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.ToCharAlphabet_Slash;
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.Date);
            }

            Console.WriteLine("ChrckString:9999/99/99");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.ToCharAlphabet_Slash;
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.Date);
            }

            Console.WriteLine("ChrckString:9999.99.99");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.ToCharAlphabet_Dot;
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.Date);
            }

            Console.WriteLine("Chrck7Int:9yyMMdd");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.To7Int.ToString();
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.Date);
            }

            Console.WriteLine("Chrck8Int:yyyyMMdd");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.To8Int.ToString();
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.Date);
            }


        }
    }
}
