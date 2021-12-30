using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;

namespace RealTimeRMT
{
    //public partial class MainForm : Form
    public partial class MainForm : C1.Win.C1Ribbon.C1RibbonForm
    {
        // Login User Info
        public string _userId = "";
        public string _userName = "";
        public int _securityCode = -1;
        public string _securityValue = "";
        public string _viewRealTime = "0";
        public string _viewSapUpdate = "0";
        public string _viewTjAsset = "0";
        public string _viewLibrary = "0";
        public string _viewAdmin = "0";
        public string _viewCov = "0";

        public bool _isClosedLoadingBar = false;

        private HomePage _homePage = null;
        private RealTimePage _realTimePage = null;
        private SapUpdatePage _sapUpdatePage = null;
        private TjAssetPage _tjAssetPage = null;
        private LibraryPage _libraryPage = null;
        private AdminPage _adminPage = null;
        private bool _inLoginPanelArea = false;


        // Slide Menu
        enum eSlideMenuStatus { SHOWING = 0, HIDING, TOGGLE, CLOSE };
        private eSlideMenuStatus _slideMenuStatus = eSlideMenuStatus.HIDING;
        private System.Timers.Timer _slideMenuTimer = null;
        delegate void SlideMenuTimerDelegate();
        const int FRMAE_PER_SEC = 32;
        private int _sizePerFrame = 0;
        const int SLIDE_MENU_WIDTH = 470;

        public MainForm()
        {
            GlobalMouseHandler gmh = new GlobalMouseHandler();
            gmh.TheMouseMoved += new MouseMovedEvent(gmh_TheMouseMoved);
            Application.AddMessageFilter(gmh);

            InitializeComponent();
        }

        private void Init()
        {
            OpenLoadingBar();

            // 각각의 탭 페이지 관리 클래스 생성
            _homePage = new HomePage(this);
            _realTimePage = new RealTimePage(this);
            _sapUpdatePage = new SapUpdatePage(this);
            _tjAssetPage = new TjAssetPage(this);
            _libraryPage = new LibraryPage(this);
            _adminPage = new AdminPage(this);

            InitControls();
            ResizeControls();
            CloseLoadingBar();
            InitSlideMenu();
        }

        private bool Login()
        {
            LoginForm loginForm = new LoginForm(this);
            DialogResult result = loginForm.ShowDialog();
            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }

        private void InitControls()
        {
            this.Width = ConstDefine.formWidth;
            this.Height = ConstDefine.formHeight;

            InitRibbonTabControl();

            _homePage.InitControls();
            _realTimePage.InitControls();
            _sapUpdatePage.InitControls();
            _tjAssetPage.InitControls();
            _libraryPage.InitControls();
            _adminPage.InitControls();


            firstSideMenuHomeButton.IconVisible = true;
            firstSideMenuRmtButton.IconVisible = true;
            firstSideMenuSapUpdateButton.IconVisible = true;
            firstSideMenuTjAssetButton.IconVisible = true;
            firstSideMenuKpiButton.IconVisible = true;
            firstSideMenuCovButton.IconVisible = true;
            firstSideMenuLibraryButton.IconVisible = true;
            firstSideMenuAdminButton.IconVisible = true;
        }

        private void InitRibbonTabControl()
        {
            SetCurretUserInfo();
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
            EnableViewByUserSecurity();
        }

        private void rtRawMaterialSumButton_Click(object sender, EventArgs e)
        {
            _realTimePage.ResetRtReportButtons((sender as ImageButton).Name);
            rtFilterTabControl.Visible = true;
            rtFilterTabControl.SelectedIndex = (int)ConstDefine.eRtFilterTab.rawMaterialSum;
            _realTimePage.MoveRtCreateReportButton();
        }

        private void rtRawMaterialDetailSumButton_Click(object sender, EventArgs e)
        {
            _realTimePage.ResetRtReportButtons((sender as ImageButton).Name);
            rtFilterTabControl.Visible = true;
            rtFilterTabControl.SelectedIndex = (int)ConstDefine.eRtFilterTab.rawMaterialDetailSum;
            _realTimePage.MoveRtCreateReportButton();
        }

        private void rtProductionHistoryButton_Click(object sender, EventArgs e)
        {
            _realTimePage.ResetRtReportButtons((sender as ImageButton).Name);
            rtFilterTabControl.Visible = false;
            _realTimePage.MoveRtCreateReportButton();
        }

        private void rtTotalWasteButton_Click(object sender, EventArgs e)
        {
            _realTimePage.ResetRtReportButtons((sender as ImageButton).Name);
            rtFilterTabControl.Visible = true;
            rtFilterTabControl.SelectedIndex = (int)ConstDefine.eRtFilterTab.totalWaste;
            _realTimePage.MoveRtCreateReportButton();
        }

        public void OpenLoadingBar()
        {
            this.Cursor = Cursors.WaitCursor;
            /*
            if (loadingBarWorker.IsBusy == false)
                loadingBarWorker.RunWorkerAsync();

            Thread.Sleep(300);
            while (!loadingBarWorker.IsBusy) ;
            */
        }

        public void CloseLoadingBar()
        {
            this.Cursor = Cursors.Default;
            //_isClosedLoadingBar = true;
        }

        public void loadingBarWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _isClosedLoadingBar = false;
            LoadingBarDlg loadingBarDlg = new LoadingBarDlg(this);

            //loadingBarDlg.StartPosition = FormStartPosition.CenterParent;
            loadingBarDlg.StartPosition = FormStartPosition.Manual;

            Point pos = new Point();
            pos.X = (this.Left + this.Width / 2) - (loadingBarDlg.Width / 2);
            pos.Y = (this.Top + this.Height / 2) - (loadingBarDlg.Height / 2);
            loadingBarDlg.Location = pos;

            loadingBarDlg.ShowDialog();
        }

        // Butt Chcek 버튼 상태 변경
        private void ResetButtValueButtonStatus(string currentName)
        {
            ImageButton[] buttonItems = { suTjButtValue1Button, suTjButtValue5Button, suTjButtValue10Button, suTjButtValue50Button, suTjButtValue100Button };

            for (int i = 0; i < buttonItems.Length; i++)
            {
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;
            }
        }

