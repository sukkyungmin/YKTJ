namespace RealTimeRMT
{
    partial class SettingTestAccount
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
            this.ip = new System.Windows.Forms.TextBox();
            this.id = new System.Windows.Forms.TextBox();
            this.pw = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ip
            // 
            this.ip.Location = new System.Drawing.Point(37, 10);
            this.ip.Name = "ip";
            this.ip.Size = new System.Drawing.Size(147, 21);
            this.ip.TabIndex = 0;
            // 
            // id
            // 
            this.id.Location = new System.Drawing.Point(37, 34);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(147, 21);
            this.id.TabIndex = 1;
            // 
            // pw
            // 
            this.pw.Location = new System.Drawing.Point(37, 58);
            this.pw.Name = "pw";
            this.pw.Size = new System.Drawing.Size(147, 21);
            this.pw.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "PW";
            // 
            // confirm
            // 
            this.confirm.Location = new System.Drawing.Point(143, 85);
            this.confirm.Name = "confirm";
            this.confirm.Size = new System.Drawing.Size(41, 23);
            this.confirm.TabIndex = 6;
            this.confirm.Text = "확인";
            this.confirm.UseVisualStyleBackColor = true;
            this.confirm.Click += new System.EventHandler(this.confirm_Click);
            // 
            // SettingTestAccount
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(191, 116);
            this.ControlBox = false;
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pw);
            this.Controls.Add(this.id);
            this.Controls.Add(this.ip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingTestAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DB 테스트 계정 입력";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ip;
        private System.Windows.Forms.TextBox id;
        private System.Windows.Forms.TextBox pw;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button confirm;
    }
}