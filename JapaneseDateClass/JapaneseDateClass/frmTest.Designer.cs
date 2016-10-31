namespace JapaneseDateClass
{
    partial class frmTest
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.japaneseDateTextBox1 = new JapaneseDateClass.Control.JapaneseDateTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDateString = new System.Windows.Forms.TextBox();
            this.lblDateRetString = new System.Windows.Forms.Label();
            this.btnStrConv = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.lblDateRetDate = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(369, 248);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.japaneseDateTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(361, 222);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Control";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(114, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Lost";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // japaneseDateTextBox1
            // 
            this.japaneseDateTextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.japaneseDateTextBox1.Location = new System.Drawing.Point(8, 28);
            this.japaneseDateTextBox1.Name = "japaneseDateTextBox1";
            this.japaneseDateTextBox1.Size = new System.Drawing.Size(100, 19);
            this.japaneseDateTextBox1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(361, 222);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Class";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtDateString);
            this.groupBox2.Controls.Add(this.lblDateRetString);
            this.groupBox2.Controls.Add(this.btnStrConv);
            this.groupBox2.Location = new System.Drawing.Point(12, 61);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(334, 110);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文字・数字の入力";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(6, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(312, 60);
            this.label2.TabIndex = 5;
            this.label2.Text = "変換可能な文字の例(区切り文字はスラッシュ/とドット.をサポート)\r\n2016/1/1【よくある日付】\r\nH28/1/1【元号がアルファベットのもの】\r\n4280" +
    "401【元号の1を明治とした1～4までの元号値】\r\n428/1/1【元号の1を明治とした1～4までの元号値】(Bug)";
            // 
            // txtDateString
            // 
            this.txtDateString.Location = new System.Drawing.Point(6, 78);
            this.txtDateString.Name = "txtDateString";
            this.txtDateString.Size = new System.Drawing.Size(100, 19);
            this.txtDateString.TabIndex = 4;
            this.txtDateString.Text = "H28/04/01";
            // 
            // lblDateRetString
            // 
            this.lblDateRetString.AutoSize = true;
            this.lblDateRetString.Location = new System.Drawing.Point(193, 81);
            this.lblDateRetString.Name = "lblDateRetString";
            this.lblDateRetString.Size = new System.Drawing.Size(39, 12);
            this.lblDateRetString.TabIndex = 3;
            this.lblDateRetString.Text = "Return";
            // 
            // btnStrConv
            // 
            this.btnStrConv.Location = new System.Drawing.Point(112, 76);
            this.btnStrConv.Name = "btnStrConv";
            this.btnStrConv.Size = new System.Drawing.Size(75, 23);
            this.btnStrConv.TabIndex = 3;
            this.btnStrConv.Text = "Conv";
            this.btnStrConv.UseVisualStyleBackColor = true;
            this.btnStrConv.Click += new System.EventHandler(this.btnStrConv_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePicker);
            this.groupBox1.Controls.Add(this.lblDateRetDate);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 48);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "日付の入力";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(6, 18);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(119, 19);
            this.dateTimePicker.TabIndex = 1;
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // lblDateRetDate
            // 
            this.lblDateRetDate.AutoSize = true;
            this.lblDateRetDate.Location = new System.Drawing.Point(131, 23);
            this.lblDateRetDate.Name = "lblDateRetDate";
            this.lblDateRetDate.Size = new System.Drawing.Size(39, 12);
            this.lblDateRetDate.TabIndex = 2;
            this.lblDateRetDate.Text = "Return";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(361, 222);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "TestUnit";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "label3";
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 267);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmTest";
            this.Text = "JapaneseDateClass";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label lblDateRetDate;
        private System.Windows.Forms.Button btnStrConv;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDateString;
        private System.Windows.Forms.Label lblDateRetString;
        private Control.JapaneseDateTextBox japaneseDateTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label3;
    }
}

