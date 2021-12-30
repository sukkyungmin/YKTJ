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

namespace WasteReport.UserCons
{
    public partial class Con_Sub : UserControl
    {
        MainForm mainForm;
        WasteSearch m_ws;
        MDBmanager dcon = null;

        DataTable dt_table1;
        DataTable dt_table2;
        DataTable dt_table3;

        decimal m_GridWidth = 0;
        decimal[] m_ColWidthRate = null;
        string p_TotalCut = "";
        DataTable dt_data = null;
         

        public Con_Sub(MainForm mf, WasteSearch ws)
        {
            InitializeComponent();

            mainForm = mf;
            m_ws = ws;

            SetBasicInfo();
            dcon = new MDBmanager(ws.machine);
            SetTotalCut();
            SetTopInfo();
            InitGridStyle();
            SetExtraStyle();
            GetGridSize();
            GridComma(cfg_topInfo, "Cuts", "Culls", "CaseCount", "Stops", "Prod/Case", ref dt_data);
            SetData();
        }

        //화면 상단에 기본정보 디스플레이
        private void SetBasicInfo()
        {
            lbl_machine.Text = m_ws.machine;
            lbl_prodDate.Text = string.Format("{0} to {1}", m_ws.staDate, m_ws.endDate);
            lbl_rptDate.Text = DateTime.Now.ToString();
        }

        //초기 그리드 스타일 세팅
        private void InitGridStyle()
        {
            cfg_topInfo.SelectionMode = SelectionModeEnum.Row;
            cfg_topInfo.ExtendLastCol = true;

            cfg_topInfo.Cols[0].Width = 0;

            //if (m_ws.staDate != m_ws.endDate)
            //{
            //    CellStyle cs = cfg_topInfo.Styles.Normal;
            //    cs = cfg_topInfo.Styles.Fixed;
            //    cs.BackColor = Color.FromArgb(235, 234, 232);
            //    cs.ForeColor = Color.FromArgb(96, 96, 96);
            //    cs.Border.Style = BorderStyleEnum.None;
            //    cs.TextAlign = TextAlignEnum.RightCenter;
            //    cs.Font = new Font("Tahoma", 14, FontStyle.Regular);

            //    CellStyle cs2 = cfg_topInfo.Styles.Normal;
            //    cs2.Font = new Font("Tahoma", 14, FontStyle.Regular);
            //    cfg_topInfo.Rows.DefaultSize = 48;
            //}
            //else
            //{
                CellStyle cs = cfg_topInfo.Styles.Normal;
                cs = cfg_topInfo.Styles.Fixed;
                cs.BackColor = Color.FromArgb(235, 234, 232);
                cs.ForeColor = Color.FromArgb(96, 96, 96);
                cs.Border.Style = BorderStyleEnum.None;
                cs.TextAlign = TextAlignEnum.RightCenter;
                cs.Font = new Font("Tahoma", 11, FontStyle.Regular);

                CellStyle cs2 = cfg_topInfo.Styles.Normal;
                cs2.Font = new Font("Tahoma", 11, FontStyle.Regular);
                cfg_topInfo.Rows.DefaultSize = 24;
            //}
        }

