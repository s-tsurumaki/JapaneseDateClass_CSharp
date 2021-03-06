﻿using System;
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
        /// 
        /// </summary>
        private enum mode
        {
            /// <summary>
            /// 
            /// </summary>
            JapaneseDate,
            /// <summary>
            /// 
            /// </summary>
            Year,
            /// <summary>
            /// 
            /// </summary>
            Month,
            /// <summary>
            /// 
            /// </summary>
            Day,
        }

        /// <summary>
        /// 
        /// </summary>
        private JapaneseDate.DateStatus jpDateState;
        /// <summary>
        /// テキストボックスの文字列
        /// </summary>
        private string textboxString;
        /// <summary>
        /// JapaneseDate
        /// </summary>
        private JapaneseDate jpDate;

        /// <summary>
        /// 和暦変換失敗時に変更する背景色
        /// </summary>
        private Color ErrorBackColor = Color.HotPink;
        
        /// <summary>
        /// JapaneseDateTextBox インスタンスを初期化します。
        /// </summary>
        public JapaneseDateTextBox()
            : base()
        {
            this.ImeMode = ImeMode.Disable; // IMEを無効にします。
            this.MaxLength = 12; // 最大入力文字を指定
            //this.jpDate = new JapaneseDate(DateTime.Now);
        }


        protected override void OnClick(EventArgs e)
        {
            if (this.jpDate == null)
            {
                return;
            }
            else if (this.jpDate.ToEraString != "")
            {
                this.Text = this.textboxString; // 保持しているテキストボックス文字列に差し替える
            }
            else
            {
                this.Text = this.jpDate.ToEraString.ToString();
            }

        }

        /// <summary>
        /// テキストボックスでロストフォーカスした時に発生します。
        /// </summary>
        /// <param name="e">EventArgs</param>
        /// <remarks>
        /// このイベントについて
        /// 
        /// 
        /// </remarks>
        protected override void OnLostFocus(EventArgs e)
        {
            if (this.Text == "") // 文字列を空白にした場合。
            {
                this.jpDate = null;
                this.BackColor = Color.White;
                return;
            }
            
            this.jpDateState = this.SetJapaneseDate(this.Text, true);

            switch (this.jpDateState)
            {
                case JapaneseDate.DateStatus.Success:
                    this.BackColor = Color.White;
                    this.textboxString = this.Text; // 現時点の文字列を保持
                    this.Text = this.jpDate.ToEraString;
                    break;
                case JapaneseDate.DateStatus.None:
                case JapaneseDate.DateStatus.RegexIsMatchError:
                case JapaneseDate.DateStatus.ConversionImpossible:
                case JapaneseDate.DateStatus.Error_NendoRenge:
                case JapaneseDate.DateStatus.Error_Fatal:
                default:
                    this.jpDate = null;
                    this.BackColor = this.ErrorBackColor;
                    break;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="createInstans"></param>
        /// <returns></returns>
        private JapaneseDate.DateStatus SetJapaneseDate(string date,bool createInstans)
        {
            this.textboxString = this.Text;

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

        /// <summary>
        /// テキストボックス内に入力したデータが日付として認識できればtrueを返します。
        /// </summary>
        /// <returns>
        /// true 日付変換可能
        /// false 日付変換不可
        /// </returns>
        private bool isInputTextisDate
        {
            get
            {
                return this.jpDateState == JapaneseDate.DateStatus.Success ? true : false;
            }
        }

        #region GetJapaneseDate
        /// <summary>
        /// JapaneseDate
        /// </summary>
        private JapaneseDate GetJapaneseDate
        {
            get
            {
                return this.jpDate;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private DateTime? GetDateTime
        {
            get
            {
                return this.jpDate.InputDate;
            }
        }

    }
}
