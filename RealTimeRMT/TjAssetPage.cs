using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using C1.Win.C1FlexGrid;

namespace RealTimeRMT
{
    class TjAssetPage
    {
        private MainForm _parent = null;
        private string _oldPpAmountValue = "";

        public int _IndexCovGroupGrid = 1;
        public int _CovGroupTotal = 0;

        enum eKpiGridView { NAME, CODE, DESCRIPTION, PROGRESS_BAR, LOSS };

        List<Label> a_weightlist1;
        List<Label> a_weightlist2;
        List<Label> a_weightlist3;

        private const int WAITING_TIME = 60; // 데이터 로드 전 대기 시간, 60초
        public TjAssetPage(MainForm parent)
        {
            _parent = parent;
        }

        public void InitControls()
        {
            _parent.machineStatusTimer.Interval = 10000;
            _parent.kpiStatusTimer.Interval = 1000 * WAITING_TIME;
            _parent.kpiWaitingTimer.Interval = 1000;
            _parent.watingProgressBar.Maximum = WAITING_TIME;

            InitOeeTotalMachineStatusGridView();
            InitProductPlanGridView();
            InitProductScheduleGridView();
            InitProductionPredictionMaterialDataView();
            InitKpiGridView();
            InitCovGridView(_parent.taCovDataGridView1);
            InitCovGridView(_parent.taCovDataGridView2);
            InitCovGridView(_parent.taCovDataGridView3);

            _parent.tjOeeTeamNameLabel.Text = "유아2팀  ";
            _parent.tjOeeSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
            _parent.tjOeeTj01Button.Checked = true;
            _parent.tjOeeTj21Button.Checked = true;

            _parent.tjPlanTeamNameLabel.Text = "유아2팀  ";
            _parent.taCcProductionPlanButton.Checked = true;

            _parent.tjScheduleTeamNameLabel.Text = "유아2팀  ";
            _parent.taCcProductionCalendarButton.Checked = true;

            _parent.tjPredictionTeamNameLabel.Text = "유아2팀  ";
            _parent.tjPredictionSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
            _parent.tjPredictionTj01Button.Checked = true;
            _parent.tjPredictionTj21Button.Checked = true;

            _parent.tjKpiTeamNameLabel.Text = "유아2팀  ";
            _parent.tjKpiSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
            _parent.tjKpiTj01Button.Checked = true;
            _parent.tjKpiTj21Button.Checked = true;

            _parent.tjCovTeamNameLabel.Text = "유아2팀  ";
            _parent.tjCovSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
            _parent.tjCovTj01Button.Checked = true;
            _parent.tjCovTj21Button.Checked = true;

            _parent.taOeeTabControl.SelectedIndex = (int)ConstDefine.eTaOeeTab.tjOee;
            _parent.taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.oee;

            SetSelectedProductionScheduleEventDay(true);

            //Cov 
            _parent.rtCovDayRadioButton.Checked = true;
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(1);
            _parent.rtCovStartDateTimePicker.Value = new DateTime(startDate.Year, startDate.Month, startDate.Day, 7, 0, 0);
            _parent.rtCovEndDateTimePicker.Value = _parent.rtCovStartDateTimePicker.Value.AddDays(1).AddSeconds(-1);
            //_parent.rtCovEndDateTimePicker.Value = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            _parent.rtCovEndDateTimePicker.Enabled = false;

            //Display View List
            a_weightlist1 = new List<Label> { _parent.taCovPadGroupLabel1, _parent.taCovCreateDateLable1, _parent.taCovProCodeLabel1, _parent.taCovProNameLabel1,
                                                                                        _parent.taCovPadAvgLabel1,_parent.taCovPadDevLabel1,_parent.taCovPadCovLabel1,_parent.taCovPadMaxLabel1,_parent.taCovPadMinLabel1};

            a_weightlist2 = new List<Label> { _parent.taCovPadGroupLabel2, _parent.taCovCreateDateLable2, _parent.taCovProCodeLabel2, _parent.taCovProNameLabel2,
                                                                                        _parent.taCovPadAvgLabel2,_parent.taCovPadDevLabel2,_parent.taCovPadCovLabel2,_parent.taCovPadMaxLabel2,_parent.taCovPadMinLabel2};

            a_weightlist3 = new List<Label> { _parent.taCovPadGroupLabel3, _parent.taCovCreateDateLable3, _parent.taCovProCodeLabel3, _parent.taCovProNameLabel3,
                                                                                        _parent.taCovPadAvgLabel3,_parent.taCovPadDevLabel3,_parent.taCovPadCovLabel3,_parent.taCovPadMaxLabel3,_parent.taCovPadMinLabel3};

        }

