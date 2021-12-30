namespace WasteReport.UserCons
{
    partial class Con_DataForDaily
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
            this.spc_main = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.pbtn_pdf = new System.Windows.Forms.PictureBox();
            this.pbtn_excel = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lbl_title = new System.Windows.Forms.Label();
            this.cfg_data = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.spc_main)).BeginInit();
            this.spc_main.Panel1.SuspendLayout();
            this.spc_main.Panel2.SuspendLayout();
            this.spc_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_pdf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_excel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cfg_data)).BeginInit();
            this.SuspendLayout();
            // 
            // spc_main
            // 
            this.spc_main.BackColor = System.Drawing.Color.White;
            this.spc_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spc_main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spc_main.Location = new System.Drawing.Point(0, 0);
            this.spc_main.Margin = new System.Windows.Forms.Padding(0);
            this.spc_main.Name = "spc_main";
            this.spc_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spc_main.Panel1
            // 
            this.spc_main.Panel1.Controls.Add(this.label1);
            this.spc_main.Panel1.Controls.Add(this.pbtn_pdf);
            this.spc_main.Panel1.Controls.Add(this.pbtn_excel);
            this.spc_main.Panel1.Controls.Add(this.pictureBox2);
            this.spc_main.Panel1.Controls.Add(this.lbl_title);
            // 
            // spc_main.Panel2
            // 
            this.spc_main.Panel2.Controls.Add(this.cfg_data);
            this.spc_main.Size = new System.Drawing.Size(930, 600);
            this.spc_main.SplitterDistance = 46;
            this.spc_main.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Myriad Pro Light", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(74)))), ((int)(((byte)(71)))));
            this.label1.Location = new System.Drawing.Point(556, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 103;
            this.label1.Text = "(Occ / Def)";
            // 
            // pbtn_pdf
            // 
            this.pbtn_pdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbtn_pdf.BackColor = System.Drawing.Color.Transparent;
            this.pbtn_pdf.BackgroundImage = global::WasteReport.Properties.Resources.sub_btn_02_pdf;
            this.pbtn_pdf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbtn_pdf.Location = new System.Drawing.Point(715, 15);
            this.pbtn_pdf.Margin = new System.Windows.Forms.Padding(0);
            this.pbtn_pdf.Name = "pbtn_pdf";
            this.pbtn_pdf.Size = new System.Drawing.Size(56, 26);
            this.pbtn_pdf.TabIndex = 102;
            this.pbtn_pdf.TabStop = false;
            this.pbtn_pdf.Click += new System.EventHandler(this.btn_pdf_Click);
            // 
            // pbtn_excel
            // 
            this.pbtn_excel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbtn_excel.BackColor = System.Drawing.Color.Transparent;
            this.pbtn_excel.BackgroundImage = global::WasteReport.Properties.Resources.sub_btn_01_excel;
            this.pbtn_excel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbtn_excel.Location = new System.Drawing.Point(653, 15);
            this.pbtn_excel.Margin = new System.Windows.Forms.Padding(0);
            this.pbtn_excel.Name = "pbtn_excel";
            this.pbtn_excel.Size = new System.Drawing.Size(56, 26);
            this.pbtn_excel.TabIndex = 102;
            this.pbtn_excel.TabStop = false;
            this.pbtn_excel.Click += new System.EventHandler(this.btn_excel_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::WasteReport.Properties.Resources.main_content_bulet;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Location = new System.Drawing.Point(6, 20);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(8, 9);
            this.pictureBox2.TabIndex = 101;
            this.pictureBox2.TabStop = false;
            // 
            // lbl_title
            // 
            this.lbl_title.BackColor = System.Drawing.Color.Transparent;
            this.lbl_title.Font = new System.Drawing.Font("Myriad Pro Light", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(74)))), ((int)(((byte)(71)))));
            this.lbl_title.Location = new System.Drawing.Point(19, 7);
            this.lbl_title.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(515, 34);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Title";
            this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cfg_data
            // 
            this.cfg_data.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cfg_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cfg_data.Location = new System.Drawing.Point(0, 0);
            this.cfg_data.Name = "cfg_data";
            this.cfg_data.Rows.DefaultSize = 20;
            this.cfg_data.Size = new System.Drawing.Size(930, 550);
            this.cfg_data.TabIndex = 0;
            // 
            // Con_DataForDaily
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.spc_main);
            this.Name = "Con_DataForDaily";
            this.Size = new System.Drawing.Size(930, 600);
            this.spc_main.Panel1.ResumeLayout(false);
            this.spc_main.Panel1.PerformLayout();
            this.spc_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spc_main)).EndInit();
            this.spc_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_pdf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbtn_excel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cfg_data)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spc_main;
        private System.Windows.Forms.Label lbl_title;
        //private C1.Win.C1FlexGrid.C1FlexGrid cfg_data;
        private System.Windows.Forms.PictureBox pictureBox2;
        //private System.Windows.Forms.PictureBox pbtn_pdf;
        //private System.Windows.Forms.PictureBox pbtn_excel;
        private System.Windows.Forms.PictureBox pbtn_pdf;
        private System.Windows.Forms.PictureBox pbtn_excel;
        private System.Windows.Forms.Label label1;
        private C1.Win.C1FlexGrid.C1FlexGrid cfg_data;
    }
}
