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

namespace WasteReport.UserCons
{
    public partial class Con_Daily : UserControl
    {
        MainForm mainForm;
        WasteSearch m_ws;
        MDBmanager dcon = null;

        decimal m_GridWidth = 0;
        decimal[] m_ColWidthRate = null;
        DataTable dt_data = null;

        public Con_Daily(MainForm mf, WasteSearch ws)
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
            SetData();
        }

        //화면 상단에 기본정보 디스플레이
        private void SetBasicInfo()
        {
            lbl_machine.Text = m_ws.machine;
            lbl_prodDate.Text = m_ws.staDate + " to " + m_ws.endDate;
            lbl_rptDate.Text = DateTime.Now.ToString();
        }

        //초기 그리드 스타일 세팅
        private void InitGridStyle()
        {
            cfg_topInfo.SelectionMode = SelectionModeEnum.Row;
            cfg_topInfo.ExtendLastCol = true;

            cfg_topInfo.Cols[0].Width = 0;

            if (m_ws.staDate != m_ws.endDate)
            {
                CellStyle cs = cfg_topInfo.Styles.Normal;
                cs = cfg_topInfo.Styles.Fixed;
                cs.BackColor = Color.FromArgb(235, 234, 232);
                cs.ForeColor = Color.FromArgb(96, 96, 96);
                cs.Border.Style = BorderStyleEnum.None;
                cs.TextAlign = TextAlignEnum.RightCenter;
                cs.Font = new Font("Tahoma", 14, FontStyle.Regular);

                CellStyle cs2 = cfg_topInfo.Styles.Normal;
                cs2.Font = new Font("Tahoma", 14, FontStyle.Regular);
                cfg_topInfo.Rows.DefaultSize = 48;
            }
            else
            {
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
            }
        }

        //추가적인 그리드 스타일 세팅
        private void SetExtraStyle()
        {
            if (dt_data.Rows.Count > 0)
            {
                if (m_ws.staDate != m_ws.endDate)
                {
                    cfg_topInfo.Cols[1].Width = 170;
                    cfg_topInfo.Cols[2].Width = 170;
                    cfg_topInfo.Cols[3].Width = 170;
                    cfg_topInfo.Cols[4].Width = 170;
                    cfg_topInfo.Cols[5].Width = 170;
                    cfg_topInfo.Cols[6].Width = 170;
                    //cfg_topInfo.Cols[7].Width = 170;

                    //CellStyle csCellStyle = cfg_topInfo.Styles.Add("CellStyle");
                    //csCellStyle.ForeColor = Color.Red;
                    //csCellStyle.Font = new System.Drawing.Font(Font, FontStyle.Bold);
                    //csCellStyle.TextAlign = TextAlignEnum.RightCenter;
                    //cfg_topInfo.SetCellStyle(1, 1, csCellStyle);
                }
                else
                {
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
                    //cfg_topInfo.SetCellStyle(3, 9, csCellStyle2);

                    if (dt_data.Rows.Count > 0)
                    {
                        cfg_topInfo.Cols[1].TextAlignFixed = TextAlignEnum.CenterCenter;
                    }
                }
                cfg_topInfo.Select(0, 0);
            }
        }

        //Datatable 토탈 로우 추가 해주기
        private void SetTotalRow(ref DataTable dt)
        {
            if (dt_data.Rows.Count > 0)
            {
                DataRow totalRow = dt_data.NewRow();

                totalRow[0] = "Total/Avg :";
                totalRow[1] = dt_data.Compute("Sum(Cuts)", string.Empty).ToString();
                totalRow[2] = dt_data.Compute("Sum(Culls)", string.Empty).ToString();
                //totalRow[3] = dt_data.Compute("Sum(Def)", string.Empty).ToString();

                totalRow[3] = Math.Round((decimal.Parse(dt_data.Rows[0][3].ToString()) + decimal.Parse(dt_data.Rows[1][3].ToString())) / 2, 2).ToString();
                totalRow[4] = dt_data.Compute("Sum([Case Count])", string.Empty).ToString();
                totalRow[5] = Math.Round((decimal.Parse(dt_data.Rows[0][5].ToString()) + decimal.Parse(dt_data.Rows[1][5].ToString())) / 2, 1).ToString();
                totalRow[6] = dt_data.Compute("Sum(Stops)", string.Empty).ToString();
                totalRow[7] = Math.Round((decimal.Parse(dt_data.Rows[0][7].ToString()) + decimal.Parse(dt_data.Rows[1][7].ToString())) / 2, 1).ToString();

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
                if (m_ws.shift == "*")
                {
                    dt_data = dcon.GetIntervalTopInfo(m_ws.staDate, m_ws.endDate);
                    cfg_topInfo.DataSource = dt_data;
                }
                else
                {
                    dt_data = dcon.GetIntervalTopWithShift(m_ws.staDate, m_ws.endDate, m_ws.shift);
                    cfg_topInfo.DataSource = dt_data;
                }
            }
        }

        private void SetData()
        {
            SetTlpRows(true, 0, "Waste Group Drilldown");
            //SetTlpRows(m_ws.bl_wasteCd, 0, "Top " + m_ws.wcTop + " Waste by WasteCode");
            //SetTlpRows(m_ws.bl_wasteGp, 1, "Top " + m_ws.wgTop + " Waste by WasteGroup");
            //SetTlpRows(m_ws.bl_wgDdown, 2, "Waste Group Drilldown");
        }

        private void SetTlpRows(bool bl, int row, string title)
        {
            if(bl)
            {
                Con_DataForDaily con_datafordaily = new Con_DataForDaily(m_ws, title, this);
                tlp_main.Controls.Add(con_datafordaily, 0, row);
                con_datafordaily.Dock = DockStyle.Fill;
                tlp_main.RowStyles[row].Height = con_datafordaily.GetGridHeight();
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
                shiftCond = " AND (PROD.shift = '" + m_ws.shift + "')";
            }

            lbl_totalCut.Text = dcon.GetTotalCut(shiftCond, m_ws.staDate, m_ws.endDate);
        }

        private void btn_home_Click(object sender, EventArgs e)
        {
            mainForm.GoToMain();
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
    }
}
