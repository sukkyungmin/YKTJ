using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace RealTimeRMT
{
    class RealTimePage
    {
        private MainForm _parent = null;
        private string _reportCreatedDate = ""; 

        public RealTimePage(MainForm parent)
        {
            _parent = parent;
        }

        public void InitControls()
        {
            _parent.rtSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
            _parent.rtTeamNameLabel.Text = "유아2팀  ";

            _parent.toolTip.SetToolTip(_parent.rtRawMaterialSumButton, "Material Total Used");
            _parent.toolTip.SetToolTip(_parent.rtRawMaterialDetailSumButton, "Material Used List");
            _parent.toolTip.SetToolTip(_parent.rtProductionHistoryButton, "Records Of The Production");
            _parent.toolTip.SetToolTip(_parent.rtTotalWasteButton, "Total Waste Report"); 

            // Period
            _parent.rtPeriodDayRadioButton.Checked = true;
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(1);
            _parent.rtPeriodStartDateTimePicker.Value = new DateTime(startDate.Year, startDate.Month, startDate.Day, 7, 0, 0);
            _parent.rtPeriodEndDateTimePicker.Value = _parent.rtPeriodStartDateTimePicker.Value.AddDays(1).AddSeconds(-1);
            //_parent.rtPeriodEndDateTimePicker.Value = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            _parent.rtPeriodEndDateTimePicker.Enabled = false;

            // Report
            _parent.rtRawMaterialSumButton.Checked = true;
            ResetRtReportButtons(_parent.rtRawMaterialSumButton.Name);

            // Filter
            _parent.rtFilterTabControl.SelectedIndex = (int)ConstDefine.eRtFilterTab.rawMaterialSum;
            MoveRtCreateReportButton();

            // Load Data
            LoadProcMnemFilter((int)ConstDefine.eTjType.tj21);
            LoadEvtNameFilter((int)ConstDefine.eTjType.tj21);
        }

        public void LoadProcMnemFilter(int tjType)
        {
            
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectRealTimeProcMnemFilter", _parent.GetTjTypeValue(tjType)));
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            _parent.rtFilterProcMnemRdsComboBox.Items.Clear();
            _parent.rtFilterProcMnemTwComboBox.Items.Clear();
            _parent.rtFilterProcMnemRdsComboBox.BeginUpdate();
            _parent.rtFilterProcMnemTwComboBox.BeginUpdate(); 
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                string procCode = dataRow["ProcCode"].ToString();
                string procMnem = dataRow["ProcMnem"].ToString();

                ComboBoxItem comboBoxItem = new ComboBoxItem(procMnem, procCode);
                _parent.rtFilterProcMnemRdsComboBox.Items.Add(comboBoxItem);
                _parent.rtFilterProcMnemTwComboBox.Items.Add(comboBoxItem); 
            }
            _parent.rtFilterProcMnemRdsComboBox.EndUpdate();
            _parent.rtFilterProcMnemTwComboBox.EndUpdate(); 
        }

        public void LoadEvtNameFilter(int tjType)
        {
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectRealTimeEvtNameFilter", _parent.GetTjTypeValue(tjType))); ;
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            _parent.rtFilterEventTypeRdsComboBox.Items.Clear();
            _parent.rtFilterEventTypeTwComboBox.Items.Clear();
            _parent.rtFilterEventTypeRdsComboBox.BeginUpdate();
            _parent.rtFilterEventTypeTwComboBox.BeginUpdate();
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                string evtCode = dataRow["EvtCode"].ToString();
                string evtName = dataRow["EvtName"].ToString();

                ComboBoxItem comboBoxItem = new ComboBoxItem(evtName, evtCode);
                _parent.rtFilterEventTypeRdsComboBox.Items.Add(comboBoxItem);
                _parent.rtFilterEventTypeTwComboBox.Items.Add(comboBoxItem);
            }
            _parent.rtFilterEventTypeRdsComboBox.EndUpdate();
            _parent.rtFilterEventTypeTwComboBox.EndUpdate();
        }

        public void AddReport()
        {
            /*
            // 리포트탭 추가
            TabPage reportTabPage = new TabPage(); 
            AddReportTabPage(ref reportTabPage);

            // Crystal Report 추가
            AddCrystalReport(ref reportTabPage);
            */

            // 리포트 아이템 추가
            FarsiLibrary.Win.FATabStripItem reportTabItem = new FarsiLibrary.Win.FATabStripItem();
            reportTabItem.BorderStyle = BorderStyle.None;
            reportTabItem.Name = "reportTabItem" + _parent.rtReportTabControl.Items.Count.ToString();
            reportTabItem.Title = GetSelectedTjName() + " " + GetSelectedReportName();
            _parent.rtReportTabControl.Items.Add(reportTabItem);
            _parent.rtReportTabControl.SelectedItem = reportTabItem;  

            // Crystal Report 추가
            AddCrystalReport(ref reportTabItem);
        }



        private void AddCrystalReport(ref FarsiLibrary.Win.FATabStripItem reportTabItem)
        {
            // Crystal Report Viewer 추가
            CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
            crystalReportViewer.ActiveViewIndex = 0;
            //crystalReportViewer.BorderStyle = BorderStyle.FixedSingle;
            crystalReportViewer.BorderStyle = BorderStyle.None;
            crystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            crystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            crystalReportViewer.Location = new System.Drawing.Point(3, 3);
            //crystalReportViewer.Name = "crystalReportViewer" + _parent.rtReportTabControl.TabCount.ToString();
            crystalReportViewer.Name = "crystalReportViewer" + _parent.rtReportTabControl.Items.Count.ToString(); 
            //crystalReportViewer.DisplayStatusBar = false;
            //crystalReportViewer.DisplayToolbar = false;
            crystalReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            crystalReportViewer.TabIndex = 0;

            _reportCreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");


            // Crystal Report 추가
            if (_parent.rtRawMaterialSumButton.Checked == true)
            {
                DataSet dataSet = DbHelper.SelectQuery(GetReportQuery());
                MaterialTotalUsedReport materialTotalUsedReport = new RealTimeRMT.MaterialTotalUsedReport();
                materialTotalUsedReport.SetDataSource(dataSet);

                DataSet dataSet2 = DbHelper.SelectQuery(GetReportQuery(true));
                //RecordsOfTheProductionReport recordsOfTheProductionReport = new RealTimeRMT.RecordsOfTheProductionReport(); ;
                materialTotalUsedReport.Subreports[0].SetDataSource(dataSet2);

                crystalReportViewer.ReportSource = materialTotalUsedReport;

                ((TextObject)materialTotalUsedReport.Section1.ReportObjects["ReportName"]).Text = GetReportName();  
                ((TextObject)materialTotalUsedReport.Section1.ReportObjects["StartDate"]).Text = GetPeriodStartDate();
                ((TextObject)materialTotalUsedReport.Section1.ReportObjects["EndDate"]).Text = GetPeriodEndDate();
                ((TextObject)materialTotalUsedReport.Section1.ReportObjects["CreatedDate"]).Text = _reportCreatedDate;  
                ((TextObject)materialTotalUsedReport.Section1.ReportObjects["ActualStartDate"]).Text = GetActualStartDate(dataSet);
                ((TextObject)materialTotalUsedReport.Section1.ReportObjects["ActualEndDate"]).Text = GetActualEndDate(dataSet); ;
                ((TextObject)materialTotalUsedReport.Section1.ReportObjects["SapCodeFilter"]).Text = GetSapCodeFilter();
                ((TextObject)materialTotalUsedReport.Section1.ReportObjects["DescriptionFilter"]).Text = GetDescriptionFilter();  
            }
            else if (_parent.rtRawMaterialDetailSumButton.Checked == true)
            {
                DataSet dataSet = DbHelper.SelectQuery(GetReportQuery());
                MaterialUsedListReport materialUsedListReport = new RealTimeRMT.MaterialUsedListReport();
                materialUsedListReport.SetDataSource(dataSet);

                DataSet dataSet2 = DbHelper.SelectQuery(GetReportQuery(true));
                //MaterialUsedListSubReport materialUsedListSubReport = new RealTimeRMT.MaterialUsedListSubReport();
                materialUsedListReport.Subreports[0].SetDataSource(dataSet2);

                string procMnemFilter = GetProcMnemFilter();
                if (procMnemFilter == "") procMnemFilter = "ALL";
                string eventTypeFilter = GetEventTypeFilter();
                if (eventTypeFilter == "") eventTypeFilter = "ALL";
                ((TextObject)materialUsedListReport.Section1.ReportObjects["ReportName"]).Text = GetReportName();
                ((TextObject)materialUsedListReport.Section1.ReportObjects["StartDate"]).Text = GetPeriodStartDate();
                ((TextObject)materialUsedListReport.Section1.ReportObjects["EndDate"]).Text = GetPeriodEndDate();
                ((TextObject)materialUsedListReport.Section1.ReportObjects["CreatedDate"]).Text = _reportCreatedDate; 
                ((TextObject)materialUsedListReport.Section1.ReportObjects["SapCodeFilter"]).Text = GetSapCodeFilter(); 
                ((TextObject)materialUsedListReport.Section1.ReportObjects["ProcMnemFilter"]).Text = procMnemFilter;
                ((TextObject)materialUsedListReport.Section1.ReportObjects["EventTypeFilter"]).Text = eventTypeFilter;
                ((TextObject)materialUsedListReport.Section1.ReportObjects["DescriptionFilter"]).Text = GetDescriptionFilter(); 


                crystalReportViewer.ReportSource = materialUsedListReport;
            }
            else if (_parent.rtProductionHistoryButton.Checked == true)
            {
                DataSet dataSet = DbHelper.SelectQuery(GetReportQuery());
                RecordsOfTheProductionReport recordsOfTheProductionReport = new RealTimeRMT.RecordsOfTheProductionReport();
                recordsOfTheProductionReport.SetDataSource(dataSet);
                crystalReportViewer.ReportSource = recordsOfTheProductionReport;

                ((TextObject)recordsOfTheProductionReport.Section1.ReportObjects["ReportName"]).Text = GetReportName();
                ((TextObject)recordsOfTheProductionReport.Section1.ReportObjects["StartDate"]).Text = GetPeriodStartDate();
                ((TextObject)recordsOfTheProductionReport.Section1.ReportObjects["EndDate"]).Text = GetPeriodEndDate();
                ((TextObject)recordsOfTheProductionReport.Section1.ReportObjects["CreatedDate"]).Text = _reportCreatedDate; 
                ((TextObject)recordsOfTheProductionReport.Section1.ReportObjects["ActualStartDate"]).Text = GetActualStartDate(dataSet);
                ((TextObject)recordsOfTheProductionReport.Section1.ReportObjects["ActualEndDate"]).Text = GetActualEndDate(dataSet); ;
            }
            else //(_parent.rtTotalWasteButton.Checked == true)
            {
                DataSet dataSet = DbHelper.SelectQuery(GetReportQuery());
                TotalWasteReport totalWasteReport = new RealTimeRMT.TotalWasteReport();
                totalWasteReport.SetDataSource(dataSet);
                crystalReportViewer.ReportSource = totalWasteReport;

                ((TextObject)totalWasteReport.Section1.ReportObjects["ReportName"]).Text = GetReportName();
                ((TextObject)totalWasteReport.Section1.ReportObjects["StartDate"]).Text = GetPeriodStartDate();
                ((TextObject)totalWasteReport.Section1.ReportObjects["EndDate"]).Text = GetPeriodEndDate();
                ((TextObject)totalWasteReport.Section1.ReportObjects["CreatedDate"]).Text = _reportCreatedDate; 

                string procMnemFilter = GetProcMnemFilter();
                if (procMnemFilter == "") procMnemFilter = "ALL";
                string eventTypeFilter = GetEventTypeFilter();
                if (eventTypeFilter == "") eventTypeFilter = "ALL";
                ((TextObject)totalWasteReport.Section1.ReportObjects["ProcMnemFilter"]).Text = procMnemFilter;
                ((TextObject)totalWasteReport.Section1.ReportObjects["EventTypeFilter"]).Text = eventTypeFilter;
                ((TextObject)totalWasteReport.Section1.ReportObjects["WasteLossFilter"]).Text = GetWasteLossFilter();
                ((TextObject)totalWasteReport.Section1.ReportObjects["SetDataFilter"]).Text = GetSetDataFilter();
                ((TextObject)totalWasteReport.Section1.ReportObjects["SpliceDataFilter"]).Text = GetSpliceDataFilter();
            }

            //reportTabPage.Controls.Add(crystalReportViewer);
            reportTabItem.Controls.Add(crystalReportViewer);
        }

        private string GetWasteLossFilter()
        {
            string wasteLoss = _parent.rtFilterWasteLossTwTextBox.Text.Trim();
            if (wasteLoss != "" && wasteLoss.LastIndexOf('.') == wasteLoss.Length - 1)
                wasteLoss = wasteLoss.Substring(0, wasteLoss.Length - 1); 

            if (wasteLoss != "")
                return string.Format("WasteLoss Between  -{0} And +{1}", Convert.ToDouble(wasteLoss).ToString("F2"), Convert.ToDouble(wasteLoss).ToString("F2"));
            else
                return "ALL"; 
        }
        private string GetSetDataFilter()
        {
            string setData = _parent.rtFilterSerDataTwTextBox.Text.Trim();
            if (setData != "" && setData.LastIndexOf('.') == setData.Length - 1)
                setData = setData.Substring(0, setData.Length - 1); 

            if (setData != "")
                return string.Format("SetData Between  -{0} And +{1}", Convert.ToDouble(setData).ToString("F2"), Convert.ToDouble(setData).ToString("F2"));
            else
                return "ALL"; 
        }
        private string GetSpliceDataFilter()
        {
            string spliceData = _parent.rtFilterSpliceDataTwTextBox.Text.Trim();
            if (spliceData != "" && spliceData.LastIndexOf('.') == spliceData.Length - 1)
                spliceData = spliceData.Substring(0, spliceData.Length - 1); 

            if (spliceData != "")
                return string.Format("SpliceData Between  -{0} And +{1}", Convert.ToDouble(spliceData).ToString("F2"), Convert.ToDouble(spliceData).ToString("F2"));
            else
                return "ALL"; 
        }
        private string GetReportQuery(bool subQuery = false)
        {
            string query = ""; 
            if (_parent.rtRawMaterialSumButton.Checked == true) 
            {
                if (subQuery == false)
                {
                    query = string.Format("{0} {1}, '{2}', '{3}', '{4}', '{5}'",
                        "EXEC SelectMaterialTotalUsedReport",
                        GetSelectedTjValue(),
                        GetPeriodStartDate(),
                        GetPeriodEndDate(),
                        GetSapCodeFilter(true),
                        GetDescriptionFilter(true));
                }
                else
                {
                    query = string.Format("{0} {1}, '{2}', '{3}'",
                        "EXEC SelectRecordsOfTheProductionReport",
                        GetSelectedTjValue(),
                        GetPeriodStartDate(),
                        GetPeriodEndDate());
                }

            }
            else if (_parent.rtRawMaterialDetailSumButton.Checked == true) 
            {
                string[] procMnems = GetProcMnemFilter().Split(',');

                if(subQuery == false) 
                {
                    query = string.Format("{0} {1}, '{2}', '{3}', '{4}', '{5}', '{6}'",
                        "EXEC SelectMaterialUsedListReport",
                        GetSelectedTjValue(),
                        procMnems.Length > 0 ? procMnems[0] : "",  
                        procMnems.Length > 1 ? procMnems[1] : "",
                        procMnems.Length > 2 ? procMnems[2] : "",
                        procMnems.Length > 3 ? procMnems[3] : "",
                        procMnems.Length > 4 ? procMnems[4] : "");
                }
                else 
                {
                    string[] eventTypes = GetEventTypeFilter().Split(',');

                    query = string.Format("{0} {1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}'",
                        "EXEC SelectMaterialUsedListSubReport",
                        GetSelectedTjValue(),   // 1
                        GetPeriodStartDate(),   // 2
                        GetPeriodEndDate(),     // 3 
                        procMnems.Length > 0 ? procMnems[0] : "",   // 4
                        procMnems.Length > 1 ? procMnems[1] : "",
                        procMnems.Length > 2 ? procMnems[2] : "",
                        procMnems.Length > 3 ? procMnems[3] : "",
                        procMnems.Length > 4 ? procMnems[4] : "", 
                        eventTypes.Length > 0 ? eventTypes[0] : "",  // 9
                        eventTypes.Length > 1 ? eventTypes[1] : "",
                        eventTypes.Length > 2 ? eventTypes[2] : "",
                        eventTypes.Length > 3 ? eventTypes[3] : "",
                        eventTypes.Length > 4 ? eventTypes[4] : "",  // 13
                        _parent.rtFilterSapCodeRdsTextBox.Text.Trim(),
                        _parent.rtFilterSapDescriptionRdsTextBox.Text.Trim());
                }
            }
            else if (_parent.rtProductionHistoryButton.Checked == true) 
            {
                query = string.Format("{0} {1}, '{2}', '{3}'",
                    "EXEC SelectRecordsOfTheProductionReport",
                    GetSelectedTjValue(),
                    GetPeriodStartDate(),
                    GetPeriodEndDate());
            }
            else //(_parent.rtTotalWasteButton.Checked == true)
            {
                string[] procMnems = GetProcMnemFilter().Split(',');
                string[] eventTypes = GetEventTypeFilter().Split(',');

                query = string.Format("{0} {1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', {14}, {15}, {16}",
                    "EXEC SelectTotalWasteReport", // 0
                    GetSelectedTjValue(),   // 1
                    GetPeriodStartDate(),   // 2
                    GetPeriodEndDate(),     // 3 
                    procMnems.Length > 0 ? procMnems[0] : "",   // 4
                    procMnems.Length > 1 ? procMnems[1] : "",
                    procMnems.Length > 2 ? procMnems[2] : "",
                    procMnems.Length > 3 ? procMnems[3] : "",
                    procMnems.Length > 4 ? procMnems[4] : "",
                    eventTypes.Length > 0 ? eventTypes[0] : "",  // 9
                    eventTypes.Length > 1 ? eventTypes[1] : "",
                    eventTypes.Length > 2 ? eventTypes[2] : "",
                    eventTypes.Length > 3 ? eventTypes[3] : "",
                    eventTypes.Length > 4 ? eventTypes[4] : "",  // 13
                    _parent.rtFilterWasteLossTwTextBox.Text.Trim() == "" ? "-1" : _parent.rtFilterWasteLossTwTextBox.Text.Trim(),
                    _parent.rtFilterSerDataTwTextBox.Text.Trim() == "" ? "-1" : _parent.rtFilterSerDataTwTextBox.Text.Trim(),
                    _parent.rtFilterSpliceDataTwTextBox.Text.Trim() == "" ? "-1" : _parent.rtFilterSpliceDataTwTextBox.Text.Trim() 
                    );    
            }
            return query; 
        }

        private string GetProcMnemFilter()
        {
            string procMnems = "";
            PresentationControls.CheckBoxComboBox checkBox = null;

            if (_parent.rtRawMaterialDetailSumButton.Checked == true)
                checkBox = _parent.rtFilterProcMnemRdsComboBox; 
            else
                checkBox = _parent.rtFilterProcMnemTwComboBox;

            for (int i = 0; i < checkBox.CheckBoxItems.Count; i++)
            {
                if (checkBox.CheckBoxItems[i].Checked == true)
                {
                    procMnems += checkBox.CheckBoxItems[i].Text + ",";
                }
            }

            if (procMnems != "")
                procMnems = procMnems.Substring(0, procMnems.Length - 1);

            return procMnems; 
        }

        private string GetEventTypeFilter()
        {
            string eventTypes = "";
            PresentationControls.CheckBoxComboBox checkBox = null;

            if (_parent.rtRawMaterialDetailSumButton.Checked == true)
                checkBox = _parent.rtFilterEventTypeRdsComboBox;
            else
                checkBox = _parent.rtFilterEventTypeTwComboBox;

            for (int i = 0; i < checkBox.CheckBoxItems.Count; i++)
            {
                if (checkBox.CheckBoxItems[i].Checked == true)
                {
                    eventTypes += checkBox.CheckBoxItems[i].Text + ",";
                }
            }

            if (eventTypes != "")
                eventTypes = eventTypes.Substring(0, eventTypes.Length - 1); 

            return eventTypes;
        }

        private string GetReportName()
        {
            string tjName = GetSelectedTjName();
            if (_parent.rtRawMaterialSumButton.Checked == true)
                return tjName + " " + _parent.rtRawMaterialSumButton.Tag.ToString();
            else if (_parent.rtRawMaterialDetailSumButton.Checked == true)
                return tjName + " " + _parent.rtRawMaterialDetailSumButton.Tag.ToString(); 
            else if (_parent.rtProductionHistoryButton.Checked == true)
                return tjName + " " + _parent.rtProductionHistoryButton.Tag.ToString(); 
            else //(_parent.rtTotalWasteButton.Checked == true)
                return tjName + " " + _parent.rtTotalWasteButton.Tag.ToString();  
        }

        private string GetReportTagName()
        {
            if (_parent.rtRawMaterialSumButton.Checked == true)
                return _parent.rtRawMaterialSumButton.Tag.ToString();
            else if (_parent.rtRawMaterialDetailSumButton.Checked == true)
                return _parent.rtRawMaterialDetailSumButton.Tag.ToString();
            else if (_parent.rtProductionHistoryButton.Checked == true)
                return _parent.rtProductionHistoryButton.Tag.ToString();
            else //(_parent.rtTotalWasteButton.Checked == true)
                return _parent.rtTotalWasteButton.Tag.ToString(); 
        }

        private string GetPeriodStartDate()
        {
            return _parent.rtPeriodStartDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string GetPeriodEndDate()
        {
            // 일일선택이면 시작시간에서 하루를 더한다.
            if (_parent.rtPeriodDayRadioButton.Checked == true)
                return _parent.rtPeriodStartDateTimePicker.Value.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            else        
                return _parent.rtPeriodEndDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string GetActualStartDate(DataSet dataSet) {
            string date = "";
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return date;

            date = (DBNull.Value.Equals(dataSet.Tables[0].Rows[0]["StartDate"])) ? "" : dataSet.Tables[0].Rows[0]["StartDate"].ToString();
            return date; 
        }

        private string GetActualEndDate(DataSet dataSet)
        {
            string date = "";
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return date;

            int last = dataSet.Tables[0].Rows.Count - 1;
            date = (DBNull.Value.Equals(dataSet.Tables[0].Rows[last]["EndDate"])) ? "" : dataSet.Tables[0].Rows[last]["EndDate"].ToString();
            return date;
        }

        private string GetSapCodeFilter(bool forQuery = false)
        {
            string sapCode = "";
            if (_parent.rtRawMaterialSumButton.Checked == true)
                sapCode = _parent.rtFilterSapCodeRsTextBox.Text.Trim(); 
            else if (_parent.rtRawMaterialDetailSumButton.Checked == true)
                sapCode = _parent.rtFilterSapCodeRdsTextBox.Text.Trim();

            if (forQuery == false)
                return (sapCode == "") ? "ALL" : sapCode;
            else 
                return sapCode;  
        }

        private string GetDescriptionFilter(bool forQuery = false)
        {
            string description = "";
            if (_parent.rtRawMaterialSumButton.Checked == true)
                description = _parent.rtFilterSapDescriptionRsTextBox.Text.Trim(); 
            else if (_parent.rtRawMaterialDetailSumButton.Checked == true)
                description = _parent.rtFilterSapDescriptionRdsTextBox.Text.Trim();

            if (forQuery == false)
                return (description == "") ? "ALL" : description;
            else
                return description;
        }

        private string GetSelectedReportName()
        {
            string reportName = ""; 
            ImageButton[] reportButtons = { _parent.rtRawMaterialSumButton, _parent.rtRawMaterialDetailSumButton, _parent.rtProductionHistoryButton, _parent.rtTotalWasteButton };
            for (int i = 0; i < reportButtons.Length; i++)
            {
                if (reportButtons[i].Checked == true)
                {
                    reportName = reportButtons[i].Tag.ToString();
                    break; 
                }
            }
            return reportName; 
        }


        private string GetSelectedTjName()
        {
            string tjName = ""; 

            if(_parent.rtSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                

                if (_parent.rtTj01Button.Checked == true)
                    tjName = "TJ01";
                else if (_parent.rtTj02Button.Checked == true)
                    tjName = "TJ02";
                else if (_parent.rtTj03Button.Checked == true)
                    tjName = "TJ03";
                else if (_parent.rtTj04Button.Checked == true)
                    tjName = "TJ04";
                else if (_parent.rtTj05Button.Checked == true)
                    tjName = "TJ05";
                //else if (_parent.rtTjBcButton.Checked == true)
                //    tjName = "BC통합";
                else
                    tjName = "TJ01";
            }
            else
            {
                

                if (_parent.rtTj21Button.Checked == true)
                    tjName = "TJ21";
                else if (_parent.rtTj22Button.Checked == true)
                    tjName = "TJ22";
                else if (_parent.rtTj23Button.Checked == true)
                    tjName = "TJ23";
                //else if (_parent.rtTjCcButton.Checked == true)
                //   tjName = "CC통합";
                else
                    tjName = "TJ21";
            }
            return tjName; 
        }


        private int GetSelectedTjValue()
        {
            int tjValue = 21;

            if (_parent.rtSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                if (_parent.rtTj01Button.Checked == true)
                    tjValue = 1;
                else if (_parent.rtTj02Button.Checked == true)
                    tjValue = 2;
                else if (_parent.rtTj03Button.Checked == true)
                    tjValue = 3;
                else if (_parent.rtTj04Button.Checked == true)
                    tjValue = 4;
                else if (_parent.rtTj05Button.Checked == true)
                    tjValue = 5;
                //else if (_parent.rtTjBcButton.Checked == true)
                //    tjValue = "BC통합";
                else
                    tjValue = 1; 
            }
            else
            {
                if (_parent.rtTj21Button.Checked == true)
                    tjValue = 21;
                else if (_parent.rtTj22Button.Checked == true)
                    tjValue = 22;
                else if (_parent.rtTj23Button.Checked == true)
                    tjValue = 23;
                //else if (_parent.rtTjCcButton.Checked == true)
                //    tjValue = "CC통합";
                else
                    tjValue = 21;
            }

            return tjValue; 
        }

        private int GetSelectedTeamTypeValue()
        {
            if (_parent.rtSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
                return 1;
            else
                return 2; 
        }

        public void ResetRtReportButtons(string currentName)
        {
            ImageButton[] reportButtons = { _parent.rtRawMaterialSumButton, _parent.rtRawMaterialDetailSumButton, _parent.rtProductionHistoryButton, _parent.rtTotalWasteButton };
            for (int i = 0; i < reportButtons.Length; i++)
            {
                reportButtons[i].Checked = (currentName == reportButtons[i].Name) ? true : false;
            }
        }

        public void MoveRtCreateReportButton()
        {
            /*
            if (_parent.rtRawMaterialSumButton.Checked == true)
                _parent.rtCreateReportButton.Top = _parent.rtFilterTabControl.Top + (_parent.rtFilterSapCodeRsTextBox.Height * 5);
            else if (_parent.rtRawMaterialDetailSumButton.Checked == true)
                _parent.rtCreateReportButton.Top = _parent.rtFilterTabControl.Top + (_parent.rtFilterSapCodeRsTextBox.Height * 7);
            else if (_parent.rtProductionHistoryButton.Checked == true)
                _parent.rtCreateReportButton.Top = _parent.rtFilterTabControl.Top + (_parent.rtFilterSapCodeRsTextBox.Height * 3);
            else if (_parent.rtTotalWasteButton.Checked == true)
                _parent.rtCreateReportButton.Top = _parent.rtFilterTabControl.Top + (_parent.rtFilterSapCodeRsTextBox.Height * 7);
                */ 
        }

        public void InsertRealTimeSearchLog()
        {
            string TeamType = GetSelectedTeamTypeValue().ToString(); 
            string TjType = GetSelectedTjValue().ToString();
            string StartDate = GetPeriodStartDate();
            string EndDate = GetPeriodEndDate();
            string ReportName = GetReportTagName();   
            string SapCodeCd = "";  
            string SapDescCd = "";  
            string ProcMnemCd = "";  
            string EventTypeCd = "";  
            string WasteLossCd = "";  
            string SetDataCd = ""; 
            string SpliceDataCd = "";  
            string IsMerge = "";

            if (_parent.rtTjBcButton.Checked == true || _parent.rtTjCcButton.Checked == true)
                IsMerge = "1";
            else
                IsMerge = "0"; 

            string CreatedDate = _reportCreatedDate; 

            if (_parent.rtRawMaterialSumButton.Checked == true)
            {
                SapCodeCd = GetSapCodeFilter();
                SapDescCd = GetDescriptionFilter();
            }
            else if (_parent.rtRawMaterialDetailSumButton.Checked == true)
            {
                ProcMnemCd = GetProcMnemFilter();
                if (ProcMnemCd == "") 
                    ProcMnemCd = "ALL";
                EventTypeCd = GetEventTypeFilter();
                if (EventTypeCd == "") 
                    EventTypeCd = "ALL";
                SapCodeCd = GetSapCodeFilter();
                SapDescCd = GetDescriptionFilter();
                IsMerge = "0"; 
            }
            else if (_parent.rtProductionHistoryButton.Checked == true)
            {
            }
            else //(_parent.rtTotalWasteButton.Checked == true)
            {
                ProcMnemCd = GetProcMnemFilter();
                if (ProcMnemCd == "") 
                    ProcMnemCd = "ALL";
                EventTypeCd = GetEventTypeFilter();
                if (EventTypeCd == "") 
                    EventTypeCd = "ALL";
                WasteLossCd = GetWasteLossFilter();
                SetDataCd = GetSetDataFilter();
                SpliceDataCd = GetSpliceDataFilter();
            }

            DbHelper.ExecuteNonQuery(string.Format("{0} {1}, {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', {13}, '{14}'",
                "EXEC InsertRealTimeSearchLog",
                TeamType,
                TjType,
                StartDate,
                EndDate,
                ReportName,
                SapCodeCd,
                SapDescCd,
                ProcMnemCd,
                EventTypeCd,
                WasteLossCd,
                SetDataCd,
                SpliceDataCd,
                IsMerge,
                CreatedDate));  
        }


        public void ResizeControls()
        {
            const int SEARCH_PANEL_HEIGHT = 167;

            _parent.rtSidePanel.Left = 0;
            _parent.rtSidePanel.Top = 0;
            _parent.rtSidePanel.Width = ConstDefine.sidePanelSize;
            _parent.rtSidePanel.Height = _parent.mainTabControl.Height;

            _parent.rtSearchPanel.Left = _parent.rtSidePanel.Width;
            _parent.rtSearchPanel.Top = 0;
            _parent.rtSearchPanel.Width = _parent.mainTabControl.Width - _parent.rtSidePanel.Width;
            _parent.rtSearchPanel.Height = SEARCH_PANEL_HEIGHT;

            _parent.rtReportTabControl.Left = _parent.rtSidePanel.Width + ConstDefine.defaultGap;
            _parent.rtReportTabControl.Top = _parent.rtSearchPanel.Height; 
            _parent.rtReportTabControl.Width = _parent.mainTabControl.Width - (_parent.rtSidePanel.Width + (ConstDefine.defaultGap * 2)); 
            _parent.rtReportTabControl.Height = _parent.mainTabControl.Height - (_parent.rtSearchPanel.Height + ConstDefine.defaultGap);
        }

        public void CheckFilter(PresentationControls.CheckBoxComboBox comboBox)
        {
            string[] values = comboBox.Text.Trim().Split(',');
            if (values != null && values.Length > 5)
            {
                MessageBox.Show("5개까지의 필터만 선택할 수 있습니다.", ConstDefine.searchTitle);
                comboBox.CheckBoxItems.Clear();
                comboBox.Text = "";
                comboBox.HideDropDown();
                return;
            }

            for (int i = 0; i < values.Length; i++)
            {
                if (comboBox.FindString(values[i].Trim()) == -1)
                {
                    MessageBox.Show("잘못된 항목이 입력되었습니다.", ConstDefine.searchTitle);
                    comboBox.CheckBoxItems.Clear();
                    comboBox.Text = "";
                    comboBox.HideDropDown();
                    return;
                }
            }
        }

        public void ResetPeriodEndDateTime()
        {
            DateTime tempDate = _parent.rtPeriodStartDateTimePicker.Value.AddDays(1);
            _parent.rtPeriodEndDateTimePicker.Value = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 6, 59, 59);
        }

        public bool IsValidPeriod()
        {
            if (_parent.rtPeriodStartDateTimePicker.Value > _parent.rtPeriodEndDateTimePicker.Value)
                return false;

            return true; 
        }
    }
}

