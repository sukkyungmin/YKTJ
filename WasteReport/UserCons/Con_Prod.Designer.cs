namespace WasteReport.UserCons
{
    partial class Con_Prod
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
            this.spl_main = new System.Windows.Forms.SplitContainer();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pbtn_main = new System.Windows.Forms.PictureBox();
            this.lbl_rptDate = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_totalCut = new System.Windows.Forms.Label();
            this.lbl_prodDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_bar = new System.Windows.Forms.Label();
            this.lbl_machine = new System.Windows.Forms.Label();
            this.tlp_bottom = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_main = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_top = new System.Windows.Forms.Panel();
            this.pbtn_pdf = new System.Windows.Forms.PictureBox();
            this.pbtn_excel = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_title = new System.Windows.Forms.Label();
            this.cfg_prodList = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.spl_main)).BeginInit();
            this.spl_main.Panel1.SuspendLayout();
            this.spl_main.Panel2.SuspendLayout();
            this.spl_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_main)).BeginInit();
            this.tlp_bottom.SuspendLayout();
            this.tlp_main.SuspendLayout();
            this.pnl_top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_pdf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_excel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cfg_prodList)).BeginInit();
            this.SuspendLayout();
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
            this.spl_main.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.spl_main.Panel1.Controls.Add(this.pictureBox3);
            this.spl_main.Panel1.Controls.Add(this.pictureBox2);
            this.spl_main.Panel1.Controls.Add(this.pbtn_main);
            this.spl_main.Panel1.Controls.Add(this.lbl_rptDate);
            this.spl_main.Panel1.Controls.Add(this.label4);
            this.spl_main.Panel1.Controls.Add(this.lbl_totalCut);
            this.spl_main.Panel1.Controls.Add(this.lbl_prodDate);
            this.spl_main.Panel1.Controls.Add(this.label1);
            this.spl_main.Panel1.Controls.Add(this.lbl_bar);
            this.spl_main.Panel1.Controls.Add(this.lbl_machine);
            // 
            // spl_main.Panel2
            // 
            this.spl_main.Panel2.AutoScroll = true;
            this.spl_main.Panel2.Controls.Add(this.tlp_bottom);
            this.spl_main.Size = new System.Drawing.Size(1263, 661);
            this.spl_main.SplitterDistance = 61;
            this.spl_main.TabIndex = 2;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::WasteReport.Properties.Resources.sub_title_03_reportgenerated;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox3.Location = new System.Drawing.Point(461, 19);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(159, 21);
            this.pictureBox3.TabIndex = 27;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::WasteReport.Properties.Resources.sub_title_02_production;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Location = new System.Drawing.Point(9, 22);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(146, 18);
            this.pictureBox2.TabIndex = 26;
            this.pictureBox2.TabStop = false;
            // 
            // pbtn_main
            // 
            this.pbtn_main.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbtn_main.BackColor = System.Drawing.Color.Transparent;
            this.pbtn_main.BackgroundImage = global::WasteReport.Properties.Resources.sub_btn_main_off;
            this.pbtn_main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbtn_main.Location = new System.Drawing.Point(1124, 14);
            this.pbtn_main.Margin = new System.Windows.Forms.Padding(0);
            this.pbtn_main.Name = "pbtn_main";
            this.pbtn_main.Size = new System.Drawing.Size(84, 36);
            this.pbtn_main.TabIndex = 25;
            this.pbtn_main.TabStop = false;
            this.pbtn_main.Click += new System.EventHandler(this.btn_home_Click);
            this.pbtn_main.MouseEnter += new System.EventHandler(this.pbtn_main_MouseEnter);
            this.pbtn_main.MouseLeave += new System.EventHandler(this.pbtn_main_MouseLeave);
            // 
            // lbl_rptDate
            // 
            this.lbl_rptDate.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_rptDate.Location = new System.Drawing.Point(633, 14);
            this.lbl_rptDate.Name = "lbl_rptDate";
            this.lbl_rptDate.Size = new System.Drawing.Size(375, 28);
            this.lbl_rptDate.TabIndex = 0;
            this.lbl_rptDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(790, 14);
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
            this.lbl_totalCut.Location = new System.Drawing.Point(884, 14);
            this.lbl_totalCut.Name = "lbl_totalCut";
            this.lbl_totalCut.Size = new System.Drawing.Size(91, 28);
            this.lbl_totalCut.TabIndex = 0;
            this.lbl_totalCut.Text = "0";
            this.lbl_totalCut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_totalCut.Visible = false;
            // 
            // lbl_prodDate
            // 
            this.lbl_prodDate.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_prodDate.Location = new System.Drawing.Point(185, 14);
            this.lbl_prodDate.Name = "lbl_prodDate";
            this.lbl_prodDate.Size = new System.Drawing.Size(255, 28);
            this.lbl_prodDate.TabIndex = 0;
            this.lbl_prodDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(956, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Machine :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Visible = false;
            // 
            // lbl_bar
            // 
            this.lbl_bar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_bar.BackColor = System.Drawing.Color.Black;
            this.lbl_bar.Location = new System.Drawing.Point(0, 60);
            this.lbl_bar.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_bar.Name = "lbl_bar";
            this.lbl_bar.Size = new System.Drawing.Size(1259, 2);
            this.lbl_bar.TabIndex = 2;
            // 
            // lbl_machine
            // 
            this.lbl_machine.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_machine.Location = new System.Drawing.Point(1055, 29);
            this.lbl_machine.Name = "lbl_machine";
            this.lbl_machine.Size = new System.Drawing.Size(80, 28);
            this.lbl_machine.TabIndex = 0;
            this.lbl_machine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_machine.Visible = false;
            // 
            // tlp_bottom
            // 
            this.tlp_bottom.ColumnCount = 1;
            this.tlp_bottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_bottom.Controls.Add(this.tlp_main, 0, 0);
            this.tlp_bottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_bottom.Location = new System.Drawing.Point(0, 0);
            this.tlp_bottom.Margin = new System.Windows.Forms.Padding(0);
            this.tlp_bottom.Name = "tlp_bottom";
            this.tlp_bottom.RowCount = 1;
            this.tlp_bottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_bottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_bottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_bottom.Size = new System.Drawing.Size(1263, 596);
            this.tlp_bottom.TabIndex = 4;
            // 
            // tlp_main
            // 
            this.tlp_main.ColumnCount = 1;
            this.tlp_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_main.Controls.Add(this.pnl_top, 0, 0);
            this.tlp_main.Controls.Add(this.cfg_prodList, 0, 1);
            this.tlp_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_main.Location = new System.Drawing.Point(0, 0);
            this.tlp_main.Margin = new System.Windows.Forms.Padding(0);
            this.tlp_main.Name = "tlp_main";
            this.tlp_main.RowCount = 3;
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_main.Size = new System.Drawing.Size(1263, 596);
            this.tlp_main.TabIndex = 3;
            // 
            // pnl_top
            // 
            this.pnl_top.Controls.Add(this.pbtn_pdf);
            this.pnl_top.Controls.Add(this.pbtn_excel);
            this.pnl_top.Controls.Add(this.pictureBox1);
            this.pnl_top.Controls.Add(this.lbl_title);
            this.pnl_top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_top.Location = new System.Drawing.Point(0, 0);
            this.pnl_top.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_top.Name = "pnl_top";
            this.pnl_top.Size = new System.Drawing.Size(1263, 30);
            this.pnl_top.TabIndex = 4;
            // 
            // pbtn_pdf
            // 
            this.pbtn_pdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbtn_pdf.BackColor = System.Drawing.Color.Transparent;
            this.pbtn_pdf.BackgroundImage = global::WasteReport.Properties.Resources.sub_btn_02_pdf;
            this.pbtn_pdf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbtn_pdf.Location = new System.Drawing.Point(1152, 2);
            this.pbtn_pdf.Margin = new System.Windows.Forms.Padding(0);
            this.pbtn_pdf.Name = "pbtn_pdf";
            this.pbtn_pdf.Size = new System.Drawing.Size(56, 26);
            this.pbtn_pdf.TabIndex = 103;
            this.pbtn_pdf.TabStop = false;
            this.pbtn_pdf.Click += new System.EventHandler(this.btn_pdf_Click);
            // 
            // pbtn_excel
            // 
            this.pbtn_excel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbtn_excel.BackColor = System.Drawing.Color.Transparent;
            this.pbtn_excel.BackgroundImage = global::WasteReport.Properties.Resources.sub_btn_01_excel;
            this.pbtn_excel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbtn_excel.Location = new System.Drawing.Point(1062, 2);
            this.pbtn_excel.Margin = new System.Windows.Forms.Padding(0);
            this.pbtn_excel.Name = "pbtn_excel";
            this.pbtn_excel.Size = new System.Drawing.Size(56, 26);
            this.pbtn_excel.TabIndex = 104;
            this.pbtn_excel.TabStop = false;
            this.pbtn_excel.Click += new System.EventHandler(this.btn_excel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::WasteReport.Properties.Resources.main_content_bulet;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(9, 12);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(8, 9);
            this.pictureBox1.TabIndex = 102;
            this.pictureBox1.TabStop = false;
            // 
            // lbl_title
            // 
            this.lbl_title.BackColor = System.Drawing.Color.Transparent;
            this.lbl_title.Font = new System.Drawing.Font("Myriad Pro Light", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(74)))), ((int)(((byte)(71)))));
            this.lbl_title.Location = new System.Drawing.Point(20, 2);
            this.lbl_title.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(664, 28);
            this.lbl_title.TabIndex = 4;
            this.lbl_title.Text = "Production List - 유아1부";
            this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cfg_prodList
            // 
            this.cfg_prodList.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cfg_prodList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cfg_prodList.Location = new System.Drawing.Point(3, 33);
            this.cfg_prodList.Name = "cfg_prodList";
            this.cfg_prodList.Rows.DefaultSize = 20;
            this.cfg_prodList.Size = new System.Drawing.Size(1257, 540);
            this.cfg_prodList.TabIndex = 5;
            this.cfg_prodList.SizeChanged += new System.EventHandler(this.cfg_prodList_SizeChanged);
            // 
            // Con_Prod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.spl_main);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Con_Prod";
            this.Size = new System.Drawing.Size(1263, 661);
            this.spl_main.Panel1.ResumeLayout(false);
            this.spl_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spl_main)).EndInit();
            this.spl_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_main)).EndInit();
            this.tlp_bottom.ResumeLayout(false);
            this.tlp_main.ResumeLayout(false);
            this.pnl_top.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_pdf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_excel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cfg_prodList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spl_main;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_bar;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lbl_totalCut;
        private System.Windows.Forms.Label lbl_rptDate;
        private System.Windows.Forms.Label lbl_prodDate;
        private System.Windows.Forms.Label lbl_machine;
        private System.Windows.Forms.TableLayoutPanel tlp_main;
        private System.Windows.Forms.Panel pnl_top;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.TableLayoutPanel tlp_bottom;
        private System.Windows.Forms.PictureBox pbtn_main;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbtn_pdf;
        private System.Windows.Forms.PictureBox pbtn_excel;
        private C1.Win.C1FlexGrid.C1FlexGrid cfg_prodList;
    }
}
