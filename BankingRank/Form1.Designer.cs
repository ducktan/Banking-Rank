namespace BankingRank
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button5 = new Button();
            textBox3 = new TextBox();
            label4 = new Label();
            button2 = new Button();
            button1 = new Button();
            textBox1 = new TextBox();
            label2 = new Label();
            tabPage2 = new TabPage();
            groupBox1 = new GroupBox();
            textBox8 = new TextBox();
            label15 = new Label();
            textBox7 = new TextBox();
            label14 = new Label();
            textBox6 = new TextBox();
            label13 = new Label();
            button8 = new Button();
            textBox5 = new TextBox();
            label12 = new Label();
            tabPage3 = new TabPage();
            button6 = new Button();
            textBox4 = new TextBox();
            label5 = new Label();
            button3 = new Button();
            button4 = new Button();
            textBox2 = new TextBox();
            label3 = new Label();
            tabPage4 = new TabPage();
            label10 = new Label();
            label11 = new Label();
            pubText = new RichTextBox();
            priText = new RichTextBox();
            button7 = new Button();
            label6 = new Label();
            formatKey = new ComboBox();
            label7 = new Label();
            pubName = new TextBox();
            label8 = new Label();
            priName = new TextBox();
            label9 = new Label();
            keysize = new TextBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(26, 20);
            label1.Name = "label1";
            label1.Size = new Size(273, 40);
            label1.TabIndex = 0;
            label1.Text = "BANKING RANK";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(34, 75);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(979, 481);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.White;
            tabPage1.Controls.Add(button5);
            tabPage1.Controls.Add(textBox3);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Controls.Add(label2);
            tabPage1.Location = new Point(4, 32);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(971, 445);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Upload data";
            // 
            // button5
            // 
            button5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.Location = new Point(761, 113);
            button5.Name = "button5";
            button5.Size = new Size(157, 31);
            button5.TabIndex = 6;
            button5.Text = "Choose file";
            button5.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(212, 114);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(479, 30);
            textBox3.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(15, 116);
            label4.Name = "label4";
            label4.Size = new Size(122, 28);
            label4.TabIndex = 4;
            label4.Text = "▶ Public key";
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(15, 181);
            button2.Name = "button2";
            button2.Size = new Size(157, 31);
            button2.TabIndex = 3;
            button2.Text = "Upload";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(761, 50);
            button1.Name = "button1";
            button1.Size = new Size(157, 31);
            button1.TabIndex = 2;
            button1.Text = "Choose file";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(212, 51);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(479, 30);
            textBox1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(15, 53);
            label2.Name = "label2";
            label2.Size = new Size(179, 28);
            label2.TabIndex = 0;
            label2.Text = "▶ Upload your file:";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.White;
            tabPage2.Controls.Add(groupBox1);
            tabPage2.Controls.Add(button8);
            tabPage2.Controls.Add(textBox5);
            tabPage2.Controls.Add(label12);
            tabPage2.Location = new Point(4, 32);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(971, 445);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Ranking";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox8);
            groupBox1.Controls.Add(label15);
            groupBox1.Controls.Add(textBox7);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(textBox6);
            groupBox1.Controls.Add(label13);
            groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(25, 151);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(902, 194);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Information";
            // 
            // textBox8
            // 
            textBox8.Location = new Point(125, 122);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(216, 34);
            textBox8.TabIndex = 10;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label15.Location = new Point(23, 125);
            label15.Name = "label15";
            label15.Size = new Size(79, 28);
            label15.TabIndex = 9;
            label15.Text = "▶ Point";
            // 
            // textBox7
            // 
            textBox7.Location = new Point(636, 53);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(228, 34);
            textBox7.TabIndex = 7;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label14.Location = new Point(374, 53);
            label14.Name = "label14";
            label14.Size = new Size(256, 28);
            label14.TabIndex = 8;
            label14.Text = "▶ Citizen identification card";
            // 
            // textBox6
            // 
            textBox6.Location = new Point(125, 48);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(216, 34);
            textBox6.TabIndex = 6;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label13.Location = new Point(23, 48);
            label13.Name = "label13";
            label13.Size = new Size(86, 28);
            label13.TabIndex = 6;
            label13.Text = "▶ Name";
            // 
            // button8
            // 
            button8.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button8.Location = new Point(30, 90);
            button8.Name = "button8";
            button8.Size = new Size(157, 31);
            button8.TabIndex = 4;
            button8.Text = "Calculation";
            button8.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(399, 37);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(490, 30);
            textBox5.TabIndex = 3;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.Location = new Point(30, 39);
            label12.Name = "label12";
            label12.Size = new Size(336, 28);
            label12.TabIndex = 2;
            label12.Text = "▶ Enter the citizen identification card";
            // 
            // tabPage3
            // 
            tabPage3.BackColor = Color.White;
            tabPage3.Controls.Add(button6);
            tabPage3.Controls.Add(textBox4);
            tabPage3.Controls.Add(label5);
            tabPage3.Controls.Add(button3);
            tabPage3.Controls.Add(button4);
            tabPage3.Controls.Add(textBox2);
            tabPage3.Controls.Add(label3);
            tabPage3.Location = new Point(4, 32);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(971, 445);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Download data";
            // 
            // button6
            // 
            button6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button6.Location = new Point(772, 112);
            button6.Name = "button6";
            button6.Size = new Size(157, 31);
            button6.TabIndex = 10;
            button6.Text = "Choose file";
            button6.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(223, 113);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(479, 30);
            textBox4.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(26, 115);
            label5.Name = "label5";
            label5.Size = new Size(129, 28);
            label5.TabIndex = 8;
            label5.Text = "▶ Private key";
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Location = new Point(26, 180);
            button3.Name = "button3";
            button3.Size = new Size(157, 31);
            button3.TabIndex = 7;
            button3.Text = "Download";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.Location = new Point(772, 49);
            button4.Name = "button4";
            button4.Size = new Size(157, 31);
            button4.TabIndex = 6;
            button4.Text = "Choose file";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(223, 50);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(479, 30);
            textBox2.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(26, 52);
            label3.Name = "label3";
            label3.Size = new Size(193, 28);
            label3.TabIndex = 4;
            label3.Text = "▶ Download all data";
            // 
            // tabPage4
            // 
            tabPage4.BackColor = Color.White;
            tabPage4.Controls.Add(label10);
            tabPage4.Controls.Add(label11);
            tabPage4.Controls.Add(pubText);
            tabPage4.Controls.Add(priText);
            tabPage4.Controls.Add(button7);
            tabPage4.Controls.Add(label6);
            tabPage4.Controls.Add(formatKey);
            tabPage4.Controls.Add(label7);
            tabPage4.Controls.Add(pubName);
            tabPage4.Controls.Add(label8);
            tabPage4.Controls.Add(priName);
            tabPage4.Controls.Add(label9);
            tabPage4.Controls.Add(keysize);
            tabPage4.Location = new Point(4, 32);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(971, 445);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Gen key";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(17, 345);
            label10.Name = "label10";
            label10.Size = new Size(103, 23);
            label10.TabIndex = 20;
            label10.Text = "PUBLIC KEY";
            label10.Click += label10_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(16, 254);
            label11.Name = "label11";
            label11.Size = new Size(111, 23);
            label11.TabIndex = 19;
            label11.Text = "PRIVATE KEY";
            // 
            // pubText
            // 
            pubText.Location = new Point(17, 371);
            pubText.Name = "pubText";
            pubText.Size = new Size(926, 44);
            pubText.TabIndex = 18;
            pubText.Text = "";
            // 
            // priText
            // 
            priText.Location = new Point(17, 292);
            priText.Name = "priText";
            priText.Size = new Size(926, 37);
            priText.TabIndex = 17;
            priText.Text = "";
            // 
            // button7
            // 
            button7.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button7.Location = new Point(15, 205);
            button7.Name = "button7";
            button7.Size = new Size(186, 39);
            button7.TabIndex = 16;
            button7.Text = "GENKEY";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label6.Location = new Point(509, 35);
            label6.Name = "label6";
            label6.Size = new Size(80, 23);
            label6.TabIndex = 15;
            label6.Text = "FORMAT";
            // 
            // formatKey
            // 
            formatKey.FormattingEnabled = true;
            formatKey.Items.AddRange(new object[] { "DER", "Base64" });
            formatKey.Location = new Point(509, 67);
            formatKey.Name = "formatKey";
            formatKey.Size = new Size(433, 31);
            formatKey.TabIndex = 14;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label7.Location = new Point(509, 118);
            label7.Name = "label7";
            label7.Size = new Size(98, 23);
            label7.TabIndex = 13;
            label7.Text = "PUB NAME";
            // 
            // pubName
            // 
            pubName.Location = new Point(509, 151);
            pubName.Name = "pubName";
            pubName.Size = new Size(433, 30);
            pubName.TabIndex = 12;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label8.Location = new Point(16, 118);
            label8.Name = "label8";
            label8.Size = new Size(91, 23);
            label8.TabIndex = 11;
            label8.Text = "PRI NAME";
            // 
            // priName
            // 
            priName.Location = new Point(16, 151);
            priName.Name = "priName";
            priName.Size = new Size(444, 30);
            priName.TabIndex = 10;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label9.Location = new Point(16, 35);
            label9.Name = "label9";
            label9.Size = new Size(79, 23);
            label9.TabIndex = 9;
            label9.Text = "KEY SIZE";
            // 
            // keysize
            // 
            keysize.Location = new Point(17, 68);
            keysize.Name = "keysize";
            keysize.Size = new Size(443, 30);
            keysize.TabIndex = 8;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1047, 575);
            Controls.Add(tabControl1);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Banking Rank";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Button button2;
        private Button button1;
        private TextBox textBox1;
        private Label label2;
        private Button button3;
        private Button button4;
        private TextBox textBox2;
        private Label label3;
        private Button button5;
        private TextBox textBox3;
        private Label label4;
        private TabPage tabPage4;
        private Button button6;
        private TextBox textBox4;
        private Label label5;
        private Label label6;
        private ComboBox formatKey;
        private Label label7;
        private TextBox pubName;
        private Label label8;
        private TextBox priName;
        private Label label9;
        private TextBox keysize;
        private Label label10;
        private Label label11;
        private RichTextBox pubText;
        private RichTextBox priText;
        private Button button7;
        private TextBox textBox5;
        private Label label12;
        private Button button8;
        private GroupBox groupBox1;
        private TextBox textBox8;
        private Label label15;
        private TextBox textBox7;
        private Label label14;
        private TextBox textBox6;
        private Label label13;
    }
}
