namespace RealTimeRMT
{
    partial class LoadingBarDlg
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
            this.components = new System.ComponentModel.Container();
            this.loadingBarTimer = new System.Windows.Forms.Timer(this.components);
            this.loadingPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // loadingBarTimer
            // 
            this.loadingBarTimer.Tick += new System.EventHandler(this.loadingBarTimer_Tick);
            // 
            // loadingPictureBox
            // 
            this.loadingPictureBox.BackColor = System.Drawing.Color.White;
            this.loadingPictureBox.Image = global::RealTimeRMT.Properties.Resources.loading;
            this.loadingPictureBox.Location = new System.Drawing.Point(0, 0);
            this.loadingPictureBox.Name = "loadingPictureBox";
            this.loadingPictureBox.Size = new System.Drawing.Size(480, 320);
            this.loadingPictureBox.TabIndex = 1;
            this.loadingPictureBox.TabStop = false;
            // 
            // LoadingBarDlg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(480, 320);
            this.Controls.Add(this.loadingPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoadingBarDlg";
            this.Text = "처리중";
            ((System.ComponentModel.ISupportInitialize)(this.loadingPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer loadingBarTimer;
        private System.Windows.Forms.PictureBox loadingPictureBox;
    }
}