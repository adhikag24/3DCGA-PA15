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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // upBtn
            // 
            this.upBtn.Location = new System.Drawing.Point(90, 435);
            this.upBtn.Name = "upBtn";
            this.upBtn.Size = new System.Drawing.Size(75, 36);
            this.upBtn.TabIndex = 3;
            this.upBtn.Text = "Up(W)";
            this.upBtn.UseVisualStyleBackColor = true;
            this.upBtn.Click += new System.EventHandler(this.upBtn_Click);
            // 
            // frontBtn
            // 
            this.frontBtn.Location = new System.Drawing.Point(9, 435);
            this.frontBtn.Name = "frontBtn";
            this.frontBtn.Size = new System.Drawing.Size(75, 36);
            this.frontBtn.TabIndex = 4;
            this.frontBtn.Text = "Front(Q)";
            this.frontBtn.UseVisualStyleBackColor = true;
            this.frontBtn.Click += new System.EventHandler(this.frontBtn_Click);
            // 
            // leftBtn
            // 
            this.leftBtn.Location = new System.Drawing.Point(9, 477);
            this.leftBtn.Name = "leftBtn";
            this.leftBtn.Size = new System.Drawing.Size(75, 36);
            this.leftBtn.TabIndex = 5;
            this.leftBtn.Text = "Left(A)";
            this.leftBtn.UseVisualStyleBackColor = true;
            this.leftBtn.Click += new System.EventHandler(this.leftBtn_Click);
            // 
            // rightBtn
            // 
            this.rightBtn.Location = new System.Drawing.Point(171, 477);
            this.rightBtn.Name = "rightBtn";
            this.rightBtn.Size = new System.Drawing.Size(75, 36);
            this.rightBtn.TabIndex = 7;
            this.rightBtn.Text = "Right(D)";
            this.rightBtn.UseVisualStyleBackColor = true;
            this.rightBtn.Click += new System.EventHandler(this.rightBtn_Click);
            // 
            // backBtn
            // 
            this.backBtn.Location = new System.Drawing.Point(171, 435);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(75, 36);
            this.backBtn.TabIndex = 6;
            this.backBtn.Text = "Back(E)";
            this.backBtn.UseVisualStyleBackColor = true;
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // translateRB
            // 
            this.translateRB.AutoSize = true;
            this.translateRB.Location = new System.Drawing.Point(9, 519);
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
            this.rotateRB.Location = new System.Drawing.Point(9, 546);
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
            this.debugTextBox.Location = new System.Drawing.Point(590, 435);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ReadOnly = true;
            this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugTextBox.Size = new System.Drawing.Size(222, 203);
            this.debugTextBox.TabIndex = 10;
            // 
            // downBtn
            // 
            this.downBtn.Location = new System.Drawing.Point(90, 477);
            this.downBtn.Name = "downBtn";
            this.downBtn.Size = new System.Drawing.Size(75, 36);
            this.downBtn.TabIndex = 11;
            this.downBtn.Text = "Down(S)";
            this.downBtn.UseVisualStyleBackColor = true;
            this.downBtn.Click += new System.EventHandler(this.downBtn_Click);
            // 
            // selectListBox
            // 
            this.selectListBox.FormattingEnabled = true;
            this.selectListBox.ItemHeight = 16;
            this.selectListBox.Location = new System.Drawing.Point(286, 435);
            this.selectListBox.Name = "selectListBox";
            this.selectListBox.Size = new System.Drawing.Size(226, 148);
            this.selectListBox.TabIndex = 1;
            // 
            // resetBtn
            // 
            this.resetBtn.Location = new System.Drawing.Point(8, 573);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(75, 36);
            this.resetBtn.TabIndex = 13;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(287, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Selected Object:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 415);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Controls:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(591, 415);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "Debug console:";
            // 
            // frontSurfaceCB
            // 
            this.frontSurfaceCB.AutoSize = true;
            this.frontSurfaceCB.Location = new System.Drawing.Point(104, 520);
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
            this.filledCB.Location = new System.Drawing.Point(104, 547);
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
            this.label4.Location = new System.Drawing.Point(6, 625);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(246, 87);
            this.label4.TabIndex = 19;
            this.label4.Text = "Shortcut keys:\r\n* T - Translate/Rotate toggle\r\n* Up Arrow - Select next object\r\n*" +
    " Down Arrow - Select previous object\r\n\r\n";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 723);
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
            this.Name = "Form1";
            this.Text = "3DCGA-PA15";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
    }
}

