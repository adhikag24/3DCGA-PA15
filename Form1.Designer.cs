namespace _3DCGA_PA15
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.upBtn = new System.Windows.Forms.Button();
            this.frontBtn = new System.Windows.Forms.Button();
            this.leftBtn = new System.Windows.Forms.Button();
            this.rightBtn = new System.Windows.Forms.Button();
            this.backBtn = new System.Windows.Forms.Button();
            this.translateRB = new System.Windows.Forms.RadioButton();
            this.rotateRB = new System.Windows.Forms.RadioButton();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.downBtn = new System.Windows.Forms.Button();
            this.selectListBox = new System.Windows.Forms.ListBox();
            this.resetBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.frontSurfaceCB = new System.Windows.Forms.CheckBox();
            this.filledCB = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.drawRB = new System.Windows.Forms.RadioButton();
            this.warnockRB = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1024, 512);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // upBtn
            // 
            this.upBtn.BackColor = System.Drawing.Color.White;
            this.upBtn.Image = ((System.Drawing.Image)(resources.GetObject("upBtn.Image")));
            this.upBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.upBtn.Location = new System.Drawing.Point(136, 546);
            this.upBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.upBtn.Name = "upBtn";
            this.upBtn.Size = new System.Drawing.Size(76, 74);
            this.upBtn.TabIndex = 3;
            this.upBtn.Text = "Up(W)";
            this.upBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.upBtn.UseVisualStyleBackColor = false;
            this.upBtn.Click += new System.EventHandler(this.upBtn_Click);
            // 
            // frontBtn
            // 
            this.frontBtn.BackColor = System.Drawing.Color.White;
            this.frontBtn.Location = new System.Drawing.Point(47, 581);
            this.frontBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.frontBtn.Name = "frontBtn";
            this.frontBtn.Size = new System.Drawing.Size(75, 36);
            this.frontBtn.TabIndex = 4;
            this.frontBtn.Text = "Front(Q)";
            this.frontBtn.UseVisualStyleBackColor = false;
            this.frontBtn.Click += new System.EventHandler(this.frontBtn_Click);
            // 
            // leftBtn
            // 
            this.leftBtn.BackColor = System.Drawing.Color.White;
            this.leftBtn.Image = ((System.Drawing.Image)(resources.GetObject("leftBtn.Image")));
            this.leftBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.leftBtn.Location = new System.Drawing.Point(12, 629);
            this.leftBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.leftBtn.Name = "leftBtn";
            this.leftBtn.Size = new System.Drawing.Size(115, 57);
            this.leftBtn.TabIndex = 5;
            this.leftBtn.Text = "Left(A)";
            this.leftBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.leftBtn.UseVisualStyleBackColor = false;
            this.leftBtn.Click += new System.EventHandler(this.leftBtn_Click);
            // 
            // rightBtn
            // 
            this.rightBtn.BackColor = System.Drawing.Color.White;
            this.rightBtn.Image = ((System.Drawing.Image)(resources.GetObject("rightBtn.Image")));
            this.rightBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rightBtn.Location = new System.Drawing.Point(217, 629);
            this.rightBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rightBtn.Name = "rightBtn";
            this.rightBtn.Size = new System.Drawing.Size(115, 57);
            this.rightBtn.TabIndex = 7;
            this.rightBtn.Text = "Right(D)";
            this.rightBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rightBtn.UseVisualStyleBackColor = false;
            this.rightBtn.Click += new System.EventHandler(this.rightBtn_Click);
            // 
            // backBtn
            // 
            this.backBtn.BackColor = System.Drawing.Color.White;
            this.backBtn.Location = new System.Drawing.Point(221, 579);
            this.backBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(75, 36);
            this.backBtn.TabIndex = 6;
            this.backBtn.Text = "Back(E)";
            this.backBtn.UseVisualStyleBackColor = false;
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // translateRB
            // 
            this.translateRB.AutoSize = true;
            this.translateRB.Location = new System.Drawing.Point(20, 713);
            this.translateRB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.translateRB.Name = "translateRB";
            this.translateRB.Size = new System.Drawing.Size(89, 21);
            this.translateRB.TabIndex = 8;
            this.translateRB.TabStop = true;
            this.translateRB.Text = "Translate";
            this.translateRB.UseVisualStyleBackColor = true;
            // 
            // rotateRB
            // 
            this.rotateRB.AutoSize = true;
            this.rotateRB.Location = new System.Drawing.Point(20, 740);
            this.rotateRB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rotateRB.Name = "rotateRB";
            this.rotateRB.Size = new System.Drawing.Size(71, 21);
            this.rotateRB.TabIndex = 9;
            this.rotateRB.TabStop = true;
            this.rotateRB.Text = "Rotate";
            this.rotateRB.UseVisualStyleBackColor = true;
            // 
            // debugTextBox
            // 
            this.debugTextBox.BackColor = System.Drawing.Color.White;
            this.debugTextBox.Location = new System.Drawing.Point(1043, 12);
            this.debugTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ReadOnly = true;
            this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugTextBox.Size = new System.Drawing.Size(359, 701);
            this.debugTextBox.TabIndex = 10;
            // 
            // downBtn
            // 
            this.downBtn.BackColor = System.Drawing.Color.White;
            this.downBtn.Image = ((System.Drawing.Image)(resources.GetObject("downBtn.Image")));
            this.downBtn.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.downBtn.Location = new System.Drawing.Point(136, 629);
            this.downBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.downBtn.Name = "downBtn";
            this.downBtn.Size = new System.Drawing.Size(75, 74);
            this.downBtn.TabIndex = 11;
            this.downBtn.Text = "Down(S)";
            this.downBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.downBtn.UseVisualStyleBackColor = false;
            this.downBtn.Click += new System.EventHandler(this.downBtn_Click);
            // 
            // selectListBox
            // 
            this.selectListBox.FormattingEnabled = true;
            this.selectListBox.ItemHeight = 16;
            this.selectListBox.Location = new System.Drawing.Point(375, 546);
            this.selectListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.selectListBox.Name = "selectListBox";
            this.selectListBox.Size = new System.Drawing.Size(225, 148);
            this.selectListBox.TabIndex = 1;
            // 
            // resetBtn
            // 
            this.resetBtn.Image = ((System.Drawing.Image)(resources.GetObject("resetBtn.Image")));
            this.resetBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.resetBtn.Location = new System.Drawing.Point(20, 765);
            this.resetBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(107, 48);
            this.resetBtn.TabIndex = 13;
            this.resetBtn.Text = "Reset";
            this.resetBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(376, 526);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Selected Object:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 526);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Controls:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(680, 526);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "Cursor coordinate:";
            // 
            // frontSurfaceCB
            // 
            this.frontSurfaceCB.AutoSize = true;
            this.frontSurfaceCB.Location = new System.Drawing.Point(115, 713);
            this.frontSurfaceCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.frontSurfaceCB.Name = "frontSurfaceCB";
            this.frontSurfaceCB.Size = new System.Drawing.Size(146, 21);
            this.frontSurfaceCB.TabIndex = 17;
            this.frontSurfaceCB.Text = "Front Surface only";
            this.frontSurfaceCB.UseVisualStyleBackColor = true;
            this.frontSurfaceCB.CheckedChanged += new System.EventHandler(this.frontSurfaceCB_CheckedChanged);
            // 
            // filledCB
            // 
            this.filledCB.AutoSize = true;
            this.filledCB.Location = new System.Drawing.Point(115, 740);
            this.filledCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.filledCB.Name = "filledCB";
            this.filledCB.Size = new System.Drawing.Size(63, 21);
            this.filledCB.TabIndex = 18;
            this.filledCB.Text = "Filled";
            this.filledCB.UseVisualStyleBackColor = true;
            this.filledCB.CheckedChanged += new System.EventHandler(this.filledCB_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(653, 661);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(246, 87);
            this.label4.TabIndex = 19;
            this.label4.Text = "Shortcut keys:\r\n* T - Translate/Rotate toggle\r\n* Up Arrow - Select next object\r\n*" +
    " Down Arrow - Select previous object\r\n\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(680, 546);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 17);
            this.label5.TabIndex = 20;
            this.label5.Text = "x = ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(680, 568);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 17);
            this.label6.TabIndex = 21;
            this.label6.Text = "y = ";
            // 
            // drawRB
            // 
            this.drawRB.AutoSize = true;
            this.drawRB.Location = new System.Drawing.Point(5, 21);
            this.drawRB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.drawRB.Name = "drawRB";
            this.drawRB.Size = new System.Drawing.Size(61, 21);
            this.drawRB.TabIndex = 22;
            this.drawRB.TabStop = true;
            this.drawRB.Text = "Draw";
            this.drawRB.UseVisualStyleBackColor = true;
            this.drawRB.CheckedChanged += new System.EventHandler(this.drawRB_CheckedChanged);
            // 
            // warnockRB
            // 
            this.warnockRB.AutoSize = true;
            this.warnockRB.Location = new System.Drawing.Point(5, 48);
            this.warnockRB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.warnockRB.Name = "warnockRB";
            this.warnockRB.Size = new System.Drawing.Size(85, 21);
            this.warnockRB.TabIndex = 23;
            this.warnockRB.TabStop = true;
            this.warnockRB.Text = "Warnock";
            this.warnockRB.UseVisualStyleBackColor = true;
            this.warnockRB.CheckedChanged += new System.EventHandler(this.warnockRB_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.drawRB);
            this.groupBox1.Controls.Add(this.warnockRB);
            this.groupBox1.Location = new System.Drawing.Point(375, 701);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Methods:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1415, 823);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.filledCB);
            this.Controls.Add(this.frontSurfaceCB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.selectListBox);
            this.Controls.Add(this.downBtn);
            this.Controls.Add(this.debugTextBox);
            this.Controls.Add(this.rotateRB);
            this.Controls.Add(this.translateRB);
            this.Controls.Add(this.rightBtn);
            this.Controls.Add(this.backBtn);
            this.Controls.Add(this.leftBtn);
            this.Controls.Add(this.frontBtn);
            this.Controls.Add(this.upBtn);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "3DCGA-PA15";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button upBtn;
        private System.Windows.Forms.Button frontBtn;
        private System.Windows.Forms.Button leftBtn;
        private System.Windows.Forms.Button rightBtn;
        private System.Windows.Forms.Button backBtn;
        private System.Windows.Forms.RadioButton translateRB;
        private System.Windows.Forms.RadioButton rotateRB;
        private System.Windows.Forms.TextBox debugTextBox;
        private System.Windows.Forms.Button downBtn;
        private System.Windows.Forms.ListBox selectListBox;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox frontSurfaceCB;
        private System.Windows.Forms.CheckBox filledCB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton drawRB;
        private System.Windows.Forms.RadioButton warnockRB;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