        public void LoadProductionPredictionCalendarList()
        {
            //_parent.taProductionScheduleCalendar.GroupBy = "";


            string startDate = _parent.taProductionScheduleCalendar.CalendarInfo.FirstDate.AddDays(-1).ToString("yyyy-MM-dd");
            string endDate = _parent.taProductionScheduleCalendar.CalendarInfo.LastDate.AddDays(1).ToString("yyyy-MM-dd");
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}', '{3}'",
                "EXEC SelectScheduleCalendarList",
                GetSelectedPCalendarTeamTypeValue(),
                startDate,
                endDate));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            _parent.taProductionScheduleCalendar.DataStorage.AppointmentStorage.DataMember = "List";
            _parent.taProductionScheduleCalendar.DataStorage.AppointmentStorage.Mappings.IdMapping.MappingName = "Id";
            _parent.taProductionScheduleCalendar.DataStorage.AppointmentStorage.Mappings.Subject.MappingName = "Subject";
            _parent.taProductionScheduleCalendar.DataStorage.AppointmentStorage.Mappings.Body.MappingName = "Body";
            _parent.taProductionScheduleCalendar.DataStorage.AppointmentStorage.Mappings.Start.MappingName = "Start";
            _parent.taProductionScheduleCalendar.DataStorage.AppointmentStorage.Mappings.End.MappingName = "End";
            _parent.taProductionScheduleCalendar.DataStorage.AppointmentStorage.Mappings.Location.MappingName = "Location";
            _parent.taProductionScheduleCalendar.DataStorage.AppointmentStorage.DataSource = dataSet;
        }


        public void LoadProductionPredictionScheduleList(bool isToday = false)
        {
            //_parent.taProductionScheduleCalendar.GroupBy = "";

            if (isToday != false && (_parent.taProductionScheduleCalendar.SelectedAppointments == null || _parent.taProductionScheduleCalendar.SelectedAppointments.Count == 0))
                return;

            DateTime startDate;
            if (isToday == true)
                startDate = DateTime.Now;
            else
                startDate = _parent.taProductionScheduleCalendar.CurrentDate;

            string startDateStr = startDate.AddDays(-1).ToString("yyyy-MM-dd");
            string endDateStr = startDate.AddDays(1).ToString("yyyy-MM-dd");

            _parent.taProductionScheduleDataGridVeiw.Rows.Clear();

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}', '{3}'",
                "EXEC SelectScheduleScheduleList",
                GetSelectedPCalendarTeamTypeValue(),
                startDateStr,
                endDateStr));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                _parent.taProductionScheduleDataGridVeiw.Rows.Add(dataSet.Tables[0].Rows[i]["ProductCode"].ToString(),
                      dataSet.Tables[0].Rows[i]["DateTime"].ToString(),
                   dataSet.Tables[0].Rows[i]["ProdLine"].ToString(),
                    Utils.SetComma(dataSet.Tables[0].Rows[i]["Amount"].ToString()),
                    dataSet.Tables[0].Rows[i]["AmtUnit"].ToString(),
                    dataSet.Tables[0].Rows[i]["ProdGender"].ToString(),
                    dataSet.Tables[0].Rows[i]["ProdSize"].ToString(),
                    dataSet.Tables[0].Rows[i]["ProdDomestic"].ToString(),
                    dataSet.Tables[0].Rows[i]["ProdCountry"].ToString(),
                    Utils.SetComma(dataSet.Tables[0].Rows[i]["ProdCountPerBag"].ToString()),
                    Utils.SetComma(dataSet.Tables[0].Rows[i]["BagCountPerCase"].ToString()),
                    Utils.SetComma(dataSet.Tables[0].Rows[i]["ProdTotalCount"].ToString()));
            }
            _parent.taProductionScheduleDataGridVeiw.ClearSelection();
        }

        public void SetSelectedProductionScheduleEventDay(bool isToday = false)
        {
            if (isToday == true)
                _parent.taSelectedProductionScheduleEventDayLabel.Text = DateTime.Now.ToString("yyyy-MM-dd");
            else
                _parent.taSelectedProductionScheduleEventDayLabel.Text = _parent.taProductionScheduleCalendar.CurrentDate.ToString("yyyy-MM-dd");
        }

        public void LoadTotalMachineStatus()
        {
            _parent.taOeeTotalMachineStatusGridView.Rows.Clear();

            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectTotalMachineStatus");
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            Image statusOn = Properties.Resources.machine_status_on_32;
            Image statusOff = Properties.Resources.machine_status_off_32;

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                _parent.taOeeTotalMachineStatusGridView.Rows.Add(
                    dataSet.Tables[0].Rows[i]["ASSET_TJ"].ToString(),
                    (dataSet.Tables[0].Rows[i]["MachineStatus"].ToString() == "1") ? statusOn : statusOff,
                    Utils.GetValueRoundOff(dataSet.Tables[0].Rows[i]["PRD"].ToString(), 1),
                    Utils.GetValueRoundOff(dataSet.Tables[0].Rows[i]["OEE"].ToString(), 1),
                    Utils.GetValueRoundOff(dataSet.Tables[0].Rows[i]["YIELD"].ToString(), 1),
                    Utils.GetValueRoundOff(dataSet.Tables[0].Rows[i]["DELAY"].ToString(), 1),
                    dataSet.Tables[0].Rows[i]["RUNNING"].ToString(),
                    dataSet.Tables[0].Rows[i]["PRDVersion"].ToString(),
                    dataSet.Tables[0].Rows[i]["PRDType"].ToString());

                /*
                if (i % 2 != 0)
                    _parent.taOeeTotalMachineStatusGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(204, 236, 254);
                else
                    _parent.taOeeTotalMachineStatusGridView.Rows[i].DefaultCellStyle.BackColor = Color.White; 
                */
            }

            _parent.taOeeTotalMachineStatusGridView.ClearSelection();
        }

        public void LoadEachMachineStatus()
        {
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} '{1}'", "EXEC SelectEachMachineStatus", GetSelectedOeeAssetTjValue()));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            DataRow dataRow = dataSet.Tables[0].Rows[0];
            _parent.taOeeSymbolMcStatusLabel.Text = (dataRow["MachineStatus"].ToString() == "1") ? "RUN" : "STOP";
            if (_parent.taOeeSymbolMcStatusImage.BackgroundImage != null)
            {
                _parent.taOeeSymbolMcStatusImage.BackgroundImage.Dispose();
                _parent.taOeeSymbolMcStatusImage.BackgroundImage = null;
            }
            _parent.taOeeSymbolMcStatusImage.BackgroundImage = (_parent.taOeeSymbolMcStatusLabel.Text == "RUN") ? Properties.Resources.machine_status_on_48 : Properties.Resources.machine_status_off_48;
            _parent.taOeeSymbolPrdLabel.Text = Utils.GetValueRoundOff(dataRow["PRD"].ToString(), 1) + "%";
            _parent.taOeeSymbolPrdGauge.Value = (int)Convert.ToDouble(dataRow["PRD"].ToString());
            _parent.taOeeSymbolOeeLabel.Text = Utils.GetValueRoundOff(dataRow["OEE"].ToString(), 1) + "%";
            _parent.taOeeSymbolOeeGauge.Value = (int)Convert.ToDouble(dataRow["OEE"].ToString());
            _parent.taOeeSymbolYieldLabel.Text = Utils.GetValueRoundOff(dataRow["YIELD"].ToString(), 1) + "%";
            _parent.taOeeSymbolYieldGauge.Value = (int)Convert.ToDouble(dataRow["YIELD"].ToString());
            _parent.taOeeSymbolDelayLabel.Text = Utils.GetValueRoundOff(dataRow["DELAY"].ToString(), 1) + "%";
            _parent.taOeeSymbolDelayGauge.Value = (int)Convert.ToDouble(dataRow["DELAY"].ToString());
            _parent.taOeeSymbolRunningLabel.Text = dataRow["RUNNING"].ToString();
            _parent.taOeeSymbolPrdVersionLabel.Text = dataRow["PRDVersion"].ToString();
            _parent.taOeeSymbolPrdTypeLabel.Text = dataRow["PRDType"].ToString();
            if (_parent.taOeeSymbolPrdTypeImage.BackgroundImage != null)
            {
                _parent.taOeeSymbolPrdTypeImage.BackgroundImage.Dispose();
                _parent.taOeeSymbolPrdTypeImage.BackgroundImage = null;
            }
            _parent.taOeeSymbolPrdTypeImage.BackgroundImage = (_parent.taOeeSymbolPrdTypeLabel.Text == "Pants") ? Properties.Resources.ta_machine_status_prd_type_pants : Properties.Resources.ta_machine_status_prd_type_diaper;
        }

        private string GetSelectedOeeAssetTjValue()
        {
            string assetTjValue = "21 MC";

            if (_parent.tjOeeSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                if (_parent.tjOeeTj01Button.Checked == true)
                    assetTjValue = "01 MC";
                else if (_parent.tjOeeTj02Button.Checked == true)
                    assetTjValue = "02 MC";
                else if (_parent.tjOeeTj03Button.Checked == true)
                    assetTjValue = "03 MC";
                else if (_parent.tjOeeTj04Button.Checked == true)
                    assetTjValue = "04 MC";
                else if (_parent.tjOeeTj05Button.Checked == true)
                    assetTjValue = "05 MC";
                else
                    assetTjValue = "01 MC";
            }
            else
            {
                if (_parent.tjOeeTj21Button.Checked == true)
                    assetTjValue = "21 MC";
                else if (_parent.tjOeeTj22Button.Checked == true)
                    assetTjValue = "22 MC";
                //else if (_parent.tjOeeTj23Button.Checked == true)
                //    assetTjValue = "23 MC";
                else
                    assetTjValue = "21 MC";
            }

            return assetTjValue;
        }

        private string GetSelectedOeeTjName()
        {
            string tjName = "TJ21";

            if (_parent.tjOeeSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                if (_parent.tjOeeTj01Button.Checked == true)
                    tjName = "TJ01";
                else if (_parent.tjOeeTj02Button.Checked == true)
                    tjName = "TJ02";
                else if (_parent.tjOeeTj03Button.Checked == true)
                    tjName = "TJ03";
                else if (_parent.tjOeeTj04Button.Checked == true)
                    tjName = "TJ04";
                else if (_parent.tjOeeTj05Button.Checked == true)
                    tjName = "TJ05";
                else
                    tjName = "TJ01";
            }
            else
            {
                if (_parent.tjOeeTj21Button.Checked == true)
                    tjName = "TJ21";
                else if (_parent.tjOeeTj22Button.Checked == true)
                    tjName = "TJ22";
                //else if (_parent.tjOeeTj23Button.Checked == true)
                //    tjName = "TJ23";
                else
                    tjName = "TJ21";
            }

            return tjName;
        }

        public void StartMachineStatusTimer()
        {
            if (_parent.taTabControl.SelectedIndex == (int)ConstDefine.eTjAssetTab.oee &&
                _parent.taOeeTabControl.SelectedIndex != (int)ConstDefine.eTaOeeTab.fl)
            {
                LoadMachineStatus();

                if (_parent.machineStatusTimer.Enabled == false)
                    _parent.machineStatusTimer.Start();
            }
            else
            {
                if (_parent.machineStatusTimer.Enabled == true)
                    _parent.machineStatusTimer.Stop();
            }
        }

        public void StartKpiStatusTimer()
        {
            if (_parent.taTabControl.SelectedIndex == (int)ConstDefine.eTjAssetTab.kpi)
            {
                _parent.watingProgressBar.Value = 0;
                LoadKpi();

                if (_parent.kpiStatusTimer.Enabled == false)
                    _parent.kpiStatusTimer.Start();

                if (_parent.kpiWaitingTimer.Enabled == false)
                    _parent.kpiWaitingTimer.Start();
            }
            else
            {
                if (_parent.kpiStatusTimer.Enabled == true)
                    _parent.kpiStatusTimer.Stop();

                if (_parent.kpiWaitingTimer.Enabled == true)
                    _parent.kpiWaitingTimer.Stop();
            }
        }

        public void changeLoadCovList()
        {
            SetExtraStyleAllClear();
            SetCovTextAllClear(a_weightlist1, 1);
            _parent.taCovPadGroupTotal.Text = "0 / 0";
        }

        public void LoadCovListTotla()
        {
            _IndexCovGroupGrid = 1;

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}', '{3}', {4}, {5}, {6} ", "EXEC SelectCovGroupInfo", GetSelectedCovTjTypeValue(), GetCovStringDate(_parent.rtCovStartDateTimePicker),
                                                                                                                                GetCovStringDate(_parent.rtCovEndDateTimePicker), 0, 0, 2));

            if (dataSet.Tables[0].Rows[0].Field<int>("TotalCount") % 3 == 0)
            {
                _CovGroupTotal = dataSet.Tables[0].Rows[0].Field<int>("TotalCount") / 3;
            }
            else
            {
                _CovGroupTotal = (dataSet.Tables[0].Rows[0].Field<int>("TotalCount") / 3) + 1;
            }

        }

        public void LoadCovList()
        {
            if ((_CovGroupTotal + 1) == _IndexCovGroupGrid)
            {
                _IndexCovGroupGrid -= 1;
                return;
            }

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}', '{3}', {4}, {5}, {6} ", "EXEC SelectCovGroupInfo", GetSelectedCovTjTypeValue(), GetCovStringDate(_parent.rtCovStartDateTimePicker),
                                                                                                                                            GetCovStringDate(_parent.rtCovEndDateTimePicker), 165, _IndexCovGroupGrid, 1));

            SetExtraStyleAllClear();
            SetCovTextAllClear(a_weightlist1, 1);

            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                _parent.taCovPadGroupTotal.Text = "0 / 0";
                MessageBox.Show("검색한 기간안에 데이터가 존재하지 않습니다.");
                return;
            }


            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                int GorupIndex = 0;

                if (GorupIndex != dataSet.Tables[0].Rows[i].Field<int>("PadGroup"))
                {
                    GorupIndex = dataSet.Tables[0].Rows[i].Field<int>("PadGroup");

                    DataTable dt = dataSet.Tables[0].Select("(PadGroup = " + GorupIndex + ")").CopyToDataTable();

                    switch (i)
                    {
                        case 0:
                            SetCovLoaddata(_parent.taCovDataGridView1, a_weightlist1, dt);
                            break;
                        case 55:
                            SetCovLoaddata(_parent.taCovDataGridView2, a_weightlist2, dt);
                            break;
                        case 110:
                            SetCovLoaddata(_parent.taCovDataGridView3, a_weightlist3, dt);
                            break;
                    }
                }
            }

            _parent.taCovPadGroupTotal.Text = _IndexCovGroupGrid.ToString() + " / " + _CovGroupTotal.ToString();

        }

        private void SetCovLoaddata(C1FlexGrid cfg, List<Label> lb, DataTable dt)
        {
            cfg.DataSource = dt;

            //Product
            lb[0].Text = dt.Rows[0].Field<int>("PadGroup").ToString();
            lb[1].Text = dt.Rows[0].Field<DateTime>("Time").ToString();
            lb[2].Text = dt.Rows[0].Field<string>("Code");
            lb[3].Text = dt.Rows[0].Field<string>("Description");

            //Pad Weight
            lb[4].Text = dt.Rows[50].Field<float>("PadWeight").ToString();
            lb[5].Text = dt.Rows[51].Field<float>("PadWeight").ToString();
            lb[6].Text = dt.Rows[52].Field<float>("PadWeight").ToString();
            lb[7].Text = dt.Rows[53].Field<float>("PadWeight").ToString();
            lb[8].Text = dt.Rows[54].Field<float>("PadWeight").ToString();

            for (int i = 55; i > 50; i--)
            {
                cfg.Rows.Remove(i);
            }

            SetExtraStyle(cfg, dt);
            ChangeSorting(cfg, 1);
            ChangeRowCorByShift(cfg);
        }

        private void SetCovTextAllClear(List<Label> lb, int Type)
        {

            if (Type == 1)
            {
                for (int i = 0; i < lb.Count; i++)
                {
                    a_weightlist1[i].Text = "";
                    a_weightlist2[i].Text = "";
                    a_weightlist3[i].Text = "";
                }
            }
            else
            {
                for (int i = 0; i < lb.Count; i++)
                {
                    lb[i].Text = "";
                }
            }

        }

        public void InitCovGridView(C1FlexGrid cfg)
        {
            cfg.SelectionMode = SelectionModeEnum.Row;
            //_parent.taCovDataGridView.ExtendLastCol = true;

            cfg.Cols[0].Width = 0;

            CellStyle cs = cfg.Styles.Normal;
            cs = cfg.Styles.Fixed;
            cs.BackColor = Color.FromArgb(235, 234, 232);
            cs.ForeColor = Color.FromArgb(96, 96, 96);
            cs.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.None;
            cs.TextAlign = TextAlignEnum.CenterCenter;
            cfg.Rows[0].TextAlign = TextAlignEnum.CenterCenter;
        }

        private void SetExtraStyleClear(C1FlexGrid cfg)
        {
            cfg.DataSource = null;
            cfg.Clear();
            InitCovGridView(cfg);
        }

        private void SetExtraStyleAllClear()
        {
            _parent.taCovDataGridView1.DataSource = null;
            _parent.taCovDataGridView1.Clear();
            InitCovGridView(_parent.taCovDataGridView1);

            _parent.taCovDataGridView2.DataSource = null;
            _parent.taCovDataGridView2.Clear();
            InitCovGridView(_parent.taCovDataGridView2);

            _parent.taCovDataGridView3.DataSource = null;
            _parent.taCovDataGridView3.Clear();
            InitCovGridView(_parent.taCovDataGridView3);
        }

        private void SetExtraStyle(C1FlexGrid cfg, DataTable dt)
        {
            if (dt.Rows.Count > 1)
            {
                cfg.Cols["Number"].Width = 60;
                cfg.Cols["PadWeight"].Width = 90;

                cfg.Cols["Code"].Width = 0;
                cfg.Cols["Description"].Width = 0;
                cfg.Cols["PadGroup"].Width = 0;
                cfg.Cols["Time"].Width = 0;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "Number" || dt.Columns[i].ColumnName == "Code" ||
                        dt.Columns[i].ColumnName == "Description")
                    {
                        cfg.Cols[i + 1].TextAlign = TextAlignEnum.CenterCenter;
                    }
                }
                cfg.Select(0, 0);
            }
        }

        private void ChangeRowCorByShift(C1FlexGrid cfg)
        {
            C1.Win.C1FlexGrid.CellStyle rs = cfg.Styles.Add("RowColor");
            rs.BackColor = Color.FromArgb(36, 154, 234);
            //rs.Border.Color = Color.FromArgb(22,234, 33);
            rs.ForeColor = Color.FromArgb(255, 255, 255);
            rs.Font = new System.Drawing.Font("굴림", 9, FontStyle.Regular);
            C1.Win.C1FlexGrid.CellStyle rs2 = cfg.Styles.Add("RowColor2");
            rs2.BackColor = Color.FromArgb(250, 250, 250);
            rs2.ForeColor = Color.FromArgb(0, 0, 0);
            rs2.Font = new System.Drawing.Font("굴림", 9, FontStyle.Regular);

            for (int i = 1; i < cfg.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    cfg.Rows[i].Style = cfg.Styles["RowColor"];
                }
                else
                {
                    cfg.Rows[i].Style = cfg.Styles["RowColor2"];
                }
            }
        }

        private void ChangeSorting(C1FlexGrid cfg, int value)
        {
            if (value == 1)
            {
                cfg.AllowSorting = AllowSortingEnum.None;
            }
            else if (value == 2)
            {
                cfg.AllowSorting = AllowSortingEnum.SingleColumn;
            }
            else if (value == 3)
            {
                cfg.AllowSorting = AllowSortingEnum.MultiColumn;
            }
        }

        public void LoadMachineStatus()
        {
            DateTime now = DateTime.Now;
            if (_parent.taOeeTabControl.SelectedIndex == (int)ConstDefine.eTaOeeTab.udOee)
            {
                LoadTotalMachineStatus();
                //_parent.taOeeUdCurrentTimeLabel.Text = now.ToString("yyyy.MM.dd.dddd hh:mm:ss tt"); 
            }
            else if (_parent.taOeeTabControl.SelectedIndex != (int)ConstDefine.eTaOeeTab.fl)
            {
                LoadEachMachineStatus();

                _parent.taOeeCurrentTimeLabel.Text = now.ToString("yyyy.MM.dd.dddd hh:mm:ss tt");
            }
        }

        public void ResetOeeButtons(string currentName)
        {
            CustomControls.ButtonEx01[] team1ButtonItems = { _parent.tjOee1TeamLayoutButton, _parent.tjOee1TeamOeeButton, _parent.tjOeeTj01Button, _parent.tjOeeTj02Button,
                                     _parent.tjOeeTj03Button, _parent.tjOeeTj04Button, _parent.tjOeeTj05Button };

            CustomControls.ButtonEx01[] team2ButtonItems = { _parent.tjOee2TeamLayoutButton, _parent.tjOee2TeamOeeButton, _parent.tjOeeTj21Button, _parent.tjOeeTj22Button };

            CustomControls.ButtonEx01[] buttonItems = _parent.tjOeeSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc ? team1ButtonItems : team2ButtonItems;

            for (int i = 0; i < buttonItems.Length; i++)
            {
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;

                if (buttonItems[i].Checked == true)
                {
                    if (i > (int)ConstDefine.eTaOeeTab.tjOee)
                        _parent.taOeeTabControl.SelectedIndex = (int)ConstDefine.eTaOeeTab.tjOee;
                    else
                        _parent.taOeeTabControl.SelectedIndex = i;
                }

            }
        }

        public void ResetProductionPlanButtons(string currentName)
        {
            CheckBox[] buttonItems = { _parent.taBcProductionPlanButton, _parent.taCcProductionPlanButton };
            for (int i = 0; i < buttonItems.Length; i++)
            {
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;
            }
        }

        public void ResetProductionCalendarButtons(string currentName)
        {
            CheckBox[] buttonItems = { _parent.taBcProductionCalendarButton, _parent.taCcProductionCalendarButton };
            for (int i = 0; i < buttonItems.Length; i++)
            {
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;
            }
        }

        public void ResetProductionPredictionButtons(string currentName)
        {
            CustomControls.ButtonEx01[] team1ButtonItems = { _parent.tjPredictionTj01Button, _parent.tjPredictionTj02Button, _parent.tjPredictionTj03Button,
                                     _parent.tjPredictionTj04Button, _parent.tjPredictionTj05Button };

            CustomControls.ButtonEx01[] team2ButtonItems = { _parent.tjPredictionTj21Button, _parent.tjPredictionTj22Button, _parent.tjPredictionTj23Button };

            CustomControls.ButtonEx01[] buttonItems = _parent.tjPredictionSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc ? team1ButtonItems : team2ButtonItems;

            for (int i = 0; i < buttonItems.Length; i++)
            {
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;
            }
        }

        public void LoadProductionPlan()
        {
            _parent.taProductionPlanGridView.Rows.Clear();

            int teamType = GetSelectedPPlanTeamTypeValue();
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectProductionPlan", teamType));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;


            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                _parent.taProductionPlanGridView.Rows.Add(
                    dataRow["ProductCode"].ToString(),
                    dataRow["DateTime"].ToString(),
                    dataRow["ProdLine"].ToString(),
                    Utils.SetComma(dataRow["Amount"].ToString()),
                    dataRow["AmtUnit"].ToString(),
                    dataRow["ProdGender"].ToString(),
                    dataRow["ProdSize"].ToString(),
                    dataRow["ProdDomestic"].ToString(),
                    dataRow["ProdCountry"].ToString(),
                    Utils.SetComma(dataRow["ProdCountPerBag"].ToString()),
                    Utils.SetComma(dataRow["BagCountPerCase"].ToString()),
                    Utils.SetComma(dataRow["ProdTotalCount"].ToString()));
            }

            _parent.taProductionPlanGridView.ClearSelection();
        }

        public void LoadProductionPrediction()
        {
            LoadPpPriductDescriptionInfo();
            LoadPpProductInfo();
            LoadPpScheduleAmountInfo();
            LoadPpMaterialInfoList();
        }

        public void LoadPpPriductDescriptionInfo()
        {
            _parent.taProductionPredictionListComboBox.Items.Clear();
            _parent.taProductionPredictionListComboBox.Text = "";


            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}'", "EXEC SelectProductDescription", GetSelectedPPredictionTeamTypeValue(), GetSelectedPpProdLineValue()));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            _parent.taProductionPredictionListComboBox.BeginUpdate();
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem(dataRow["ProductCode"].ToString() + " | " + dataRow["Description"].ToString(), dataRow["ProductCode"].ToString());
                _parent.taProductionPredictionListComboBox.Items.Add(comboBoxItem);
            }
            _parent.taProductionPredictionListComboBox.EndUpdate();

            if (_parent.taProductionPredictionListComboBox.Items.Count > 0)
                _parent.taProductionPredictionListComboBox.SelectedIndex = 0;
        }

        public void LoadPpProductInfo()
        {
            _parent.taProductionPredictionMcLineTextBox.Text = "";
            _parent.taProductionPredictionGenderTextBox.Text = ""; ;
            _parent.taProductionPredictionSizeTextBox.Text = "";
            _parent.taProductionPredictionProductCodeTextBox.Text = "";
            _parent.taProductionPredictionDomesticTextBox.Text = "";
            _parent.taProductionPredictionCountryTextBox.Text = "";
            _parent.taProductionPredictionProductCbTextBox.Text = "";
            _parent.taProductionPredictionBagCcTextBox.Text = "";
            _parent.taProductionPredictionProductCcTextBox.Text = "";

            if (_parent.taProductionPredictionListComboBox.SelectedIndex == -1)
                return;

            int teamType = GetSelectedPPredictionTeamTypeValue();
            string productCode = _parent.taProductionPredictionListComboBox.Text.Substring(0, _parent.taProductionPredictionListComboBox.Text.IndexOf("|"));
            productCode = productCode.Trim();
            string prodLine = GetSelectedPpProdLineValue();

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}', '{3}'", "EXEC SelectProductInfo", teamType, prodLine, productCode));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            _parent.taProductionPredictionMcLineTextBox.Text = prodLine;
            _parent.taProductionPredictionGenderTextBox.Text = dataSet.Tables[0].Rows[0]["ProdGender"].ToString();
            _parent.taProductionPredictionSizeTextBox.Text = dataSet.Tables[0].Rows[0]["ProdSize"].ToString();
            _parent.taProductionPredictionProductCodeTextBox.Text = productCode;
            _parent.taProductionPredictionDomesticTextBox.Text = dataSet.Tables[0].Rows[0]["ProdDomestic"].ToString();
            _parent.taProductionPredictionCountryTextBox.Text = dataSet.Tables[0].Rows[0]["ProdCountry"].ToString();
            _parent.taProductionPredictionProductCbTextBox.Text = dataSet.Tables[0].Rows[0]["ProdCountPerBag"].ToString();
            _parent.taProductionPredictionBagCcTextBox.Text = dataSet.Tables[0].Rows[0]["BagCountPerCase"].ToString();
            _parent.taProductionPredictionProductCcTextBox.Text = dataSet.Tables[0].Rows[0]["ProdTotalCount"].ToString();
        }

        public void LoadPpScheduleAmountInfo()
        {
            _parent.taProductionPredictionAmountTextBox.Text = "";

            if (_parent.taProductionPredictionListComboBox.SelectedIndex == -1)
                return;

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}', '{3}'",
                "EXEC SelectScheduleAmountInfo",
                GetSelectedPPredictionTeamTypeValue(),
                _parent.taProductionPredictionMcLineTextBox.Text.Trim(),
                _parent.taProductionPredictionProductCodeTextBox.Text.Trim()));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                _parent.taProductionPredictionAmountTextBox.Text = "No Plan";
                _parent.taProductionPredictionAmountTextBox.Enabled = false;
                _parent.taProductionPredictionAmountUnitLabel.Visible = false;
            }
            else
            {
                _parent.taProductionPredictionAmountTextBox.Text = dataSet.Tables[0].Rows[0]["Amount"].ToString();
                _parent.taProductionPredictionAmountTextBox.Enabled = true;
                _parent.taProductionPredictionAmountUnitLabel.Text = dataSet.Tables[0].Rows[0]["AmtUnit"].ToString();
                _parent.taProductionPredictionAmountUnitLabel.Visible = true;
            }

            _parent.taProductionPredictionEstimateButton.Enabled = false;
            _parent.taProductionPredictionEstimateButton.BackColor = Color.DarkGray;
        }

        public void LoadPpMaterialInfoList()
        {
            _parent.taProductionPredictionMaterialDataView.Rows.Clear();

            if (_parent.taProductionPredictionListComboBox.SelectedIndex == -1)
                return;

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}', '{3}', {4}",
                "EXEC SelectMaterialInfoList",
                GetSelectedPPredictionTeamTypeValue(),
                GetSelectedPpProdLineValue(),
                _parent.taProductionPredictionProductCodeTextBox.Text.Trim(),
                GetSelectedPpTjTypeValue()));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;


            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                double productCntCase = 0;
                double amount = 0;
                double fpsData = 0;
                double unitQuantity = 0;
                string estimateRollCountValueStr = "";

                ListViewItem lvItem = new ListViewItem();
                if (dataRow["FPSData"].ToString().Trim() != "")
                    fpsData = Convert.ToDouble(dataRow["FPSData"].ToString().Trim());

                if (Utils.IsDigit(_parent.taProductionPredictionAmountTextBox.Text.Trim()) == true)
                {
                    if (_parent.taProductionPredictionProductCcTextBox.Text.Trim() != "")
                        productCntCase = Convert.ToDouble(_parent.taProductionPredictionProductCcTextBox.Text.Trim());
                    if (_parent.taProductionPredictionAmountTextBox.Text.Trim() != "")
                        amount = Convert.ToDouble(_parent.taProductionPredictionAmountTextBox.Text.Trim());
                    if (dataRow["UnitQuantity"].ToString().Trim() != "")
                        unitQuantity = Convert.ToDouble(dataRow["UnitQuantity"].ToString().Trim());

                    double estimateRollCountValue = (productCntCase * amount * fpsData) / (unitQuantity * 1000f);
                    estimateRollCountValueStr = string.Format("{0} ({1})", Math.Ceiling(estimateRollCountValue).ToString(), Math.Round(estimateRollCountValue, 2));
                }
                else
                {
                    estimateRollCountValueStr = "NULL";
                }

                _parent.taProductionPredictionMaterialDataView.Rows.Add(
                    dataRow["ProcEnum"].ToString(),
                    dataRow["ProcMnem"].ToString(),
                    dataRow["CompCode"].ToString(),
                    dataRow["Description"].ToString(),
                    Math.Round(fpsData, 2).ToString(),
                    Utils.SetComma(dataRow["UnitQuantity"].ToString()),
                    estimateRollCountValueStr,
                    dataRow["FPSData"].ToString().Trim()
                    );
            }

            _parent.taProductionPredictionMaterialDataView.ClearSelection();
        }

        private string GetSelectedPpProdLineValue()
        {
            string prodLineValue = "TJDP21";

            if (_parent.tjPredictionSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                if (_parent.tjPredictionTj01Button.Checked == true)
                    prodLineValue = "TJCD01";
                else if (_parent.tjPredictionTj02Button.Checked == true)
                    prodLineValue = "TJCD02";
                else if (_parent.tjPredictionTj03Button.Checked == true)
                    prodLineValue = "TJCD03";
                else if (_parent.tjPredictionTj04Button.Checked == true)
                    prodLineValue = "TJCD04";
                else if (_parent.tjPredictionTj05Button.Checked == true)
                    prodLineValue = "TJCD05";
                else
                    prodLineValue = "TJCD01";
            }
            else
            {

                if (_parent.tjPredictionTj21Button.Checked == true)
                    prodLineValue = "TJDP21";
                else if (_parent.tjPredictionTj22Button.Checked == true)
                    prodLineValue = "TJDP22";
                else if (_parent.tjPredictionTj23Button.Checked == true)
                    prodLineValue = "TJDP23";
                else
                    prodLineValue = "TJDP21";
            }


            return prodLineValue;
        }

        private int GetSelectedPPredictionTeamTypeValue()
        {
            if (_parent.tjPredictionSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
                return 1;
            else
                return 2;
        }

        private int GetSelectedPPlanTeamTypeValue()
        {
            int teamTypeValue = 2;

            if (_parent.taBcProductionPlanButton.Checked == true)
                teamTypeValue = 1;
            else if (_parent.taCcProductionPlanButton.Checked == true)
                teamTypeValue = 2;

            return teamTypeValue;
        }

        private int GetSelectedPCalendarTeamTypeValue()
        {
            int teamTypeValue = 2;

            if (_parent.taBcProductionCalendarButton.Checked == true)
                teamTypeValue = 1;
            else if (_parent.taCcProductionCalendarButton.Checked == true)
                teamTypeValue = 2;

            return teamTypeValue;
        }

        private int GetSelectedPpTjTypeValue()
        {
            int tjTypeValue = 21;

            if (_parent.tjPredictionSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                if (_parent.tjPredictionTj01Button.Checked == true)
                    tjTypeValue = 1;
                else if (_parent.tjPredictionTj02Button.Checked == true)
                    tjTypeValue = 2;
                else if (_parent.tjPredictionTj03Button.Checked == true)
                    tjTypeValue = 3;
                else if (_parent.tjPredictionTj04Button.Checked == true)
                    tjTypeValue = 4;
                else if (_parent.tjPredictionTj05Button.Checked == true)
                    tjTypeValue = 5;
                else
                    tjTypeValue = 1;
            }
            else
            {
                if (_parent.tjPredictionTj21Button.Checked == true)
                    tjTypeValue = 21;
                else if (_parent.tjPredictionTj22Button.Checked == true)
                    tjTypeValue = 22;
                else if (_parent.tjPredictionTj23Button.Checked == true)
                    tjTypeValue = 23;
                else
                    tjTypeValue = 21;
            }

            return tjTypeValue;
        }

        public void SetPpOldAmountValue()
        {
            _oldPpAmountValue = _parent.taProductionPredictionAmountTextBox.Text.Trim();
        }

        public void EstimateProductionPrediction()
        {
            if (Utils.IsDigit(_parent.taProductionPredictionAmountTextBox.Text.Trim()) == false)
            {
                MessageBox.Show("Enter numbers only.");
                _parent.taProductionPredictionAmountTextBox.Text = _oldPpAmountValue;
                return;
            }

            double productCntCase = 0;
            double amount = 0;
            double fpsData = 0;
            double unitQuantity = 0;

            if (_parent.taProductionPredictionProductCcTextBox.Text.Trim() != "")
                productCntCase = Convert.ToDouble(_parent.taProductionPredictionProductCcTextBox.Text.Trim());
            if (_parent.taProductionPredictionAmountTextBox.Text.Trim() != "")
                amount = Convert.ToDouble(_parent.taProductionPredictionAmountTextBox.Text.Trim());

            for (int i = 0; i < _parent.taProductionPredictionMaterialDataView.Rows.Count; i++)
            {
                DataGridViewRow row = _parent.taProductionPredictionMaterialDataView.Rows[i];

                if (row.Cells[(int)ConstDefine.eProductionPredictionMaterialListView.unitQty].Value.ToString().Trim() != "")
                    unitQuantity = Convert.ToDouble(row.Cells[(int)ConstDefine.eProductionPredictionMaterialListView.unitQty].Value.ToString().Trim());

                if (row.Cells[(int)ConstDefine.eProductionPredictionMaterialListView.fpsValue].Value.ToString().Trim() != "")
                    fpsData = Convert.ToDouble(row.Cells[(int)ConstDefine.eProductionPredictionMaterialListView.fpsValue].Value.ToString().Trim());

                double estimateRollCountValue = (productCntCase * amount * fpsData) / (unitQuantity * 1000f);
                row.Cells[(int)ConstDefine.eProductionPredictionMaterialListView.estimateRollCount].Value =
                    string.Format("{0} ({1})", Math.Ceiling(estimateRollCountValue).ToString(), Math.Round(estimateRollCountValue, 2));
            }
        }


        public void ResizeControls()
        {

            _parent.taTabControl.Left = 0;
            _parent.taTabControl.Top = 0;
            _parent.taTabControl.Width = _parent.mainTabControl.Width;
            _parent.taTabControl.Height = _parent.mainTabControl.Height;

            _parent.taOeeAsidePanel.Height = _parent.taTabControl.Height;

            // OEE
            _parent.taOeeTabControl.Left = _parent.taOeeAsidePanel.Width;
            _parent.taOeeTabControl.Height = _parent.taOeeAsidePanel.Height;
            _parent.taOeeTabControl.Width = _parent.mainTabControl.Width - _parent.taOeeAsidePanel.Width;

            // OEE - Layout
            _parent.tjLayoutPanel.Left = (_parent.taOeeTabControl.Width / 2) - (_parent.tjLayoutPanel.Width / 2);
            _parent.tjLayoutPanel.Top = (_parent.taOeeTabControl.Height / 2) - (_parent.tjLayoutPanel.Height / 2);

            /*
            _parent.taOeeTotalMachineStatusGridView.Left = _parent.taProductionPlanAsidePanel.Width + ConstDefine.defaultGap;
            _parent.taOeeTotalMachineStatusGridView.Top = ConstDefine.defaultGap;
            _parent.taOeeTotalMachineStatusGridView.Width = _parent.taTabControl.Width - (_parent.taProductionPlanAsidePanel.Width + (ConstDefine.defaultGap * 2));
            _parent.taOeeTotalMachineStatusGridView.Height _parent.taTabControl.Height - (ConstDefine.defaultGap * 2);
            */


            // OEE - 통합OEE
            _parent.taOeeTotalMachineStatusGridView.Left = ConstDefine.defaultGap;
            _parent.taOeeTotalMachineStatusGridView.Top = ConstDefine.defaultGap * 3;
            _parent.taOeeTotalMachineStatusGridView.Width = _parent.taTabControl.Width - (_parent.taProductionPlanAsidePanel.Width + (ConstDefine.defaultGap * 2));
            _parent.taOeeTotalMachineStatusGridView.Height = _parent.taTabControl.Height - (ConstDefine.defaultGap * 4);
            _parent.taOeeRefreshTotalMachineStatusButton.Left = _parent.taTabControl.Width - (_parent.taProductionPlanAsidePanel.Width + _parent.taOeeRefreshTotalMachineStatusButton.Width + ConstDefine.defaultGap);

            // 개별 OEE
            _parent.tjOeePanel.Left = (_parent.taOeeTabControl.Width / 2) - (_parent.tjOeePanel.Width / 2);
            _parent.tjOeePanel.Top = (_parent.taOeeTabControl.Height / 2) - (_parent.tjOeePanel.Height / 2);


            int eachColWidth = _parent.taOeeTabControl.Width / _parent.taOeeTotalMachineStatusGridView.Columns.Count;
            int errorWidth = _parent.taOeeTabControl.Width - (eachColWidth * _parent.taOeeTotalMachineStatusGridView.Columns.Count);
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.machine].Width = eachColWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.status].Width = eachColWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.prd].Width = eachColWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.oee].Width = eachColWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.yield].Width = eachColWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.delay].Width = eachColWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.running].Width = eachColWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.prdVer].Width = eachColWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eMachineStatusListView.prdType].Width = eachColWidth + errorWidth;

            _parent.taProductionPlanAsidePanel.Left = 0;
            _parent.taProductionPlanAsidePanel.Top = 0;
            _parent.taProductionPlanAsidePanel.Height = _parent.taTabControl.Height;

            ////
            // 생산계획
            _parent.taProductionPlanGridView.Left = _parent.taProductionPlanAsidePanel.Width + ConstDefine.defaultGap; ;
            _parent.taProductionPlanGridView.Top = ConstDefine.defaultGap;
            _parent.taProductionPlanGridView.Width = _parent.taTabControl.Width - (_parent.taProductionPlanAsidePanel.Width + (ConstDefine.defaultGap * 2));
            _parent.taProductionPlanGridView.Height = _parent.taTabControl.Height - (ConstDefine.defaultGap * 2);

            int gridWidth = _parent.taProductionPlanGridView.Width - ConstDefine.scrollSize;
            int colWidth = gridWidth / _parent.taProductionPlanGridView.Columns.Count;
            int totalColWidth = 0;
            for (int i = 0; i < _parent.taProductionPlanGridView.Columns.Count; i++)
            {
                _parent.taProductionPlanGridView.Columns[i].Width = colWidth;
            }
            ////

            ////
            // 생산달력
            _parent.taProductionCalendarAsidePanel.Left = 0;
            _parent.taProductionCalendarAsidePanel.Top = 0;
            _parent.taProductionCalendarAsidePanel.Height = _parent.taTabControl.Height;

            _parent.taProductionScheduleCalendar.Left = _parent.taProductionCalendarAsidePanel.Width + ConstDefine.defaultGap;
            _parent.taProductionScheduleCalendar.Top = ConstDefine.defaultGap;
            _parent.taProductionScheduleCalendar.Width = _parent.taTabControl.Width - (_parent.taProductionCalendarAsidePanel.Width + (ConstDefine.defaultGap * 2));
            _parent.taProductionScheduleCalendar.Height = _parent.taTabControl.Height - (_parent.taTodayProductionSchedulePanel.Height + (ConstDefine.defaultGap * 2));

            _parent.taTodayProductionSchedulePanel.Left = _parent.taProductionScheduleCalendar.Left;
            _parent.taTodayProductionSchedulePanel.Top = _parent.taProductionScheduleCalendar.Bottom + ConstDefine.defaultGap;
            _parent.taTodayProductionSchedulePanel.Width = _parent.taProductionScheduleCalendar.Width;

            _parent.taProductionScheduleDataGridVeiw.Top = 10;
            _parent.taProductionScheduleDataGridVeiw.Width = _parent.taTodayProductionSchedulePanel.Width - _parent.taProductionScheduleDataGridVeiw.Left;
            _parent.taProductionScheduleDataGridVeiw.Height = _parent.taTodayProductionSchedulePanel.Height - (ConstDefine.defaultGap * 2);

            gridWidth = _parent.taProductionScheduleDataGridVeiw.Width - ConstDefine.scrollSize;
            colWidth = gridWidth / _parent.taProductionScheduleDataGridVeiw.Columns.Count;
            for (int i = 0; i < _parent.taProductionScheduleDataGridVeiw.Columns.Count; i++)
            {
                _parent.taProductionScheduleDataGridVeiw.Columns[i].Width = colWidth;
            }
            ////



            //
            // 생산예측
            _parent.taProductionPredictionAsidePanel.Left = 0;
            _parent.taProductionPredictionAsidePanel.Top = 0;
            _parent.taProductionPredictionAsidePanel.Height = _parent.taTabControl.Height;

            _parent.taProductionPredictionTopInpuPanel.Left = _parent.taProductionPredictionAsidePanel.Width;
            _parent.taProductionPredictionTopInpuPanel.Top = 0;
            _parent.taProductionPredictionTopInpuPanel.Width = _parent.taTabControl.Width - _parent.taProductionPredictionAsidePanel.Width;

            _parent.taProductionPredictionMaterialDataView.Left = _parent.taProductionPredictionTopInpuPanel.Left + ConstDefine.defaultGap;
            _parent.taProductionPredictionMaterialDataView.Top = _parent.taProductionPredictionTopInpuPanel.Height;
            _parent.taProductionPredictionMaterialDataView.Width = _parent.taProductionPredictionTopInpuPanel.Width - (ConstDefine.defaultGap * 2);
            _parent.taProductionPredictionMaterialDataView.Height = _parent.taTabControl.Height - (_parent.taProductionPredictionTopInpuPanel.Height + (ConstDefine.defaultGap));

            _parent.taOeeCurrentTimeLabel.Left = _parent.taOeeTabControl.Width - (_parent.taOeeCurrentTimeLabel.Width + ConstDefine.defaultGap);


            gridWidth = _parent.taProductionPredictionMaterialDataView.Width - ConstDefine.scrollSize;
            colWidth = gridWidth / (_parent.taProductionPredictionMaterialDataView.Columns.Count - 1);
            totalColWidth = 0;
            for (int i = 0; i < _parent.taProductionPredictionMaterialDataView.Columns.Count; i++)
            {
                if ((int)ConstDefine.eProductionPredictionMaterialListView.no == i)
                    _parent.taProductionPredictionMaterialDataView.Columns[i].Width = 50;
                else if ((int)ConstDefine.eProductionPredictionMaterialListView.fpsValue == i)
                    _parent.taProductionPredictionMaterialDataView.Columns[i].Width = 0;
                else
                    _parent.taProductionPredictionMaterialDataView.Columns[i].Width = colWidth;

                totalColWidth += _parent.taProductionPredictionMaterialDataView.Columns[i].Width;
            }
            if (gridWidth > totalColWidth)
                _parent.taProductionPredictionMaterialDataView.Columns[(int)ConstDefine.eProductionPredictionMaterialListView.description].Width += (gridWidth - totalColWidth);
            ////

            ////
            // KPI
            _parent.taKpiAsidePanel.Left = 0;
            _parent.taKpiAsidePanel.Top = 0;
            _parent.taKpiAsidePanel.Height = _parent.taTabControl.Height;
            _parent.taKpiCurrentTimeLabel.Left = _parent.taTabControl.Width - (_parent.taKpiCurrentTimeLabel.Width + ConstDefine.defaultGap);

            //_parent.tjKpiPanel.Left = ((_parent.taTabControl.Width / 2) - (_parent.tjKpiPanel.Width / 2)) + (_parent.taKpiAsidePanel.Width / 2);
            _parent.tjKpiPanel.Left = _parent.taKpiAsidePanel.Right + 60;
            _parent.tjKpiPanel.Top = (_parent.taTabControl.Height / 2) - (_parent.tjKpiPanel.Height / 2);
            _parent.tjKpiPanel.Width = _parent.taTabControl.Width - (_parent.taKpiAsidePanel.Right + (60 * 2));

            _parent.watingProgressBar.Left = _parent.taKpiDataGridView.Left + _parent.tjKpiPanel.Left;
            _parent.watingProgressBar.Width = _parent.taKpiCurrentTimeLabel.Left - (_parent.watingProgressBar.Left + 20);


            _parent.taKpiDataGridView.Width = _parent.tjKpiPanel.Width - _parent.taKpiDataGridView.Left;


            if (_parent.taKpiDataGridView.Columns.Count != 0)
            {
                int nameColWidth = 100;
                int codeColWidth = 100;
                //int descriptionColWidth = 250; 
                int progressBarColWidth = 80;
                int lossColWidth = 60;

                _parent.taKpiDataGridView.Columns[(int)eKpiGridView.NAME].Width = nameColWidth;
                _parent.taKpiDataGridView.Columns[(int)eKpiGridView.CODE].Width = codeColWidth;
                //_parent.taKpiDataGridView.Columns[(int)eKpiGridView.DESCRIPTION].Width = ;
                _parent.taKpiDataGridView.Columns[(int)eKpiGridView.PROGRESS_BAR].Width = progressBarColWidth;
                _parent.taKpiDataGridView.Columns[(int)eKpiGridView.LOSS].Width = lossColWidth;

                int descriptionColWidth = _parent.taKpiDataGridView.Width - (nameColWidth + codeColWidth + progressBarColWidth + lossColWidth);
                if (descriptionColWidth < 0)
                    descriptionColWidth = 0;
                _parent.taKpiDataGridView.Columns[(int)eKpiGridView.DESCRIPTION].Width = descriptionColWidth;
            }


            //COV

            _parent.taCovAsidePanel.Left = 0;
            _parent.taCovAsidePanel.Top = 0;
            _parent.taCovAsidePanel.Height = _parent.taTabControl.Height;

            //_parent.tjCovPanel.Left = _parent.taCovAsidePanel.Right + 60;
            //_parent.tjCovPanel.Top = (_parent.taTabControl.Height / 2) - (_parent.tjCovPanel.Height / 2);
            //_parent.tjCovPanel.Width = _parent.taTabControl.Width - (_parent.taCovAsidePanel.Right + (60 * 2)); 


            _parent.tjCovPanel.Left = ((_parent.taTabControl.Width / 2) - (_parent.tjCovPanel.Width / 2)) + (_parent.taCovAsidePanel.Width / 2);
            _parent.tjCovPanel.Top = (_parent.taTabControl.Height / 2) - (_parent.tjCovPanel.Height / 2);

        }

        public void LoadTaData()
        {
            if (_parent.machineStatusTimer.Enabled == true)
                _parent.machineStatusTimer.Stop();

            if (_parent.kpiStatusTimer.Enabled == true)
                _parent.kpiStatusTimer.Stop();


            if (_parent.taTabControl.SelectedIndex == (int)ConstDefine.eTjAssetTab.oee)
            {
                StartMachineStatusTimer();
            }
            else if (_parent.taTabControl.SelectedIndex == (int)ConstDefine.eTjAssetTab.productionPlan)
            {
                LoadProductionPlan();
            }
            else if (_parent.taTabControl.SelectedIndex == (int)ConstDefine.eTjAssetTab.productionCalender)
            {
                LoadProductionPredictionCalendarList();
                LoadProductionPredictionScheduleList();
                SetSelectedProductionScheduleEventDay();
            }
            else if (_parent.taTabControl.SelectedIndex == (int)ConstDefine.eTjAssetTab.productionPrediction)
            {
                LoadProductionPrediction();
            }
            else if (_parent.taTabControl.SelectedIndex == (int)ConstDefine.eTjAssetTab.kpi)
            {
                StartKpiStatusTimer();
            }
            else if (_parent.taTabControl.SelectedIndex == (int)ConstDefine.eTjAssetTab.Cov)
            {
                changeLoadCovList();
            }
        }

        public void LayoutMouseMove()
        {
            /*
            if (InArea(_parent.taOeeTj01AreaPanel) == true || InArea(_parent.taOeeTj02AreaPanel) == true ||
                InArea(_parent.taOeeTj03AreaPanel) == true || InArea(_parent.taOeeTj04AreaPanel) == true ||
                InArea(_parent.taOeeTj05AreaPanel) == true || InArea(_parent.taOeeTj21AreaPanel) == true ||
                InArea(_parent.taOeeTj22AreaPanel) == true)
            {
                if (Cursor.Current != Cursors.Hand)
                    _parent.Cursor = Cursors.Hand;
            }
            else
            {
                if (Cursor.Current != Cursors.Arrow)
                    _parent.Cursor = Cursors.Arrow;
            }
            */

            if (Utils.InArea(_parent.taOeeTj01AreaPanel, _parent.taOeeLayoutPictureBox) == true || Utils.InArea(_parent.taOeeTj02AreaPanel, _parent.taOeeLayoutPictureBox) == true ||
              Utils.InArea(_parent.taOeeTj03AreaPanel, _parent.taOeeLayoutPictureBox) == true || Utils.InArea(_parent.taOeeTj04AreaPanel, _parent.taOeeLayoutPictureBox) == true ||
              Utils.InArea(_parent.taOeeTj05AreaPanel, _parent.taOeeLayoutPictureBox) == true || Utils.InArea(_parent.taOeeTj21AreaPanel, _parent.taOeeLayoutPictureBox) == true ||
              Utils.InArea(_parent.taOeeTj22AreaPanel, _parent.taOeeLayoutPictureBox) == true)
            {
                if (Cursor.Current != Cursors.Hand)
                    _parent.Cursor = Cursors.Hand;
            }
            else
            {
                if (Cursor.Current != Cursors.Arrow)
                    _parent.Cursor = Cursors.Arrow;
            }
        }

        public void LayoutClick()
        {
            Panel[] layouts = {_parent.taOeeTj01AreaPanel, _parent.taOeeTj02AreaPanel, _parent.taOeeTj03AreaPanel, _parent.taOeeTj04AreaPanel, 
            _parent.taOeeTj05AreaPanel, _parent.taOeeTj21AreaPanel, _parent.taOeeTj22AreaPanel};

            for (int i = 0; i < layouts.Length; i++)
            {
                if (Utils.InArea(layouts[i], _parent.taOeeLayoutPictureBox) == true)
                {
                    _parent.Cursor = Cursors.Arrow;

                    if (layouts[i].Name == _parent.taOeeTj01AreaPanel.Name || layouts[i].Name == _parent.taOeeTj02AreaPanel.Name || layouts[i].Name == _parent.taOeeTj03AreaPanel.Name ||
                        layouts[i].Name == _parent.taOeeTj04AreaPanel.Name | layouts[i].Name == _parent.taOeeTj05AreaPanel.Name)
                        _parent.tjOeeSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.bc;
                    else
                        _parent.tjOeeSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;

                    ResetOeeButtons(layouts[i].Tag.ToString());
                    StartMachineStatusTimer();
                    break;
                }
            }
        }

        public void ResetKpiButtons(string currentName)
        {
            CustomControls.ButtonEx01[] team1ButtonItems = { _parent.tjKpiTj01Button, _parent.tjKpiTj02Button, _parent.tjKpiTj03Button,
                                     _parent.tjKpiTj04Button, _parent.tjKpiTj05Button};

            CustomControls.ButtonEx01[] team2ButtonItems = { _parent.tjKpiTj21Button, _parent.tjKpiTj22Button, _parent.tjKpiTj23Button };

            CustomControls.ButtonEx01[] buttonItems = _parent.tjKpiSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc ? team1ButtonItems : team2ButtonItems;

            for (int i = 0; i < buttonItems.Length; i++)
            {
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;
            }
        }

        public void ResetCovButtons(string currentName)
        {
            CustomControls.ButtonEx01[] team1ButtonItems = { _parent.tjCovTj01Button, _parent.tjCovTj02Button, 
                                     _parent.tjCovTj04Button};

            CustomControls.ButtonEx01[] team2ButtonItems = { _parent.tjCovTj21Button, _parent.tjCovTj22Button, _parent.tjCovTj23Button };

            CustomControls.ButtonEx01[] buttonItems = _parent.tjCovSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc ? team1ButtonItems : team2ButtonItems;

            for (int i = 0; i < buttonItems.Length; i++)
            {
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;
            }
        }

        public void LoadKpi()
        {
            ClearKpi();

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} '{1}'", "EXEC SelectKpiStatus", GetSelectedKpiTjTypeValue()));

            _parent.taKpiCurrentTimeLabel.Text = DateTime.Now.ToString("yyyy.MM.dd.dddd hh:mm:ss tt");

            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            /*
             * 
             * 	SELECT [TjType]
		  ,[Speed]
		  ,[McStopCount]
		  ,[McDelayCount]
		  ,[CurrentOee]
		  ,[TargetOee]
		  ,[CurrentProductivity]
		  ,[TargetProductivity]
		  ,[CurrentMWaste]
		  ,[TargetMWaste]
		  ,[CurrentProductCode]
		  ,[CurrentProductName]
		  ,[CurrentProductCase]
		  ,[TargetProductCase]

            */

            ////
            // Product Case
            _parent.currentProductCodeLabel.Text = Utils.GetString(dataSet.Tables[0].Rows[0]["CurrentProductCode"]);
            _parent.currentProductNameLabel.Text = Utils.GetString(dataSet.Tables[0].Rows[0]["CurrentProductName"]);
            _parent.currentProductCaseLabel.Text = Utils.GetString(dataSet.Tables[0].Rows[0]["CurrentProductCase"]);
            _parent.targetProductCaseLabel.Text = Utils.GetString(dataSet.Tables[0].Rows[0]["TargetProductCase"]);
            int currentProductCase = Utils.GetIntValue(_parent.currentProductCaseLabel.Text);
            int targetProductCase = Utils.GetIntValue(_parent.targetProductCaseLabel.Text);
            _parent.remainedProductCaseLabel.Text = (targetProductCase - currentProductCase).ToString();
            int productCompletePercent = 0;
            if (targetProductCase != 0)
                productCompletePercent = (int)(((double)currentProductCase / (double)targetProductCase) * 100);

            _parent.productCompletePercentLabel.Text = productCompletePercent.ToString();
            _parent.productCompleteProgressBar.MinValue = 0;
            _parent.productCompleteProgressBar.MaxValue = 100;
            if (productCompletePercent > 100)
                _parent.productCompleteProgressBar.Value = 100;
            else if (productCompletePercent > 0)
                _parent.productCompleteProgressBar.Value = productCompletePercent;
            else
                _parent.productCompleteProgressBar.Value = 0;

            ////
            // Speed
            _parent.taKpiSpeedGauge.MinValue = 0;
            _parent.taKpiSpeedGauge.MaxValue = 100;
            int currentSpeed = (int)(Utils.GetDoubleValue(Utils.GetString(dataSet.Tables[0].Rows[0]["Speed"])) * 0.1);
            if (currentSpeed > _parent.taKpiSpeedGauge.MaxValue)
                _parent.taKpiSpeedGauge.MaxValue = currentSpeed;
            _parent.taKpiSpeedGauge.Value = currentSpeed;
            _parent.taKpiSpeedLabel.Text = Utils.GetString(dataSet.Tables[0].Rows[0]["Speed"]);

            ////
            // M/C Status
            _parent.taKpiMcStopCountLabel.Text = Utils.GetString(dataSet.Tables[0].Rows[0]["McStopCount"]);
            _parent.taKpiMcDelayMinuteLabel.Text = Utils.GetString(dataSet.Tables[0].Rows[0]["McDelayCount"]);


            /*
            ////
            // Running Time
            string runningTime = Utils.GetString(dataSet.Tables[0].Rows[0]["RunningTime"]);
            if(runningTime.Length == 8)
            {
                string [] runningTimeArrary = runningTime.Split(':');
                if(runningTimeArrary.Length == 3)
                {
                    _parent.taKpiRunningTimeHour01Label.Text = runningTimeArrary[0].Substring(0, 1); 
                    _parent.taKpiRunningTimeHour02Label.Text = runningTimeArrary[0].Substring(1, 1); 
                    _parent.taKpiRunningTimeMinute01Label.Text = runningTimeArrary[1].Substring(0, 1); 
                    _parent.taKpiRunningTimeMinute02Label.Text = runningTimeArrary[1].Substring(1, 1); 
                    _parent.taKpiRunningTimeSecond01Label.Text = runningTimeArrary[2].Substring(0, 1); 
                    _parent.taKpiRunningTimeSecond02Label.Text = runningTimeArrary[2].Substring(1, 1); 
                }
            }
            ////
            // Remaining Time
            string remainingTime = Utils.GetString(dataSet.Tables[0].Rows[0]["RemainingTime"]);
            if(remainingTime.Length == 8)
            {
                string [] remainingTimeArray = remainingTime.Split(':');
                if(remainingTimeArray.Length == 3)
                {
                    _parent.taKpiRemainingTimeHour01Label.Text = remainingTimeArray[0].Substring(0, 1); 
                    _parent.taKpiRemainingTimeHour02Label.Text = remainingTimeArray[0].Substring(1, 1); 
                    _parent.taKpiRemainingTimeMinute01Label.Text = remainingTimeArray[1].Substring(0, 1); 
                    _parent.taKpiRemainingTimeMinute02Label.Text = remainingTimeArray[1].Substring(1, 1); 
                    _parent.taKpiRemainingTimeSecond01Label.Text = remainingTimeArray[2].Substring(0, 1); 
                    _parent.taKpiRemainingTimeSecond02Label.Text = remainingTimeArray[2].Substring(1, 1); 
                }
            }
            */

            ////
            // OEE
            double currentOee = Utils.GetDoubleValue(Utils.GetString(dataSet.Tables[0].Rows[0]["CurrentOee"]));
            double targetOee = Utils.GetDoubleValue(Utils.GetString(dataSet.Tables[0].Rows[0]["TargetOee"]));
            _parent.taKpiCurrentOeeLabel.Text = (currentOee >= 100) ? "100" : currentOee.ToString();
            _parent.taKpiTargetOeeLabel.Text = (targetOee >= 100) ? "100" : targetOee.ToString();
            _parent.taKpiOeeProgressBar.Minimum = 0;
            _parent.taKpiOeeProgressBar.Maximum = 100;
            double diffOee = currentOee - targetOee;
            /*
            if (diffOee > 0)
                _parent.taKpiOeeProgressBar.Value = (diffOee > 100) ? 100 : (int)diffOee;
            else
                _parent.taKpiOeeProgressBar.Value = 0;
            */
            if (Convert.ToInt32(Math.Round(currentOee)) > 100)
                _parent.taKpiOeeProgressBar.Value = 100;
            else if (Convert.ToInt32(Math.Round(currentOee)) < 0)
                _parent.taKpiOeeProgressBar.Value = 0;
            else
                _parent.taKpiOeeProgressBar.Value = Convert.ToInt32(Math.Round(currentOee));

            if (Convert.ToInt32(Math.Round(targetOee)) > 100)
                _parent.taKpiOeeProgressBar.lineValue = 100;
            else if (Convert.ToInt32(Math.Round(targetOee)) < 0)
                _parent.taKpiOeeProgressBar.lineValue = 0;
            else
                _parent.taKpiOeeProgressBar.lineValue = Convert.ToInt32(Math.Round(targetOee));
            _parent.taKpiDiffOeeLabel.Text = Utils.GetDecimalString(string.Format("{0:f1}", diffOee));
            _parent.taKpiDiffOeePictureBox.BackgroundImage = (diffOee >= 0) ? Properties.Resources.kpi_oee_plus : Properties.Resources.kpi_oee_minus;
            ////

            ////
            // Productivity
            double currentProductivity = Utils.GetDoubleValue(Utils.GetString(dataSet.Tables[0].Rows[0]["CurrentProductivity"]));
            double targetProductivity = Utils.GetDoubleValue(Utils.GetString(dataSet.Tables[0].Rows[0]["TargetProductivity"]));
            _parent.taKpiCurrentProductivityLabel.Text = (currentProductivity >= 100) ? "100" : currentProductivity.ToString();
            _parent.taKpiTargetProductivityLabel.Text = (targetProductivity >= 100) ? "100" : targetProductivity.ToString();
            _parent.taKpiProductivityProgressBar.Minimum = 0;
            //_parent.taKpiProductivityProgressBar.Maximum = 100;
            _parent.taKpiProductivityProgressBar.Maximum = 55;
            double diffProductivity = currentProductivity - targetProductivity;
            /*
            if (diffProductivity > 0)
                _parent.taKpiProductivityProgressBar.Value = (diffProductivity > 100) ? 100 : (int)diffProductivity;
            else
                _parent.taKpiProductivityProgressBar.Value = 0;
            */
            if (Convert.ToInt32(Math.Round(currentProductivity)) > 55)
                _parent.taKpiProductivityProgressBar.Value = 55;
            else if (Convert.ToInt32(Math.Round(currentProductivity)) < 0)
                _parent.taKpiProductivityProgressBar.Value = 0;
            else
                _parent.taKpiProductivityProgressBar.Value = Convert.ToInt32(Math.Round(currentProductivity));

            if (Convert.ToInt32(Math.Round(targetProductivity)) > 55)
                _parent.taKpiProductivityProgressBar.lineValue = 55;
            else if (Convert.ToInt32(Math.Round(targetProductivity)) < 0)
                _parent.taKpiProductivityProgressBar.lineValue = 0;
            else
                _parent.taKpiProductivityProgressBar.lineValue = Convert.ToInt32(Math.Round(targetProductivity));
            _parent.taKpiDiffProductivityLabel.Text = Utils.GetDecimalString(string.Format("{0:f1}", diffProductivity));
            _parent.taKpiDiffProductivityPictureBox.BackgroundImage = (diffProductivity >= 0) ? Properties.Resources.kpi_oee_plus : Properties.Resources.kpi_oee_minus;

            ////
            //M/Waste
            double currentMWaste = Utils.GetDoubleValue(Utils.GetString(dataSet.Tables[0].Rows[0]["CurrentMWaste"]));
            double targetMWaste = Utils.GetDoubleValue(Utils.GetString(dataSet.Tables[0].Rows[0]["TargetMWaste"]));
            _parent.taKpiCurrentMWasteLabel.Text = (currentMWaste >= 100) ? "100" : currentMWaste.ToString();
            _parent.taKpiTargetMWasteLabel.Text = (targetMWaste >= 100) ? "100" : targetMWaste.ToString();
            _parent.taKpiMWasteProgressBar.Minimum = 0;
            _parent.taKpiMWasteProgressBar.Maximum = 10;
            double diffMWaste = currentMWaste - targetMWaste;
            /*
            if (diffMWaste > 0)
                _parent.taKpiMWasteProgressBar.Value = (diffMWaste > 100) ? 100 : (int)diffMWaste;
            else
                _parent.taKpiMWasteProgressBar.Value = 0;
            */
            if (Convert.ToInt32(Math.Round(currentMWaste)) > 10)
                _parent.taKpiMWasteProgressBar.Value = 10;
            else if (Convert.ToInt32(Math.Round(currentMWaste)) < 0)
                _parent.taKpiMWasteProgressBar.Value = 0;
            else
                _parent.taKpiMWasteProgressBar.Value = Convert.ToInt32(Math.Round(currentMWaste));

            if (Convert.ToInt32(Math.Round(targetMWaste)) > 10)
                _parent.taKpiMWasteProgressBar.lineValue = 10;
            else if (Convert.ToInt32(Math.Round(targetMWaste)) < 0)
                _parent.taKpiMWasteProgressBar.lineValue = 0;
            else
                _parent.taKpiMWasteProgressBar.lineValue = Convert.ToInt32(Math.Round(targetMWaste));
            _parent.taKpiDiffMWasteLabel.Text = Utils.GetDecimalString(string.Format("{0:f1}", diffMWaste));
            _parent.taKpiDiffMWastePictureBox.BackgroundImage = (diffMWaste >= 0) ? Properties.Resources.kpi_oee_plus : Properties.Resources.kpi_oee_minus;


            string productCode = _parent.currentProductCodeLabel.Text.Trim();
            _parent.taKpiDataGridView.Rows.Clear();
            if (productCode != "")
            {
                dataSet = DbHelper.SelectQuery(string.Format("{0} {1}, '{2}'", "EXEC SelectKpiTotalWaste", GetSelectedKpiTjTypeValue(), productCode));
                if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                    return;


                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    float loss = (float)Utils.GetDoubleValue(dataRow["WasteLoss"]);
                    loss = 100 - loss;
                    _parent.taKpiDataGridView.Rows.Add(
                        Utils.GetString(dataRow["ProcMnem"]),
                        Utils.GetString(dataRow["CompCode"]),
                        Utils.GetString(dataRow["Description"]),
                        (int)loss,
                        String.Format("{0:0.00}", loss));

                    /*
                    ////
                    // Test
                    for(int i=0; i<9; i++)
                    _parent.taKpiDataGridView.Rows.Add(
    Utils.GetString(dataRow["ProcMnem"]),
    Utils.GetString(dataRow["CompCode"]),
    Utils.GetString(dataRow["Description"]),
    (int)80);
                    /////
                    */
                }

                _parent.taKpiDataGridView.ClearSelection();
            }



        }

        private void ClearKpi()
        {
            _parent.currentProductCodeLabel.Text = "";
            _parent.currentProductNameLabel.Text = "";
            _parent.currentProductCaseLabel.Text = "0";
            _parent.targetProductCaseLabel.Text = "0";
            _parent.remainedProductCaseLabel.Text = "0";
            _parent.productCompletePercentLabel.Text = "0";
            _parent.productCompleteProgressBar.Value = 0;


            _parent.taKpiSpeedGauge.Value = 0;
            _parent.taKpiSpeedLabel.Text = "0";
            _parent.taKpiMcStopCountLabel.Text = "0";
            _parent.taKpiMcDelayMinuteLabel.Text = "0";

            /*
            _parent.taKpiRunningTimeHour01Label.Text = ""; 
            _parent.taKpiRunningTimeHour02Label.Text = ""; 
            _parent.taKpiRunningTimeMinute01Label.Text = "";
            _parent.taKpiRunningTimeMinute02Label.Text = ""; 
            _parent.taKpiRunningTimeSecond01Label.Text = "";
            _parent.taKpiRunningTimeSecond02Label.Text = ""; 

            _parent.taKpiRemainingTimeHour01Label.Text = ""; 
            _parent.taKpiRemainingTimeHour02Label.Text = ""; 
            _parent.taKpiRemainingTimeMinute01Label.Text = ""; 
            _parent.taKpiRemainingTimeMinute02Label.Text = ""; 
            _parent.taKpiRemainingTimeSecond01Label.Text = ""; 
            _parent.taKpiRemainingTimeSecond02Label.Text = ""; 
            */

            _parent.taKpiCurrentOeeLabel.Text = "0";
            _parent.taKpiTargetOeeLabel.Text = "0";
            _parent.taKpiDiffOeeLabel.Text = "0";
            _parent.taKpiCurrentProductivityLabel.Text = "0";
            _parent.taKpiTargetProductivityLabel.Text = "0";
            _parent.taKpiDiffProductivityLabel.Text = "0";
            _parent.taKpiCurrentMWasteLabel.Text = "0";
            _parent.taKpiTargetMWasteLabel.Text = "0";
            _parent.taKpiDiffMWasteLabel.Text = "0";


            _parent.taKpiDataGridView.Rows.Clear();
        }

        public void InitKpiGridView()
        {
            //const int MAX_ROW = 13;
            const int MAX_ROW = 14;
            _parent.taKpiDataGridView.Columns.Add("", "NAME");
            _parent.taKpiDataGridView.Columns.Add("", "CODE");
            _parent.taKpiDataGridView.Columns.Add("", "DESCRIPTION");
            ControlClasses.DataGridViewProgressColumn column = new ControlClasses.DataGridViewProgressColumn();
            _parent.taKpiDataGridView.Columns.Add(column);
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.PROGRESS_BAR].HeaderText = "PROGRESS";
            _parent.taKpiDataGridView.Columns.Add("", "");

            Color GRID_COLUMN_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_COLUMN_BACK_COLOR = Color.FromArgb(58, 65, 74);
            Color GRID_ROW_FORE_COLOR = Color.FromArgb(216, 217, 218);
            //Color GRID_ROW_BACK_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_ROW_BACK_COLOR = Color.FromArgb(55, 63, 71);
            Color GRID_COLOR = Color.FromArgb(108, 115, 123);
            //Color GRID_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_BACK_COLOR = Color.FromArgb(60, 69, 80);

            int GRID_COLUMN_HEIGHT = _parent.taKpiDataGridView.Height / MAX_ROW;
            int GRID_ROW_HEIGHT = _parent.taKpiDataGridView.Height / MAX_ROW;
            if (GRID_ROW_HEIGHT * MAX_ROW < _parent.taKpiDataGridView.Height)
                GRID_COLUMN_HEIGHT += (_parent.taKpiDataGridView.Height - (GRID_ROW_HEIGHT * MAX_ROW));

            // Default column style
            _parent.taKpiDataGridView.ColumnHeadersVisible = true;
            _parent.taKpiDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.taKpiDataGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.taKpiDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 10, FontStyle.Bold);
            _parent.taKpiDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.taKpiDataGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.taKpiDataGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.taKpiDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taKpiDataGridView.AllowUserToResizeColumns = false;
            _parent.taKpiDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.taKpiDataGridView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taKpiDataGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.taKpiDataGridView.RowHeadersVisible = false;

            // Default row style
            //_parent.taKpiDataGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 10, FontStyle.Regular);
            //_parent.taKpiDataGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.taKpiDataGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.taKpiDataGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.taKpiDataGridView.AllowUserToResizeRows = false;
            _parent.taKpiDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            //_parent.taKpiDataGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //_parent.dataGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            //_parent.dataGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None; 

            // Each column style 
            //dataGridView.Columns[(int)eDailyTestDataGridView.ID].Visible = false;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.NAME].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.CODE].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.DESCRIPTION].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.PROGRESS_BAR].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taKpiDataGridView.Columns[(int)(int)eKpiGridView.LOSS].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //_parent.taKpiDataGridView.Columns[(int)eKpiGridView.NAME].DefaultCellStyle.ForeColor = Color.FromArgb(43, 157, 184); 
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.NAME].DefaultCellStyle.ForeColor = Color.FromArgb(205, 207, 208);
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.CODE].DefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.DESCRIPTION].DefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.PROGRESS_BAR].DefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.LOSS].DefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;

            Font basicFont = new Font("BareunDotum 1", 10, FontStyle.Regular);
            Font boldFont = new Font("BareunDotum 1", 10, FontStyle.Bold);
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.NAME].DefaultCellStyle.Font = boldFont;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.CODE].DefaultCellStyle.Font = basicFont;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.DESCRIPTION].DefaultCellStyle.Font = basicFont;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.PROGRESS_BAR].DefaultCellStyle.Font = basicFont;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.LOSS].DefaultCellStyle.Font = basicFont;


            // Common style
            _parent.taKpiDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.taKpiDataGridView.GridColor = GRID_COLOR;
            _parent.taKpiDataGridView.BackgroundColor = GRID_ROW_BACK_COLOR; // GRID_BACK_COLOR;         // BackgroundColor 
            _parent.taKpiDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            //dataGridView.ReadOnly = true;
            _parent.taKpiDataGridView.ScrollBars = ScrollBars.Vertical;


            _parent.taKpiDataGridView.Width = _parent.tjKpiPanel.Width - _parent.taKpiDataGridView.Left;

            int nameColWidth = 100;
            int codeColWidth = 100;
            //int descriptionColWidth = 250; 
            int progressBarColWidth = 80;
            int lossColWidth = 60;

            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.NAME].Width = nameColWidth;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.CODE].Width = codeColWidth;
            //_parent.taKpiDataGridView.Columns[(int)eKpiGridView.DESCRIPTION].Width = ;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.PROGRESS_BAR].Width = progressBarColWidth;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.LOSS].Width = lossColWidth;

            int descriptionColWidth = _parent.taKpiDataGridView.Width - (nameColWidth + codeColWidth + progressBarColWidth + lossColWidth);
            if (descriptionColWidth < 0)
                descriptionColWidth = 0;
            _parent.taKpiDataGridView.Columns[(int)eKpiGridView.DESCRIPTION].Width = descriptionColWidth;


        }

        private int GetSelectedKpiTjTypeValue()
        {
            int tjTypeValue = 21;

            if (_parent.tjKpiSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                if (_parent.tjKpiTj01Button.Checked == true)
                    tjTypeValue = 1;
                else if (_parent.tjKpiTj02Button.Checked == true)
                    tjTypeValue = 2;
                else if (_parent.tjKpiTj03Button.Checked == true)
                    tjTypeValue = 3;
                else if (_parent.tjKpiTj04Button.Checked == true)
                    tjTypeValue = 4;
                else if (_parent.tjKpiTj05Button.Checked == true)
                    tjTypeValue = 5;
                else
                    tjTypeValue = 1;
            }
            else
            {
                if (_parent.tjKpiTj21Button.Checked == true)
                    tjTypeValue = 21;
                else if (_parent.tjKpiTj22Button.Checked == true)
                    tjTypeValue = 22;
                else if (_parent.tjKpiTj23Button.Checked == true)
                    tjTypeValue = 23;
                else
                    tjTypeValue = 21;
            }

            return tjTypeValue;
        }

        private int GetSelectedCovTjTypeValue()
        {
            int tjTypeValue = 21;

            if (_parent.tjCovSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                if (_parent.tjCovTj01Button.Checked == true)
                    tjTypeValue = 1;
                else if (_parent.tjCovTj02Button.Checked == true)
                    tjTypeValue = 2;
                else if (_parent.tjCovTj04Button.Checked == true)
                    tjTypeValue = 4;
                else
                    tjTypeValue = 1;
            }
            else
            {
                if (_parent.tjCovTj21Button.Checked == true)
                    tjTypeValue = 21;
                else if (_parent.tjCovTj22Button.Checked == true)
                    tjTypeValue = 22;
                else if (_parent.tjCovTj23Button.Checked == true)
                    tjTypeValue = 23;
                else
                    tjTypeValue = 21;
            }

            return tjTypeValue;
        }

        private string GetCovStringDate(DateTimePicker dtp)
        {
            string st;
            st = dtp.Value.ToString("yyyy-MM-dd HH:mm:ss");
            return st;
        }

        public void ResetCovEndDateTime()
        {
            if (_parent.rtCovDayRadioButton.Checked)
            {
                DateTime tempDate = _parent.rtCovStartDateTimePicker.Value.AddDays(1);
                _parent.rtCovEndDateTimePicker.Value = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 6, 59, 59);
            }
            else
            {
                return;
            }
        }

        public bool IsValidCov()
        {
            if (_parent.rtCovStartDateTimePicker.Value > _parent.rtCovEndDateTimePicker.Value)
                return false;

            return true;
        }

        private void InitOeeTotalMachineStatusGridView()
        {
            Color GRID_COLUMN_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_COLUMN_BACK_COLOR = Color.FromArgb(58, 65, 74);
            Color GRID_ROW_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_ROW_BACK_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_COLOR = Color.FromArgb(108, 115, 123);
            //Color GRID_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_BACK_COLOR = Color.FromArgb(60, 69, 80);

            const int GRID_COLUMN_HEIGHT = 35;
            const int GRID_ROW_HEIGHT = 35;

            // Default column style
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersVisible = true;
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.taOeeTotalMachineStatusGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taOeeTotalMachineStatusGridView.AllowUserToResizeColumns = true;
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taOeeTotalMachineStatusGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.taOeeTotalMachineStatusGridView.RowHeadersVisible = false;

            // Default row style
            _parent.taOeeTotalMachineStatusGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.taOeeTotalMachineStatusGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.taOeeTotalMachineStatusGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.taOeeTotalMachineStatusGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.taOeeTotalMachineStatusGridView.AllowUserToResizeRows = false;
            //_parent.taOeeTotalMachineStatusGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.taOeeTotalMachineStatusGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taOeeTotalMachineStatusGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taOeeTotalMachineStatusGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taOeeTotalMachineStatusGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taOeeTotalMachineStatusGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.taOeeTotalMachineStatusGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.taOeeTotalMachineStatusGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.taOeeTotalMachineStatusGridView.GridColor = GRID_COLOR;
            _parent.taOeeTotalMachineStatusGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.taOeeTotalMachineStatusGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.taOeeTotalMachineStatusGridView.MultiSelect = false;
            _parent.taOeeTotalMachineStatusGridView.ReadOnly = true;
            _parent.taOeeTotalMachineStatusGridView.ScrollBars = ScrollBars.None;


            /*
            _parent.taOeeTotalMachineStatusGridView.Height = GRID_COLUMN_HEIGHT + (GRID_ROW_HEIGHT * 13);
            int colWidth = _parent.taOeeTotalMachineStatusGridView.Width / 3;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eSapButtListview.processCode].Width = colWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eSapButtListview.processName].Width = colWidth;
            _parent.taOeeTotalMachineStatusGridView.Columns[(int)ConstDefine.eSapButtListview.butt].Width = _parent.taOeeTotalMachineStatusGridView.Width - (colWidth * 2);
            */

            _parent.taOeeTotalMachineStatusGridView.Refresh();
        }

        public void InitProductPlanGridView()
        {
            Color GRID_COLUMN_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_COLUMN_BACK_COLOR = Color.FromArgb(58, 65, 74);
            Color GRID_ROW_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_ROW_BACK_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_COLOR = Color.FromArgb(108, 115, 123);
            //Color GRID_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_BACK_COLOR = Color.FromArgb(60, 69, 80);

            const int GRID_COLUMN_HEIGHT = 35;
            const int GRID_ROW_HEIGHT = 35;

            // Default column style
            _parent.taProductionPlanGridView.ColumnHeadersVisible = true;
            _parent.taProductionPlanGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.taProductionPlanGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.taProductionPlanGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.taProductionPlanGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.taProductionPlanGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.taProductionPlanGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.taProductionPlanGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taProductionPlanGridView.AllowUserToResizeColumns = true;
            _parent.taProductionPlanGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionPlanGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.taProductionPlanGridView.RowHeadersVisible = false;

            // Default row style
            _parent.taProductionPlanGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.taProductionPlanGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.taProductionPlanGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.taProductionPlanGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.taProductionPlanGridView.AllowUserToResizeRows = false;
            //_parent.taProductionPlanGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.taProductionPlanGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taProductionPlanGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionPlanGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionPlanGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionPlanGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.taProductionPlanGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.taProductionPlanGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.taProductionPlanGridView.GridColor = GRID_COLOR;
            _parent.taProductionPlanGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.taProductionPlanGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.taProductionPlanGridView.MultiSelect = false;
            _parent.taProductionPlanGridView.ReadOnly = true;
            _parent.taProductionPlanGridView.ScrollBars = ScrollBars.Vertical;


            /*
            _parent.taProductionPlanGridView.Height = GRID_COLUMN_HEIGHT + (GRID_ROW_HEIGHT * 13);
            int colWidth = _parent.taProductionPlanGridView.Width / 3;
            _parent.taProductionPlanGridView.Columns[(int)ConstDefine.eSapButtListview.processCode].Width = colWidth;
            _parent.taProductionPlanGridView.Columns[(int)ConstDefine.eSapButtListview.processName].Width = colWidth;
            _parent.taProductionPlanGridView.Columns[(int)ConstDefine.eSapButtListview.butt].Width = _parent.taProductionPlanGridView.Width - (colWidth * 2);
            */

            _parent.taProductionPlanGridView.Refresh();

        }

        public void InitProductScheduleGridView()
        {
            Color GRID_COLUMN_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_COLUMN_BACK_COLOR = Color.FromArgb(58, 65, 74);
            Color GRID_ROW_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_ROW_BACK_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_COLOR = Color.FromArgb(108, 115, 123);
            //Color GRID_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_BACK_COLOR = Color.FromArgb(60, 69, 80);

            const int GRID_COLUMN_HEIGHT = 25;
            const int GRID_ROW_HEIGHT = 25;

            // Default column style
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersVisible = true;
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.taProductionScheduleDataGridVeiw.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taProductionScheduleDataGridVeiw.AllowUserToResizeColumns = true;
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionScheduleDataGridVeiw.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.taProductionScheduleDataGridVeiw.RowHeadersVisible = false;

            // Default row style
            _parent.taProductionScheduleDataGridVeiw.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.taProductionScheduleDataGridVeiw.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.taProductionScheduleDataGridVeiw.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.taProductionScheduleDataGridVeiw.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.taProductionScheduleDataGridVeiw.AllowUserToResizeRows = false;
            //_parent.taProductionScheduleDataGridVeiw.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.taProductionScheduleDataGridVeiw.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taProductionScheduleDataGridVeiw.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionScheduleDataGridVeiw.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionScheduleDataGridVeiw.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionScheduleDataGridVeiw.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.taProductionScheduleDataGridVeiw.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.taProductionScheduleDataGridVeiw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _parent.taProductionScheduleDataGridVeiw.GridColor = GRID_COLOR;
            _parent.taProductionScheduleDataGridVeiw.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.taProductionScheduleDataGridVeiw.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.taProductionScheduleDataGridVeiw.MultiSelect = false;
            _parent.taProductionScheduleDataGridVeiw.ReadOnly = true;
            _parent.taProductionScheduleDataGridVeiw.ScrollBars = ScrollBars.Vertical;


            /*
            _parent.taProductionScheduleDataGridVeiw.Height = GRID_COLUMN_HEIGHT + (GRID_ROW_HEIGHT * 13);
            int colWidth = _parent.taProductionScheduleDataGridVeiw.Width / 3;
            _parent.taProductionScheduleDataGridVeiw.Columns[(int)ConstDefine.eSapButtListview.processCode].Width = colWidth;
            _parent.taProductionScheduleDataGridVeiw.Columns[(int)ConstDefine.eSapButtListview.processName].Width = colWidth;
            _parent.taProductionScheduleDataGridVeiw.Columns[(int)ConstDefine.eSapButtListview.butt].Width = _parent.taProductionScheduleDataGridVeiw.Width - (colWidth * 2);
            */

            _parent.taProductionScheduleDataGridVeiw.Refresh();
        }

        private void InitProductionPredictionMaterialDataView()
        {
            Color GRID_COLUMN_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_COLUMN_BACK_COLOR = Color.FromArgb(58, 65, 74);
            Color GRID_ROW_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_ROW_BACK_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_COLOR = Color.FromArgb(108, 115, 123);
            //Color GRID_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_BACK_COLOR = Color.FromArgb(60, 69, 80);

            const int GRID_COLUMN_HEIGHT = 35;
            const int GRID_ROW_HEIGHT = 35;

            // Default column style
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersVisible = true;
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.taProductionPredictionMaterialDataView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taProductionPredictionMaterialDataView.AllowUserToResizeColumns = true;
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionPredictionMaterialDataView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.taProductionPredictionMaterialDataView.RowHeadersVisible = false;

            // Default row style
            _parent.taProductionPredictionMaterialDataView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.taProductionPredictionMaterialDataView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.taProductionPredictionMaterialDataView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.taProductionPredictionMaterialDataView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.taProductionPredictionMaterialDataView.AllowUserToResizeRows = false;
            //_parent.taProductionPredictionMaterialDataView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.taProductionPredictionMaterialDataView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.taProductionPredictionMaterialDataView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionPredictionMaterialDataView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionPredictionMaterialDataView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.taProductionPredictionMaterialDataView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.taProductionPredictionMaterialDataView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.taProductionPredictionMaterialDataView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.taProductionPredictionMaterialDataView.GridColor = GRID_COLOR;
            _parent.taProductionPredictionMaterialDataView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.taProductionPredictionMaterialDataView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.taProductionPredictionMaterialDataView.MultiSelect = false;
            _parent.taProductionPredictionMaterialDataView.ReadOnly = true;
            _parent.taProductionPredictionMaterialDataView.ScrollBars = ScrollBars.Vertical;


            _parent.taProductionPredictionMaterialDataView.Columns[(int)ConstDefine.eProductionPredictionMaterialListView.fpsValue].Visible = false;

            _parent.taProductionPredictionMaterialDataView.Refresh();

        }

        public void UpdateWaitingProgressBar()
        {
            if (_parent.watingProgressBar.Value < WAITING_TIME)
                _parent.watingProgressBar.Value = _parent.watingProgressBar.Value + 1;
        }
    }
}
