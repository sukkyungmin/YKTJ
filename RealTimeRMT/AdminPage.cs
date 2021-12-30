using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace RealTimeRMT
{
    public class AdminPage
    {
        private MainForm _parent = null;

        public AdminPage(MainForm parent)
        {
            _parent = parent;
        }

        public void InitControls()
        {
            InitUserGridView();
            InitSecurityGridView(); 
        }

        public void LoadUserList()
        {
            _parent.anUserGridView.Rows.Clear(); 
            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectUserList '', '', '', '', '', ''");
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;


            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++ )
            {
                InsertUserListViewItem(
                    Utils.ByteArrayToImage(DBNull.Value.Equals(dataSet.Tables[0].Rows[i]["ProfilePicture"]) ? null : (Byte[])dataSet.Tables[0].Rows[i]["ProfilePicture"]),
                    dataSet.Tables[0].Rows[i]["UserName"].ToString(),
                    dataSet.Tables[0].Rows[i]["UserId"].ToString(),
                    dataSet.Tables[0].Rows[i]["Password"].ToString(),
                    dataSet.Tables[0].Rows[i]["SecurityValue"].ToString(),
                    dataSet.Tables[0].Rows[i]["PhoneNumber"].ToString(),
                    dataSet.Tables[0].Rows[i]["MobileNumber"].ToString(),
                    dataSet.Tables[0].Rows[i]["Email"].ToString(),
                    dataSet.Tables[0].Rows[i]["PositionValue"].ToString(),
                    dataSet.Tables[0].Rows[i]["TeamTypeValue"].ToString(),
                    dataSet.Tables[0].Rows[i]["TjTypeValue"].ToString(),
                    dataSet.Tables[0].Rows[i]["SecurityCode"].ToString(),
                    dataSet.Tables[0].Rows[i]["PositionCode"].ToString(),
                    dataSet.Tables[0].Rows[i]["teamTypeCode"].ToString(),
                    dataSet.Tables[0].Rows[i]["tjTypeCode"].ToString()
                    );

                /*
                if (i % 2 != 0)
                    _parent.anUserGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(204, 236, 254);
                else
                    _parent.anUserGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    */ 
            }
            _parent.anUserGridView.ClearSelection(); 

            ResetStatusBar(); 
        }

        public void LoadSecurityList()
        {
            _parent.anSecurityGridView.Rows.Clear(); 

            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectSecurityList -1");
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++ )
            {
                InsertSecurityListViewItem(
                    dataSet.Tables[0].Rows[i]["SecurityCode"].ToString(),
                    dataSet.Tables[0].Rows[i]["SecurityValue"].ToString(),
                    dataSet.Tables[0].Rows[i]["ViewRealTime"].ToString(),
                    dataSet.Tables[0].Rows[i]["ViewSapUpdate"].ToString(),
                    dataSet.Tables[0].Rows[i]["ViewTjAsset"].ToString(),
                    dataSet.Tables[0].Rows[i]["ViewLibrary"].ToString(),
                    dataSet.Tables[0].Rows[i]["ViewAdmin"].ToString());

                /*
                if (i % 2 != 0)
                    _parent.anSecurityGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(204, 236, 254);
                else
                    _parent.anSecurityGridView.Rows[i].DefaultCellStyle.BackColor = Color.White; 
                    */ 
            }
            //_parent.anSecurityGridView.Refresh(); 
            _parent.anSecurityGridView.ClearSelection(); 
        }

        public void InsertSecurityListViewItem(string securityCode, string securityValue, string viewRealTime, string viewSapUpdate,
            string viewTjAsset, string viewLibrary, string viewAdmin)
        {
            _parent.anSecurityGridView.Rows.Add(
                securityCode,
                securityValue,
                (viewRealTime == "1") ? true : false,
                (viewSapUpdate == "1") ? true : false,
                (viewTjAsset == "1") ? true : false,
                (viewLibrary == "1") ? true : false,
                (viewAdmin == "1") ? true : false);   
            //_parent.anSecurityGridView.Rows.Add(dgvRow);

            int lastRow = _parent.anSecurityGridView.Rows.Count - 1; 

            if (securityCode == ConstDefine.securityAdmin.ToString())
            {
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewRealTime].ReadOnly = true;
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewSapUpdate].ReadOnly = true;
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewTjAsset].ReadOnly = true;
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewLibrary].ReadOnly = true;
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewAdmin].ReadOnly = true;
            }
            else
            {
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewRealTime].ReadOnly = false;
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewSapUpdate].ReadOnly = false;
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewTjAsset].ReadOnly = false;
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewLibrary].ReadOnly = false;
                _parent.anSecurityGridView.Rows[lastRow].Cells[(int)ConstDefine.eSecurityListView.viewAdmin].ReadOnly = true;
            }
        }

        public void ChangeViewSecurity(int row, int column)
        {
            if (column == (int)ConstDefine.eSecurityListView.securityCode ||
                column == (int)ConstDefine.eSecurityListView.securityValue ||
                _parent.anSecurityGridView.Rows[row].Cells[(int)ConstDefine.eSecurityListView.securityCode].Value == null)
                return; 
            
            string securityCode = _parent.anSecurityGridView.Rows[row].Cells[(int)ConstDefine.eSecurityListView.securityCode].Value.ToString();
            if (securityCode == ConstDefine.securityAdmin.ToString())
            {
                MessageBox.Show("Admin 보안레벨은 권한을 변경 할 수 없습니다.", ConstDefine.adminTitle);
                return; 
            }
            else
            {
                if (column == (int)ConstDefine.eSecurityListView.viewAdmin)
                {
                    MessageBox.Show("Admin 보안레벨만 볼 수 있는 화면입니다", ConstDefine.adminTitle);
                    return; 
                }
            }

            string viewType = ""; 
            string viewValue = "";
            if (column == (int)ConstDefine.eSecurityListView.viewRealTime)
                viewType = "viewRealTime";
            else if (column == (int)ConstDefine.eSecurityListView.viewSapUpdate)
                viewType = "viewSapUpdate";
            else if (column == (int)ConstDefine.eSecurityListView.viewTjAsset)
                viewType = "viewTjAsset";
            else if (column == (int)ConstDefine.eSecurityListView.viewLibrary)
                viewType = "viewLibrary";
            else if (column == (int)ConstDefine.eSecurityListView.viewAdmin)
                viewType = "viewAdmin";

            if (Convert.ToBoolean(_parent.anSecurityGridView.Rows[row].Cells[column].Value) == true)
                viewValue = "0";
            else 
                viewValue = "1"; 

            int retVal = DbHelper.ExecuteNonQuery(string.Format("{0} {1}, '{2}', {3}", "EXEC UpdateSecurityView", securityCode, viewType, viewValue)); 
            if(retVal < 1)
            {
                MessageBox.Show("권한을 변경할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.adminTitle);
                return; 
            }
        }


        public void InsertUserListViewItem(Image profilePicture, string userName, string userId, string password,
            string securityValue, string phoneNumber, string mobileNumber, string email, string positionValue,
            string teamTypeValue, string tjTypeValue, string securityCode, string positionCode, string teamTypeCode, string tjTypeCode)
        {
            _parent.anUserGridView.Rows.Add(
                (_parent.anUserGridView.Rows.Count + 1).ToString(),
                profilePicture, 
                userName,
                userId,
                Utils.ReplacePasswordToAsterisk(password),
                securityValue,
                phoneNumber,
                mobileNumber,
                email,
                positionValue,
                teamTypeValue,
                tjTypeValue,
                password, // plane password
                securityCode,
                positionCode,
                teamTypeCode,
                tjTypeCode);
        }

        public void UpdateUserListViewItem(int row, Image profilePicture, string userName, string password,
            string securityValue, string phoneNumber, string mobileNumber, string email, string positionValue,
            string teamTypeValue, string tjTypeValue, string securityCode, string positionCode, string teamTypeCode, string tjTypeCode)
        {
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.profilePicture].Value = profilePicture;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.userName].Value = userName;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.password].Value = Utils.ReplacePasswordToAsterisk(password);
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.securityValue].Value = securityValue;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.phoneNumber].Value = phoneNumber;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.mobileNumber].Value = mobileNumber;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.email].Value = email;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.positionValue].Value = positionValue;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.teamTypeValue].Value = teamTypeValue;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.tjTypeValue].Value = tjTypeValue;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.planePassword].Value = password;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.securityCode].Value = securityCode;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.positionCode].Value = positionCode;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.teamTypeCode].Value = teamTypeCode;
            _parent.anUserGridView.Rows[row].Cells[(int)ConstDefine.eUserListView.tjTypeCode].Value = tjTypeCode;
        }

        public void ResetStatusBar()
        {
            _parent.anTotalUserCountLabel.Text = Utils.SetComma(_parent.anUserGridView.Rows.Count.ToString());
        }

        public void ResetUserListNo()
        {
            int count = _parent.anUserGridView.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                _parent.anUserGridView.Rows[i].Cells[(int)ConstDefine.eUserListView.no].Value = (i + 1).ToString();
            }
        }

        public void OpenUserForm(bool newUser, int rowIndex = -1)
        {
            UserForm userForm = new UserForm(newUser, this, _parent.anUserGridView, rowIndex);
            userForm.StartPosition = FormStartPosition.CenterParent;
            userForm.ShowDialog(); 
        }

        public void InitUserGridView()
        {
            Color GRID_COLUMN_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_COLUMN_BACK_COLOR = Color.FromArgb(58, 65, 74);
            Color GRID_ROW_FORE_COLOR = Color.FromArgb(216, 217, 218);
            Color GRID_ROW_BACK_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_COLOR = Color.FromArgb(108, 115, 123);
            //Color GRID_COLOR = Color.FromArgb(60, 69, 80);
            Color GRID_BACK_COLOR = Color.FromArgb(60, 69, 80);

            const int GRID_COLUMN_HEIGHT = 35;
            const int GRID_ROW_HEIGHT = 80;

            // Default column style
            _parent.anUserGridView.ColumnHeadersVisible = true;
            _parent.anUserGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.anUserGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.anUserGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.anUserGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.anUserGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.anUserGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.anUserGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.anUserGridView.AllowUserToResizeColumns = true;
            _parent.anUserGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.anUserGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.anUserGridView.RowHeadersVisible = false;

            // Default row style
            _parent.anUserGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.anUserGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.anUserGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.anUserGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.anUserGridView.AllowUserToResizeRows = false;
            //_parent.anUserGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.anUserGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.anUserGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.anUserGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.anUserGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.anUserGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.anUserGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.anUserGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.anUserGridView.GridColor = GRID_COLOR;
            _parent.anUserGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.anUserGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.anUserGridView.MultiSelect = false;
            _parent.anUserGridView.ReadOnly = true;
            _parent.anUserGridView.ScrollBars = ScrollBars.Vertical;

            _parent.anUserGridView.Refresh();
        }

        public void InitSecurityGridView()
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
            _parent.anSecurityGridView.ColumnHeadersVisible = true;
            _parent.anSecurityGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.anSecurityGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.anSecurityGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.anSecurityGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.anSecurityGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.anSecurityGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.anSecurityGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.anSecurityGridView.AllowUserToResizeColumns = true;
            _parent.anSecurityGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.anSecurityGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.anSecurityGridView.RowHeadersVisible = false;

            // Default row style
            _parent.anSecurityGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.anSecurityGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.anSecurityGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.anSecurityGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.anSecurityGridView.AllowUserToResizeRows = false;
            //_parent.anSecurityGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.anSecurityGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.anSecurityGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.anSecurityGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.anSecurityGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.anSecurityGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.anSecurityGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.anSecurityGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.anSecurityGridView.GridColor = GRID_COLOR;
            _parent.anSecurityGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.anSecurityGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.anSecurityGridView.MultiSelect = false;
            _parent.anSecurityGridView.ReadOnly = false;
            _parent.anSecurityGridView.ScrollBars = ScrollBars.Vertical;

            _parent.anSecurityGridView.Refresh();
        }

        public void ResizeControls()
        {
            _parent.anSidePanel.Width = ConstDefine.sidePanelSize;

            _parent.anTabControl.Left = _parent.anSidePanel.Width;
            _parent.anTabControl.Top = 0;
            _parent.anTabControl.Width = _parent.mainTabControl.Width - _parent.anSidePanel.Width;
            _parent.anTabControl.Height = _parent.mainTabControl.Height;

            _parent.anUserGridView.Left = ConstDefine.defaultGap;
            _parent.anUserGridView.Top = ConstDefine.defaultGap;
            _parent.anUserGridView.Width = _parent.anTabControl.Width - (ConstDefine.defaultGap * 2);
            _parent.anUserGridView.Height = _parent.anTabControl.Height - (_parent.anUserStatusBarPanel.Height + (ConstDefine.defaultGap * 2));

            _parent.anUserStatusBarPanel.Top = _parent.anUserGridView.Bottom;
            _parent.anUserStatusBarPanel.Left = _parent.anUserGridView.Left;
            _parent.anUserStatusBarPanel.Width = _parent.anUserGridView.Width;

            int noColWidth = 50;
            int profilePictureWidth = 100;
            int fixedColCount = 2;
            int invisibleColCount = 5;
            int eachColWidth = (_parent.anUserGridView.Width - (noColWidth + profilePictureWidth)) / (_parent.anUserGridView.Columns.Count - (fixedColCount + invisibleColCount));
            int errorWidth = _parent.anUserGridView.Width - ((noColWidth + profilePictureWidth) + (eachColWidth * (_parent.anUserGridView.Columns.Count - (fixedColCount + invisibleColCount))));
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.no].Width = noColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.profilePicture].Width = profilePictureWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.userName].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.userId].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.password].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.securityValue].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.phoneNumber].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.mobileNumber].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.email].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.positionValue].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.teamTypeValue].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.tjTypeValue].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.planePassword].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.securityCode].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.positionCode].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.teamTypeCode].Width = eachColWidth;
            _parent.anUserGridView.Columns[(int)ConstDefine.eUserListView.tjTypeCode].Width = eachColWidth + errorWidth;


            _parent.anSecurityGridView.Left = ConstDefine.defaultGap;
            _parent.anSecurityGridView.Top = ConstDefine.defaultGap;
            _parent.anSecurityGridView.Width = _parent.anTabControl.Width - (ConstDefine.defaultGap * 2);
            _parent.anSecurityGridView.Height = _parent.anTabControl.Height - (ConstDefine.defaultGap * 2);

            eachColWidth = _parent.anSecurityGridView.Width / _parent.anSecurityGridView.Columns.Count;
            errorWidth = _parent.anSecurityGridView.Width - (eachColWidth * _parent.anSecurityGridView.Columns.Count);
            _parent.anSecurityGridView.Columns[(int)ConstDefine.eSecurityListView.securityCode].Width = eachColWidth;
            _parent.anSecurityGridView.Columns[(int)ConstDefine.eSecurityListView.securityValue].Width = eachColWidth;
            _parent.anSecurityGridView.Columns[(int)ConstDefine.eSecurityListView.viewRealTime].Width = eachColWidth;
            _parent.anSecurityGridView.Columns[(int)ConstDefine.eSecurityListView.viewSapUpdate].Width = eachColWidth;
            _parent.anSecurityGridView.Columns[(int)ConstDefine.eSecurityListView.viewTjAsset].Width = eachColWidth;
            _parent.anSecurityGridView.Columns[(int)ConstDefine.eSecurityListView.viewLibrary].Width = eachColWidth;
            _parent.anSecurityGridView.Columns[(int)ConstDefine.eSecurityListView.viewAdmin].Width = eachColWidth + errorWidth;
        }

        public void LoadAnData() 
        {
            if (_parent.anTabControl.SelectedIndex == (int)ConstDefine.eAdminTab.userList)
                LoadUserList();
            else if (_parent.anTabControl.SelectedIndex == (int)ConstDefine.eAdminTab.security) 
                LoadSecurityList(); 
        }

    }
}
