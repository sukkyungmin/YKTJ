namespace RealTimeRMT
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.pwTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.settingTestAccount = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cancelButton = new System.Windows.Forms.ImageButton();
            this.loginButton = new System.Windows.Forms.ImageButton();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cancelButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginButton)).BeginInit();
            this.SuspendLayout();
            // 
            // idTextBox
            // 
            this.idTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(190)))), ((int)(((byte)(192)))));
            this.idTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.idTextBox.Font = new System.Drawing.Font("바른돋움 1", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.idTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.idTextBox.Location = new System.Drawing.Point(94, 65);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(194, 27);
            this.idTextBox.TabIndex = 0;
            this.idTextBox.Text = "Pants";
            this.idTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.idTextBox_KeyPress);
            // 
            // pwTextBox
            // 
            this.pwTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(190)))), ((int)(((byte)(192)))));
            this.pwTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pwTextBox.Font = new System.Drawing.Font("바른돋움 1", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pwTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.pwTextBox.Location = new System.Drawing.Point(94, 101);
            this.pwTextBox.Name = "pwTextBox";
            this.pwTextBox.PasswordChar = '*';
            this.pwTextBox.Size = new System.Drawing.Size(194, 27);
            this.pwTextBox.TabIndex = 1;
            this.pwTextBox.Text = "1111";
            this.pwTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pwTextBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("바른돋움 1", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(49, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "I D";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("바른돋움 1", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(47, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "PW";
            // 
            // settingTestAccount
            // 
            this.settingTestAccount.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(235)))), ((int)(((byte)(253)))));
            this.settingTestAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingTestAccount.Location = new System.Drawing.Point(12, 194);
            this.settingTestAccount.Name = "settingTestAccount";
            this.settingTestAccount.Size = new System.Drawing.Size(39, 22);
            this.settingTestAccount.TabIndex = 10;
            this.settingTestAccount.Text = "Test";
            this.settingTestAccount.UseVisualStyleBackColor = true;
            this.settingTestAccount.Visible = false;
            this.settingTestAccount.Click += new System.EventHandler(this.settingTestAccount_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::RealTimeRMT.Properties.Resources.유한킴벌리로고;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(121, 21);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.Checked = false;
            this.cancelButton.CheckedImage = null;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cancelButton.DisabledImag = null;
            this.cancelButton.DownImage = global::RealTimeRMT.Properties.Resources.Cancel_down_;
            this.cancelButton.Font = new System.Drawing.Font("바른돋움 1", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cancelButton.HoverImage = global::RealTimeRMT.Properties.Resources.Cancel_down_;
            this.cancelButton.Location = new System.Drawing.Point(194, 146);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.NormalImage = global::RealTimeRMT.Properties.Resources.Cancel_normal_;
            this.cancelButton.Size = new System.Drawing.Size(94, 35);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.TabStop = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.Transparent;
            this.loginButton.Checked = false;
            this.loginButton.CheckedImage = null;
            this.loginButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.loginButton.DisabledImag = null;
            this.loginButton.DownImage = global::RealTimeRMT.Properties.Resources.Login_down_;
            this.loginButton.Font = new System.Drawing.Font("바른돋움 1", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.loginButton.HoverImage = global::RealTimeRMT.Properties.Resources.Login_down_;
            this.loginButton.Location = new System.Drawing.Point(94, 146);
            this.loginButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.loginButton.Name = "loginButton";
            this.loginButton.NormalImage = global::RealTimeRMT.Properties.Resources.Login_normal_1;
            this.loginButton.Size = new System.Drawing.Size(94, 35);
            this.loginButton.TabIndex = 2;
            this.loginButton.TabStop = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("바른돋움 1", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(136, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "v2.5.3";
            // 
            // LoginForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(340, 228);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.settingTestAccount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pwTextBox);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.loginButton);
            this.Font = new System.Drawing.Font("바른돋움 1", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RealTime(Raw Material Tracking)";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cancelButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageButton loginButton;
        private System.Windows.Forms.ImageButton cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button settingTestAccount;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.TextBox pwTextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
    }
}