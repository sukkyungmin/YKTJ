using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WasteReport.CS;
using C1.Win.C1FlexGrid;
using System.Threading;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WasteReport.UserCons
{
    public partial class Con_Prod : UserControl
    {
        MainForm mainForm;
        WasteSearch m_ws;
        MDBmanager dcon = null;
        string docTitleStr = "";

        decimal m_GridWidth = 0;
        decimal[] m_ColWidthRate = null;
        DataTable dt_data = null;

        LoadingForm loadingForm = null;

        public bool bl_finish = false;

        public Con_Prod(MainForm mf, WasteSearch ws)
        {
            InitializeComponent();

            mainForm = mf;
            m_ws = ws;

            SetBasicInfo();

            dcon = new MDBmanager("유아1부");

            this.Hide();
            loadingForm = new LoadingForm(this);
            loadingForm.Show();
            loadingForm.Refresh();
            loadingForm.StartThrd();

            SetTotalCut();
            SetProdList("유아1부");


            bl_finish = true;

            InitGridStyle("유아1부");
            SetExtraStyle("유아1부");
            GetGridSize("유아1부");
            GridComma(cfg_prodList, "Cuts", "Culls", "CaseCount", "Stops", "Prod/Case", ref dt_data);


            //switch(m_ws.machine)
            //{
            //    case "유아1부":
            //        tlp_bottom.RowStyles[1].Height = 0;
            //        tlp_bottom.RowStyles[2].Height = 0;
            //        break;
            //}

            docTitleStr = string.Format("Machine: 유아1부      Production Date : {0} ~ {1}", m_ws.staDate, m_ws.endDate);

        }

        //화면 상단 기본데이터 디스플레이
        private void SetBasicInfo()
        {
            lbl_machine.Text = m_ws.machine;
            lbl_prodDate.Text = string.Format("{0} to {1}", m_ws.staDate, m_ws.endDate);
            lbl_rptDate.Text = DateTime.Now.ToString();
        }

        private void SetProdList(string machine)
        {
            switch (machine)
            {
                case "유아1부":
                    SetProdList_2(cfg_prodList, ref dt_data, dcon);
                    break;
            }
        }

        private void SetProdList_2(C1FlexGrid cfg, ref DataTable dt, MDBmanager dataCon)
        {
            dt = new DataTable();

            dt = dataCon.GetProdList(m_ws.staDate, m_ws.endDate, m_ws.shift);
            SetTotalRow(ref dt);
            cfg.DataSource = dt;
        }

        //초기 그리드 스타일 세팅
        private void InitGridStyle(string machine)
        {
            switch (machine)
            {
                case "유아1부":
                    InitGridStyle_2(cfg_prodList, ref dt_data);
                    for (int i = 1; i < 3; i++)
                    {
                        cfg_prodList.Cols[i].TextAlignFixed = TextAlignEnum.CenterCenter;
                        cfg_prodList.Cols[i].TextAlign = TextAlignEnum.CenterCenter;
                    }

                break;
            }
        }

        private void InitGridStyle_2(C1FlexGrid cfg, ref DataTable dt)
        {
            cfg.SelectionMode = SelectionModeEnum.Row;
            cfg.ExtendLastCol = true;

            cfg.Cols[0].Width = 0;

            CellStyle cs = cfg.Styles.Normal;
            cs = cfg.Styles.Fixed;
            cs.BackColor = Color.FromArgb(235, 234, 232);
            cs.ForeColor = Color.FromArgb(96, 96, 96);
            cs.Border.Style = BorderStyleEnum.None;
            cs.TextAlign = TextAlignEnum.RightCenter;

            //if (dt.Rows.Count > 0)
            //{
            //    cfg.Cols[1].TextAlignFixed = TextAlignEnum.LeftCenter;
            //    cfg.Cols[2].TextAlignFixed = TextAlignEnum.LeftCenter;
            //}
        }

        //추가적인 그리드 스타일 세팅
        private void SetExtraStyle(string machine)
        {
            switch (machine)
            {
                case "유아1부":
                    SetExtraStyle_2(cfg_prodList, ref dt_data);
                    break;
            }
        }

        private void SetExtraStyle_2(C1FlexGrid cfg, ref DataTable dt)
        {
            if (dt.Rows.Count > 1 || dt != null)
            {

                cfg.Cols[1].Width = 80;
                cfg.Cols[2].Width = 80;
                cfg.Cols[3].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[4].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[5].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[6].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[7].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[8].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[9].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[10].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[11].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[12].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[13].Width = (cfg.Size.Width - 180) / 12;
                cfg.Cols[14].Width = (cfg.Size.Width - 180) / 12;

                CellStyle csCellStyle2 = cfg.Styles.Add("CellStyle2");
                csCellStyle2.ForeColor = Color.Black;
                csCellStyle2.Font = new System.Drawing.Font(Font, FontStyle.Bold | FontStyle.Underline);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 2, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 3, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 4, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 5, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 6, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 7, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 8, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 9, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 10, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 11, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 12, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 13, csCellStyle2);
                cfg.SetCellStyle(cfg.Rows.Count - 1, 14, csCellStyle2);

                cfg.Select(0, 0);
            }
        }

        private void GridComma(C1FlexGrid cfg, string St1, string St2, string St3, string St4, string St5, ref DataTable dt)
        {

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == St1 || dt.Columns[i].ColumnName == St2 || dt.Columns[i].ColumnName == St3
                    || dt.Columns[i].ColumnName == St4 || dt.Columns[i].ColumnName == St5)
                {
                    cfg.Cols[i + 1].Style.Format = "##,0";
                }
                //else if (dt.Columns[i].ColumnName == St4 || dt.Columns[i].ColumnName == St5)
                //{
                //    cfg.Cols[i + 1].Style.Format = "#0.00";
                //}
            }
        }

        //Datatable 토탈 로우 추가 해주기
        private void SetTotalRow(ref DataTable dt)
        {
            if (dt.Rows.Count > 1)
            {
                DataRow totalRow = dt.NewRow();

                //totalRow[0] = "Total/Avg :";
                totalRow[2] = dt.Compute("Sum(Cuts)", string.Empty).ToString();
                totalRow[3] = dt.Compute("Sum(Culls)", string.Empty).ToString();

                //if (decimal.Parse(totalRow[2].ToString()) == 0)
                //{
                //    totalRow[3] = "0";
                //}
                //else
                //{
                //    totalRow[3] = Math.Round((decimal.Parse(totalRow[2].ToString()) / decimal.Parse(totalRow[1].ToString())) * 100, 2).ToString();
                //}

                totalRow[4] = Math.Round(decimal.Parse(dt.Compute("Avg([TW%])", string.Empty).ToString()), 1).ToString();
                totalRow[5] = Math.Round(decimal.Parse(dt.Compute("Avg([CW%])", string.Empty).ToString()), 1).ToString();
                totalRow[6] = Math.Round(decimal.Parse(dt.Compute("Avg([PW%])", string.Empty).ToString()), 1).ToString();
                totalRow[7] = dt.Compute("Sum(CaseCount)", string.Empty).ToString();
                totalRow[8] = dt.Compute("Sum(CCC)", string.Empty).ToString();
                totalRow[9] = Math.Round(decimal.Parse(dt.Compute("Avg([Avg_MDPH])", string.Empty).ToString()), 1).ToString();
                totalRow[10] = Math.Round(decimal.Parse(dt.Compute("Avg([%Down])", string.Empty).ToString()), 1).ToString();
                totalRow[11] = dt.Compute("Sum(Stops)", string.Empty).ToString();
                totalRow[12] = Math.Round(decimal.Parse(dt.Compute("Avg([AvgDPM])", string.Empty).ToString()), 1).ToString();

                dt.Rows.Add(totalRow);
            }
        }


        //그리드 리사이즈시 필요 변수값 가져오기
        public void GetGridSize(string machine)
        {
            switch (machine)
            {
                case "유아1부":
                    GetGridSize_2(cfg_prodList);
                    break;
            }
        }

        public void GetGridSize_2(C1FlexGrid cfg)
        {
            m_GridWidth = cfg.Width;
            m_ColWidthRate = new decimal[cfg.Cols.Count];
            for (int c = cfg.Cols.Fixed; c < cfg.Cols.Count; c++)
            {
                m_ColWidthRate[c] = (cfg.Cols[c].Width / m_GridWidth) * 100;
            }
        }


        //토탈컷 디스플레이
        private void SetTotalCut()
        {
            string shiftCond = " AND 1 = 1";

            if (m_ws.shift != "*")
            {
                shiftCond = string.Format(" AND (PROD.shift = '{0}')", m_ws.shift);
            }

            lbl_totalCut.Text = dcon.GetTotalCut(shiftCond, m_ws.staDate, m_ws.endDate);
        }

        private void btn_home_Click(object sender, EventArgs e)
        {
            mainForm.GoToMain();
        }

        private void cfg_prodList_Resize(object sender, EventArgs e)
        {
            try
            {
                for (int c = cfg_prodList.Cols.Fixed; c < cfg_prodList.Cols.Count; c++)
                {
                    cfg_prodList.Cols[c].Width = Convert.ToInt32((Convert.ToDecimal(cfg_prodList.Width) / 100) * m_ColWidthRate[c]);
                }
            }
            catch { }
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"출력을 하시겠습니까? (저장경로 - C:\EXCEL_FILE)", "출력 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                return;
            }

            if (dt_data.Rows.Count < 1)
            {
                MessageBox.Show("출력 할 DATA가 없습니다.");
                return;
            }

            try
            {
                XLWorkbook workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Production");

                ClosedXML.Excel.IXLRange exlRange;
                exlRange = worksheet.Range(1, 1, 1, 8);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 16;

                exlRange = worksheet.Range(2, 1, 2, 8);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 13;
                exlRange.Row(1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                worksheet.Cell("A1").Value = docTitleStr;
                worksheet.Cell("A2").Value = lbl_title.Text;
                worksheet.Cell("A3").InsertTable(dt_data, "Production", true);

                worksheet.Columns(1, 9).Width = 15;

                string currentTime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

                workbook.SaveAs(string.Format(@"C:\EXCEL_FILE\Production_{0}.xlsx", currentTime));

                string path = @"C:\EXCEL_FILE";
                System.Diagnostics.Process.Start(string.Format(@"C:\EXCEL_FILE\Production_{0}.xlsx", currentTime), path);
                //System.Diagnostics.Process.Start("explorer.exe", path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_pdf_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"출력을 하시겠습니까? (저장경로 - C:\PDF_FILE)", "출력 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                return;
            }

            if (dt_data.Rows.Count < 1)
            {
                MessageBox.Show("출력 할 DATA가 없습니다.");
                return;
            }

            string BatangFont = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\batang.ttc";
            string GulimFont = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\gulim.ttc";
            FontFactory.Register(BatangFont); FontFactory.Register(GulimFont);
            iTextSharp.text.Font HeaderFont = FontFactory.GetFont("굴림체", BaseFont.IDENTITY_H, 8);
            iTextSharp.text.Font TitleFont = FontFactory.GetFont("굴림체", BaseFont.IDENTITY_H, 9);
            iTextSharp.text.Font DataFont = FontFactory.GetFont("굴림체", BaseFont.IDENTITY_H, 7);

            //Creating iTextSharp Table from the DataTable data

            PdfPTable pdfTable = new PdfPTable(dt_data.Columns.Count);

            pdfTable.DefaultCell.Padding = 1;

            pdfTable.WidthPercentage = 100;

            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

            //pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row

            foreach (DataColumn column in dt_data.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, HeaderFont));

                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);

                pdfTable.AddCell(cell);
            }


            //Adding DataRow

            foreach (DataRow row in dt_data.Rows)
            {
                if (dt_data.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_data.Columns.Count; i++)
                    {
                        if (dt_data.Columns[i].ColumnName == "Date")
                        {
                            if (row[i].ToString() == "")
                            {
                                pdfTable.AddCell(new Phrase("Total Count", DataFont));
                            }
                            else
                            {
                                pdfTable.AddCell(new Phrase(row[i].ToString().Substring(0, 10), DataFont));
                            }
                        }
                        else
                        {
                            pdfTable.AddCell(new Phrase(row[i].ToString(), DataFont));
                        }
                    }
                }
            }


            //Exporting to PDF

            const string folderPath = "C:\\PDF_FILE\\";

            if (!Directory.Exists(folderPath))
            {

                Directory.CreateDirectory(folderPath);

            }

            string currentTime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
            using (FileStream stream = new FileStream(string.Format("{0}Production_{1}.pdf", folderPath, currentTime), FileMode.Create))
            {

                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                int[] intTblWidth = new int[dt_data.Columns.Count];
                for (int i = 0; i < dt_data.Columns.Count; i++)
                {
                    intTblWidth[i] = 50;
                }
                pdfTable.SetWidths(intTblWidth);

                PdfWriter.GetInstance(pdfDoc, stream);

                pdfDoc.Open();

                Paragraph para = new Paragraph(docTitleStr + "\n", TitleFont);
                para.SpacingAfter = 10;
                pdfDoc.Add(para);

                pdfDoc.Add(pdfTable);

                pdfDoc.Close();

                stream.Close();
            }

            string path = @"C:\PDF_FILE";
            System.Diagnostics.Process.Start(string.Format("{0}Production_{1}.pdf", folderPath, currentTime), path);
            //System.Diagnostics.Process.Start("explorer.exe", path);
        }

      
        private void pbtn_main_MouseEnter(object sender, EventArgs e)
        {
            pbtn_main.BackgroundImage = Properties.Resources.sub_btn_main_on;
        }

        private void pbtn_main_MouseLeave(object sender, EventArgs e)
        {
            pbtn_main.BackgroundImage = Properties.Resources.sub_btn_main_off;
        }

        private void cfg_prodList_SizeChanged(object sender, EventArgs e)
        {
            //SetExtraStyle_2(cfg_prodList, ref dt_data);
        }
    }
}
