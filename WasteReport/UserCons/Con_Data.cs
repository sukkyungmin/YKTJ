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
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WasteReport.UserCons
{
    public partial class Con_Data : UserControl
    {
        WasteSearch m_ws;
        Con_Sub m_cs;
        MDBmanager dcon = null;
        string m_title = "";
        string p_totalcut = "";
        string m_shift = "";
        decimal totalCut = 0;

        DataTable dt_data = new DataTable();

        decimal m_GridWidth = 0;
        decimal[] m_ColWidthRate = null;

        string docTitleStr = "";

        public Con_Data(WasteSearch ws, string title, string shift, string totalcut,Con_Sub cs, ref  DataTable dt_data1)
        {
            InitializeComponent();
             
            m_ws = ws;
            m_cs = cs;
            lbl_title.Text = title;
            m_title = title;
            p_totalcut = totalcut;
            m_shift = shift;
            if (m_cs.lbl_totalCut.Text == "")
            {
                totalCut = 0;
            }
            else
            {
                totalCut = decimal.Parse(m_cs.lbl_totalCut.Text);
            }

            dcon = new MDBmanager(ws.machine);

            GetData();
            InitGridStyle();
            //cfg_data.AutoSizeCols();
            GetGridSize();

            docTitleStr = "Machine: " + m_ws.machine + "      Production Date : " + m_ws.staDate + " ~ " + m_ws.endDate 
                                     + Environment.NewLine + m_title + "                                          ";

            dt_data1 = dt_data;
        }

        //초기 그리드 스타일 세팅
        private void InitGridStyle()
        {
            cfg_data.AllowSorting = AllowSortingEnum.None;
            cfg_data.SelectionMode = SelectionModeEnum.Row;
            cfg_data.ExtendLastCol = true;

                cfg_data.Cols[0].Width = 0;


                CellStyle cs = cfg_data.Styles.Normal;
                cs = cfg_data.Styles.Fixed;
                cs.BackColor = Color.FromArgb(235, 234, 232);
                cs.ForeColor = Color.FromArgb(96, 96, 96);
                cs.Border.Style = BorderStyleEnum.None;
                cs.TextAlign = TextAlignEnum.RightCenter;

                if (dt_data.Rows.Count > 0)
                {
                        for (int i = 1; i < 3; i++)
                        {
                            cfg_data.Cols[i].TextAlignFixed = TextAlignEnum.CenterCenter;
                            cfg_data.Cols[i].TextAlign = TextAlignEnum.CenterCenter;

                                if (cfg_data.Cols[i].Name == "Description")
                                {
                                    cfg_data.Cols[i].TextAlign = TextAlignEnum.LeftCenter;
                                }

                        }
                 }
        }

        //그리드 리사이즈시 필요 변수값 가져오기
        public void GetGridSize()
        {
            m_GridWidth = cfg_data.Width;
            m_ColWidthRate = new decimal[cfg_data.Cols.Count];
            for (int c = cfg_data.Cols.Fixed; c < cfg_data.Cols.Count; c++)
            {
                m_ColWidthRate[c] = (cfg_data.Cols[c].Width / m_GridWidth) * 100;
            }
        }

        //DB컨트롤 객체의 함수를 호출해서 그리드에 들어갈 데이타소스를 넣어주기
        private void GetData()
        {
            string wcTopCond = m_ws.wcTop;
            //string wgTopCond = m_ws.wgTop;

            if (m_ws.wcTop == "*")
            {
                wcTopCond = "9999999";
            }

            dt_data.Clear();
            string groupCond = " AND 1 = 1";
            //if (m_ws.group != "*")
            //{
            //    groupCond = string.Format(" AND (W_CODE.WasteGroup = '{0}')", m_ws.group);
            //}

            if (m_title.Contains("by Shift"))
            {

                dt_data = dcon.GetByWstCdOnlyShift(m_shift, wcTopCond, p_totalcut, m_ws.staDate, m_ws.endDate, groupCond);


                SetTotalRow(ref dt_data);
                cfg_data.DataSource = dt_data;
                SetExtraStyle();
                GridComma(cfg_data, "OCCR", "PROD", "", "", "", ref dt_data);
            }
            else
            {
                dt_data = dcon.GetByWstCdOnlySum(m_ws.shift, wcTopCond, p_totalcut, m_ws.staDate, m_ws.endDate, groupCond);


                SetTotalRow(ref dt_data);
                cfg_data.DataSource = dt_data;
                SetExtraStyle();
                GridComma(cfg_data, "OCCR", "PROD", "", "", "", ref dt_data);
            }
            //else if (m_title.Contains("by WasteGroup"))
            //{
            //    if (m_ws.shift == "*")
            //    {
            //        dt_data = dcon.GetByWstGp(wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            //    }
            //    else
            //    {
            //        dt_data = dcon.GetByWstGpWithShift(m_ws.shift, wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            //    }

            //    SetTotalRow(ref dt_data);
            //    cfg_data.DataSource = dt_data;
            //    SetExtraStyle();
            //}
            //else if (m_title.Contains("Group Drilldown"))
            //{
            //    if (m_ws.shift == "*")
            //    {
            //        dt_data = dcon.GetWgDdown(wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            //    }
            //    else
            //    {
            //        dt_data = dcon.GetWgDdownWithShift(m_ws.shift, wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            //    }

            //    SetTotalRow(ref dt_data);
            //    cfg_data.DataSource = dt_data;
            //    GroupBy(cfg_data, "Waste Group", 0);
            //    //GroupBy(cfg_data, "Waste Code", 1);

            //    cfg_data.Cols["Waste Group"].Width = 0;

            //    cfg_data.AllowMerging = AllowMergingEnum.Nodes;

            //    AddSubtotals(cfg_data, 0, "Occ");
            //    AddSubtotals(cfg_data, 0, "Def");
            //    AddSubtotals(cfg_data, 0, "%Waste");
            //    AddSubtotals(cfg_data, 0, "Def/Occ");
            //    AddSubtotals(cfg_data, 0, "Def/Cut");
            //    AddSubtotals(cfg_data, 0, "Cut/Occ");

            //    cfg_data.Tree.Column = 0;
            //    cfg_data.AutoSizeCol(cfg_data.Tree.Column);
            //    cfg_data.Tree.Show(0);

            //    //cfg_data.Subtotal(AggregateEnum.Sum, -1, -1, 3, "");
            //    //cfg_data.Subtotal(AggregateEnum.Sum, -1, -1, 4, "");
            //    //cfg_data.Subtotal(AggregateEnum.Sum, -1, -1, 5, "");
            //    //cfg_data.Subtotal(AggregateEnum.Sum, -1, -1, 6, "");
            //    //cfg_data.Subtotal(AggregateEnum.Sum, -1, -1, 7, "");
            //    //cfg_data.Subtotal(AggregateEnum.Sum, -1, -1, 8, "");

            //    //cfg_data.Rows[1][1] = "Grand Totals :";

            //    cfg_data.AutoSizeCols();
            //    SetExtraStyle();
            //}

            cfg_data.Select(0, 0);
        }

        //Datatable 토탈 로우 추가 해주기
        private void SetTotalRow(ref DataTable dt)
        {
            if (dt_data.Rows.Count > 0)
            {
                DataRow totalRow = dt_data.NewRow();

                    totalRow[1] = "Totals :";
                    totalRow[2] = dt_data.Compute("Sum(OCCR)", string.Empty).ToString();
                    totalRow[3] = dt_data.Compute("Sum(PROD)", string.Empty).ToString();
                    totalRow[4] = Math.Round(decimal.Parse(dt_data.Compute("Sum(PROD)", string.Empty).ToString()) / 
                                             decimal.Parse(dt_data.Compute("Sum(OCCR)", string.Empty).ToString()), 1).ToString();
                //totalRow[4] = dt_data.Compute("Sum([Def/Occ])", string.Empty).ToString();
                //totalRow[5] = dt_data.Compute("Sum([Def/Cut])", string.Empty).ToString();
                //totalRow[6] = dt_data.Compute("Sum([Cut/Occ])", string.Empty).ToString();

                dt.Rows.InsertAt(totalRow, 0);
                
            }
        }

        //추가적인 그리드 스타일 세팅
        private void SetExtraStyle()
        {
            if (dt_data.Rows.Count > 0)
            {
                    cfg_data.Cols[1].Width = 100;
                    cfg_data.Cols[2].Width = 700;
                    cfg_data.Cols[3].Width = (cfg_data.Size.Width - 810) / 3;
                    cfg_data.Cols[4].Width = (cfg_data.Size.Width - 810) / 3;
                    cfg_data.Cols[5].Width = (cfg_data.Size.Width - 810) / 3;

                    CellStyle csCellStyle = cfg_data.Styles.Add("CellStyle");
                    csCellStyle.ForeColor = Color.Red;
                    csCellStyle.Font = new System.Drawing.Font(Font, FontStyle.Bold);
                    csCellStyle.TextAlign = TextAlignEnum.RightCenter;
                    cfg_data.SetCellStyle(1, 2, csCellStyle);

                    CellStyle csCellStyle2 = cfg_data.Styles.Add("CellStyle2");
                    csCellStyle2.ForeColor = Color.Black;
                    csCellStyle2.Font = new System.Drawing.Font(Font, FontStyle.Bold);
                    cfg_data.SetCellStyle(1, 3, csCellStyle2);
                    cfg_data.SetCellStyle(1, 4, csCellStyle2);
                    cfg_data.SetCellStyle(1, 5, csCellStyle2);
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

        private void GetTotalCut()
        {
            string shiftCond = " AND 1 = 1";

            if (m_ws.shift != "*")
            {
                shiftCond = string.Format(" AND (PROD.shift = '{0}')", m_ws.shift);
            }

            totalCut = int.Parse(dcon.GetTotalCut(shiftCond, m_ws.staDate, m_ws.endDate));
        }

        private void GroupBy(C1FlexGrid _flex, string columnName, int level)
        {
            object current = null;
            for (int r = _flex.Rows.Fixed; r < _flex.Rows.Count; r++)
            {
                if (!_flex.Rows[r].IsNode)
                {
                    var value = _flex[r, columnName];
                    if (!object.Equals(value, current) && (string)value != "Grand Totals :")
                    {
                        // value changed: insert node
                        _flex.Rows.InsertNode(r, level);

                        // show group name in first scrollable column
                        _flex[r, _flex.Cols.Fixed] = value;

                        // update current value
                        current = value;
                    }
                }
            }
        }

        private void AddSubtotals(C1FlexGrid _flex, int level, string colName)
        {
            // get column we are going to total on
            int colIndex = _flex.Cols.IndexOf(colName);

            // scan rows looking for nodes at the right level
            for (int r = _flex.Rows.Fixed; r < _flex.Rows.Count; r++)
            {
                if (_flex.Rows[r].IsNode)
                {
                    var node = _flex.Rows[r].Node;
                    if (node.Level == level)
                    {
                        // found a node, calculate the sum of extended price
                        var range = node.GetCellRange();
                        var sum = _flex.Aggregate(AggregateEnum.Sum,
                            range.r1, colIndex, range.r2, colIndex,
                            AggregateFlags.ExcludeNodes);

                        // show the sum on the grid 
                        // (will use the column format automatically)
                        _flex[r, colIndex] = sum;

                        switch (colName)
                        {
                            case "%Waste":
                                if (totalCut == 0)
                                {
                                    _flex.Rows[r][5] = "0";
                                }
                                else
                                {
                                    _flex.Rows[r][5] = Math.Round(((decimal.Parse(_flex.Rows[r][4].ToString()) / totalCut) * 100), 2).ToString();
                                }
                                break;
                            case "Def/Occ":
                                if (decimal.Parse(_flex.Rows[r][3].ToString()) == 0)
                                {
                                    _flex.Rows[r][6] = "0";
                                }
                                else
                                {
                                    _flex.Rows[r][6] = Math.Round(decimal.Parse(_flex.Rows[r][4].ToString()) / decimal.Parse(_flex.Rows[r][3].ToString()), 1).ToString();
                                }
                                break;
                            case "Def/Cut":
                                if (totalCut == 0)
                                {
                                    _flex.Rows[r][7] = "0";
                                }
                                else
                                {
                                    _flex.Rows[r][7] = Math.Round((decimal.Parse(_flex.Rows[r][4].ToString()) / totalCut) * 10000, 1).ToString();
                                }
                                break;
                            case "Cut/Occ":
                                if (decimal.Parse(_flex.Rows[r][7].ToString()) == 0)
                                {
                                    _flex.Rows[r][8] = "0";
                                }
                                else
                                {
                                    _flex.Rows[r][8] = Math.Round(decimal.Parse(_flex.Rows[r][6].ToString()) / decimal.Parse(_flex.Rows[r][7].ToString()), 2).ToString();
                                }
                                break;
                        }

                    }
                }
            }
        }


        //다른 폼에서 그리드 크기를 가져갈수 있게 하는 메쏘드
        //(이 콘트롤을 띄워 줄 패널의 로우사이즈를 결정하기위해)
        public int GetGridHeight()
        {
            return (cfg_data.Rows.Count * 20) + 100;
        }

        //그리드 리사이즈시 컬럼 크기 화면에 맞게 재설정
        private void cfg_data_Resize(object sender, EventArgs e)
        {
            try
            {
                for (int c = cfg_data.Cols.Fixed; c < cfg_data.Cols.Count; c++)
                {
                    cfg_data.Cols[c].Width = Convert.ToInt32((Convert.ToDecimal(cfg_data.Width) / 100) * m_ColWidthRate[c]);
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
                var worksheet = workbook.Worksheets.Add("Waste");

                ClosedXML.Excel.IXLRange exlRange = worksheet.Range(1, 1, 1, 7);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 16;

                exlRange = worksheet.Range(2, 1, 2, 7);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 13;
                exlRange.Row(1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                worksheet.Cell("A1").Value = docTitleStr;
                worksheet.Cell("A2").Value = m_title;
                worksheet.Cell("A3").InsertTable(dt_data, "Waste", true);

              

                if (m_title.Contains("Group Drilldown"))
                {
                    worksheet.Columns(1, 1).Width = 18;
                    worksheet.Columns(2, 2).Width = 50;
                    worksheet.Columns(3, 8).Width = 12;
                }
                else
                {
                    worksheet.Columns(1, 1).Width = 50;
                    worksheet.Columns(2, 7).Width = 12;
                }

                string currentTime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

                workbook.SaveAs(string.Format(@"C:\EXCEL_FILE\Waste_{0}.xlsx", currentTime));

                string path = @"C:\EXCEL_FILE";
                System.Diagnostics.Process.Start(string.Format(@"C:\EXCEL_FILE\Waste_{0}.xlsx", currentTime), path);
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
                        if (dt_data.Columns[i].ColumnName == "DateStamp")
                        {
                            pdfTable.AddCell(new Phrase(row[i].ToString().Substring(0, 10), DataFont));
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
            using (FileStream stream = new FileStream(string.Format("{0}Waste_{1}.pdf", folderPath, currentTime), FileMode.Create))
            {

                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                int[] intTblWidth = new int[dt_data.Columns.Count];
                for (int i = 0; i < dt_data.Columns.Count; i++)
                {
                    switch (dt_data.Columns[i].ColumnName)
                    {
                        case "Waste Code":
                            intTblWidth[i] = 200;
                            break;
                        case "Waste Group":
                            intTblWidth[i] = 90;
                            break;
                        default:
                            intTblWidth[i] = 40;
                            break;
                    }
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
            System.Diagnostics.Process.Start(string.Format("{0}Waste_{1}.pdf", folderPath, currentTime), path);
            //System.Diagnostics.Process.Start("explorer.exe", path);
        }

        private void cfg_data_SizeChanged(object sender, EventArgs e)
        {
            SetExtraStyle();
        }
    }
}
