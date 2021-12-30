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
using C1.Win.C1Chart;
using C1.C1Preview;

namespace WasteReport.UserCons
{
    public partial class Con_Chart : UserControl
    {
        MainForm mainForm;
        WasteSearch m_ws;
        MDBmanager dcon = null;
        DataTable dt_data = new DataTable();
        string docTitleStr = "";

        public Con_Chart(MainForm mf, WasteSearch ws)
        {
            InitializeComponent();

            mainForm = mf;
            m_ws = ws;

            SetBasicInfo();
            dcon = new MDBmanager(ws.machine);
            SetTotalCut();

            SetGrpPieChart();
            SetCdPieChart();
            if (m_ws.staDate == m_ws.endDate)
            {
                SetGrpColChart();
                c1Chat_grpLine.Visible = false;
                c1Chart_grpCol.Visible = true;
            }
            else
            {
                SetGrpLineChart();
                c1Chat_grpLine.Visible = true;
                c1Chart_grpCol.Visible = false;
            }

            docTitleStr = "Machine: " + m_ws.machine + "      Production Date : " + m_ws.staDate + " ~ " + m_ws.endDate;
        }

        //화면 상단 기본데이터 디스플레이
        private void SetBasicInfo()
        {
            lbl_machine.Text = m_ws.machine;
            lbl_prodDate.Text = m_ws.staDate + " to " + m_ws.endDate;
            lbl_rptDate.Text = DateTime.Now.ToString();
        }

        //토탈컷 디스플레이
        private void SetTotalCut()
        {
            string shiftCond = " AND 1 = 1";

            if (m_ws.shift != "*")
            {
                shiftCond = " AND (PROD.shift = '" + m_ws.shift + "')";
            }

            lbl_totalCut.Text = dcon.GetTotalCut(shiftCond, m_ws.staDate, m_ws.endDate);
        }

