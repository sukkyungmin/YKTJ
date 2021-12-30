namespace WasteReport.UserCons
{
    partial class Con_Chart
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Con_Chart));
            this.c1Chat_grpPie = new C1.Win.C1Chart.C1Chart();
            this.c1Chat_grpLine = new C1.Win.C1Chart.C1Chart();
            this.spl_main = new System.Windows.Forms.SplitContainer();
            this.lbl_bar = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_totalCut = new System.Windows.Forms.Label();
            this.lbl_rptDate = new System.Windows.Forms.Label();
            this.lbl_prodDate = new System.Windows.Forms.Label();
            this.lbl_machine = new System.Windows.Forms.Label();
            this.c1Chat_cdPie = new C1.Win.C1Chart.C1Chart();
            this.c1Chart_grpCol = new C1.Win.C1Chart.C1Chart();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbtn_main = new System.Windows.Forms.PictureBox();
            this.pbtn_print = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.c1Chat_grpPie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Chat_grpLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spl_main)).BeginInit();
            this.spl_main.Panel1.SuspendLayout();
            this.spl_main.Panel2.SuspendLayout();
            this.spl_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c1Chat_cdPie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Chart_grpCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_print)).BeginInit();
            this.SuspendLayout();
            // 
            // c1Chat_grpPie
            // 
            this.c1Chat_grpPie.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1Chat_grpPie.BackColor = System.Drawing.Color.White;
            this.c1Chat_grpPie.Location = new System.Drawing.Point(14, 6);
            this.c1Chat_grpPie.Name = "c1Chat_grpPie";
            this.c1Chat_grpPie.PropBag = resources.GetString("c1Chat_grpPie.PropBag");
            this.c1Chat_grpPie.Size = new System.Drawing.Size(989, 318);
            this.c1Chat_grpPie.TabIndex = 0;
            // 
            // c1Chat_grpLine
            // 
            this.c1Chat_grpLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1Chat_grpLine.BackColor = System.Drawing.Color.White;
            this.c1Chat_grpLine.Location = new System.Drawing.Point(14, 328);
            this.c1Chat_grpLine.Name = "c1Chat_grpLine";
            this.c1Chat_grpLine.PropBag = resources.GetString("c1Chat_grpLine.PropBag");
            this.c1Chat_grpLine.Size = new System.Drawing.Size(989, 318);
            this.c1Chat_grpLine.TabIndex = 0;
            // 
            // spl_main
            // 
            this.spl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spl_main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spl_main.Location = new System.Drawing.Point(0, 0);
            this.spl_main.Margin = new System.Windows.Forms.Padding(0);
            this.spl_main.Name = "spl_main";
            this.spl_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spl_main.Panel1
            // 
            this.spl_main.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(191)))), ((int)(((byte)(186)))));
            this.spl_main.Panel1.Controls.Add(this.pbtn_main);
            this.spl_main.Panel1.Controls.Add(this.pictureBox3);
            this.spl_main.Panel1.Controls.Add(this.pictureBox2);
            this.spl_main.Panel1.Controls.Add(this.pictureBox1);
            this.spl_main.Panel1.Controls.Add(this.lbl_bar);
            this.spl_main.Panel1.Controls.Add(this.label4);
            this.spl_main.Panel1.Controls.Add(this.lbl_totalCut);
            this.spl_main.Panel1.Controls.Add(this.lbl_rptDate);
            this.spl_main.Panel1.Controls.Add(this.lbl_prodDate);
            this.spl_main.Panel1.Controls.Add(this.lbl_machine);
            // 
            // spl_main.Panel2
            // 
            this.spl_main.Panel2.AutoScroll = true;
            this.spl_main.Panel2.Controls.Add(this.pbtn_print);
            this.spl_main.Panel2.Controls.Add(this.c1Chat_cdPie);
            this.spl_main.Panel2.Controls.Add(this.c1Chart_grpCol);
            this.spl_main.Panel2.Controls.Add(this.c1Chat_grpPie);
            this.spl_main.Panel2.Controls.Add(this.c1Chat_grpLine);
            this.spl_main.Size = new System.Drawing.Size(1263, 661);
            this.spl_main.SplitterDistance = 56;
            this.spl_main.TabIndex = 1;
            // 
            // lbl_bar
            // 
            this.lbl_bar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_bar.BackColor = System.Drawing.Color.Black;
            this.lbl_bar.Location = new System.Drawing.Point(0, 54);
            this.lbl_bar.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_bar.Name = "lbl_bar";
            this.lbl_bar.Size = new System.Drawing.Size(1261, 2);
            this.lbl_bar.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(948, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 28);
            this.label4.TabIndex = 0;
            this.label4.Text = "Total Cut :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Visible = false;
            // 
            // lbl_totalCut
            // 
            this.lbl_totalCut.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_totalCut.Location = new System.Drawing.Point(1042, 14);
            this.lbl_totalCut.Name = "lbl_totalCut";
            this.lbl_totalCut.Size = new System.Drawing.Size(91, 28);
            this.lbl_totalCut.TabIndex = 0;
            this.lbl_totalCut.Text = "0";
            this.lbl_totalCut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_totalCut.Visible = false;
            // 
            // lbl_rptDate
            // 
            this.lbl_rptDate.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_rptDate.Location = new System.Drawing.Point(778, 14);
            this.lbl_rptDate.Name = "lbl_rptDate";
            this.lbl_rptDate.Size = new System.Drawing.Size(198, 28);
            this.lbl_rptDate.TabIndex = 0;
            this.lbl_rptDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_prodDate
            // 
            this.lbl_prodDate.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_prodDate.Location = new System.Drawing.Point(331, 14);
            this.lbl_prodDate.Name = "lbl_prodDate";
            this.lbl_prodDate.Size = new System.Drawing.Size(255, 28);
            this.lbl_prodDate.TabIndex = 0;
            this.lbl_prodDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_machine
            // 
            this.lbl_machine.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_machine.Location = new System.Drawing.Point(109, 14);
            this.lbl_machine.Name = "lbl_machine";
            this.lbl_machine.Size = new System.Drawing.Size(49, 28);
            this.lbl_machine.TabIndex = 0;
            this.lbl_machine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // c1Chat_cdPie
            // 
            this.c1Chat_cdPie.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1Chat_cdPie.BackColor = System.Drawing.Color.White;
            this.c1Chat_cdPie.Location = new System.Drawing.Point(14, 650);
            this.c1Chat_cdPie.Name = "c1Chat_cdPie";
            this.c1Chat_cdPie.PropBag = resources.GetString("c1Chat_cdPie.PropBag");
            this.c1Chat_cdPie.Size = new System.Drawing.Size(989, 318);
            this.c1Chat_cdPie.TabIndex = 0;
            // 
            // c1Chart_grpCol
            // 
            this.c1Chart_grpCol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1Chart_grpCol.BackColor = System.Drawing.Color.White;
            this.c1Chart_grpCol.Location = new System.Drawing.Point(14, 328);
            this.c1Chart_grpCol.Name = "c1Chart_grpCol";
            this.c1Chart_grpCol.PropBag = resources.GetString("c1Chart_grpCol.PropBag");
            this.c1Chart_grpCol.Size = new System.Drawing.Size(989, 318);
            this.c1Chart_grpCol.TabIndex = 0;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::WasteReport.Properties.Resources.sub_title_03_reportgenerated;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox3.Location = new System.Drawing.Point(604, 19);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(159, 21);
            this.pictureBox3.TabIndex = 21;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::WasteReport.Properties.Resources.sub_title_02_production;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Location = new System.Drawing.Point(175, 19);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(146, 18);
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::WasteReport.Properties.Resources.sub_title_01_machine;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(14, 19);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(82, 18);
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // pbtn_main
            // 
            this.pbtn_main.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbtn_main.BackColor = System.Drawing.Color.Transparent;
            this.pbtn_main.BackgroundImage = global::WasteReport.Properties.Resources.sub_btn_main_off;
            this.pbtn_main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbtn_main.Location = new System.Drawing.Point(1136, 11);
            this.pbtn_main.Margin = new System.Windows.Forms.Padding(0);
            this.pbtn_main.Name = "pbtn_main";
            this.pbtn_main.Size = new System.Drawing.Size(84, 36);
            this.pbtn_main.TabIndex = 24;
            this.pbtn_main.TabStop = false;
            this.pbtn_main.Click += new System.EventHandler(this.btn_home_Click);
            this.pbtn_main.MouseEnter += new System.EventHandler(this.pbtn_main_MouseEnter);
            this.pbtn_main.MouseLeave += new System.EventHandler(this.pbtn_main_MouseLeave);
            // 
            // pbtn_print
            // 
            this.pbtn_print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbtn_print.BackColor = System.Drawing.Color.Transparent;
            this.pbtn_print.BackgroundImage = global::WasteReport.Properties.Resources.sub_btn_03_print;
            this.pbtn_print.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbtn_print.Location = new System.Drawing.Point(1136, 6);
            this.pbtn_print.Margin = new System.Windows.Forms.Padding(0);
            this.pbtn_print.Name = "pbtn_print";
            this.pbtn_print.Size = new System.Drawing.Size(56, 26);
            this.pbtn_print.TabIndex = 103;
            this.pbtn_print.TabStop = false;
            this.pbtn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // Con_Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.spl_main);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Con_Chart";
            this.Size = new System.Drawing.Size(1263, 661);
            ((System.ComponentModel.ISupportInitialize)(this.c1Chat_grpPie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Chat_grpLine)).EndInit();
            this.spl_main.Panel1.ResumeLayout(false);
            this.spl_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spl_main)).EndInit();
            this.spl_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.c1Chat_cdPie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Chart_grpCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_print)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1Chart.C1Chart c1Chat_grpPie;
        private C1.Win.C1Chart.C1Chart c1Chat_grpLine;
        private System.Windows.Forms.SplitContainer spl_main;
        private System.Windows.Forms.Label lbl_bar;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lbl_totalCut;
        private System.Windows.Forms.Label lbl_rptDate;
        private System.Windows.Forms.Label lbl_prodDate;
        private System.Windows.Forms.Label lbl_machine;
        private C1.Win.C1Chart.C1Chart c1Chat_cdPie;
        private C1.Win.C1Chart.C1Chart c1Chart_grpCol;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbtn_main;
        private System.Windows.Forms.PictureBox pbtn_print;
    }
}