        //추가적인 그리드 스타일 세팅
        private void SetExtraStyle()
        {
            if (dt_data.Rows.Count > 0)
            {
                //if (m_ws.staDate != m_ws.endDate)
                //{
                //    cfg_topInfo.Cols[1].Width = 170;
                //    cfg_topInfo.Cols[2].Width = 170;
                //    cfg_topInfo.Cols[3].Width = 170;
                //    cfg_topInfo.Cols[4].Width = 170;
                //    cfg_topInfo.Cols[5].Width = 170;
                //    cfg_topInfo.Cols[6].Width = 170;
                //}
                //else
                //{
                    cfg_topInfo.Cols[1].Width = 150;
                    cfg_topInfo.Cols[2].Width = 150;
                    cfg_topInfo.Cols[3].Width = 150;
                    cfg_topInfo.Cols[4].Width = 150;
                    cfg_topInfo.Cols[5].Width = 150;
                    cfg_topInfo.Cols[6].Width = 150;
                    cfg_topInfo.Cols[7].Width = 150;
                    //cfg_topInfo.Cols[8].Width = 150;

                    CellStyle csCellStyle3 = cfg_topInfo.Styles.Add("CellStyle3");
                    csCellStyle3.TextAlign = TextAlignEnum.CenterCenter;
                    csCellStyle3.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    cfg_topInfo.SetCellStyle(3, 1, csCellStyle3);

                    CellStyle csCellStyle = cfg_topInfo.Styles.Add("CellStyle");
                    csCellStyle.TextAlign = TextAlignEnum.CenterCenter;
                    cfg_topInfo.SetCellStyle(1, 1, csCellStyle);
                    cfg_topInfo.SetCellStyle(2, 1, csCellStyle);

                    CellStyle csCellStyle2 = cfg_topInfo.Styles.Add("CellStyle2");
                    csCellStyle2.ForeColor = Color.Black;
                    csCellStyle2.Font = new Font("Tahoma", 11, FontStyle.Bold);
                    cfg_topInfo.SetCellStyle(3, 2, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 3, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 4, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 5, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 6, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 7, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 8, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 9, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 10, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 11, csCellStyle2);
                    cfg_topInfo.SetCellStyle(3, 12, csCellStyle2);
                //cfg_topInfo.SetCellStyle(3, 9, csCellStyle2);

                if (dt_data.Rows.Count > 0)
                    {
                        cfg_topInfo.Cols[1].TextAlignFixed = TextAlignEnum.CenterCenter;
                    }
                //}
                    cfg_topInfo.Select(0, 0);
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
            p_TotalCut = "";

            if (dt_data.Rows.Count > 0)
            {
                DataRow totalRow = dt_data.NewRow();

                totalRow[1] = dt.Compute("Sum(Cuts)", string.Empty).ToString();
                p_TotalCut = totalRow[1].ToString();
                totalRow[2] = dt.Compute("Sum(Culls)", string.Empty).ToString();
                totalRow[3] = Math.Round(decimal.Parse(dt.Compute("Avg([TW%])", string.Empty).ToString()), 1).ToString();
                totalRow[4] = Math.Round(decimal.Parse(dt.Compute("Avg([CW%])", string.Empty).ToString()), 1).ToString();
                totalRow[5] = Math.Round(decimal.Parse(dt.Compute("Avg([PW%])", string.Empty).ToString()), 1).ToString();
                totalRow[6] = dt.Compute("Sum(CaseCount)", string.Empty).ToString();
                totalRow[7] = dt.Compute("Sum(CCC)", string.Empty).ToString();
                totalRow[8] = Math.Round(decimal.Parse(dt.Compute("Avg([Avg_MDPH])", string.Empty).ToString()), 1).ToString();
                totalRow[9] = Math.Round(decimal.Parse(dt.Compute("Avg([%Down])", string.Empty).ToString()), 1).ToString();
                totalRow[10] = dt.Compute("Sum(Stops)", string.Empty).ToString();
                totalRow[11] = Math.Round(decimal.Parse(dt.Compute("Avg([AvgDPM])", string.Empty).ToString()), 1).ToString();

                dt.Rows.Add(totalRow);
            }
        }


        //그리드 리사이즈시 필요 변수값 가져오기
        public void GetGridSize()
        {
            m_GridWidth = cfg_topInfo.Width;
            m_ColWidthRate = new decimal[cfg_topInfo.Cols.Count];
            for (int c = cfg_topInfo.Cols.Fixed; c < cfg_topInfo.Cols.Count; c++)
            {
                m_ColWidthRate[c] = (cfg_topInfo.Cols[c].Width / m_GridWidth) * 100;
            }
        }

        private void SetTopInfo()
        {
            dt_data = new DataTable();

            if (m_ws.staDate == m_ws.endDate)
            {
                dt_data = dcon.GetDailyTopInfo(m_ws.staDate, m_ws.endDate);
                SetTotalRow(ref dt_data);
                cfg_topInfo.DataSource = dt_data;
            }
            else
            {
                //if (m_ws.shift == "*")
                //{
                dt_data = dcon.GetIntervalTopInfo(m_ws.staDate, m_ws.endDate);
                SetTotalRow(ref dt_data);
                cfg_topInfo.DataSource = dt_data;
                //}
                //else
                //{
                //    dt_data = dcon.GetIntervalTopWithShift(m_ws.staDate, m_ws.endDate, m_ws.shift);
                //    cfg_topInfo.DataSource = dt_data;
                //}
            }
        }

        private void SetData()
        {
            SetTlpRows(true, 0, string.Format("Top {0} Waste by Shift 1", m_ws.wcTop), "1", ref dt_table1);
            SetTlpRows(true, 1, string.Format("Top {0} Waste by Shift 2", m_ws.wcTop), "2", ref dt_table2);
            SetTlpRows(true, 2, string.Format("Top {0} Waste by Day Total", m_ws.wcTop), "",ref dt_table3);
        }

        private void SetTlpRows(bool bl, int row, string title, string shift,ref DataTable dt_table)
        {
            if (bl)
            {
                Con_Data con_data = new Con_Data(m_ws, title, shift, p_TotalCut ,this, ref dt_table);
                tlp_main.Controls.Add(con_data, 0, row);
                con_data.Dock = DockStyle.Fill;
                tlp_main.RowStyles[row].Height = con_data.GetGridHeight();
            }
            else
            {
                tlp_main.RowStyles[row].Height = 0;
            }
        }

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

        private void tlp_main_MouseEnter(object sender, EventArgs e)
        {
            tlp_main.Select();
        }

        private void cfg_topInfo_Resize(object sender, EventArgs e)
        {
            try
            {
                for (int c = cfg_topInfo.Cols.Fixed; c < cfg_topInfo.Cols.Count; c++)
                {
                    cfg_topInfo.Cols[c].Width = Convert.ToInt32((Convert.ToDecimal(cfg_topInfo.Width) / 100) * m_ColWidthRate[c]);
                }
            }
            catch { }
        }

        private void pbtn_main_MouseEnter(object sender, EventArgs e)
        {
            pbtn_main.BackgroundImage = Properties.Resources.sub_btn_main_on;
        }

        private void pbtn_main_MouseLeave(object sender, EventArgs e)
        {
            pbtn_main.BackgroundImage = Properties.Resources.sub_btn_main_off;
        }

        private void pbtn_excel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"출력을 하시겠습니까? (저장경로 - C:\EXCEL_FILE)", "출력 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                return;
            }

