using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace RealTimeRMT
{
    public class HomePage
    {
        private MainForm _parent = null;

        public HomePage(MainForm parent)
        {
            _parent = parent;
        }

        public void InitControls()
        {
            _parent.homeMachineStatusTimer.Interval = 10000;

            InitSearchLogGridView();
            InitNoticeBoardGridView();
            InitFreeBoardGridView();

            LoadRealTimeSearchLogList();
            StartMachineStatusTimer();
            LoadSapUpdateLogList();
            LoadNoticeBoardList();
            LoadFreeBoardList();
        }

        public void LoadNoticeBoardList()
        {
            //string userId = _parent.GetCurrentUserId();

            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectNoticeBoardList 'HOME'");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            _parent.heNoticeBoardGridView.Rows.Clear();
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                InsertNoticeBoardGridViewItem(dataRow["No"].ToString(), dataRow["Title"].ToString(),
                    dataRow["ViewCount"].ToString(), dataRow["CreatedDate"].ToString());
            }

            _parent.heNoticeBoardGridView.ClearSelection();
        }

        public void InsertNoticeBoardGridViewItem(string no, string title, string viewCount, string createdDate)
        {
            _parent.heNoticeBoardGridView.Rows.Add(createdDate.Substring(0, 10), title, Utils.SetComma(viewCount), no);
        }

        public void LoadFreeBoardList()
        {
            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectFreeBoardList 'HOME'");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            _parent.heFreeBoardGridView.Rows.Clear();
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                InsertFreeBoardGridViewItem(dataRow["No"].ToString(), dataRow["Title"].ToString(),
                    dataRow["ViewCount"].ToString(), dataRow["CreatedDate"].ToString());
            }

            _parent.heFreeBoardGridView.ClearSelection();
        }

        public void InsertFreeBoardGridViewItem(string no, string title, string viewCount, string createdDate)
        {
            _parent.heFreeBoardGridView.Rows.Add(createdDate.Substring(0, 10), title, Utils.SetComma(viewCount), no);
        }

        public void LoadSapUpdateLogList()
        {
            _parent.heScheduleUpdateDateLabel.Text = "-";
            _parent.heProductUpdateDateLabel.Text = "-";
            _parent.heBomUpdateDateLabel.Text = "-";

            DataSet dataSetS = DbHelper.SelectQuery("EXEC SelectSapUpdateLogForHome 'SCHEDULE'");
            if (dataSetS != null && dataSetS.Tables.Count != 0 && dataSetS.Tables[0].Rows.Count != 0)
            {
                _parent.heScheduleUpdateDateLabel.Text = dataSetS.Tables[0].Rows[0]["UpdateDate"].ToString().Substring(0, 16) + "  " +
                dataSetS.Tables[0].Rows[0]["UpdateUser"].ToString();
            }

            DataSet dataSetP = DbHelper.SelectQuery("EXEC SelectSapUpdateLogForHome 'PRODUCT'");
            if (dataSetP != null && dataSetP.Tables.Count != 0 && dataSetP.Tables[0].Rows.Count != 0)
            {
                _parent.heProductUpdateDateLabel.Text = dataSetP.Tables[0].Rows[0]["UpdateDate"].ToString().Substring(0, 16) + "  " +
                dataSetP.Tables[0].Rows[0]["UpdateUser"].ToString();
            }

            DataSet dataSetB = DbHelper.SelectQuery("EXEC SelectSapUpdateLogForHome 'BOM'");
            if (dataSetB != null && dataSetB.Tables.Count != 0 && dataSetB.Tables[0].Rows.Count != 0)
            {
                _parent.heBomUpdateDateLabel.Text = dataSetB.Tables[0].Rows[0]["UpdateDate"].ToString().Substring(0, 16) + "  " +
                dataSetB.Tables[0].Rows[0]["UpdateUser"].ToString();
            }
        }

        public void LoadRealTimeSearchLogList()
        {
            _parent.heSearchLogGridView.Rows.Clear();

            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectRealTimeSearchLogList");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            // TOP 5 쿼리로 최대 5개만 가져오고 있다. 
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                if (i == 5)
                    break;

                _parent.heSearchLogGridView.Rows.Add(GetTjNameFromTjType(Convert.ToInt32(dataSet.Tables[0].Rows[i]["TjType"].ToString())),
                    dataSet.Tables[0].Rows[i]["StartDate"].ToString().Substring(0, 10),
                    dataSet.Tables[0].Rows[i]["StartDate"].ToString().Substring(11, 8),
                    "~",
                    dataSet.Tables[0].Rows[i]["EndDate"].ToString().Substring(0, 10),
                    dataSet.Tables[0].Rows[i]["EndDate"].ToString().Substring(11, 8),
                    dataSet.Tables[0].Rows[i]["ReportName"].ToString(),
                    "보기",
                    dataSet.Tables[0].Rows[i]["No"].ToString());
            }
            _parent.heSearchLogGridView.ClearSelection();
        }

        public string GetTjNameFromTjType(int tjType)
        {
            string tjName = "";
            if (tjType < 6)
                tjName = "TJ0" + tjType.ToString();
            else
                tjName = "TJ" + tjType.ToString();

            return tjName;
        }

        public void OpenRealTimeSearchConditionForm(int col, int row)
        {
            if ((int)ConstDefine.eHeSearchLogListView.searchCondition != col)
                return;

            string logNo = _parent.heSearchLogGridView.Rows[row].Cells[(int)ConstDefine.eHeSearchLogListView.logNo].Value.ToString();

            RealTimeSearchConditionForm conditionForm = new RealTimeSearchConditionForm(this);
            conditionForm.SetLogNo(logNo);
            conditionForm.StartPosition = FormStartPosition.CenterParent;
            conditionForm.ShowDialog();
        }

        public void InitSearchLogGridView()
        {
            // Row Height
            _parent.heSearchLogGridView.RowTemplate.Height = 40;

            // Cell Width
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.machine].Width = 60;
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.startDate].Width = 65;
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.startTime].Width = 50;
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.dash].Width = 15;
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.endDate].Width = 65;
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.endTime].Width = 50;
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.reportName].Width = 150;
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.searchCondition].Width = 50;

            // Default Cell Style
            Padding defaultCellPadding = new Padding(0, 0, 0, 0);
            _parent.heSearchLogGridView.RowsDefaultCellStyle.Padding = defaultCellPadding;
            _parent.heSearchLogGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 7, FontStyle.Regular);
            _parent.heSearchLogGridView.RowsDefaultCellStyle.ForeColor = Color.FromArgb(112, 112, 112);
            _parent.heSearchLogGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(233, 234, 235);

            // Each Cell Style 
            Padding searchConditionCellPadding = new Padding(7, 7, 7, 7);
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.searchCondition].DefaultCellStyle.Padding = searchConditionCellPadding;
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.startTime].DefaultCellStyle.ForeColor = Color.FromArgb(187, 187, 187);
            _parent.heSearchLogGridView.Columns[(int)ConstDefine.eHeSearchLogListView.endTime].DefaultCellStyle.ForeColor = Color.FromArgb(187, 187, 187);

            // Other Styles
            _parent.heSearchLogGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //_parent.heSearchLogGridView.DefaultCellStyle.SelectionBackColor = Color.White; 
            _parent.heSearchLogGridView.BorderStyle = BorderStyle.None;
            _parent.heSearchLogGridView.BackgroundColor = Color.FromArgb(233, 234, 235);
            _parent.heSearchLogGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _parent.heSearchLogGridView.ColumnHeadersVisible = false;
            _parent.heSearchLogGridView.RowHeadersVisible = false;
            _parent.heSearchLogGridView.ReadOnly = true;
            _parent.heSearchLogGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.heSearchLogGridView.ScrollBars = ScrollBars.None;
            _parent.heSearchLogGridView.AllowUserToResizeRows = false;
            _parent.heSearchLogGridView.AllowUserToResizeColumns = false;
        }


        public void InitNoticeBoardGridView()
        {
            _parent.heNoticeBoardGridView.Width = 525;

            // Row Height
            _parent.heNoticeBoardGridView.RowTemplate.Height = 30;

            // Cell Width
            _parent.heNoticeBoardGridView.Columns[(int)ConstDefine.eHeBoardListView.createdDate].Width = 85;
            _parent.heNoticeBoardGridView.Columns[(int)ConstDefine.eHeBoardListView.title].Width = _parent.heNoticeBoardGridView.Width - 135;
            _parent.heNoticeBoardGridView.Columns[(int)ConstDefine.eHeBoardListView.viewCount].Width = 50;

            // Default Cell Style
            Padding defaultCellPadding = new Padding(0, 0, 0, 0);
            _parent.heNoticeBoardGridView.RowsDefaultCellStyle.Padding = defaultCellPadding;
            _parent.heNoticeBoardGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 8, FontStyle.Regular);
            _parent.heNoticeBoardGridView.RowsDefaultCellStyle.ForeColor = Color.FromArgb(112, 112, 112);
            _parent.heNoticeBoardGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(233, 234, 235);

            // Each Cell Style
            _parent.heNoticeBoardGridView.Columns[(int)ConstDefine.eHeBoardListView.title].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Other Styles
            _parent.heNoticeBoardGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //_parent.heNoticeBoardGridView.DefaultCellStyle.SelectionBackColor = Color.White; 
            _parent.heNoticeBoardGridView.BorderStyle = BorderStyle.None;
            _parent.heNoticeBoardGridView.BackgroundColor = Color.FromArgb(233, 234, 235);
            _parent.heNoticeBoardGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _parent.heNoticeBoardGridView.ColumnHeadersVisible = false;
            _parent.heNoticeBoardGridView.RowHeadersVisible = false;
            _parent.heNoticeBoardGridView.ReadOnly = true;
            _parent.heNoticeBoardGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.heNoticeBoardGridView.ScrollBars = ScrollBars.None;
            _parent.heNoticeBoardGridView.AllowUserToResizeRows = false;
            _parent.heNoticeBoardGridView.AllowUserToResizeColumns = false;
        }

        public void InitFreeBoardGridView()
        {
            _parent.heFreeBoardGridView.Width = 525;

            // Row Height
            _parent.heFreeBoardGridView.RowTemplate.Height = 30;

            // Cell Width
            _parent.heFreeBoardGridView.Columns[(int)ConstDefine.eHeBoardListView.createdDate].Width = 85;
            _parent.heFreeBoardGridView.Columns[(int)ConstDefine.eHeBoardListView.title].Width = _parent.heFreeBoardGridView.Width - 135;
            _parent.heFreeBoardGridView.Columns[(int)ConstDefine.eHeBoardListView.viewCount].Width = 50;

            // Default Cell Style
            Padding defaultCellPadding = new Padding(0, 0, 0, 0);
            _parent.heFreeBoardGridView.RowsDefaultCellStyle.Padding = defaultCellPadding;
            _parent.heFreeBoardGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 8, FontStyle.Regular);
            _parent.heFreeBoardGridView.RowsDefaultCellStyle.ForeColor = Color.FromArgb(112, 112, 112);
            _parent.heFreeBoardGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(233, 234, 235);

            // Each Cell Style
            _parent.heFreeBoardGridView.Columns[(int)ConstDefine.eHeBoardListView.title].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Other Styles
            _parent.heFreeBoardGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //_parent.heFreeBoardGridView.DefaultCellStyle.SelectionBackColor = Color.White; 
            _parent.heFreeBoardGridView.BorderStyle = BorderStyle.None;
            _parent.heFreeBoardGridView.BackgroundColor = Color.FromArgb(233, 234, 235);
            _parent.heFreeBoardGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _parent.heFreeBoardGridView.ColumnHeadersVisible = false;
            _parent.heFreeBoardGridView.RowHeadersVisible = false;
            _parent.heFreeBoardGridView.ReadOnly = true;
            _parent.heFreeBoardGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.heFreeBoardGridView.ScrollBars = ScrollBars.None;
            _parent.heFreeBoardGridView.AllowUserToResizeRows = false;
            _parent.heFreeBoardGridView.AllowUserToResizeColumns = false;
        }

        public void ResizeControls()
        {
            if (_parent.mainTabControl.Width > (_parent.heMainPanel.Width + _parent.heSidePanel.Width))
                _parent.heMainPanel.Left = ((_parent.mainTabControl.Width - _parent.heMainPanel.Width)  / 2) + (_parent.heSidePanel.Width / 2);
            else
                _parent.heMainPanel.Left = _parent.heSidePanel.Width;

            if (_parent.mainTabControl.Height > _parent.mainTabControl.Height)
                _parent.heMainPanel.Top = (_parent.mainTabControl.Height - _parent.heMainPanel.Height) / 2;
            else
                _parent.heMainPanel.Top = 0;

            //_parent.heMainPanel.Width = _parent.mainTabControl.Width - _parent.heSidePanel.Width;
            //_parent.heMainPanel.Height = _parent.mainTabControl.Height;

            //_parent.heLogoPictureBox.Width = _parent.heMainPanel.Width - 15;
        }

        public void LoadHeData()
        {
            LoadRealTimeSearchLogList();
            StartMachineStatusTimer();
            LoadSapUpdateLogList();
            LoadNoticeBoardList();
            LoadFreeBoardList();
        }

        public void SetCurretUserInfo(string userName)
        {
            _parent.SetUserName(userName);
            _parent.SetCurretUserInfo();
        }

        public void OpenUserForm(string userId)
        {
            HomeUserForm homeUserForm = new HomeUserForm(this, userId);
            homeUserForm.StartPosition = FormStartPosition.CenterParent;
            homeUserForm.ShowDialog();
        }

        public void StartMachineStatusTimer()
        {
            LoadTotalMachineStatus();

            if (_parent.homeMachineStatusTimer.Enabled == false)
                _parent.homeMachineStatusTimer.Start();
        }

        public void LoadTotalMachineStatus()
        {
            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectTotalMachineStatus");
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            Image statusOn = Properties.Resources.machine_status_on_32;
            Image statusOff = Properties.Resources.machine_status_off_32;

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                string machine = dataSet.Tables[0].Rows[i]["ASSET_TJ"].ToString();
                string status = dataSet.Tables[0].Rows[i]["MachineStatus"].ToString();

                if (machine == "01 MC")
                    _parent.heMachineStatusTj01PictureBox.BackgroundImage = (status == "1") ? Properties.Resources.TJ01_1 : Properties.Resources.TJ01_2;
                else if (machine == "02 MC")
                    _parent.heMachineStatusTj02PictureBox.BackgroundImage = (status == "1") ? Properties.Resources.TJ02_1 : Properties.Resources.TJ02_2;
                else if (machine == "03 MC")
                    _parent.heMachineStatusTj03PictureBox.BackgroundImage = (status == "1") ? Properties.Resources.TJ03_1 : Properties.Resources.TJ03_2;
                else if (machine == "04 MC")
                    _parent.heMachineStatusTj04PictureBox.BackgroundImage = (status == "1") ? Properties.Resources.TJ04_1 : Properties.Resources.TJ04_2;
                else if (machine == "05 MC")
                    _parent.heMachineStatusTj05PictureBox.BackgroundImage = (status == "1") ? Properties.Resources.TJ05_1 : Properties.Resources.TJ05_2;
                else if (machine == "21 MC")
                    _parent.heMachineStatusTj21PictureBox.BackgroundImage = (status == "1") ? Properties.Resources.TJ21_1 : Properties.Resources.TJ21_2;
                else if (machine == "22 MC")
                    _parent.heMachineStatusTj22PictureBox.BackgroundImage = (status == "1") ? Properties.Resources.TJ22_1 : Properties.Resources.TJ22_2;
                else if (machine == "23 MC")
                    _parent.heMachineStatusTj23PictureBox.BackgroundImage = (status == "1") ? Properties.Resources.TJ23_1 : Properties.Resources.TJ23_2;


                //Console.WriteLine(machine + " : " + status);
            }
        }
    }
}