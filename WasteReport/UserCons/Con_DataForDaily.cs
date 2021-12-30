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
    public partial class Con_DataForDaily : UserControl
    {
        WasteSearch m_ws;
        Con_Daily m_cs;
        MDBmanager dcon = null;
        string m_title = "";
        decimal totalCut = 0;

        DataTable dt_data = new DataTable();

        decimal m_GridWidth = 0;
        decimal[] m_ColWidthRate = null;

        string docTitleStr = "";

        public Con_DataForDaily(WasteSearch ws, string title, Con_Daily cs)
        {
            InitializeComponent();

            m_ws = ws;
            m_cs = cs;
            lbl_title.Text = title;
            m_title = title;
            if (m_cs.lbl_totalCut.Text == "")
            {
                totalCut = 0;
            }
            else
            {
                totalCut = decimal.Parse(m_cs.lbl_totalCut.Text);
            }

            dcon = new MDBmanager(ws.machine);

            cfg_data.Cols.Frozen = int.Parse("1");

            GetData();
            
            //cfg_data.Cols[1].Frozen

            InitGridStyle();
            GetGridSize();

            docTitleStr = "Machine: " + m_ws.machine + "      Production Date : " + m_ws.staDate + " ~ " + m_ws.endDate
                                     + Environment.NewLine + m_title + "                                          ";
        }

        //초기 그리드 스타일 세팅
        private void InitGridStyle()
        {
            cfg_data.AllowSorting = AllowSortingEnum.None;
            cfg_data.SelectionMode = SelectionModeEnum.Row;
            cfg_data.ExtendLastCol = true;
            if (!m_title.Contains("Group Drilldown"))
            {
                cfg_data.Cols[0].Width = 0;
            }
            else
            {
                cfg_data.Cols[0].Width = 20;
            }

                CellStyle cs = cfg_data.Styles.Normal;
                cs = cfg_data.Styles.Fixed;
                cs.BackColor = Color.FromArgb(235, 234, 232);
                cs.ForeColor = Color.FromArgb(96, 96, 96);
                cs.Border.Style = BorderStyleEnum.None;
                cs.TextAlign = TextAlignEnum.RightCenter;

                if (dt_data.Rows.Count > 0)
                {
                    cfg_data.Cols[1].TextAlignFixed = TextAlignEnum.LeftCenter;
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
                cfg_data.Cols[c].TextAlignFixed = TextAlignEnum.LeftCenter;
            }
        }

        //DB컨트롤 객체의 함수를 호출해서 그리드에 들어갈 데이타소스를 넣어주기
        private void GetData()
        {
            string wgTopCond = m_ws.wgTop;

            if (m_ws.wgTop == "*")
            {
                wgTopCond = "9999999";
            }

            dt_data.Clear();
            string groupCond = " AND 1 = 1";
            if (m_ws.group != "*")
            {
                groupCond = " AND (W_CODE.WasteGroup = '" + m_ws.group + "')";
            }

            if (m_title.Contains("Group Drilldown"))
            {
                if (m_ws.shift == "*")
                {
                    dt_data = dcon.GetDailyWgDdown(wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
                }
                else
                {
                    dt_data = dcon.GetDailyWgDownWithShift(m_ws.shift, wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
                }

                

                //SetTotalRow(ref dt_data);
                cfg_data.DataSource = dtPivot(dt_data);
                //cfg_data.DataSource = dt_data;

                //GroupBy(cfg_data, "Date Stamp", 0);

                //cfg_data.Cols["Date Stamp"].Width = 0;

                //cfg_data.AllowMerging = AllowMergingEnum.Nodes;

                //AddSubtotals(cfg_data, 0, "Occ");
                //AddSubtotals(cfg_data, 0, "Def");

                //cfg_data.Tree.Column = 0;
                //cfg_data.AutoSizeCol(cfg_data.Tree.Column);
                //cfg_data.Tree.Show(0);

                cfg_data.AutoSizeCols();
                //SetExtraStyle();
            }

            cfg_data.Select(0, 0);
        }


        private DataTable dtPivot(DataTable dt)
        {
            PivotTbl test = new PivotTbl();
            DataTable dst = null;
            dst = test.Pivot(dt, "Waste Code","Date Stamp", "Occ", "Def");
            return dst;
            
        }

        //Datatable 토탈 로우 추가 해주기
        private void SetTotalRow(ref DataTable dt)
        {
            if (dt_data.Rows.Count > 0)
            {
                DataRow totalRow = dt_data.NewRow();

                if (m_title.Contains("Group Drilldown"))
                {
                    //totalRow[0] = "Grand Totals :";
                    totalRow[2] = dt_data.Compute("Sum(Occ)", string.Empty).ToString();
                    totalRow[3] = dt_data.Compute("Sum(Def)", string.Empty).ToString();

                    if (totalCut == 0)
                    {
                        //totalRow[4] = "0";
                        //totalRow[6] = "0";
                    }
                    //else
                    //{
                    //    totalRow[4] = Math.Round(((decimal.Parse(totalRow[3].ToString()) / totalCut) * 100), 2).ToString();
                    //    totalRow[6] = Math.Round((decimal.Parse(totalRow[3].ToString()) / totalCut) * 10000, 1).ToString();
                    //}

                    //if (decimal.Parse(totalRow[2].ToString()) == 0)
                    //{
                    //    totalRow[5] = "0";
                    //}
                    //else
                    //{
                    //    totalRow[5] = Math.Round(decimal.Parse(totalRow[3].ToString()) / decimal.Parse(totalRow[2].ToString()), 1).ToString();
                    //}

                    //if (decimal.Parse(totalRow[6].ToString()) == 0)
                    //{
                    //    totalRow[7] = "0";
                    //}
                    //else
                    //{
                    //    totalRow[7] = Math.Round(decimal.Parse(totalRow[5].ToString()) / decimal.Parse(totalRow[6].ToString()), 2).ToString();
                    //}

                    dt.Rows.InsertAt(totalRow, 0);
                }
                else
                {
                    totalRow[0] = "Totals :";
                    totalRow[1] = dt_data.Compute("Sum(Occ)", string.Empty).ToString();
                    totalRow[2] = dt_data.Compute("Sum(Def)", string.Empty).ToString();
                    //totalRow[3] = dt_data.Compute("Sum([%Waste])", string.Empty).ToString();
                    //totalRow[4] = dt_data.Compute("Sum([Def/Occ])", string.Empty).ToString();
                    //totalRow[5] = dt_data.Compute("Sum([Def/Cut])", string.Empty).ToString();
                    //totalRow[6] = dt_data.Compute("Sum([Cut/Occ])", string.Empty).ToString();

                    if (totalCut == 0)
                    {
                        totalRow[3] = "0";
                        totalRow[5] = "0";
                    }
                    else
                    {
                        totalRow[3] = Math.Round(((decimal.Parse(totalRow[2].ToString()) / totalCut) * 100), 2).ToString();
                        totalRow[5] = Math.Round((decimal.Parse(totalRow[2].ToString()) / totalCut) * 10000, 1).ToString();
                    }

                    if (decimal.Parse(totalRow[1].ToString()) == 0)
                    {
                        totalRow[4] = "0";
                    }
                    else
                    {
                        totalRow[4] = Math.Round(decimal.Parse(totalRow[2].ToString()) / decimal.Parse(totalRow[1].ToString()), 1).ToString();
                    }

                    if (decimal.Parse(totalRow[5].ToString()) == 0)
                    {
                        totalRow[6] = "0";
                    }
                    else
                    {
                        totalRow[6] = Math.Round(decimal.Parse(totalRow[4].ToString()) / decimal.Parse(totalRow[5].ToString()), 2).ToString();
                    }

                    dt.Rows.InsertAt(totalRow, 0);
                }
            }
        }

        //추가적인 그리드 스타일 세팅
        private void SetExtraStyle()
        {
            if (dt_data.Rows.Count > 0)
            {
                if (m_title.Contains("Group Drilldown"))
                {
                    cfg_data.Cols[1].Width = 150;
                    cfg_data.Cols[2].Width = 280;
                    cfg_data.Cols[3].Width = 130;
                    cfg_data.Cols[4].Width = 130;
                    //cfg_data.Cols[5].Width = 130;
                    //cfg_data.Cols[6].Width = 130;
                    //cfg_data.Cols[7].Width = 130;

                    CellStyle csCellStyle = cfg_data.Styles.Add("CellStyle");
                    csCellStyle.ForeColor = Color.Red;
                    csCellStyle.Font = new System.Drawing.Font(Font, FontStyle.Bold);
                    //csCellStyle.TextAlign = TextAlignEnum.RightCenter;
                    csCellStyle.TextAlign = TextAlignEnum.LeftCenter;
                    cfg_data.SetCellStyle(1, 1, csCellStyle);

                    CellStyle csCellStyle2 = cfg_data.Styles.Add("CellStyle2");
                    csCellStyle2.ForeColor = Color.Black;
                    csCellStyle2.Font = new System.Drawing.Font(Font, FontStyle.Bold | FontStyle.Underline);
                    cfg_data.SetCellStyle(1, 3, csCellStyle2);
                    cfg_data.SetCellStyle(1, 4, csCellStyle2);
                    //cfg_data.SetCellStyle(1, 5, csCellStyle2);
                    //cfg_data.SetCellStyle(1, 6, csCellStyle2);
                    //cfg_data.SetCellStyle(1, 7, csCellStyle2);
                    //cfg_data.SetCellStyle(1, 8, csCellStyle2);
                }
                
            }
        }

        private void GetTotalCut()
        {
            string shiftCond = " AND 1 = 1";

            if (m_ws.shift != "*")
            {
                shiftCond = " AND (PROD.shift = '" + m_ws.shift + "')";
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
                    //if (!object.Equals(value, current) && (string)value != "Grand Totals :")
                    if (!object.Equals(value, current))
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

                        //switch (colName)
                        //{
                        //    case "%Waste":
                        //        if (totalCut == 0)
                        //        {
                        //            _flex.Rows[r][5] = "0";
                        //        }
                        //        else
                        //        {
                        //            _flex.Rows[r][5] = Math.Round(((decimal.Parse(_flex.Rows[r][4].ToString()) / totalCut) * 100), 2).ToString();
                        //        }
                        //        break;
                        //    case "Def/Occ":
                        //        if (decimal.Parse(_flex.Rows[r][3].ToString()) == 0)
                        //        {
                        //            _flex.Rows[r][6] = "0";
                        //        }
                        //        else
                        //        {
                        //            _flex.Rows[r][6] = Math.Round(decimal.Parse(_flex.Rows[r][4].ToString()) / decimal.Parse(_flex.Rows[r][3].ToString()), 1).ToString();
                        //        }
                        //        break;
                        //    case "Def/Cut":
                        //        if (totalCut == 0)
                        //        {
                        //            _flex.Rows[r][7] = "0";
                        //        }
                        //        else
                        //        {
                        //            _flex.Rows[r][7] = Math.Round((decimal.Parse(_flex.Rows[r][4].ToString()) / totalCut) * 10000, 1).ToString();
                        //        }
                        //        break;
                        //    case "Cut/Occ":
                        //        if (decimal.Parse(_flex.Rows[r][7].ToString()) == 0)
                        //        {
                        //            _flex.Rows[r][8] = "0";
                        //        }
                        //        else
                        //        {
                        //            _flex.Rows[r][8] = Math.Round(decimal.Parse(_flex.Rows[r][6].ToString()) / decimal.Parse(_flex.Rows[r][7].ToString()), 2).ToString();
                        //        }
                        //        break;
                        //}

                    }
                }
            }
        }


        //다른 폼에서 그리드 크기를 가져갈수 있게 하는 메쏘드
        //(이 콘트롤을 띄워 줄 패널의 로우사이즈를 결정하기위해)
        public int GetGridHeight()
        {
            //return (cfg_data.Rows.Count * 20) + 56;
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

                ClosedXML.Excel.IXLRange exlRange;
                exlRange = worksheet.Range(1, 1, 1, 7);
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

                workbook.SaveAs(@"C:\EXCEL_FILE\Waste_" + currentTime + ".xlsx");

                string path = @"C:\EXCEL_FILE";
                System.Diagnostics.Process.Start(@"C:\EXCEL_FILE\Waste_" + currentTime + ".xlsx", path);
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

            string folderPath = "C:\\PDF_FILE\\";

            if (!Directory.Exists(folderPath))
            {

                Directory.CreateDirectory(folderPath);

            }

            string currentTime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
            using (FileStream stream = new FileStream(folderPath + "Waste_" + currentTime + ".pdf", FileMode.Create))
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
            System.Diagnostics.Process.Start(folderPath + "Waste_" + currentTime + ".pdf", path);
            //System.Diagnostics.Process.Start("explorer.exe", path);
        }
    }
}
