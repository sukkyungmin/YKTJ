using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace RealTimeRMT
{
    public class LibraryPage
    {
        public MainForm _parent = null;
        private bool[] _newLibraryFile = {false, false};

        public LibraryPage(MainForm parent)
        {
            _parent = parent;
        }

        public void InitControls()
        {
            _parent.lyTabControl.SelectedIndex = (int)ConstDefine.eLibraryTab.commonLibrary;

            InitSearchUserSectionComboBox();
            InitCommonLibraryGridView();
            InitPersonalLibraryGridView();
            InitNoticeBoardGridView();
            InitFreeBoardGridView();
            InitUserGridView(); 
        }

        public void ResetStatusBar(int libraryType)
        {
            Label[] fileCount = { _parent.lyTotalCommonFileCountLabel, _parent.lyTotalPersonalFileCountLabel };
            DataGridView[] libraryListView = { _parent.lyCommonLibraryGridView, _parent.lyPersonalLibraryGridView };

            fileCount[libraryType].Text = Utils.SetComma(libraryListView[libraryType].Rows.Count.ToString());
        }

        public void LoadLibraryList(int libraryType)
        {
            DataGridView[] libraryListView = { _parent.lyCommonLibraryGridView, _parent.lyPersonalLibraryGridView };
            string libraryTypeString = (libraryType == (int)ConstDefine.eLibraryType.common) ? "COMMON" : "PERSONAL";
            string userId = _parent.GetCurrentUserId();

            libraryListView[libraryType].Rows.Clear();

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} '{1}', '{2}'", "EXEC SelectLibraryList", libraryTypeString, userId));
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;
            
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                InsertLibraryListViewItem(libraryType, dataRow["No"].ToString(), dataRow["FileName"].ToString(), dataRow["FileDescription"].ToString(),
                    dataRow["ViewCount"].ToString(), dataRow["DownloadCount"].ToString(), dataRow["UserName"].ToString(), dataRow["CreatedDate"].ToString()); 
            }
            libraryListView[libraryType].ClearSelection();

            ResetStatusBar(libraryType);
        }

        public void OpenLibraryForm(bool newFile, int libraryType)
        {
             LibraryForm libraryForm = new LibraryForm(
                 newFile, 
                 this,
                 (libraryType == (int)ConstDefine.eLibraryType.common) ? _parent.lyCommonLibraryGridView : _parent.lyPersonalLibraryGridView,
                 libraryType);

             libraryForm.StartPosition = FormStartPosition.CenterParent; 
             libraryForm.ShowDialog(); 
        }

        public void InsertLibraryListViewItem(int libraryType, string no, string fileName, string fileDescription, string viewCount, 
            string downloadCount, string userName, string createdDate) 
        { 
            DataGridView[] libraryListView = { _parent.lyCommonLibraryGridView, _parent.lyPersonalLibraryGridView };
            libraryListView[libraryType].Rows.Add(no, fileName, fileDescription, Utils.SetComma(viewCount), Utils.SetComma(downloadCount), userName, createdDate); 
        }

        public void LoadNoticeBoardList()
        {
            _parent.lyNoticeBoardGridView.Rows.Clear();

            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectNoticeBoardList ''");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;
            
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                InsertNoticeBoardListViewItem(dataRow["No"].ToString(), dataRow["Title"].ToString(), dataRow["Contents"].ToString(),
                    dataRow["ViewCount"].ToString(), dataRow["FileName"].ToString(), dataRow["UserName"].ToString(), dataRow["CreatedDate"].ToString());
            }

            _parent.lyNoticeBoardGridView.ClearSelection();
        }

        public void InsertNoticeBoardListViewItem(string no, string title, string contents, string viewCount, string fileName,
            string userName, string createdDate)
        {
            _parent.lyNoticeBoardGridView.Rows.Add(no, title, Utils.SetComma(viewCount), userName, createdDate, fileName); 
        }

        public void OpenNoticeBoardForm(bool newBoard)
        {
            NoticeBoardForm noticeBoardForm = new NoticeBoardForm(newBoard, this, _parent.lyNoticeBoardGridView);
            noticeBoardForm.StartPosition = FormStartPosition.CenterParent;
            noticeBoardForm.ShowDialog();
        }


        public void LoadFreeBoardList()
        {
            _parent.lyFreeBoardGridView.Rows.Clear();

            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectFreeBoardList ''");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                InsertFreeBoardListViewItem(dataRow["No"].ToString(), dataRow["Title"].ToString(), dataRow["Contents"].ToString(),
                    dataRow["ViewCount"].ToString(), dataRow["FileName"].ToString(), dataRow["UserName"].ToString(), dataRow["CreatedDate"].ToString());
            }
            _parent.lyFreeBoardGridView.ClearSelection();
        }

        public void InsertFreeBoardListViewItem(string no, string title, string contents, string viewCount, string fileName,
            string userName, string createdDate)
        {
            _parent.lyFreeBoardGridView.Rows.Add(no, title, Utils.SetComma(viewCount), userName, createdDate, fileName);
        }

        public void OpenFreeBoardForm(bool newBoard)
        {
            FreeBoardForm freeBoardForm = new FreeBoardForm(newBoard, this, _parent.lyFreeBoardGridView);
            freeBoardForm.StartPosition = FormStartPosition.CenterParent;
            freeBoardForm.ShowDialog();
        }


        public void LoadUserList()
        {
            string key = ""; 
            string value = ""; 

            string userId = "";
            string userName = ""; 
            string securityValue = ""; 
            string positionValue = ""; 
            string teamTypeValue = ""; 
            string tjTypeValue = "";

            if (_parent.lySearchUserSectionComboBox.SelectedIndex != -1 && _parent.lySearchUserSectionComboBox.SelectedIndex != 0) 
            {
                key = (_parent.lySearchUserSectionComboBox.SelectedItem as ComboBoxItem).Value.ToString();
                value = _parent.lySearchUserValueTextBox.Text.Trim(); 

                if (key == ((int)ConstDefine.eLyUserListView.userId).ToString())
                    userId = value;
                else if (key == ((int)ConstDefine.eLyUserListView.userName).ToString())
                    userName = value;
                else if (key == ((int)ConstDefine.eLyUserListView.securityValue).ToString())
                    securityValue = value;
                else if (key == ((int)ConstDefine.eLyUserListView.positionValue).ToString())
                    positionValue = value;
                else if (key == ((int)ConstDefine.eLyUserListView.teamTypeValue).ToString())
                    teamTypeValue = value;
                else if (key == ((int)ConstDefine.eLyUserListView.tjTypeValue).ToString())
                    tjTypeValue = value;  
            }

            _parent.lyUserGridView.Rows.Clear();

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} '{1}', '{2}', '{3}', '{4}', '{5}', '{6}'",
                "EXEC SelectUserList", userId, userName, securityValue, positionValue, teamTypeValue, tjTypeValue));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++ )
            {

                InsertUserListViewItem(
                    Utils.ByteArrayToImage((Byte[])dataSet.Tables[0].Rows[i]["ProfilePicture"]),
                    dataSet.Tables[0].Rows[i]["UserName"].ToString(),
                    dataSet.Tables[0].Rows[i]["UserId"].ToString(),
                    //dataSet.Tables[0].Rows[i]["Password"].ToString(),
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
                    _parent.lyUserGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(204, 236, 254);
                else
                    _parent.lyUserGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    */ 
            }
            _parent.lyUserGridView.ClearSelection();
            _parent.lyUserGridView.Refresh();
        }

        public void InsertUserListViewItem(Image profilePicture, string userName, string userId, //string password,
            string securityValue, string phoneNumber, string mobileNumber, string email, string positionValue,
            string teamTypeValue, string tjTypeValue, string securityCode, string positionCode, string teamTypeCode, string tjTypeCode)
        {
            _parent.lyUserGridView.Rows.Add(
                (_parent.lyUserGridView.Rows.Count + 1).ToString(),
                profilePicture,
                userName,
                userId,
                //Utils.ReplacePasswordToAsterisk(password),
                securityValue,
                phoneNumber,
                mobileNumber,
                email,
                positionValue,
                teamTypeValue,
                tjTypeValue,
                //password, // plane password
                securityCode,
                positionCode,
                teamTypeCode,
                tjTypeCode);
        }

        public void InitSearchUserSectionComboBox()
        {
            _parent.lySearchUserSectionComboBox.Items.Clear();
            _parent.lySearchUserSectionComboBox.BeginUpdate();

            _parent.lySearchUserSectionComboBox.Items.Add(new ComboBoxItem("전체", 0));
            _parent.lySearchUserSectionComboBox.Items.Add(new ComboBoxItem("이름", (int)ConstDefine.eLyUserListView.userName));
            _parent.lySearchUserSectionComboBox.Items.Add(new ComboBoxItem("아이디", (int)ConstDefine.eLyUserListView.userId));
            _parent.lySearchUserSectionComboBox.Items.Add(new ComboBoxItem("보안 레벨", (int)ConstDefine.eLyUserListView.securityValue));
            _parent.lySearchUserSectionComboBox.Items.Add(new ComboBoxItem("직급", (int)ConstDefine.eLyUserListView.positionValue));
            _parent.lySearchUserSectionComboBox.Items.Add(new ComboBoxItem("담당팀", (int)ConstDefine.eLyUserListView.teamTypeValue));
            _parent.lySearchUserSectionComboBox.Items.Add(new ComboBoxItem("담당호기", (int)ConstDefine.eLyUserListView.tjTypeValue));

            _parent.lySearchUserSectionComboBox.EndUpdate();

            _parent.lySearchUserSectionComboBox.SelectedIndex = 0; 
        }

        public void ResizeControls()
        {
            _parent.lySidePanel.Width = ConstDefine.sidePanelSize; 

            _parent.lyTabControl.Left = _parent.lySidePanel.Width;
            _parent.lyTabControl.Top = 0;
            _parent.lyTabControl.Width = _parent.mainTabControl.Width - _parent.lySidePanel.Width;
            _parent.lyTabControl.Height = _parent.mainTabControl.Height;

            ////
            // 공용 라이브러리
            _parent.lyCommonLibraryGridView.Left = ConstDefine.defaultGap;
            _parent.lyCommonLibraryGridView.Top = ConstDefine.defaultGap;
            _parent.lyCommonLibraryGridView.Width = _parent.lyTabControl.Width - (ConstDefine.defaultGap *2);
            _parent.lyCommonLibraryGridView.Height = _parent.lyTabControl.Height - (_parent.lyCommonLibraryStatusBarPanel.Height + (ConstDefine.defaultGap *2));
            _parent.lyCommonLibraryStatusBarPanel.Top = _parent.lyCommonLibraryGridView.Bottom;
            _parent.lyCommonLibraryStatusBarPanel.Left = _parent.lyCommonLibraryGridView.Left;
            _parent.lyCommonLibraryStatusBarPanel.Width = _parent.lyCommonLibraryGridView.Width;

            int gridWidth = _parent.lyCommonLibraryGridView.Width - ConstDefine.scrollSize;
            int colWidth = gridWidth / _parent.lyCommonLibraryGridView.Columns.Count;
            int totalColWidth = 0;
            for (int i = 0; i < _parent.lyCommonLibraryGridView.Columns.Count; i++)
            {
                if ((int)ConstDefine.eLibraryListView.no == i)
                    _parent.lyCommonLibraryGridView.Columns[i].Width = 50;
                else
                    _parent.lyCommonLibraryGridView.Columns[i].Width = colWidth;

                totalColWidth += _parent.lyCommonLibraryGridView.Columns[i].Width;
            }
            if (gridWidth > totalColWidth)
                _parent.lyCommonLibraryGridView.Columns[(int)ConstDefine.eLibraryListView.fileDescription].Width += (gridWidth - totalColWidth);


            ////
            // 개인 라이브러리
            _parent.lyPersonalLibraryGridView.Left = ConstDefine.defaultGap;
            _parent.lyPersonalLibraryGridView.Top = ConstDefine.defaultGap;
            _parent.lyPersonalLibraryGridView.Width = _parent.lyTabControl.Width - (ConstDefine.defaultGap * 2);
            _parent.lyPersonalLibraryGridView.Height = _parent.lyTabControl.Height - (_parent.lyPersonalLibraryStatusBarPanel.Height + (ConstDefine.defaultGap * 2));
            _parent.lyPersonalLibraryStatusBarPanel.Top = _parent.lyPersonalLibraryGridView.Bottom;
            _parent.lyPersonalLibraryStatusBarPanel.Left = _parent.lyPersonalLibraryGridView.Left;
            _parent.lyPersonalLibraryStatusBarPanel.Width = _parent.lyPersonalLibraryGridView.Width;

            gridWidth = _parent.lyPersonalLibraryGridView.Width - ConstDefine.scrollSize;
            colWidth = gridWidth / _parent.lyPersonalLibraryGridView.Columns.Count;
            totalColWidth = 0;
            for (int i = 0; i < _parent.lyPersonalLibraryGridView.Columns.Count; i++)
            {
                if ((int)ConstDefine.eLibraryListView.no == i)
                    _parent.lyPersonalLibraryGridView.Columns[i].Width = 50;
                else
                    _parent.lyPersonalLibraryGridView.Columns[i].Width = colWidth;

                totalColWidth += _parent.lyPersonalLibraryGridView.Columns[i].Width;
            }
            if (gridWidth > totalColWidth)
                _parent.lyPersonalLibraryGridView.Columns[(int)ConstDefine.eLibraryListView.fileDescription].Width += (gridWidth - totalColWidth);


            ////
            // 공지사항
            _parent.lyNoticeBoardGridView.Left = ConstDefine.defaultGap;
            _parent.lyNoticeBoardGridView.Top = ConstDefine.defaultGap;
            _parent.lyNoticeBoardGridView.Width = _parent.lyTabControl.Width - (ConstDefine.defaultGap * 2);
            _parent.lyNoticeBoardGridView.Height = _parent.lyTabControl.Height - (_parent.lyNoticeBoardStatusBarPanel.Height + (ConstDefine.defaultGap * 2));
            _parent.lyNoticeBoardStatusBarPanel.Top = _parent.lyNoticeBoardGridView.Bottom;
            _parent.lyNoticeBoardStatusBarPanel.Left = _parent.lyNoticeBoardGridView.Left;
            _parent.lyNoticeBoardStatusBarPanel.Width = _parent.lyNoticeBoardGridView.Width;
            gridWidth = _parent.lyNoticeBoardGridView.Width - ConstDefine.scrollSize;
            colWidth = _parent.lyNoticeBoardGridView.Width / (_parent.lyNoticeBoardGridView.Columns.Count - 1);
            totalColWidth = 0;
            for (int i = 0; i < _parent.lyNoticeBoardGridView.Columns.Count; i++)
            {
                if ((int)ConstDefine.eBoardListView.no == i)
                    _parent.lyNoticeBoardGridView.Columns[i].Width = 50;
                else if ((int)ConstDefine.eBoardListView.fileName == i)
                    _parent.lyNoticeBoardGridView.Columns[i].Width = 0;
                else
                    _parent.lyNoticeBoardGridView.Columns[i].Width = colWidth;

                totalColWidth += _parent.lyNoticeBoardGridView.Columns[i].Width;
            }
            if (gridWidth > totalColWidth)
                _parent.lyNoticeBoardGridView.Columns[(int)ConstDefine.eBoardListView.title].Width += (gridWidth - totalColWidth);


            ////
            // 자유게시판
            _parent.lyFreeBoardGridView.Left = ConstDefine.defaultGap;
            _parent.lyFreeBoardGridView.Top = ConstDefine.defaultGap;
            _parent.lyFreeBoardGridView.Width = _parent.lyTabControl.Width - (ConstDefine.defaultGap * 2);
            _parent.lyFreeBoardGridView.Height = _parent.lyTabControl.Height - (_parent.lyFreeBoardStatusBarPanel.Height + (ConstDefine.defaultGap * 2));
            _parent.lyFreeBoardStatusBarPanel.Top = _parent.lyFreeBoardGridView.Bottom;
            _parent.lyFreeBoardStatusBarPanel.Left = _parent.lyFreeBoardGridView.Left;
            _parent.lyFreeBoardStatusBarPanel.Width = _parent.lyFreeBoardGridView.Width;
            gridWidth = _parent.lyFreeBoardGridView.Width - ConstDefine.scrollSize;
            colWidth = _parent.lyFreeBoardGridView.Width / (_parent.lyFreeBoardGridView.Columns.Count - 1);
            totalColWidth = 0;
            for (int i = 0; i < _parent.lyFreeBoardGridView.Columns.Count; i++)
            {
                if ((int)ConstDefine.eBoardListView.no == i)
                    _parent.lyFreeBoardGridView.Columns[i].Width = 50;
                else if ((int)ConstDefine.eBoardListView.fileName == i)
                    _parent.lyFreeBoardGridView.Columns[i].Width = 0;
                else
                    _parent.lyFreeBoardGridView.Columns[i].Width = colWidth;

                totalColWidth += _parent.lyFreeBoardGridView.Columns[i].Width;
            }
            if (gridWidth > totalColWidth)
                _parent.lyFreeBoardGridView.Columns[(int)ConstDefine.eBoardListView.title].Width += (gridWidth - totalColWidth);


            ////
            // 주소록
            const int ADDRESS_BOOK_SEARCH_PANEL_HEIGHT = 63; 
            _parent.lyAddressBookSearchPanel.Left = ConstDefine.defaultGap;
            _parent.lyAddressBookSearchPanel.Top = ConstDefine.defaultGap;
            _parent.lyAddressBookSearchPanel.Width = _parent.lyTabControl.Width;
            _parent.lyAddressBookSearchPanel.Height = ADDRESS_BOOK_SEARCH_PANEL_HEIGHT;

            _parent.lyUserGridView.Left = _parent.lyAddressBookSearchPanel.Left;
            _parent.lyUserGridView.Top = _parent.lyAddressBookSearchPanel.Bottom + ConstDefine.defaultGap;
            _parent.lyUserGridView.Width = _parent.lyTabControl.Width - (ConstDefine.defaultGap * 2);
            _parent.lyUserGridView.Height = _parent.lyTabControl.Height - (_parent.lyAddressBookSearchPanel.Height + (ConstDefine.defaultGap * 3));

            gridWidth = _parent.lyUserGridView.Width - ConstDefine.scrollSize;
            int noColWidth = 50;
            int profilePictureWidth = 100;
            int fixedColCount = 2;
            int invisibleColCount = 4;
            int eachColWidth = (gridWidth - (noColWidth + profilePictureWidth)) / (_parent.lyUserGridView.Columns.Count - (fixedColCount + invisibleColCount));
            int errorWidth = gridWidth - ((noColWidth + profilePictureWidth) + (eachColWidth * (_parent.lyUserGridView.Columns.Count - (fixedColCount + invisibleColCount))));
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.no].Width = noColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.profilePicture].Width = profilePictureWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.userName].Width = eachColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.userId].Width = eachColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.securityValue].Width = eachColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.phoneNumber].Width = eachColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.mobileNumber].Width = eachColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.email].Width = eachColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.positionValue].Width = eachColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.teamTypeValue].Width = eachColWidth;
            _parent.lyUserGridView.Columns[(int)ConstDefine.eLyUserListView.tjTypeValue].Width = eachColWidth + errorWidth;
        }

        public void LoadLyData()
        {
            if (_parent.lyTabControl.SelectedIndex == (int)ConstDefine.eLibraryTab.commonLibrary)
                LoadLibraryList((int)ConstDefine.eLibraryType.common);
            else if (_parent.lyTabControl.SelectedIndex == (int)ConstDefine.eLibraryTab.personalLibrary)
                LoadLibraryList((int)ConstDefine.eLibraryType.personal);
            else if (_parent.lyTabControl.SelectedIndex == (int)ConstDefine.eLibraryTab.noticeBoard)
                LoadNoticeBoardList();
            else if (_parent.lyTabControl.SelectedIndex == (int)ConstDefine.eLibraryTab.freeBoard)
                LoadFreeBoardList();
            else if(_parent.lyTabControl.SelectedIndex == (int)ConstDefine.eLibraryTab.addressBook)
                LoadUserList(); ;
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
            _parent.lyUserGridView.ColumnHeadersVisible = true;
            _parent.lyUserGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.lyUserGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.lyUserGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyUserGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.lyUserGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.lyUserGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.lyUserGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyUserGridView.AllowUserToResizeColumns = true;
            _parent.lyUserGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyUserGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.lyUserGridView.RowHeadersVisible = false;

            // Default row style
            _parent.lyUserGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyUserGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.lyUserGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.lyUserGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.lyUserGridView.AllowUserToResizeRows = false;
            //_parent.lyUserGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.lyUserGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyUserGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyUserGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyUserGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyUserGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.lyUserGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.lyUserGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.lyUserGridView.GridColor = GRID_COLOR;
            _parent.lyUserGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.lyUserGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.lyUserGridView.MultiSelect = false;
            _parent.lyUserGridView.ReadOnly = true;
            _parent.lyUserGridView.ScrollBars = ScrollBars.Vertical;

            _parent.lyUserGridView.Refresh();
        }
        public void InitCommonLibraryGridView()
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
            _parent.lyCommonLibraryGridView.ColumnHeadersVisible = true;
            _parent.lyCommonLibraryGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.lyCommonLibraryGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.lyCommonLibraryGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyCommonLibraryGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.lyCommonLibraryGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.lyCommonLibraryGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.lyCommonLibraryGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyCommonLibraryGridView.AllowUserToResizeColumns = true;
            _parent.lyCommonLibraryGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyCommonLibraryGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.lyCommonLibraryGridView.RowHeadersVisible = false;

            // Default row style
            _parent.lyCommonLibraryGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyCommonLibraryGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.lyCommonLibraryGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.lyCommonLibraryGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.lyCommonLibraryGridView.AllowUserToResizeRows = false;
            //_parent.lyCommonLibraryGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.lyCommonLibraryGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyCommonLibraryGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyCommonLibraryGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyCommonLibraryGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyCommonLibraryGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.lyCommonLibraryGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.lyCommonLibraryGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.lyCommonLibraryGridView.GridColor = GRID_COLOR;
            _parent.lyCommonLibraryGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.lyCommonLibraryGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.lyCommonLibraryGridView.MultiSelect = false;
            _parent.lyCommonLibraryGridView.ReadOnly = true;
            _parent.lyCommonLibraryGridView.ScrollBars = ScrollBars.Vertical;

            _parent.lyCommonLibraryGridView.Refresh();
        }
        public void InitPersonalLibraryGridView()
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
            _parent.lyPersonalLibraryGridView.ColumnHeadersVisible = true;
            _parent.lyPersonalLibraryGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.lyPersonalLibraryGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.lyPersonalLibraryGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyPersonalLibraryGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.lyPersonalLibraryGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.lyPersonalLibraryGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.lyPersonalLibraryGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyPersonalLibraryGridView.AllowUserToResizeColumns = true;
            _parent.lyPersonalLibraryGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyPersonalLibraryGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.lyPersonalLibraryGridView.RowHeadersVisible = false;

            // Default row style
            _parent.lyPersonalLibraryGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyPersonalLibraryGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.lyPersonalLibraryGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.lyPersonalLibraryGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.lyPersonalLibraryGridView.AllowUserToResizeRows = false;
            //_parent.lyPersonalLibraryGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.lyPersonalLibraryGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyPersonalLibraryGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyPersonalLibraryGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyPersonalLibraryGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyPersonalLibraryGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.lyPersonalLibraryGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.lyPersonalLibraryGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.lyPersonalLibraryGridView.GridColor = GRID_COLOR;
            _parent.lyPersonalLibraryGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.lyPersonalLibraryGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.lyPersonalLibraryGridView.MultiSelect = false;
            _parent.lyPersonalLibraryGridView.ReadOnly = true;
            _parent.lyPersonalLibraryGridView.ScrollBars = ScrollBars.Vertical;

            _parent.lyPersonalLibraryGridView.Refresh();

        }
        public void InitNoticeBoardGridView()
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
            _parent.lyNoticeBoardGridView.ColumnHeadersVisible = true;
            _parent.lyNoticeBoardGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.lyNoticeBoardGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.lyNoticeBoardGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyNoticeBoardGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.lyNoticeBoardGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.lyNoticeBoardGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.lyNoticeBoardGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyNoticeBoardGridView.AllowUserToResizeColumns = true;
            _parent.lyNoticeBoardGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyNoticeBoardGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.lyNoticeBoardGridView.RowHeadersVisible = false;

            // Default row style
            _parent.lyNoticeBoardGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyNoticeBoardGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.lyNoticeBoardGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.lyNoticeBoardGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.lyNoticeBoardGridView.AllowUserToResizeRows = false;
            //_parent.lyNoticeBoardGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.lyNoticeBoardGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyNoticeBoardGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyNoticeBoardGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyNoticeBoardGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyNoticeBoardGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.lyNoticeBoardGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.lyNoticeBoardGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.lyNoticeBoardGridView.GridColor = GRID_COLOR;
            _parent.lyNoticeBoardGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.lyNoticeBoardGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.lyNoticeBoardGridView.MultiSelect = false;
            _parent.lyNoticeBoardGridView.ReadOnly = true;
            _parent.lyNoticeBoardGridView.ScrollBars = ScrollBars.Vertical;

            _parent.lyNoticeBoardGridView.Columns[(int)ConstDefine.eBoardListView.fileName].Visible = false;

            _parent.lyNoticeBoardGridView.Refresh();
        }
        public void InitFreeBoardGridView()
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
            _parent.lyFreeBoardGridView.ColumnHeadersVisible = true;
            _parent.lyFreeBoardGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.lyFreeBoardGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.lyFreeBoardGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyFreeBoardGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.lyFreeBoardGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.lyFreeBoardGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.lyFreeBoardGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyFreeBoardGridView.AllowUserToResizeColumns = true;
            _parent.lyFreeBoardGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyFreeBoardGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.lyFreeBoardGridView.RowHeadersVisible = false;

            // Default row style
            _parent.lyFreeBoardGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.lyFreeBoardGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.lyFreeBoardGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.lyFreeBoardGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.lyFreeBoardGridView.AllowUserToResizeRows = false;
            //_parent.lyFreeBoardGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.lyFreeBoardGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.lyFreeBoardGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyFreeBoardGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyFreeBoardGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.lyFreeBoardGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.lyFreeBoardGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.lyFreeBoardGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.lyFreeBoardGridView.GridColor = GRID_COLOR;
            _parent.lyFreeBoardGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.lyFreeBoardGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.lyFreeBoardGridView.MultiSelect = false;
            _parent.lyFreeBoardGridView.ReadOnly = true;
            _parent.lyFreeBoardGridView.ScrollBars = ScrollBars.Vertical;

            _parent.lyFreeBoardGridView.Columns[(int)ConstDefine.eBoardListView.fileName].Visible = false;

            _parent.lyFreeBoardGridView.Refresh();
        }

        public int GetCurrentSecurityCode()
        {
            return _parent.GetCurrentSecurityCode(); 
        }

        public bool SelectNoticeBoardItem(string no)
        {
            for (int i = 0; i < _parent.lyNoticeBoardGridView.Rows.Count; i++)
            {
                if(no == _parent.lyNoticeBoardGridView.Rows[i].Cells[(int)ConstDefine.eBoardListView.no].Value.ToString().Trim())
                {
                    _parent.lyNoticeBoardGridView.Rows[i].Selected = true;
                    //_parent.lyNoticeBoardGridView.Rows[i].Focused = true; 
                    return true; 
                }
            }
            return false; 
        }

        public bool SelectFreeBoardItem(string no)
        {
            for (int i = 0; i < _parent.lyFreeBoardGridView.Rows.Count; i++)
            {
                if (no == _parent.lyFreeBoardGridView.Rows[i].Cells[(int)ConstDefine.eBoardListView.no].Value.ToString().Trim())
                {
                    _parent.lyFreeBoardGridView.Rows[i].Selected = true;
                    //_parent.lyFreeBoardGridView.Rows[i].Focused = true; 
                    return true;
                }
            }
            return false;
        }
    }
}
