namespace RealTimeRMT
{
    partial class NoticeBoardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoticeBoardForm));
            this.noticeBoardPanel = new System.Windows.Forms.Panel();
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
            this.noticeBoardPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveBoardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteBoardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadFileButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.changeFileButton)).BeginInit();
            this.SuspendLayout();
            // 
            // noticeBoardPanel
            // 
            this.noticeBoardPanel.BackColor = System.Drawing.Color.White;
            this.noticeBoardPanel.Controls.Add(this.saveBoardButton);
            this.noticeBoardPanel.Controls.Add(this.deleteBoardButton);
            this.noticeBoardPanel.Controls.Add(this.downloadFileButton);
            this.noticeBoardPanel.Controls.Add(this.listBoardButton);
            this.noticeBoardPanel.Controls.Add(this.changeFileButton);
            this.noticeBoardPanel.Controls.Add(this.createdDateTextBox);
            this.noticeBoardPanel.Controls.Add(this.label5);
            this.noticeBoardPanel.Controls.Add(this.titleTextBox);
            this.noticeBoardPanel.Controls.Add(this.label7);
            this.noticeBoardPanel.Controls.Add(this.writerTextBox);
            this.noticeBoardPanel.Controls.Add(this.label6);
            this.noticeBoardPanel.Controls.Add(this.viewCountTextBox);
            this.noticeBoardPanel.Controls.Add(this.label4);
            this.noticeBoardPanel.Controls.Add(this.contentsTextBox);
            this.noticeBoardPanel.Controls.Add(this.label3);
            this.noticeBoardPanel.Controls.Add(this.fileNameTextBox);
            this.noticeBoardPanel.Controls.Add(this.label2);
            this.noticeBoardPanel.Controls.Add(this.noTextBox);
            this.noticeBoardPanel.Controls.Add(this.label1);
            this.noticeBoardPanel.Location = new System.Drawing.Point(12, 13);
            this.noticeBoardPanel.Margin = new System.Windows.Forms.Padding(0);
            this.noticeBoardPanel.Name = "noticeBoardPanel";
            this.noticeBoardPanel.Size = new System.Drawing.Size(740, 561);
            this.noticeBoardPanel.TabIndex = 28;
            this.noticeBoardPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.noticeBoardPanel_Paint);
            // 
            // saveBoardButton
            // 
            this.saveBoardButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.saveBoardButton.Checked = false;
            this.saveBoardButton.CheckedImage = null;
            this.saveBoardButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.saveBoardButton.DisabledImag = global::RealTimeRMT.Properties.Resources.Library_저장_disabled_;
            this.saveBoardButton.DownImage = global::RealTimeRMT.Properties.Resources.Library_저장_down_;
            this.saveBoardButton.Font = new System.Drawing.Font("BareunDotum 1", 8.25F, System.Drawing.FontStyle.Bold);
            this.saveBoardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.saveBoardButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_저장_down_;
            this.saveBoardButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.saveBoardButton.Location = new System.Drawing.Point(531, 524);
            this.saveBoardButton.Name = "saveBoardButton";
            this.saveBoardButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_저장_normal_;
            this.saveBoardButton.Size = new System.Drawing.Size(41, 24);
            this.saveBoardButton.TabIndex = 55;
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
            this.deleteBoardButton.Font = new System.Drawing.Font("BareunDotum 1", 8.25F, System.Drawing.FontStyle.Bold);
            this.deleteBoardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.deleteBoardButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_삭제_down_;
            this.deleteBoardButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.deleteBoardButton.Location = new System.Drawing.Point(578, 524);
            this.deleteBoardButton.Name = "deleteBoardButton";
            this.deleteBoardButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_삭제_normal_;
            this.deleteBoardButton.Size = new System.Drawing.Size(41, 24);
            this.deleteBoardButton.TabIndex = 56;
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
            this.downloadFileButton.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold);
            this.downloadFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.downloadFileButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_다운로드_down_;
            this.downloadFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.downloadFileButton.Location = new System.Drawing.Point(672, 524);
            this.downloadFileButton.Margin = new System.Windows.Forms.Padding(0);
            this.downloadFileButton.Name = "downloadFileButton";
            this.downloadFileButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_다운로드_normal_;
            this.downloadFileButton.Size = new System.Drawing.Size(57, 24);
            this.downloadFileButton.TabIndex = 54;
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
            this.listBoardButton.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold);
            this.listBoardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.listBoardButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_목록_down_;
            this.listBoardButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listBoardButton.Location = new System.Drawing.Point(625, 524);
            this.listBoardButton.Margin = new System.Windows.Forms.Padding(0);
            this.listBoardButton.Name = "listBoardButton";
            this.listBoardButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_목록_normal_;
            this.listBoardButton.Size = new System.Drawing.Size(41, 24);
            this.listBoardButton.TabIndex = 53;
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
            this.changeFileButton.Font = new System.Drawing.Font("BareunDotum 1", 8.25F, System.Drawing.FontStyle.Bold);
            this.changeFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.changeFileButton.HoverImage = global::RealTimeRMT.Properties.Resources.Library_변경_down_;
            this.changeFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.changeFileButton.Location = new System.Drawing.Point(690, 489);
            this.changeFileButton.Name = "changeFileButton";
            this.changeFileButton.NormalImage = global::RealTimeRMT.Properties.Resources.Library_변경_normal_;
            this.changeFileButton.Size = new System.Drawing.Size(39, 23);
            this.changeFileButton.TabIndex = 52;
            this.changeFileButton.TabStop = false;
            this.changeFileButton.Click += new System.EventHandler(this.changeFileNameButton_Click);
            // 
            // createdDateTextBox
            // 
            this.createdDateTextBox.BackColor = System.Drawing.Color.White;
            this.createdDateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.createdDateTextBox.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.createdDateTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.createdDateTextBox.Location = new System.Drawing.Point(582, 14);
            this.createdDateTextBox.Name = "createdDateTextBox";
            this.createdDateTextBox.ReadOnly = true;
            this.createdDateTextBox.Size = new System.Drawing.Size(148, 13);
            this.createdDateTextBox.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.label5.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label5.Location = new System.Drawing.Point(476, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 21);
            this.label5.TabIndex = 45;
            this.label5.Text = "작성일";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleTextBox
            // 
            this.titleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleTextBox.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.titleTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.titleTextBox.Location = new System.Drawing.Point(119, 16);
            this.titleTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(350, 13);
            this.titleTextBox.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.label7.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label7.Location = new System.Drawing.Point(12, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 44;
            this.label7.Text = "제목";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // writerTextBox
            // 
            this.writerTextBox.BackColor = System.Drawing.Color.White;
            this.writerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.writerTextBox.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.writerTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.writerTextBox.Location = new System.Drawing.Point(120, 41);
            this.writerTextBox.Name = "writerTextBox";
            this.writerTextBox.ReadOnly = true;
            this.writerTextBox.Size = new System.Drawing.Size(183, 13);
            this.writerTextBox.TabIndex = 42;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.label6.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label6.Location = new System.Drawing.Point(12, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 21);
            this.label6.TabIndex = 43;
            this.label6.Text = "작성자";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // viewCountTextBox
            // 
            this.viewCountTextBox.BackColor = System.Drawing.Color.White;
            this.viewCountTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewCountTextBox.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.viewCountTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.viewCountTextBox.Location = new System.Drawing.Point(630, 41);
            this.viewCountTextBox.Name = "viewCountTextBox";
            this.viewCountTextBox.ReadOnly = true;
            this.viewCountTextBox.Size = new System.Drawing.Size(100, 13);
            this.viewCountTextBox.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.label4.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label4.Location = new System.Drawing.Point(523, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 41;
            this.label4.Text = "조회수";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contentsTextBox
            // 
            this.contentsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contentsTextBox.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentsTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.contentsTextBox.Location = new System.Drawing.Point(119, 68);
            this.contentsTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.contentsTextBox.Multiline = true;
            this.contentsTextBox.Name = "contentsTextBox";
            this.contentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.contentsTextBox.Size = new System.Drawing.Size(610, 414);
            this.contentsTextBox.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.label3.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 39;
            this.label3.Text = "내용";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fileNameTextBox.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.fileNameTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.fileNameTextBox.Location = new System.Drawing.Point(119, 493);
            this.fileNameTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.ReadOnly = true;
            this.fileNameTextBox.Size = new System.Drawing.Size(564, 13);
            this.fileNameTextBox.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.label2.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label2.Location = new System.Drawing.Point(12, 490);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 38;
            this.label2.Text = "파일 이름";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // noTextBox
            // 
            this.noTextBox.BackColor = System.Drawing.Color.White;
            this.noTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.noTextBox.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.noTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.noTextBox.Location = new System.Drawing.Point(416, 41);
            this.noTextBox.Name = "noTextBox";
            this.noTextBox.ReadOnly = true;
            this.noTextBox.Size = new System.Drawing.Size(100, 13);
            this.noTextBox.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.label1.Font = new System.Drawing.Font("BareunDotum 1", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label1.Location = new System.Drawing.Point(309, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 35;
            this.label1.Text = "번호";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NoticeBoardForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.ClientSize = new System.Drawing.Size(764, 587);
            this.Controls.Add(this.noticeBoardPanel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NoticeBoardForm";
            this.Text = "공지사항 등록";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NoticeBoardForm_FormClosing);
            this.noticeBoardPanel.ResumeLayout(false);
            this.noticeBoardPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveBoardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteBoardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadFileButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.changeFileButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel noticeBoardPanel;
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
        private System.Windows.Forms.ImageButton saveBoardButton;
        private System.Windows.Forms.ImageButton deleteBoardButton;
        private System.Windows.Forms.ImageButton downloadFileButton;
        private System.Windows.Forms.ImageButton listBoardButton;
        private System.Windows.Forms.ImageButton changeFileButton;
    }
}