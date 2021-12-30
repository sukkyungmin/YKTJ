namespace WasteReport.UserCons
{
    partial class Con_Daily
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
            this.tlp_top = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_bar = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbtn_main = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_totalCut = new System.Windows.Forms.Label();
            this.lbl_rptDate = new System.Windows.Forms.Label();
            this.lbl_prodDate = new System.Windows.Forms.Label();
            this.lbl_machine = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cfg_topInfo = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tlp_main = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.spl_main)).BeginInit();
            this.spl_main.Panel1.SuspendLayout();
            this.spl_main.Panel2.SuspendLayout();
            this.spl_main.SuspendLayout();
            this.tlp_top.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cfg_topInfo)).BeginInit();
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
            this.spl_main.Panel1.Controls.Add(this.tlp_top);
            // 
            // spl_main.Panel2
            // 
            this.spl_main.Panel2.AutoScroll = true;
            this.spl_main.Panel2.Controls.Add(this.tlp_main);
            this.spl_main.Size = new System.Drawing.Size(1263, 661);
            this.spl_main.SplitterDistance = 177;
            this.spl_main.TabIndex = 0;
            // 
            // tlp_top
            // 
            this.tlp_top.ColumnCount = 1;
            this.tlp_top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_top.Controls.Add(this.lbl_bar, 0, 2);
            this.tlp_top.Controls.Add(this.panel1, 0, 0);
            this.tlp_top.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tlp_top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_top.Location = new System.Drawing.Point(0, 0);
            this.tlp_top.Margin = new System.Windows.Forms.Padding(0);
            this.tlp_top.Name = "tlp_top";
            this.tlp_top.RowCount = 3;
            this.tlp_top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlp_top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlp_top.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tlp_top.Size = new System.Drawing.Size(1263, 177);
            this.tlp_top.TabIndex = 3;
            // 
            // lbl_bar
            // 
            this.lbl_bar.BackColor = System.Drawing.Color.Black;
            this.lbl_bar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_bar.Location = new System.Drawing.Point(0, 175);
            this.lbl_bar.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_bar.Name = "lbl_bar";
            this.lbl_bar.Size = new System.Drawing.Size(1263, 2);
            this.lbl_bar.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pbtn_main);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbl_totalCut);
            this.panel1.Controls.Add(this.lbl_rptDate);
            this.panel1.Controls.Add(this.lbl_prodDate);
            this.panel1.Controls.Add(this.lbl_machine);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1263, 70);
            this.panel1.TabIndex = 4;
            // 
            // pbtn_main
            // 
            this.pbtn_main.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbtn_main.BackColor = System.Drawing.Color.Transparent;
            this.pbtn_main.BackgroundImage = global::WasteReport.Properties.Resources.sub_btn_main_off;
            this.pbtn_main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbtn_main.Location = new System.Drawing.Point(1150, 18);
            this.pbtn_main.Margin = new System.Windows.Forms.Padding(0);
            this.pbtn_main.Name = "pbtn_main";
            this.pbtn_main.Size = new System.Drawing.Size(84, 36);
            this.pbtn_main.TabIndex = 21;
            this.pbtn_main.TabStop = false;
            this.pbtn_main.Click += new System.EventHandler(this.btn_home_Click);
            this.pbtn_main.MouseEnter += new System.EventHandler(this.pbtn_main_MouseEnter);
            this.pbtn_main.MouseLeave += new System.EventHandler(this.pbtn_main_MouseLeave);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::WasteReport.Properties.Resources.sub_title_03_reportgenerated;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox3.Location = new System.Drawing.Point(614, 26);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(159, 21);
            this.pictureBox3.TabIndex = 20;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::WasteReport.Properties.Resources.sub_title_02_production;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Location = new System.Drawing.Point(185, 26);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(146, 18);
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::WasteReport.Properties.Resources.sub_title_01_machine;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(24, 26);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(82, 18);
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(961, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 28);
            this.label4.TabIndex = 12;
            this.label4.Text = "Total Cut :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Visible = false;
            // 
            // lbl_totalCut
            // 
            this.lbl_totalCut.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_totalCut.Location = new System.Drawing.Point(1055, 22);
            this.lbl_totalCut.Name = "lbl_totalCut";
            this.lbl_totalCut.Size = new System.Drawing.Size(91, 28);
            this.lbl_totalCut.TabIndex = 15;
            this.lbl_totalCut.Text = "0";
            this.lbl_totalCut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_totalCut.Visible = false;
            // 
            // lbl_rptDate
            // 
            this.lbl_rptDate.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_rptDate.Location = new System.Drawing.Point(784, 21);
            this.lbl_rptDate.Name = "lbl_rptDate";
            this.lbl_rptDate.Size = new System.Drawing.Size(198, 28);
            this.lbl_rptDate.TabIndex = 16;
            this.lbl_rptDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_prodDate
            // 
            this.lbl_prodDate.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_prodDate.Location = new System.Drawing.Point(344, 21);
            this.lbl_prodDate.Name = "lbl_prodDate";
            this.lbl_prodDate.Size = new System.Drawing.Size(255, 28);
            this.lbl_prodDate.TabIndex = 17;
            this.lbl_prodDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_machine
            // 
            this.lbl_machine.Font = new System.Drawing.Font("Myriad Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_machine.Location = new System.Drawing.Point(119, 21);
            this.lbl_machine.Name = "lbl_machine";
            this.lbl_machine.Size = new System.Drawing.Size(49, 28);
            this.lbl_machine.TabIndex = 18;
            this.lbl_machine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Controls.Add(this.cfg_topInfo, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 73);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1257, 99);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // cfg_topInfo
            // 
            this.cfg_topInfo.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cfg_topInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cfg_topInfo.Location = new System.Drawing.Point(28, 3);
            this.cfg_topInfo.Name = "cfg_topInfo";
            this.cfg_topInfo.Rows.DefaultSize = 20;
            this.cfg_topInfo.Size = new System.Drawing.Size(1201, 93);
            this.cfg_topInfo.TabIndex = 0;
            // 
            // tlp_main
            // 
            this.tlp_main.AutoScroll = true;
            this.tlp_main.BackColor = System.Drawing.Color.Transparent;
            this.tlp_main.ColumnCount = 1;
            this.tlp_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_main.Location = new System.Drawing.Point(0, 0);
            this.tlp_main.Name = "tlp_main";
            this.tlp_main.RowCount = 3;
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlp_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tlp_main.Size = new System.Drawing.Size(1263, 480);
            this.tlp_main.TabIndex = 0;
            // 
            // Con_Daily
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.spl_main);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Con_Daily";
            this.Size = new System.Drawing.Size(1263, 661);
            this.spl_main.Panel1.ResumeLayout(false);
            this.spl_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spl_main)).EndInit();
            this.spl_main.ResumeLayout(false);
            this.tlp_top.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cfg_topInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spl_main;
        public System.Windows.Forms.TableLayoutPanel tlp_main;
        private System.Windows.Forms.TableLayoutPanel tlp_top;
        private System.Windows.Forms.Label lbl_bar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lbl_totalCut;
        private System.Windows.Forms.Label lbl_rptDate;
        private System.Windows.Forms.Label lbl_prodDate;
        private System.Windows.Forms.Label lbl_machine;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbtn_main;
        private C1.Win.C1FlexGrid.C1FlexGrid cfg_topInfo;
    }
}