        private void suTj21ButtTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.AcceptOnlyDigit(sender, e);
        }

        private void suTj21ButtTextBox_TextChanged(object sender, EventArgs e)
        {
            Utils.SetComma(sender);
        }

        private void suTj22ButtTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.AcceptOnlyDigit(sender, e);
        }

        private void suTj22ButtTextBox_TextChanged(object sender, EventArgs e)
        {
            Utils.SetComma(sender);
        }

        private void ssSapUpdateTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sapUpdatePage.LoadSaData();
        }

        private void rtPeriodDayRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rtPeriodEndDateTimePicker.Enabled = !rtPeriodDayRadioButton.Checked;
        }

        private void rtPeriodPeriodRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rtPeriodEndDateTimePicker.Enabled = rtPeriodPeriodRadioButton.Checked;
        }

        public int GetTjTypeValue(int tjType)
        {
            int tjTypeValue = tjType + 1; // 0 based 에서 1 based(db value)
            /*
            if (tjTypeValue > 5)
                tjTypeValue += 15; // tj21, 22 값
            */
            if (tjType == (int)ConstDefine.eTjType.tj21)
                tjTypeValue = 21;
            else if (tjType == (int)ConstDefine.eTjType.tj22)
                tjTypeValue = 22;
            else if (tjType == (int)ConstDefine.eTjType.tj23)
                tjTypeValue = 23;

            return tjTypeValue;
        }

        private void rtCreateReportButton_Click(object sender, EventArgs e)
        {
            if (false == _realTimePage.IsValidPeriod())
            {
                MessageBox.Show("검색기간을 올바르게 설정해 주십시오.", ConstDefine.searchTitle);
                return;
            }

            OpenLoadingBar();
            _realTimePage.AddReport();
            _realTimePage.InsertRealTimeSearchLog();
            CloseLoadingBar();
        }

        private void taOeeFlButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeUdButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeTj01Button_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeTj02Button_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeTj03Button_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeTj04Button_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeTj05Button_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeTj21Button_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeTj22Button_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetOeeButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartMachineStatusTimer();
        }

        private void taOeeRefreshTotalMachineStatusButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.LoadMachineStatus();
        }

        private void machineStatusTimer_Tick(object sender, EventArgs e)
        {
            _tjAssetPage.LoadMachineStatus();
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (homeMachineStatusTimer.Enabled == true)
                homeMachineStatusTimer.Stop();

            if (machineStatusTimer.Enabled == true)
                machineStatusTimer.Stop();

            if (kpiStatusTimer.Enabled == true)
                kpiStatusTimer.Stop();

            if (mainTabControl.SelectedIndex == (int)ConstDefine.eMainTab.home)
            {
                SetCurretUserInfo();
                _homePage.LoadHeData();
            }
            else if (mainTabControl.SelectedIndex == (int)ConstDefine.eMainTab.realTime)
            {
                // Nothing to do
            }
            else if (mainTabControl.SelectedIndex == (int)ConstDefine.eMainTab.sapUpdate)
            {
                _sapUpdatePage.LoadSaData();
            }
            else if (mainTabControl.SelectedIndex == (int)ConstDefine.eMainTab.tjAsset)
            {
                _tjAssetPage.LoadTaData();
            }
            else if (mainTabControl.SelectedIndex == (int)ConstDefine.eMainTab.library)
            {
                _libraryPage.LoadLyData();
            }
            else if (mainTabControl.SelectedIndex == (int)ConstDefine.eMainTab.admin)
            {
                _adminPage.LoadAnData();
            }
        }

        private void taTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            _tjAssetPage.LoadTaData();
        }

        private void taProductionCalendarButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetProductionCalendarButtons((sender as CheckBox).Name);
            _tjAssetPage.LoadProductionPredictionCalendarList();
            _tjAssetPage.LoadProductionPredictionScheduleList();
        }

        private void taProductionPredictionButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetProductionPredictionButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.LoadProductionPrediction();
        }

        private void suSelectScheduleFileButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.SelectSapSourceFile((int)ConstDefine.eSapFileType.schedule);
        }

        private void suSelectProductFileButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.SelectSapSourceFile((int)ConstDefine.eSapFileType.product);
        }

        private void suSelectBomFileButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.SelectSapSourceFile((int)ConstDefine.eSapFileType.bom);
        }

        private void suSelectLengthFileButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.SelectSapSourceFile((int)ConstDefine.eSapFileType.length);
        }

        private void suCancelFilesButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.InitUpdate();
        }

        private void suUpdateScheduleButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.UpdateSchedule();
        }

        private void suUpdateProductButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.UpdateProduct();
        }

        private void suUpdateBomButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.UpdateBom();
        }

        private void suUpdateBatchButton_Click(object sender, EventArgs e)
        {
            OpenLoadingBar();
            if (false == _sapUpdatePage.UpdateSchedule(true) ||
                false == _sapUpdatePage.UpdateProduct(true) ||
                false == _sapUpdatePage.UpdateBom(true))
            {
                CloseLoadingBar();
                return;
            }

            _sapUpdatePage.SetUpdatedDbState((int)ConstDefine.eSapFileType.batch);

            CloseLoadingBar();
        }

        private void suTjButtListView_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.SetButtInfo();
        }

        private void suTjButtValue1Button_Click(object sender, EventArgs e)
        {
            ResetButtValueButtonStatus((sender as ImageButton).Name);
            _sapUpdatePage.SetButtValue("1");
        }

        private void suTjButtValue5Button_Click(object sender, EventArgs e)
        {
            ResetButtValueButtonStatus((sender as ImageButton).Name);
            _sapUpdatePage.SetButtValue("5");
        }

        private void suTjButtValue10Button_Click(object sender, EventArgs e)
        {
            ResetButtValueButtonStatus((sender as ImageButton).Name);
            _sapUpdatePage.SetButtValue("10");
        }

        private void suTjButtValue50Button_Click(object sender, EventArgs e)
        {
            ResetButtValueButtonStatus((sender as ImageButton).Name);
            _sapUpdatePage.SetButtValue("50");
        }

        private void suTjButtValue100Button_Click(object sender, EventArgs e)
        {
            ResetButtValueButtonStatus((sender as ImageButton).Name);
            _sapUpdatePage.SetButtValue("100");
        }

        private void suTjUpdateButtButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.UpdateButtValue();
        }

        private void taBcProductionPlanButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetProductionPlanButtons((sender as CheckBox).Name);
            _tjAssetPage.LoadProductionPlan();
        }

        private void taCcProductionPlanButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetProductionPlanButtons((sender as CheckBox).Name);
            _tjAssetPage.LoadProductionPlan();
        }

        private void taOeeRefreshMachineStatusButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.LoadMachineStatus();
        }



        private void taProductionPredictionListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _tjAssetPage.LoadPpProductInfo();
            _tjAssetPage.LoadPpScheduleAmountInfo();
            _tjAssetPage.LoadPpMaterialInfoList();
        }


        /**
         * 아래 두 함수의 로직은 별 의미가 없는 것 같다. 협의가 필요하다. 
         */
        private void taProductionPredictionEstimateButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.EstimateProductionPrediction();
            taProductionPredictionEstimateButton.Enabled = false;
            taProductionPredictionEstimateButton.BackColor = Color.DarkGray;
        }

        private void taProductionPredictionAmountTextBox_Enter(object sender, EventArgs e)
        {
            _tjAssetPage.SetPpOldAmountValue();
            taProductionPredictionEstimateButton.Enabled = true;
            taProductionPredictionEstimateButton.BackColor = Color.FromArgb(241, 241, 241);
        }

        private void anSecurityGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            _adminPage.ChangeViewSecurity(e.RowIndex, e.ColumnIndex);
        }

        private void anAddUserButton_Click(object sender, EventArgs e)
        {
            _adminPage.OpenUserForm(true);
        }

        private void anUserGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            _adminPage.OpenUserForm(false, e.RowIndex);
        }

        public string GetCurrentUserName()
        {
            return _userName;
        }

        public string GetCurrentUserId()
        {
            return _userId;
        }

        private void lyAddCommonLibraryButton_Click(object sender, EventArgs e)
        {
            _libraryPage.OpenLibraryForm(true, (int)ConstDefine.eLibraryType.common);
        }

        private void lyAddPersonalLibraryButton_Click(object sender, EventArgs e)
        {
            _libraryPage.OpenLibraryForm(true, (int)ConstDefine.eLibraryType.personal);
        }

        private void lyCommonLibraryListView_DoubleClick(object sender, EventArgs e)
        {
            _libraryPage.OpenLibraryForm(false, (int)ConstDefine.eLibraryType.common);
        }

        private void lyPersonalLibraryListView_DoubleClick(object sender, EventArgs e)
        {
            _libraryPage.OpenLibraryForm(false, (int)ConstDefine.eLibraryType.personal);
        }

        private void lyAddNoticeBoardButton_Click(object sender, EventArgs e)
        {
            _libraryPage.OpenNoticeBoardForm(true);
        }

        private void lyNoticeBoardListView_DoubleClick(object sender, EventArgs e)
        {
            _libraryPage.OpenNoticeBoardForm(false);
        }

        private void lyFreeBoardButton_Click(object sender, EventArgs e)
        {
            _libraryPage.OpenFreeBoardForm(true);
        }

        private void lyFreeBoardListView_DoubleClick(object sender, EventArgs e)
        {
            _libraryPage.OpenFreeBoardForm(false);
        }

        private void lySearchUserButton_Click(object sender, EventArgs e)
        {
            _libraryPage.LoadUserList();
        }

        private void rtFilterWasteLossTwTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.AcceptOnlyDecimal(sender, e);
        }

        private void rtFilterSerDataTwTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.AcceptOnlyDecimal(sender, e);
        }

        private void rtFilterSpliceDataTwTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.AcceptOnlyDecimal(sender, e);
        }

        private void heSearchLogGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            _homePage.OpenRealTimeSearchConditionForm(e.ColumnIndex, e.RowIndex);
        }

        private void ResizeControls()
        {

            topPanel.Left = 0;
            topPanel.Top = 0;
            topPanel.Width = this.ClientSize.Width;
            topPanel.Height = ConstDefine.TOP_PANEL_HEIGHT;

            loginUserPanel.Left = topPanel.Width - loginUserPanel.Width;
            loginUserLabel.Left = loginUserPictureBox.Left - loginUserLabel.Width;

            // Main Tab
            mainTabControl.Left = 0;
            mainTabControl.Top = ConstDefine.TOP_PANEL_HEIGHT;
            mainTabControl.Width = this.ClientSize.Width;
            mainTabControl.Height = this.ClientSize.Height - ConstDefine.TOP_PANEL_HEIGHT;

            if (_homePage != null)
                _homePage.ResizeControls();

            if (_realTimePage != null)
                _realTimePage.ResizeControls();

            if (_sapUpdatePage != null)
                _sapUpdatePage.ResizeControls();

            if (_tjAssetPage != null)
                _tjAssetPage.ResizeControls();

            if (_libraryPage != null)
                _libraryPage.ResizeControls();

            if (_adminPage != null)
                _adminPage.ResizeControls();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void topRibbonTabControl_MinimizedChanged(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void suDbUpdateButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.ResetTeamButtons((sender as CheckBox).Name);
            _sapUpdatePage.InitUpdate();
            _sapUpdatePage.LoadSapUpdateLog();
        }

        private void suTjButton_Click(object sender, EventArgs e)
        {
            _sapUpdatePage.ResetTjButtons((sender as CustomControls.ButtonEx01).Name);
            _sapUpdatePage.InitSetting();
            _sapUpdatePage.LoadButtInfo();
        }

        private void anTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            _adminPage.LoadAnData();
        }

        private void lyTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            _libraryPage.LoadLyData();
        }

        private void suTjButtTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.AcceptOnlyDigit(sender, e);
        }

        private void currentUserRibbonMenu_Click(object sender, EventArgs e)
        {
            _homePage.OpenUserForm(_userId);
        }

        public void SetUserName(string userName)
        {
            _userName = userName;
        }
        public void SetCurretUserInfo()
        {
            loginUserLabel.Text = string.Format("{0} {1} 님", _securityValue, _userName);
        }

        public void EnableViewByUserSecurity()
        {
            firstSideMenuRmtButton.Enabled = _viewRealTime.Trim() == "1" ? true : false;
            firstSideMenuSapUpdateButton.Enabled = _viewSapUpdate.Trim() == "1" ? true : false;
            firstSideMenuTjAssetButton.Enabled = _viewTjAsset.Trim() == "1" ? true : false;
            firstSideMenuKpiButton.Enabled = _viewTjAsset.Trim() == "1" ? true : false;
            firstSideMenuLibraryButton.Enabled = _viewLibrary.Trim() == "1" ? true : false;
            firstSideMenuAdminButton.Enabled = _viewAdmin.Trim() == "1" ? true : false;

            /*
            lyAddCommonLibraryButton.Enabled = _viewAdmin.Trim() == "1" ? true : false;
            lyAddNoticeBoardButton.Enabled = _viewAdmin.Trim() == "1" ? true : false;

            lyAddCommonLibraryButton.BackColor = _viewAdmin.Trim() == "1" ? Color.FromArgb(240, 240, 240) : Color.DarkGray;
            lyAddNoticeBoardButton.BackColor = _viewAdmin.Trim() == "1" ? Color.FromArgb(240, 240, 240) : Color.DarkGray;
            */
        }

        public int GetCurrentSecurityCode()
        {
            return _securityCode;
        }

        private void taViewTodayProductionScheduleList_Click(object sender, EventArgs e)
        {
            _tjAssetPage.LoadProductionPredictionScheduleList(true);
            _tjAssetPage.SetSelectedProductionScheduleEventDay(true);
        }

        private void taProductionScheduleCalendar_Click(object sender, EventArgs e)
        {
            _tjAssetPage.LoadProductionPredictionScheduleList();
            _tjAssetPage.SetSelectedProductionScheduleEventDay();
        }

        /*
        private void taProductionScheduleCalendar_DoubleClick(object sender, EventArgs e)
        {
            if (taProductionScheduleCalendar.ViewType != C1.Win.C1Schedule.ScheduleViewEnum.MonthView)
                taProductionScheduleCalendar.ViewType = C1.Win.C1Schedule.ScheduleViewEnum.MonthView;
        }
        */

        private void heSearchLogGridView_Leave(object sender, EventArgs e)
        {
            heSearchLogGridView.ClearSelection();
        }

        private void heNoticeBoardGridView_Leave(object sender, EventArgs e)
        {
            heNoticeBoardGridView.ClearSelection();
        }

        private void heFreeBoardGridView_Leave(object sender, EventArgs e)
        {
            heFreeBoardGridView.ClearSelection();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_userId != "")
                Init();
            else
                Close();
        }

        private void taOeeLayoutPictureBox_Click(object sender, EventArgs e)
        {
            _tjAssetPage.LayoutClick();
        }

        private void taOeeLayoutPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            _tjAssetPage.LayoutMouseMove();
        }

        private void rtFilterComboBox_TextChanged(object sender, EventArgs e)
        {
            _realTimePage.CheckFilter(sender as PresentationControls.CheckBoxComboBox);
        }

        private void rtFilterSapCodeRsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.AcceptOnlyDigit(sender, e);
        }

        private void rtFilterSapCodeRdsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.AcceptOnlyDigit(sender, e);
        }

        private void heNoticeBoardGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_viewLibrary == "0")
            {
                MessageBox.Show("공지사항의 글을 볼 수 있는 권한이 없습니다.", ConstDefine.noticeBoardTitle);
                return;
            }

            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library;
            lyTabControl.SelectedIndex = (int)ConstDefine.eLibraryTab.noticeBoard;
            string no = heNoticeBoardGridView.Rows[e.RowIndex].Cells[(int)ConstDefine.eHeBoardListView.no].Value.ToString();
            _libraryPage.SelectNoticeBoardItem(no);
            _libraryPage.OpenNoticeBoardForm(false);
        }

        private void heFreeBoardGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_viewLibrary == "0")
            {
                MessageBox.Show("자유게시판의 글을 볼 수 있는 권한이 없습니다.", ConstDefine.freeBoardTitle);
                return;
            }
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library;
            lyTabControl.SelectedIndex = (int)ConstDefine.eLibraryTab.freeBoard;
            string no = heFreeBoardGridView.Rows[e.RowIndex].Cells[(int)ConstDefine.eHeBoardListView.no].Value.ToString();
            _libraryPage.SelectFreeBoardItem(no);
            _libraryPage.OpenFreeBoardForm(false);
        }

        private void rtPeriodStartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            _realTimePage.ResetPeriodEndDateTime();
        }

        private void taProductionScheduleCalendar_DoubleClick(object sender, EventArgs e)
        {
            if (taProductionScheduleCalendar.ViewType != C1.Win.C1Schedule.ScheduleViewEnum.MonthView)
                taProductionScheduleCalendar.ViewType = C1.Win.C1Schedule.ScheduleViewEnum.MonthView;
        }

        private void taProductionScheduleCalendar_BeforeAppointmentFormat(object sender, C1.Win.C1Schedule.BeforeAppointmentFormatEventArgs e)
        {
            e.Text = e.Appointment.Subject;
        }

        private void taTjKpiCheckBox_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetKpiButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.StartKpiStatusTimer();
        }



        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            ////
            // 가로 병합
            if (IsHorMergeCell(e.RowIndex, e.ColumnIndex) == true)
            {
                Graphics g = e.Graphics;
                //SolidBrush backColorBrush = new SolidBrush(Color.FromArgb(240, 240, 240)); // new SolidBrush(e.CellStyle.BackColor);
                SolidBrush backColorBrush = new SolidBrush(Color.FromArgb(255, 255, 255)); // new SolidBrush(e.CellStyle.BackColor);
                Pen gridLinePen = new Pen(new SolidBrush(Color.Gray));

                // Clear cell 
                g.FillRectangle(backColorBrush, e.CellBounds);

                //Bottom line drawing
                //g.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

                //top line drawing
                //if (e.RowIndex == 0 && (sender as DataGridView).ColumnHeadersVisible == false)
                //    g.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Top);

                //Drawing Right line
                if (IsEndHorMergeCell(e.RowIndex, e.ColumnIndex) == true)
                {
                    //If e.ColumnIndex = 3 Then
                    //g.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

                    //Inserting text
                    //string formatCode = GetTestFormatCode(sender as DataGridView);
                    //int mergeCount = GetMergeCount(formatCode);
                    int mergeCount = 2;
                    float mergeCellsWidth = mergeCount * (e.CellBounds.Right - e.CellBounds.Left);
                    float mergeCellLeft = e.CellBounds.Right - mergeCellsWidth;
                    float stringPos = mergeCellsWidth / 2;
                    Size textSize = TextRenderer.MeasureText(e.Value.ToString(), e.CellStyle.Font);
                    stringPos -= (textSize.Width / 2);
                    stringPos += mergeCellLeft;

                    g.DrawString(e.Value.ToString(), e.CellStyle.Font, new SolidBrush(e.CellStyle.ForeColor), stringPos, e.CellBounds.Y + 10);
                }

                e.Handled = true;
            }
            ////
        }

        private bool IsHorMergeCell(int row, int col)
        {
            if (row == -1 && col >= 2 && col <= 3)
                return true;
            else
                return false;
        }


        private bool IsEndHorMergeCell(int row, int col)
        {
            if (row == -1 && col == 3)
                return true;
            else
                return false;
        }


        private void InitSlideMenu()
        {
            // 
            mainTempPage.Controls.Clear();
            this.Controls.Add(slideMenuPanel);
            this.Controls.SetChildIndex(slideMenuPanel, 0);
            //
            slideMenuPanel.Top = 0; // slideMenuButton.Top;
            slideMenuPanel.Left = heSidePanel.Right;
            slideMenuPanel.Width = 0;
            slideMenuPanel.Left = heSidePanel.Width - slideMenuPanel.Width;

            _slideMenuStatus = eSlideMenuStatus.HIDING;
            _slideMenuTimer = new System.Timers.Timer();
            _slideMenuTimer.Interval = 1000 / FRMAE_PER_SEC; // 32프레임/sec
            //_sizePerFrame = slideMenuPanel.Width / (FRMAE_PER_SEC / 2); // 객체 이동
            _sizePerFrame = SLIDE_MENU_WIDTH / 2; // (FRMAE_PER_SEC / 2); // 객체 리사이즈
            _slideMenuTimer.Elapsed += new System.Timers.ElapsedEventHandler(slideMenuTimer_ElapsedEventHandler);
        }


        private void slideMenuButton_Click(object sender, EventArgs e)
        {
            ShowHideSlideMenu();
        }

        private void slideMenuButton_MouseEnter(object sender, EventArgs e)
        {
            ShowHideSlideMenu();
        }

        private void ShowHideSlideMenu(eSlideMenuStatus status = eSlideMenuStatus.TOGGLE)
        {
            if (status == eSlideMenuStatus.TOGGLE)
            {
                _slideMenuStatus = (_slideMenuStatus == eSlideMenuStatus.HIDING || _slideMenuStatus == eSlideMenuStatus.CLOSE) ? eSlideMenuStatus.SHOWING : eSlideMenuStatus.HIDING;
            }
            else
            {
                _slideMenuStatus = status;
            }


            if (_slideMenuStatus == eSlideMenuStatus.HIDING || _slideMenuStatus == eSlideMenuStatus.CLOSE)
                UnselectSideMenuItems(slideMenuPanel);

            if (_slideMenuStatus != eSlideMenuStatus.CLOSE)
            {
                if (_slideMenuTimer.Enabled == false)
                    _slideMenuTimer.Start();
            }
            else
            {
                if (_slideMenuTimer.Enabled == true)
                    _slideMenuTimer.Stop();

                slideMenuPanel.Width = 0;
            }

        }

        private void slideMenuTimer_ElapsedEventHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            BeginInvoke(new SlideMenuTimerDelegate(slideMenuTimer_DoWork));
        }

        private void slideMenuTimer_DoWork()
        {
            if (_slideMenuStatus == eSlideMenuStatus.SHOWING)
            {
                if (slideMenuPanel.Width + _sizePerFrame >= SLIDE_MENU_WIDTH)
                {
                    _slideMenuTimer.Stop();
                    slideMenuPanel.Width = SLIDE_MENU_WIDTH;
                }
                else
                {
                    slideMenuPanel.Width += _sizePerFrame;
                }
            }
            else
            {
                if (slideMenuPanel.Width - _sizePerFrame <= 0)
                {
                    _slideMenuTimer.Stop();
                    slideMenuPanel.Width = 0;
                }
                else
                {
                    slideMenuPanel.Width -= _sizePerFrame;
                }
            }

            /*
            if (mSlideMenuStatus == eSlideMenuStatus.SHOWING)
            {
                if(slideMenuPanel.Left + mSizePerFrame >= leftMenuPanel.Width)
                {
                    mSlideMenuTimer.Stop();
                    slideMenuPanel.Left = leftMenuPanel.Width; 
                }
                else
                {
                    slideMenuPanel.Left += mSizePerFrame;
                }
            }
            else
            {
                if (slideMenuPanel.Left - mSizePerFrame <= leftMenuPanel.Width - slideMenuPanel.Width)
                {
                    mSlideMenuTimer.Stop();
                    slideMenuPanel.Left = leftMenuPanel.Width - slideMenuPanel.Width;
                }
                else
                {
                    slideMenuPanel.Left -= mSizePerFrame;
                }
            }
            */
        }

        /**
         * Home Side Menu
         */
        #region Home Side Menu
        private void firstSideMenuHomeButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }

        private void firstSideMenuHomeButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(sideFirstMenuPanel, (sender as CustomControls.ButtonEx01));
            secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }
        #endregion

        /**
        *  RMT Side Menu
        */
        #region RMT Side Menu
        private void firstSideMenuRmtButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.realTime;
        }

        private void firstSideMenuRmtButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(sideFirstMenuPanel, (sender as CustomControls.ButtonEx01));
            secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.realTime;
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }

        private void secondSideMenuRmtTeam1Button_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            rtTeamNameLabel.Text = "유아1팀  ";
            rtSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.bc;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.realTime;
        }

        private void secondSideMenuRmtTeam2Button_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            rtTeamNameLabel.Text = "유아2팀  ";
            rtSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.realTime;
        }

        #endregion

        /**
         *  Sap Update Side Menu
         */
        #region Sap Update Side Menu
        private void firstSideMenuSapUpdateButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.sapUpdate;
        }

        private void firstSideMenuSapUpdateButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(sideFirstMenuPanel, (sender as CustomControls.ButtonEx01));
            secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.sapUpdate;
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }

        private void secondSideMenuSapUpdateButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            suTabControl.SelectedIndex = (int)ConstDefine.eSapUpdateTab.update;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.sapUpdate;
        }

        private void secondSideMenuSapUpdateButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(secondSideMenuSapUpdatePage, (sender as CustomControls.ButtonEx01));
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.sapUpdate;
        }

        private void secondSideMenuSapSettingButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            suTabControl.SelectedIndex = (int)ConstDefine.eSapUpdateTab.setting;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.sapUpdate;
        }

        private void secondSideMenuSapSettingButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(secondSideMenuSapUpdatePage, (sender as CustomControls.ButtonEx01));
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.sapUpdate;
        }
        private void thirdSideMenuSapTeam1Button_ButtonClick(object sender, EventArgs e)
        {
            if (secondSideMenuSapUpdateButton.Checked == true)
            {
                suUpdateTeamNameLabel.Text = "유아1팀  ";
                suDbUpdateButton_Click(suBcDbUpdateButton, null);
                suTabControl.SelectedIndex = (int)ConstDefine.eSapUpdateTab.update;
            }
            else
            {
                suSettingTeamNameLabel.Text = "유아1팀  ";
                suSettingSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.bc;
                suTabControl.SelectedIndex = (int)ConstDefine.eSapUpdateTab.setting;
            }

            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);

            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.sapUpdate;
        }

        private void thirdSideMenuSapTeam2Button_ButtonClick(object sender, EventArgs e)
        {

            if (secondSideMenuSapUpdateButton.Checked == true)
            {
                suUpdateTeamNameLabel.Text = "유아2팀  ";
                suDbUpdateButton_Click(suCcDbUpdateButton, null);
                suTabControl.SelectedIndex = (int)ConstDefine.eSapUpdateTab.update;
            }
            else
            {
                suSettingTeamNameLabel.Text = "유아2팀  ";
                suSettingSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
                suTabControl.SelectedIndex = (int)ConstDefine.eSapUpdateTab.setting;
            }

            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);

            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.sapUpdate;

        }
        #endregion

        /**
         * TJ Asset Side Menu
         */
        #region TJ Asset Side Menu
        private void firstSideMenuTjAssetButton_ButtonClick(object sender, EventArgs e)
        {
            ////
            // 메뉴 삭제와 슬라이드 메뉴 아이템 위치 변경으로 인한 임시적 처리
            // 요구사항이 계속 바뀌고 있어서 해당 소스를 삭제하지 않고 남겨둠
            if (secondSideMenuTjAssetPlanButton.Checked == false &&
                secondSideMenuTjAssetPredictionButton.Checked == false &&
                secondSideMenuTjAssetScheculeButton.Checked == false)
            {
                secondSideMenuTjAssetPlanButton.Checked = true;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionPlan;
            }

            //secondSideMenuTjAssetKpiButton.Checked = false;
            ////


            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void firstSideMenuTjAssetButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(sideFirstMenuPanel, (sender as CustomControls.ButtonEx01));
            secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }

        private void secondSideMenuTjAssetOeeButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.oee;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuTjAssetPlanButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionPlan;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuTjAssetScheculeButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionCalender;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuTjAssetPredictionButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionPrediction;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuTjAssetKpi_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.kpi;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuTjAssetCov_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.Cov;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuTjAssetOeeButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(secondSideMenuTjAssetTabPage, (sender as CustomControls.ButtonEx01));
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuTjAssetPlanButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(secondSideMenuTjAssetTabPage, (sender as CustomControls.ButtonEx01));
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuTjAssetScheculeButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(secondSideMenuTjAssetTabPage, (sender as CustomControls.ButtonEx01));
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }
        private void secondSideMenuTjAssetPredictionButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(secondSideMenuTjAssetTabPage, (sender as CustomControls.ButtonEx01));
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }
        private void secondSideMenuTjAssetKpiButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(secondSideMenuTjAssetTabPage, (sender as CustomControls.ButtonEx01));
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }
        private void secondSideMenuTjAssetCovButton_ButtonMouseEnter(object sender, EventArgs e)
        {

        }
        private void thirdSideMenuTjAsset1TeamButton_ButtonClick(object sender, EventArgs e)
        {
            if (secondSideMenuTjAssetOeeButton.Checked == true)
            {
                tjOeeTeamNameLabel.Text = "유아1팀  ";
                tjOeeSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.bc;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.oee;
            }
            else if (secondSideMenuTjAssetPlanButton.Checked == true)
            {
                tjPlanTeamNameLabel.Text = "유아1팀  ";
                taBcProductionPlanButton_Click(taBcProductionPlanButton, null);
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionPlan;
            }
            else if (secondSideMenuTjAssetScheculeButton.Checked == true)
            {
                tjScheduleTeamNameLabel.Text = "유아1팀  ";
                taProductionCalendarButton_Click(taBcProductionCalendarButton, null);
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionCalender;
            }
            else if (secondSideMenuTjAssetPredictionButton.Checked == true)
            {
                tjPredictionTeamNameLabel.Text = "유아1팀  ";
                tjPredictionSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.bc;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionPrediction;
            }
            else if (secondSideMenuTjAssetKpiButton.Checked == true)
            {
                tjKpiTeamNameLabel.Text = "유아1팀  ";
                tjKpiSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.bc;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.kpi;
            }
            else if (secondSideMenuTjAssetCovButton.Checked == true)
            {
                tjCovTeamNameLabel.Text = "유아1팀  ";
                tjCovSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.bc;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.Cov;
                _tjAssetPage.changeLoadCovList();
            }

            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);

            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }
        private void thirdSideMenuTjAsset2TeamButton_ButtonClick(object sender, EventArgs e)
        {
            if (secondSideMenuTjAssetOeeButton.Checked == true)
            {
                tjOeeTeamNameLabel.Text = "유아2팀  ";
                tjOeeSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.oee;
            }
            else if (secondSideMenuTjAssetPlanButton.Checked == true)
            {
                tjPlanTeamNameLabel.Text = "유아2팀  ";
                taCcProductionPlanButton_Click(taCcProductionPlanButton, null);
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionPlan;
            }
            else if (secondSideMenuTjAssetScheculeButton.Checked == true)
            {
                tjScheduleTeamNameLabel.Text = "유아2팀  ";
                taProductionCalendarButton_Click(taCcProductionCalendarButton, null);
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionCalender;
            }
            else if (secondSideMenuTjAssetPredictionButton.Checked == true)
            {
                tjPredictionTeamNameLabel.Text = "유아2팀  ";
                tjPredictionSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.productionPrediction;
            }
            else if (secondSideMenuTjAssetKpiButton.Checked == true)
            {
                tjKpiTeamNameLabel.Text = "유아2팀  ";
                tjKpiSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.kpi;
            }
            else if (secondSideMenuTjAssetCovButton.Checked == true)
            {
                tjCovTeamNameLabel.Text = "유아2팀  ";
                tjCovSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
                taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.Cov;
                _tjAssetPage.changeLoadCovList();
            }

            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);

            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }
        #endregion

        /**
         * Library Side Menu
         */
        #region Library Side Menu
        private void firstSideMenuLibraryButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library;

            secondSideMenuLibraryNoticeButton_ButtonClick(null, null);
        }

        private void firstSideMenuLibraryButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(sideFirstMenuPanel, (sender as CustomControls.ButtonEx01));
            secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library + 1;
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }
        private void secondSideMenuLibraryCommonButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            lyMenuNameLabel.Text = "공용  ";
            lyTabControl.SelectedIndex = (int)ConstDefine.eLibraryTab.commonLibrary;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library;
        }
        private void secondSideMenuLibraryPersonalButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            lyMenuNameLabel.Text = "개인  ";
            lyTabControl.SelectedIndex = (int)ConstDefine.eLibraryTab.personalLibrary;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library;
        }
        private void secondSideMenuLibraryNoticeButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            lyMenuNameLabel.Text = "공지사항  ";
            lyTabControl.SelectedIndex = (int)ConstDefine.eLibraryTab.noticeBoard;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library;
        }
        private void secondSideMenuLibraryFreeBoradButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            lyMenuNameLabel.Text = "게시판  ";
            lyTabControl.SelectedIndex = (int)ConstDefine.eLibraryTab.freeBoard;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library;
        }
        private void secondSideMenuLibraryAddressButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            lyMenuNameLabel.Text = "주소록  ";
            lyTabControl.SelectedIndex = (int)ConstDefine.eLibraryTab.addressBook;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.library;
        }
        #endregion

        /**
         * Admin Side Menu
         */
        #region Admin Side Menu
        private void firstSideMenuAdminButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.admin;
        }
        private void firstSideMenuAdminButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(sideFirstMenuPanel, (sender as CustomControls.ButtonEx01));
            secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.admin + 1;
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }
        private void secondSideMenuAdminUserListButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            anMenuNameLabel.Text = "User List  ";
            anTabControl.SelectedIndex = (int)ConstDefine.eAdminTab.userList;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.admin;
        }
        private void secondSideMenuAdminSecurityButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            anMenuNameLabel.Text = "Security  ";
            anTabControl.SelectedIndex = (int)ConstDefine.eAdminTab.security;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.admin;
        }
        #endregion

        /**
         * Cov Side Menu
         */
        #region Cov Side Menu
        private void firstSideMenuCOVButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(sideFirstMenuPanel, (sender as CustomControls.ButtonEx01));
            secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset + 4;
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }

        private void firstSideMenuCovButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.Cov;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void secondSideMenuCovTeam1Button_ButtonClick(object sender, EventArgs e)
        {
            secondSideMenuTjAssetCovButton.Checked = true;
            thirdSideMenuTjAsset1TeamButton_ButtonClick(null, null);
        }

        private void secondSideMenuCovTeam2Button_ButtonClick(object sender, EventArgs e)
        {
            secondSideMenuTjAssetCovButton.Checked = true;
            thirdSideMenuTjAsset2TeamButton_ButtonClick(null, null);
        }

        private void taTjCovCheckBox_Click(object sender, EventArgs e)
        {
            _tjAssetPage.ResetCovButtons((sender as CustomControls.ButtonEx01).Name);
            _tjAssetPage.changeLoadCovList();
        }

        private void taCovNextButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage._IndexCovGroupGrid += 1;
            _tjAssetPage.LoadCovList();
        }

        private void taCovPreButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage._IndexCovGroupGrid -= 1;

            if (_tjAssetPage._IndexCovGroupGrid < 1)
            {
                _tjAssetPage._IndexCovGroupGrid += 1;
                return;
            }

            _tjAssetPage.LoadCovList();
        }

        private void taCovSearchButton_Click(object sender, EventArgs e)
        {
            _tjAssetPage.LoadCovListTotla();
            _tjAssetPage.LoadCovList();
        }

        private void rtCovDayRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rtCovEndDateTimePicker.Enabled = !rtCovDayRadioButton.Checked;
        }

        private void rtCovPeriodRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rtCovEndDateTimePicker.Enabled = rtCovPeriodRadioButton.Checked;
        }

        private void rtCovStartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            _tjAssetPage.ResetCovEndDateTime();
        }

        #endregion

        private void UnselectSideMenuItems(Control root)
        {
            var controlList = GetControlHierarchy(root).ToList();

            Type button = typeof(CustomControls.ButtonEx01);

            foreach (var control in controlList)
            {
                Type childControl = control.GetType();
                if (childControl.Equals(button) == true)
                    (control as CustomControls.ButtonEx01).Checked = false;
            }

            if (root.Name == "slideMenuPanel")
            {
                secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
                thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
            }
        }

        private void UnselectButtonItems(Control root)
        {
            var controlList = GetControlHierarchy(root).ToList();

            Type button = typeof(CustomControls.ButtonEx01);

            foreach (var control in controlList)
            {
                Type childControl = control.GetType();
                if (childControl.Equals(button) == true)
                    (control as CustomControls.ButtonEx01).Checked = false;
            }
        }
        private IEnumerable<Control> GetControlHierarchy(Control root)
        {
            var queue = new Queue<Control>();

            queue.Enqueue(root);

            do
            {
                var control = queue.Dequeue();

                yield return control;

                foreach (var child in control.Controls.OfType<Control>())
                    queue.Enqueue(child);

            } while (queue.Count > 0);

        }

        private void ResetSideMenuItemSelectionStatus(Control root, CustomControls.ButtonEx01 targetButton)
        {
            UnselectSideMenuItems(root);
            targetButton.Checked = true;
        }

        private void ResetButtonItemSelectionStatus(Control root, CustomControls.ButtonEx01 targetButton)
        {
            UnselectButtonItems(root);
            targetButton.Checked = true;
        }

        private void rtTj01Button_ButtonClick(object sender, EventArgs e)
        {
            // Load Data
            //this._realTimePage.LoadProcMnemFilter((int)ConstDefine.eTjType.tj01);
            //this._realTimePage.LoadEvtNameFilter((int)ConstDefine.eTjType.tj01);
            ResetButtonItemSelectionStatus(rtSideMenu1TeamPage, rtTj01Button);
        }

        private void rtTj02Button_ButtonClick(object sender, EventArgs e)
        {
            // Load Data
            //this._realTimePage.LoadProcMnemFilter((int)ConstDefine.eTjType.tj01);
            //this._realTimePage.LoadEvtNameFilter((int)ConstDefine.eTjType.tj01);
            ResetButtonItemSelectionStatus(rtSideMenu1TeamPage, rtTj02Button);
        }

        private void rtTj03Button_ButtonClick(object sender, EventArgs e)
        {
            // Load Data
            //this._realTimePage.LoadProcMnemFilter((int)ConstDefine.eTjType.tj01);
            //this._realTimePage.LoadEvtNameFilter((int)ConstDefine.eTjType.tj01);
            ResetButtonItemSelectionStatus(rtSideMenu1TeamPage, rtTj03Button);
        }

        private void rtTj04Button_ButtonClick(object sender, EventArgs e)
        {
            // Load Data
            //this._realTimePage.LoadProcMnemFilter((int)ConstDefine.eTjType.tj01);
            //this._realTimePage.LoadEvtNameFilter((int)ConstDefine.eTjType.tj01);
            ResetButtonItemSelectionStatus(rtSideMenu1TeamPage, rtTj04Button);
        }

        private void rtTj05Button_ButtonClick(object sender, EventArgs e)
        {
            // Load Data
            //this._realTimePage.LoadProcMnemFilter((int)ConstDefine.eTjType.tj01);
            //this._realTimePage.LoadEvtNameFilter((int)ConstDefine.eTjType.tj01);
            ResetButtonItemSelectionStatus(rtSideMenu1TeamPage, rtTj05Button);
        }

        private void rtTjBcButton_ButtonClick(object sender, EventArgs e)
        {
            ResetButtonItemSelectionStatus(rtSideMenu1TeamPage, rtTjBcButton);
        }

        private void rtTj21Button_ButtonClick(object sender, EventArgs e)
        {
            // Load Data
            //this._realTimePage.LoadProcMnemFilter((int)ConstDefine.eTjType.tj01);
            //this._realTimePage.LoadEvtNameFilter((int)ConstDefine.eTjType.tj01);
            ResetButtonItemSelectionStatus(rtSideMenu2TeamPage, rtTj21Button);
        }

        private void rtTj22Button_ButtonClick(object sender, EventArgs e)
        {
            // Load Data
            //this._realTimePage.LoadProcMnemFilter((int)ConstDefine.eTjType.tj01);
            //this._realTimePage.LoadEvtNameFilter((int)ConstDefine.eTjType.tj01);
            ResetButtonItemSelectionStatus(rtSideMenu2TeamPage, rtTj22Button);
        }

        private void rtTj23Button_ButtonClick(object sender, EventArgs e)
        {
            // Load Data
            //this._realTimePage.LoadProcMnemFilter((int)ConstDefine.eTjType.tj01);
            //this._realTimePage.LoadEvtNameFilter((int)ConstDefine.eTjType.tj01);
            ResetButtonItemSelectionStatus(rtSideMenu2TeamPage, rtTj23Button);
        }

        private void rtTjCcButton_ButtonClick(object sender, EventArgs e)
        {
            ResetButtonItemSelectionStatus(rtSideMenu2TeamPage, rtTjCcButton);
        }

        private void tjPredictionSideTeamTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            _tjAssetPage.LoadProductionPrediction();
        }

        private void homeMachineStatusTimer_Tick(object sender, EventArgs e)
        {
            _homePage.LoadTotalMachineStatus();
        }

        private void loginUserPanel_Click(object sender, EventArgs e)
        {
            _inLoginPanelArea = false;
            loginUserLabel.ForeColor = Color.White;
            loginUserPictureBox.BackgroundImage = Properties.Resources.main_login_normal_;


            const int CONTEXT_MENU_WIDTH = 160;
            EventHandler menuItemClickHandler = new EventHandler(loginUserPanel_ItemClick);
            MenuItem[] menuItems = { new MenuItem("개인정보변경", menuItemClickHandler), new MenuItem("로그아웃", menuItemClickHandler) };
            ContextMenu contextMenu = new ContextMenu(menuItems);
            contextMenu.Show(loginUserPanel, new Point(loginUserPictureBox.Right - CONTEXT_MENU_WIDTH, loginUserPictureBox.Bottom + 10));
        }

        void loginUserPanel_ItemClick(object sender, EventArgs e)
        {
            string menuText = (sender as MenuItem).Text;
            if (menuText == "개인정보변경")
            {
                _homePage.OpenUserForm(_userId);
            }
            else if (menuText == "로그아웃")
            {
                // Login User Info
                _userId = "";
                _userName = "";
                _securityCode = -1;
                _securityValue = "";
                _viewRealTime = "0";
                _viewSapUpdate = "0";
                _viewTjAsset = "0";
                _viewLibrary = "0";
                _viewAdmin = "0";

                SetCurretUserInfo();
                if (mainTabControl.SelectedIndex != (int)ConstDefine.eMainTab.home)
                {
                    mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
                }

                if (true == Login())
                {
                    SetCurretUserInfo();
                    EnableViewByUserSecurity();
                    _homePage.LoadHeData();
                }
                else
                {
                    Close();
                }
            }
        }

        private void loginUserPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (Utils.InArea(loginUserLabel, loginUserPanel) == true || Utils.InArea(loginUserPictureBox, loginUserPanel) == true)
            {
                if (_inLoginPanelArea == false)
                {
                    _inLoginPanelArea = true;
                    loginUserLabel.ForeColor = Color.FromArgb(42, 158, 186);
                    loginUserPictureBox.BackgroundImage = Properties.Resources.main_login_down_;
                }
            }
            else
            {
                if (_inLoginPanelArea == true)
                {
                    _inLoginPanelArea = false;
                    loginUserLabel.ForeColor = Color.White;
                    loginUserPictureBox.BackgroundImage = Properties.Resources.main_login_normal_;
                }
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_slideMenuStatus != eSlideMenuStatus.SHOWING)
                return;

            if (Utils.InArea(slideMenuPanel, this) != false)
                ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
        }

        void gmh_TheMouseMoved()
        {
            if (_slideMenuStatus != eSlideMenuStatus.SHOWING || slideMenuPanel.Width != SLIDE_MENU_WIDTH)
                return;

            if (Utils.InArea(slideMenuPanel, this) == false && Utils.InArea(slideMenuButton, slideMenuTopPanel) == false)
                ShowHideSlideMenu(eSlideMenuStatus.CLOSE);

        }

        private void kpiStatusTimer_Tick(object sender, EventArgs e)
        {
            watingProgressBar.Value = 0;
            _tjAssetPage.LoadKpi();
        }

        private void kpiWaitingTimer_Tick(object sender, EventArgs e)
        {
            _tjAssetPage.UpdateWaitingProgressBar();
        }

        private void firstSideMenuKpiButton_ButtonClick(object sender, EventArgs e)
        {
            ShowHideSlideMenu(eSlideMenuStatus.CLOSE);
            //mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;


            taTabControl.SelectedIndex = (int)ConstDefine.eTjAssetTab.kpi;
            mainTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset;
        }

        private void firstSideMenuKpiButton_ButtonMouseEnter(object sender, EventArgs e)
        {
            ResetSideMenuItemSelectionStatus(sideFirstMenuPanel, (sender as CustomControls.ButtonEx01));
            secondSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.tjAsset + 1;
            thirdSideMenuTabControl.SelectedIndex = (int)ConstDefine.eMainTab.home;
        }

        private void secondSideMenuKpiTeam1Button_ButtonClick(object sender, EventArgs e)
        {
            secondSideMenuTjAssetKpiButton.Checked = true;
            thirdSideMenuTjAsset1TeamButton_ButtonClick(null, null);
        }


        private void secondSideMenuKpiTeam2Button_ButtonClick(object sender, EventArgs e)
        {
            secondSideMenuTjAssetKpiButton.Checked = true;
            thirdSideMenuTjAsset2TeamButton_ButtonClick(null, null);
        }

    }



    public delegate void MouseMovedEvent();

    public class GlobalMouseHandler : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;

        public event MouseMovedEvent TheMouseMoved;

        #region IMessageFilter Members

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSEMOVE)
            {
                if (TheMouseMoved != null)
                {
                    TheMouseMoved();
                }
            }
            // Always allow message to continue to the next filter control
            return false;
        }

        #endregion
    }
}