        //그룹별 파이 차트 세팅
        private void SetGrpPieChart()
        {
            string wgTopCond = "9999999";

            dt_data.Clear();
            string groupCond = " AND 1 = 1";

            if (m_ws.shift == "*")
            {
                dt_data = dcon.GetByWstGp(wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            }
            else
            {
                dt_data = dcon.GetByWstGpWithShift(m_ws.shift, wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            }

            // Set chart type
            c1Chat_grpPie.ChartArea.Inverted = true;
            c1Chat_grpPie.ChartGroups[0].ChartType = C1.Win.C1Chart.Chart2DTypeEnum.Pie;

            // Clear previous data
            c1Chat_grpPie.ChartGroups[0].ChartData.SeriesList.Clear();

            // Add Data
            string[] groupNames = new string[dt_data.Rows.Count];
            string[] groupX = new string[dt_data.Rows.Count];
            string[] groupXper = new string[dt_data.Rows.Count];

            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                groupNames[i] = dt_data.Rows[i]["Waste Group"].ToString();
                groupX[i] = dt_data.Rows[i]["Def"].ToString();
                groupXper[i] = dt_data.Rows[i]["%Waste"].ToString();
            }

            //get series collection
            ChartDataSeriesCollection dscoll = c1Chat_grpPie.ChartGroups[0].ChartData.SeriesList;
            dscoll.Clear();

            //populate the series
            for (int i = 0; i < groupXper.Length; i++)
            {
                ChartDataSeries series = dscoll.AddNewSeries();
                //Add one point to show one pie
                series.PointData.Length = 1;
                //Assign the value to the Y Data series
                series.Y[0] = decimal.Parse(groupXper[i]);
                //format the group name and group x value on the legend
                series.Label = string.Format("{0} ({1:c})", groupNames[i], groupX[i]);


                C1.Win.C1Chart.Label lbl = c1Chat_grpPie.ChartLabels.LabelsCollection.AddNewLabel();
                lbl.Text = string.Format("{0}", groupXper[i] + "%"); 
                lbl.Compass = LabelCompassEnum.Radial; lbl.Offset = 20; 
                lbl.Connected = true; lbl.Visible = true; 
                lbl.AttachMethod = AttachMethodEnum.DataIndex; 
                AttachMethodData am = lbl.AttachMethodData; 
                am.GroupIndex = 0;
                am.SeriesIndex = i; 
                am.PointIndex = 0;
            }

            // show pie Legend
            c1Chat_grpPie.Legend.Visible = true;

            C1.Win.C1Chart.Style s2 = c1Chat_grpPie.Legend.Style; 
            s2.Font = new Font("Tahoma", 10);

            //add a title to the chart legend
            c1Chat_grpPie.Legend.Text = "Waste Trend By Waste Group";

            //
            C1.Win.C1Chart.Style s = c1Chat_grpPie.ChartLabels.DefaultLabelStyle;
            s.Font = new Font("Tahoma", 9);
            //s.BackColor = SystemColors.Info;
            s.Opaque = true;
            //s.Border.BorderStyle = BorderStyleEnum.Solid;

        }

        //그룹별 컬럼 차트 세팅
        private void SetGrpColChart()
        {
            string wgTopCond = "9999999";

            dt_data.Clear();
            string groupCond = " AND 1 = 1";

            if (m_ws.shift == "*")
            {
                dt_data = dcon.GetByWstGp(wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            }
            else
            {
                dt_data = dcon.GetByWstGpWithShift(m_ws.shift, wgTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            }

            // Set chart type
            c1Chart_grpCol.ChartArea.Inverted = true;
            c1Chart_grpCol.ChartGroups[0].ChartType = C1.Win.C1Chart.Chart2DTypeEnum.Bar;
            //c1Chart_grpCol.ChartGroups[0].Stacked = true;
            c1Chart_grpCol.ChartArea.AxisY.Min = 0;

            // Clear previous data
            c1Chart_grpCol.ChartGroups[0].ChartData.SeriesList.Clear();

            if (dt_data.Rows.Count < 1)
            {
                return;
            }

            // Add Data
            string[] groupNames = new string[dt_data.Rows.Count];
            int[] groupY = new int[dt_data.Rows.Count];
            string[] groupYper = new string[dt_data.Rows.Count];

            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                groupNames[i] = dt_data.Rows[i]["Waste Group"].ToString();
                groupY[i] = int.Parse(dt_data.Rows[i]["Def"].ToString());
                groupYper[i] = dt_data.Rows[i]["%Waste"].ToString();
            }

            //get series collection
            ChartDataSeriesCollection dscoll = c1Chart_grpCol.ChartGroups[0].ChartData.SeriesList;
            dscoll.Clear();
            c1Chart_grpCol.ChartGroups[0].Stacked = true;

            if (dt_data.Rows.Count > 0)
            {
                for (int i = 0; i < groupY.Length; i++)
                {
                    C1.Win.C1Chart.ChartDataSeries dSeries = dscoll.AddNewSeries();
                    dSeries.Label = string.Format("{0} ({1:c} %)", i.ToString() + "." + groupNames[i], groupYper[i]);
                    //var type = Type.GetType("System.String");
                    //dSeries.X.DataType = type;
                    dSeries.X.Add(i);
                    dSeries.Y.Add(groupY[i]);
                }
            }

            //create new font for the X and Y axes
            Font f = new Font("Arial", 10, FontStyle.Bold);
            c1Chart_grpCol.ChartArea.Style.ForeColor = Color.DarkGray;
            c1Chart_grpCol.ChartArea.AxisX.Font = f;
            c1Chart_grpCol.ChartArea.AxisX.Text = "Groups";
            c1Chart_grpCol.ChartArea.AxisX.GridMajor.Visible = true;
            c1Chart_grpCol.ChartArea.AxisX.GridMajor.Color = Color.LightGray;
            c1Chart_grpCol.ChartArea.AxisY.Font = f;
            c1Chart_grpCol.ChartArea.AxisY.Text = "Waste";
            c1Chart_grpCol.ChartArea.AxisY.GridMajor.Visible = true;
            c1Chart_grpCol.ChartArea.AxisY.GridMajor.Color = Color.LightGray;

            // show chart Legend
            c1Chart_grpCol.Legend.Visible = true;

            C1.Win.C1Chart.Style s2 = c1Chart_grpCol.Legend.Style;
            s2.Font = new Font("Tahoma", 10);

            //add a title to the chart legend
            c1Chart_grpCol.Legend.Text = "Waste Trend By Waste Group";

            //
            C1.Win.C1Chart.Style s = c1Chart_grpCol.ChartLabels.DefaultLabelStyle;
            s.Font = new Font("Tahoma", 9);
            //s.BackColor = SystemColors.Info;
            s.Opaque = true;
            //s.Border.BorderStyle = BorderStyleEnum.Solid;
        }

        //코드별 탑10차트 세팅
        private void SetCdPieChart()
        {
            string wcTopCond = "10";

            dt_data.Clear();
            string groupCond = " AND 1 = 1";

            if (m_ws.shift == "*")
            {
                dt_data = dcon.GetByWstCd(wcTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            }
            else
            {
                dt_data = dcon.GetByWstCdWithShift(m_ws.shift, wcTopCond, m_ws.staDate, m_ws.endDate, groupCond);
            }

            // Set chart type
            c1Chat_cdPie.ChartArea.Inverted = true;
            c1Chat_cdPie.ChartGroups[0].ChartType = C1.Win.C1Chart.Chart2DTypeEnum.Pie;

            // Clear previous data
            c1Chat_cdPie.ChartGroups[0].ChartData.SeriesList.Clear();

            // Add Data
            string[] groupNames = new string[dt_data.Rows.Count];
            string[] groupX = new string[dt_data.Rows.Count];
            string[] groupXper = new string[dt_data.Rows.Count];

            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                groupNames[i] = dt_data.Rows[i]["Waste Code"].ToString();
                groupX[i] = dt_data.Rows[i]["Def"].ToString();
                groupXper[i] = dt_data.Rows[i]["%Waste"].ToString();
            }

            //get series collection
            ChartDataSeriesCollection dscoll = c1Chat_cdPie.ChartGroups[0].ChartData.SeriesList;
            dscoll.Clear();

            //populate the series
            for (int i = 0; i < groupXper.Length; i++)
            {
                ChartDataSeries series = dscoll.AddNewSeries();
                //Add one point to show one pie
                series.PointData.Length = 1;
                //Assign the value to the Y Data series
                series.Y[0] = decimal.Parse(groupXper[i]);
                //format the group name and group x value on the legend
                series.Label = string.Format("{0} ({1:c})", groupNames[i], groupX[i]);


                C1.Win.C1Chart.Label lbl = c1Chat_cdPie.ChartLabels.LabelsCollection.AddNewLabel();
                lbl.Text = string.Format("{0}", groupXper[i] + "%");
                lbl.Compass = LabelCompassEnum.Radial; lbl.Offset = 20;
                lbl.Connected = true; lbl.Visible = true;
                lbl.AttachMethod = AttachMethodEnum.DataIndex;
                AttachMethodData am = lbl.AttachMethodData;
                am.GroupIndex = 0;
                am.SeriesIndex = i;
                am.PointIndex = 0;
            }

            // show pie Legend
            c1Chat_cdPie.Legend.Visible = true;

            C1.Win.C1Chart.Style s2 = c1Chat_cdPie.Legend.Style;
            s2.Font = new Font("Tahoma", 10);

            //add a title to the chart legend
            c1Chat_cdPie.Legend.Text = "Top 10 Waste Trend By Waste Code";

            //
            C1.Win.C1Chart.Style s = c1Chat_cdPie.ChartLabels.DefaultLabelStyle;
            s.Font = new Font("Tahoma", 9);
            //s.BackColor = SystemColors.Info;
            s.Opaque = true;
            //s.Border.BorderStyle = BorderStyleEnum.Solid;

        }

        //그룹별 라인 차트 세팅
        private void SetGrpLineChart()
        {
            C1.Win.C1Chart.Axis xa = c1Chat_grpLine.ChartArea.AxisX;

            // set axis annotation format
            xa.AnnoFormat = C1.Win.C1Chart.FormatEnum.DateShort;

            // set axis maximum
            //xa.Max = DateTime.Now.ToOADate();

            DataTable dt_tempGrp = new DataTable();

            dt_tempGrp = dcon.GetWstGroups();
            string[] groups = new string[dt_tempGrp.Rows.Count];
            for (int i = 0; i < dt_tempGrp.Rows.Count; i++)
            {
                groups[i] = dt_tempGrp.Rows[i]["WasteGroup"].ToString();
            }

            if (dt_tempGrp.Rows.Count < 1)
            {
                return;
            }

            //clear previous series
            c1Chat_grpLine.ChartGroups[0].ChartData.SeriesList.Clear();


            for (int i = 0; i < dt_tempGrp.Rows.Count; i++)
            {
                dt_data.Clear();
                if (m_ws.shift == "*")
                {
                    dt_data = dcon.GetAofWstGp(m_ws.staDate, m_ws.endDate, groups[i]);
                }
                else
                {
                    dt_data = dcon.GetAofWstGpWithShift(m_ws.shift, m_ws.staDate, m_ws.endDate, groups[i]);
                }

                //add one series to the chart
                C1.Win.C1Chart.ChartDataSeries ds = c1Chat_grpLine.ChartGroups[0].ChartData.SeriesList.AddNewSeries();
                ds.Label = groups[i].ToString();

                if (dt_data.Rows.Count > 0)
                {
                    DateTime[] wDate = new DateTime[dt_data.Rows.Count];
                    double[] wValue = new double[dt_data.Rows.Count];

                    for (int j = 0; j < dt_data.Rows.Count; j++)
                    {
                        wDate[j] = Convert.ToDateTime(dt_data.Rows[j]["DateStamp"].ToString());
                        wValue[j] = double.Parse(dt_data.Rows[j]["Def"].ToString());
                    }

                    //copy the x and y data
                    ds.X.CopyDataIn(wDate);
                    ds.Y.CopyDataIn(wValue);

                    //modify line style appearance
                    ds.LineStyle.Pattern = LinePatternEnum.Solid;
                    ds.LineStyle.Thickness = 1;

                    //modify the symbol style appearance
                    ds.SymbolStyle.Shape = SymbolShapeEnum.None;
                    //ds.SymbolStyle.OutlineColor = Color.Black;
                    ds.SymbolStyle.Size = 2;
                    ds.SymbolStyle.OutlineWidth = 1;
                }
            }


            //set the chart type
            c1Chat_grpLine.ChartGroups[0].ChartType = C1.Win.C1Chart.Chart2DTypeEnum.XYPlot;

            //create new font for the X and Y axes
            Font f = new Font("Arial", 10, FontStyle.Bold);
            c1Chat_grpLine.ChartArea.Style.ForeColor = Color.DarkGray;
            c1Chat_grpLine.ChartArea.AxisX.Font = f;
            c1Chat_grpLine.ChartArea.AxisX.Text = "Date";
            c1Chat_grpLine.ChartArea.AxisX.GridMajor.Visible = true;
            c1Chat_grpLine.ChartArea.AxisX.GridMajor.Color = Color.LightGray;
            c1Chat_grpLine.ChartArea.AxisY.Font = f;
            c1Chat_grpLine.ChartArea.AxisY.Text = "Waste";
            c1Chat_grpLine.ChartArea.AxisY.GridMajor.Visible = true;
            c1Chat_grpLine.ChartArea.AxisY.GridMajor.Color = Color.LightGray;

            c1Chat_grpLine.ChartArea.PlotArea.BackColor = Color.White;

            // show pie Legend
            c1Chat_grpLine.Legend.Visible = true;

            C1.Win.C1Chart.Style s2 = c1Chat_grpLine.Legend.Style;
            s2.Font = new Font("Tahoma", 10);

            //add a title to the chart legend
            c1Chat_grpLine.Legend.Text = "Waste Trend By Waste Group";

            //
            C1.Win.C1Chart.Style s = c1Chat_grpLine.ChartLabels.DefaultLabelStyle;
            s.Font = new Font("Tahoma", 9);
            //s.BackColor = SystemColors.Info;
            s.Opaque = true;
            //s.Border.BorderStyle = BorderStyleEnum.Solid;
        }

        //홈 버튼 클릭시
        private void btn_home_Click(object sender, EventArgs e)
        {
            mainForm.GoToMain();
        }

        //프린트 버튼 클릭시 (C1차트 이미지 렌더링해서 도큐먼트로)
        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                C1.Win.C1Preview.C1PrintPreviewDialog ppd = new C1.Win.C1Preview.C1PrintPreviewDialog();
                C1PrintDocument doc = new C1PrintDocument();
                doc.PageLayouts.Default.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
                doc.PageLayouts.Default.PageSettings.Landscape = true;
                //doc.PageLayouts.Default.PageSettings.LeftMargin = 1;
                //doc.PageLayouts.Default.PageSettings.RightMargin = 1;
                //doc.PageLayouts.Default.PageSettings.TopMargin = 1;
                //doc.PageLayouts.Default.PageSettings.BottomMargin = 1;

                RenderText rt = new RenderText();
                rt.Text = docTitleStr;
                rt.Style.Font = new Font("Tahoma", 12);
                doc.Body.Children.Add(rt);

                RenderText rt2 = new RenderText();
                rt2.Text = " ";
                rt2.Style.Font = new Font("Tahoma", 12);
                doc.Body.Children.Add(rt2);

                RenderImage ri = new RenderImage();
                Size temp_size1 = c1Chat_grpPie.Size;
                ri.Control = this.c1Chat_grpPie;
                ri.Control.Size = new Size(900, 280);
                doc.Body.Children.Add(ri);

                RenderImage ri2 = new RenderImage();
                Size temp_size2 = c1Chart_grpCol.Size; 
                if (m_ws.staDate == m_ws.endDate)
                {
                    temp_size2 = c1Chart_grpCol.Size;
                    ri2.Control = this.c1Chart_grpCol;
                    ri2.Control.Size = new Size(900, 280);
                }
                else
                {
                    temp_size2 = c1Chat_grpLine.Size;
                    ri2.Control = this.c1Chat_grpLine;
                    ri2.Control.Size = new Size(900, 280);
                }
                doc.Body.Children.Add(ri2);

                RenderText rt3 = new RenderText();
                rt3.Text = docTitleStr;
                rt3.Style.Font = new Font("Tahoma", 12);
                doc.Body.Children.Add(rt3);

                RenderText rt4 = new RenderText();
                rt4.Text = " ";
                rt4.Style.Font = new Font("Tahoma", 12);
                doc.Body.Children.Add(rt4);

                RenderImage ri3 = new RenderImage();
                Size temp_size3 = c1Chat_cdPie.Size;
                ri3.Control = this.c1Chat_cdPie;
                ri3.Control.Size = new Size(900, 280);
                doc.Body.Children.Add(ri3);

                ppd.Document = doc;
                ppd.WindowState = FormWindowState.Maximized;
                ppd.Show();

                //이미지 렌더링 후 폼의 차트가 프린트 되는 사이즈로 변하는걸 방지
                c1Chat_grpPie.Size = temp_size1;
                if (m_ws.staDate == m_ws.endDate)
                {
                    c1Chart_grpCol.Size = temp_size2;
                }
                else
                {
                    c1Chat_grpLine.Size = temp_size2;
                }
                c1Chat_cdPie.Size = temp_size3;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void pbtn_main_MouseEnter(object sender, EventArgs e)
        {
            pbtn_main.BackgroundImage = Properties.Resources.sub_btn_main_off;
        }

        private void pbtn_main_MouseLeave(object sender, EventArgs e)
        {
            pbtn_main.BackgroundImage = Properties.Resources.sub_btn_main_on;
        }
    }
}
