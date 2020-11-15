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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // upBtn
            // 
            this.upBtn.Location = new System.Drawing.Point(256, 418);
            this.upBtn.Name = "upBtn";
            this.upBtn.Size = new System.Drawing.Size(75, 36);
            this.upBtn.TabIndex = 3;
            this.upBtn.Text = "Up";
            this.upBtn.UseVisualStyleBackColor = true;
            this.upBtn.Click += new System.EventHandler(this.upBtn_Click);
            // 
            // frontBtn
            // 
            this.frontBtn.Location = new System.Drawing.Point(175, 418);
            this.frontBtn.Name = "frontBtn";
            this.frontBtn.Size = new System.Drawing.Size(75, 36);
            this.frontBtn.TabIndex = 4;
            this.frontBtn.Text = "Front";
            this.frontBtn.UseVisualStyleBackColor = true;
            this.frontBtn.Click += new System.EventHandler(this.frontBtn_Click);
            // 
            // leftBtn
            // 
            this.leftBtn.Location = new System.Drawing.Point(175, 460);
            this.leftBtn.Name = "leftBtn";
            this.leftBtn.Size = new System.Drawing.Size(75, 36);
            this.leftBtn.TabIndex = 5;
            this.leftBtn.Text = "Left";
            this.leftBtn.UseVisualStyleBackColor = true;
            this.leftBtn.Click += new System.EventHandler(this.leftBtn_Click);
            // 
            // rightBtn
            // 
            this.rightBtn.Location = new System.Drawing.Point(337, 460);
            this.rightBtn.Name = "rightBtn";
            this.rightBtn.Size = new System.Drawing.Size(75, 36);
            this.rightBtn.TabIndex = 7;
            this.rightBtn.Text = "Right";
            this.rightBtn.UseVisualStyleBackColor = true;
            this.rightBtn.Click += new System.EventHandler(this.rightBtn_Click);
            // 
            // backBtn
            // 
            this.backBtn.Location = new System.Drawing.Point(337, 418);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(75, 36);
            this.backBtn.TabIndex = 6;
            this.backBtn.Text = "Back";
            this.backBtn.UseVisualStyleBackColor = true;
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // translateRB
            // 
            this.translateRB.AutoSize = true;
            this.translateRB.Location = new System.Drawing.Point(13, 419);
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
            this.rotateRB.Location = new System.Drawing.Point(13, 446);
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
            this.debugTextBox.Location = new System.Drawing.Point(419, 13);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ReadOnly = true;
            this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugTextBox.Size = new System.Drawing.Size(835, 399);
            this.debugTextBox.TabIndex = 10;
            // 
            // downBtn
            // 
            this.downBtn.Location = new System.Drawing.Point(256, 460);
            this.downBtn.Name = "downBtn";
            this.downBtn.Size = new System.Drawing.Size(75, 36);
            this.downBtn.TabIndex = 11;
            this.downBtn.Text = "Down";
            this.downBtn.UseVisualStyleBackColor = true;
            this.downBtn.Click += new System.EventHandler(this.downBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 515);
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
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
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
    }
}

