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
            lblDateRetDate.Text = jpd.ToEraString;
        }

        /// <summary>
        /// 日付を和暦に変換します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStrConv_Click(object sender, EventArgs e)
        {
            using (JapaneseDate jpd = new JapaneseDate(txtDateString.Text))
            {
                JapaneseDate.DateStatus s =  jpd.ConversionStatus;
                switch (s)
                {
                    case JapaneseDate.DateStatus.Success:
                        lblDateRetString.Text = jpd.DateToString;
                        break;
                    case JapaneseDate.DateStatus.None:
                    case JapaneseDate.DateStatus.RegexIsMatchError:
                    case JapaneseDate.DateStatus.ConversionImpossible:
                    case JapaneseDate.DateStatus.Error_NendoRenge:
                    case JapaneseDate.DateStatus.Error_Fatal:
                    default:
                        lblDateRetString.Text = s.ToString();
                        break;
                }
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
            // このテストについて


            Test.TestUnitItem cxt = new TestUnitItem();

            Console.WriteLine("ChrckString:X99.99.99");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.ToCharAlphabet_Dot;
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w,item.Answer, jpd.ToEraString);
            }

            Console.WriteLine("ChrckString:X99/99/99");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.ToCharAlphabet_Slash;
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.ToEraString);
            }

            Console.WriteLine("ChrckString:9999/99/99");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.ToCharAlphabet_Slash;
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.ToEraString);
            }

            Console.WriteLine("ChrckString:9999.99.99");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.ToCharAlphabet_Dot;
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.ToEraString);
            }

            Console.WriteLine("Chrck7Int:9yyMMdd");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.To7Int.ToString();
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.ToEraString);
            }

            Console.WriteLine("Chrck8Int:yyyyMMdd");
            foreach (var item in cxt.TestUnit_NendoLimit)
            {
                string w = item.To8Int.ToString();
                JapaneseDate jpd = new JapaneseDate(w);
                cxt.ConsoleWriteLineComparisonData(w, item.Answer, jpd.ToEraString);
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            JapaneseDate jpd = new JapaneseDate(DateTime.Now);

            jpd.ChangeWarekiFormat = JapaneseDate.WarekiFormat.Wareki_Abbreviation_JP;
            Console.WriteLine(jpd.ToEraString);



        }
    }
}
