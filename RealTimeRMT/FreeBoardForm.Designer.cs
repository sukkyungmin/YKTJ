namespace RealTimeRMT
{
    partial class FreeBoardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FreeBoardForm));
            this.freeBoardPanel = new System.Windows.Forms.Panel();
            this.saveBoardButton = new System.Windows.Forms.ImageButton();
            this.deleteBoardButton = new System.Windows.Forms.ImageButton();
            this.downloadFileButton = new System.Windows.Forms.ImageButton();
            this.listBoardButton = new System.Windows.Forms.ImageButton();
            this.changeFileButton = new System.Windows.Forms.ImageButton();
            this.createdDateTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.writerTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.viewCountTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contentsTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.noTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.freeBoardPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveBoardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteBoardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadFileButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.changeFileButton)).BeginInit();
            this.SuspendLayout();
            // 
            // freeBoardPanel
            // 
            this.freeBoardPanel.BackColor = System.Drawing.Color.White;
            this.freeBoardPanel.Controls.Add(this.saveBoardButton);
            this.freeBoardPanel.Controls.Add(this.deleteBoardButton);
            this.freeBoardPanel.Controls.Add(this.downloadFileButton);
            this.freeBoardPanel.Controls.Add(this.listBoardButton);
            this.freeBoardPanel.Controls.Add(this.changeFileButton);
            this.freeBoardPanel.Controls.Add(this.createdDateTextBox);
            this.freeBoardPanel.Controls.Add(this.label5);
            this.freeBoardPanel.Controls.Add(this.titleTextBox);
            this.freeBoardPanel.Controls.Add(this.label7);
            this.freeBoardPanel.Controls.Add(this.writerTextBox);
            this.freeBoardPanel.Controls.Add(this.label6);
            this.freeBoardPanel.Controls.Add(this.viewCountTextBox);
            this.freeBoardPanel.Controls.Add(this.label4);
            this.freeBoardPanel.Controls.Add(this.contentsTextBox);
            this.freeBoardPanel.Controls.Add(this.label3);
            this.freeBoardPanel.Controls.Add(this.fileNameTextBox);
            this.freeBoardPanel.Controls.Add(this.label2);
            this.freeBoardPanel.Controls.Add(this.noTextBox);
            this.freeBoardPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.freeBoardPanel, "freeBoardPanel");
            this.freeBoardPanel.Name = "freeBoardPanel";
            this.freeBoardPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.freeBoardPanel_Paint);
            // 
            // saveBoardButton
            // 
            this.saveBoardButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.saveBoardButton.Checked = false;
            this.saveBoardButton.CheckedImage = null;
            this.saveBoardButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.saveBoardButton.DisabledImag = global::RealTimeRMT.Properties.Resources.Library_저장_disabled_;
            this.saveBoardButton.DownImage = global::RealTimeRMT.Properties.Resources.Library_저장_down_;
            resources.ApplyResources(this.saveBoardButton, "saveBoardButton");
            this.saveBoardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.saveBoardButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_저장_down_;
            this.saveBoardButton.Name = "saveBoardButton";
            this.saveBoardButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_저장_normal_;
            this.saveBoardButton.TabStop = false;
            this.saveBoardButton.Click += new System.EventHandler(this.saveBoardButton_Click);
            // 
            // deleteBoardButton
            // 
            this.deleteBoardButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.deleteBoardButton.Checked = false;
            this.deleteBoardButton.CheckedImage = null;
            this.deleteBoardButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.deleteBoardButton.DisabledImag = global::RealTimeRMT.Properties.Resources.Library_삭제_disabled_;
            this.deleteBoardButton.DownImage = global::RealTimeRMT.Properties.Resources.Library_삭제_down_;
            resources.ApplyResources(this.deleteBoardButton, "deleteBoardButton");
            this.deleteBoardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.deleteBoardButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_삭제_down_;
            this.deleteBoardButton.Name = "deleteBoardButton";
            this.deleteBoardButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_삭제_normal_;
            this.deleteBoardButton.TabStop = false;
            this.deleteBoardButton.Click += new System.EventHandler(this.deleteBoardButton_Click);
            // 
            // downloadFileButton
            // 
            this.downloadFileButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.downloadFileButton.Checked = false;
            this.downloadFileButton.CheckedImage = null;
            this.downloadFileButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.downloadFileButton.DisabledImag = global::RealTimeRMT.Properties.Resources.Library_다운로드_disabled_;
            this.downloadFileButton.DownImage = global::RealTimeRMT.Properties.Resources.Library_다운로드_down_;
            resources.ApplyResources(this.downloadFileButton, "downloadFileButton");
            this.downloadFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.downloadFileButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_다운로드_down_;
            this.downloadFileButton.Name = "downloadFileButton";
            this.downloadFileButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_다운로드_normal_;
            this.downloadFileButton.TabStop = false;
            this.downloadFileButton.Click += new System.EventHandler(this.downloadFileButton_Click);
            // 
            // listBoardButton
            // 
            this.listBoardButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.listBoardButton.Checked = false;
            this.listBoardButton.CheckedImage = null;
            this.listBoardButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.listBoardButton.DisabledImag = global::RealTimeRMT.Properties.Resources.Library_목록_normal_;
            this.listBoardButton.DownImage = global::RealTimeRMT.Properties.Resources.Library_목록_down_;
            resources.ApplyResources(this.listBoardButton, "listBoardButton");
            this.listBoardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.listBoardButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_목록_down_;
            this.listBoardButton.Name = "listBoardButton";
            this.listBoardButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_목록_normal_;
            this.listBoardButton.TabStop = false;
            this.listBoardButton.Click += new System.EventHandler(this.cancelBoardButton_Click);
            // 
            // changeFileButton
            // 
            this.changeFileButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.changeFileButton.Checked = false;
            this.changeFileButton.CheckedImage = null;
            this.changeFileButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.changeFileButton.DisabledImag = global::RealTimeRMT.Properties.Resources.Library_변경_disabled_;
            this.changeFileButton.DownImage = global::RealTimeRMT.Properties.Resources.Library_변경_down_;
            resources.ApplyResources(this.changeFileButton, "changeFileButton");
            this.changeFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.changeFileButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_변경_down_;
            this.changeFileButton.Name = "changeFileButton";
            this.changeFileButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_변경_normal_;
            this.changeFileButton.TabStop = false;
            this.changeFileButton.Click += new System.EventHandler(this.changeFileNameButton_Click);
            // 
            // createdDateTextBox
            // 
            this.createdDateTextBox.BackColor = System.Drawing.Color.White;
            this.createdDateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.createdDateTextBox, "createdDateTextBox");
            this.createdDateTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.createdDateTextBox.Name = "createdDateTextBox";
            this.createdDateTextBox.ReadOnly = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label5.Name = "label5";
            // 
            // titleTextBox
            // 
            this.titleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.titleTextBox, "titleTextBox");
            this.titleTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.titleTextBox.Name = "titleTextBox";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label7.Name = "label7";
            // 
            // writerTextBox
            // 
            this.writerTextBox.BackColor = System.Drawing.Color.White;
            this.writerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.writerTextBox, "writerTextBox");
            this.writerTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.writerTextBox.Name = "writerTextBox";
            this.writerTextBox.ReadOnly = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label6.Name = "label6";
            // 
            // viewCountTextBox
            // 
            this.viewCountTextBox.BackColor = System.Drawing.Color.White;
            this.viewCountTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.viewCountTextBox, "viewCountTextBox");
            this.viewCountTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.viewCountTextBox.Name = "viewCountTextBox";
            this.viewCountTextBox.ReadOnly = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label4.Name = "label4";
            // 
            // contentsTextBox
            // 
            this.contentsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.contentsTextBox, "contentsTextBox");
            this.contentsTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.contentsTextBox.Name = "contentsTextBox";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label3.Name = "label3";
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(227)))), ((int)(((byte)(228)))));
            this.fileNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.fileNameTextBox, "fileNameTextBox");
            this.fileNameTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label2.Name = "label2";
            // 
            // noTextBox
            // 
            this.noTextBox.BackColor = System.Drawing.Color.White;
            this.noTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.noTextBox, "noTextBox");
            this.noTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.noTextBox.Name = "noTextBox";
            this.noTextBox.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label1.Name = "label1";
            // 
            // FreeBoardForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.freeBoardPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FreeBoardForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FreeBoardForm_FormClosing);
            this.freeBoardPanel.ResumeLayout(false);
            this.freeBoardPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveBoardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteBoardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadFileButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.changeFileButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel freeBoardPanel;
        private System.Windows.Forms.TextBox createdDateTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox writerTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox viewCountTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox contentsTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox noTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageButton changeFileButton;
        private System.Windows.Forms.ImageButton saveBoardButton;
        private System.Windows.Forms.ImageButton deleteBoardButton;
        private System.Windows.Forms.ImageButton downloadFileButton;
        private System.Windows.Forms.ImageButton listBoardButton;
    }
}