            if(dt_data == null && dt_table1 == null && dt_table2 == null)
            {
                    MessageBox.Show("출력 할 DATA가 없습니다.");
                    return;
            }

            //if (dt_table1.Rows.Count < 1)
            //{
            //    MessageBox.Show("출력 할 DATA가 없습니다.");
            //    return;
            //}

            try
            {
                int WasteCode = dt_data.Rows.Count - 1;
                int WasteGroup = WasteCode + (dt_table1.Rows.Count - 1);
                int WasteDrilldown = WasteGroup + (dt_table2.Rows.Count - 1);


                //WasteDrilldown = Convert.ToInt16(m_ws.wcTop);

                XLWorkbook workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Waste");

                ClosedXML.Excel.IXLRange exlRange = worksheet.Range(1, 1, 1, 7);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 16;

                exlRange = worksheet.Range(2, 1, 2, 8);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 13;
                exlRange.Row(1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                exlRange = worksheet.Range(WasteCode + 6, 1, WasteCode + 6, 7);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 13;
                exlRange.Row(1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                exlRange = worksheet.Range((WasteGroup + 10), 1, (WasteGroup + 10), 7);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 13;
                exlRange.Row(1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                exlRange = worksheet.Range((WasteDrilldown + 14), 1, (WasteDrilldown + 14), 8);
                exlRange.Merge();
                exlRange.Row(1).Style.Font.FontSize = 13;
                exlRange.Row(1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                worksheet.Cell("A1").Value = string.Format("Machine: {0}      Production Date : {1} ~ {2}", m_ws.machine, m_ws.staDate, m_ws.endDate);

                worksheet.Cell("A2").Value = " Production";
                worksheet.Cell("A3").InsertTable(dt_data, "Production", true);

                worksheet.Cell("A" + (WasteCode + 6).ToString()).Value = string.Format("Top {0}  Waste by Waste Shift 1", m_ws.wcTop);
                worksheet.Cell("A" + (WasteCode+ 7).ToString()).InsertTable(dt_table1, "Waste", true);

                worksheet.Cell("A" + (WasteGroup +10).ToString()).Value = string.Format("Top {0}  Waste by Waste Shift 2", m_ws.wcTop);
                worksheet.Cell("A" + (WasteGroup +11).ToString()).InsertTable(dt_table2, "Waste2", true);

                worksheet.Cell("A" + (WasteDrilldown + 14).ToString()).Value = string.Format("Top {0}  Waste by Sum", m_ws.wcTop);
                worksheet.Cell("A" + (WasteDrilldown + 15).ToString()).InsertTable(dt_table3, "Waste3", true);

                //worksheet.Cell("A" + ((WasteGroup * 3) + 1).ToString()).Value = "Top " + m_ws.wcTop + "Waste Group Drilldown";
                //worksheet.Cell("A3").InsertTable(dt_table3, "Waste", true);



                //if (m_title.Contains("Group Drilldown"))
                //{
                //    worksheet.Columns(1, 1).Width = 18;
                //    worksheet.Columns(2, 2).Width = 50;
                //    worksheet.Columns(3, 8).Width = 12;
                //}
                //else
                //{
                worksheet.Columns(1, 1).Width = 50;
                worksheet.Columns(2, 7).Width = 12;
                //}

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
    }
}
