using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using JapaneseDateClass.Class;


namespace JapaneseDateClass.Control
{

    /// <summary>
    /// 和暦テキストボックスです。
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(DateTimePicker))]
    [Description("和暦テキストボックスです。")]
    class JapaneseDateTextBox : TextBox
    {
        /// <summary>
        /// テキストボックスの文字列
        /// </summary>
        private string textboxString;
        /// <summary>
        /// JapaneseDate
        /// </summary>
        private JapaneseDate jpDate;

        /// <summary>
        /// JapaneseDateTextBox インスタンスを初期化します。
        /// </summary>
        public JapaneseDateTextBox()
            : base()
        {
            this.ImeMode = ImeMode.Disable; // IMEを無効にします。
            //this.jpDate = new JapaneseDate(DateTime.Now);
        }


        protected override void OnClick(EventArgs e)
        {
            if (this.jpDate == null)
            {
                return;
            }

            if (this.jpDate.Date != "")
            {
                this.Text = this.textboxString; // 保持しているテキストボックス文字列に差し替える
            }
            else
            {
                this.Text = this.jpDate.Date.ToString();
            }

        }

        protected override void OnLostFocus(EventArgs e)
        {
            switch (this.SetJapaneseDate(this.Text,true))
            {
                case JapaneseDate.DateStatus.None:
                    break;
                case JapaneseDate.DateStatus.Success:
                    this.textboxString = this.Text; // 現時点の文字列を保持
                    this.Text = this.jpDate.Date;
                    break;
                case JapaneseDate.DateStatus.RegexIsMatchError:
                    break;
                case JapaneseDate.DateStatus.ConversionImpossible:
                    break;
                case JapaneseDate.DateStatus.Error_NendoRenge:
                    break;
                case JapaneseDate.DateStatus.Error_Fatal:
                    break;
                default:
                    break;
            }
        }


        private JapaneseDate.DateStatus SetJapaneseDate(string date,bool createInstans)
        {
            if (this.jpDate == null)
            {
                if (createInstans)
                {
                    this.jpDate = new JapaneseDate(DateTime.Now);
                    return this.jpDate.SetData(date);
                }
                else
                {
                    return JapaneseDate.DateStatus.None;
                }

            }
            else
            {
                return this.jpDate.SetData(date);
            }

        }

    }
}
