using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealTimeRMT
{
    public partial class LoginForm : Form
    //public partial class LoginForm : C1.Win.C1Ribbon.C1RibbonForm
    {
        // 시연 테스트
        public string _ip = "";
        public string _id = "";
        public string _pw = "";
        public string _dbPassword = ""; 


        private MainForm _parent = null;
        public LoginForm(MainForm parent)
        {
            InitializeComponent();
            _parent = parent;

            InitControls();
        }

        private void InitControls()
        {
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (CheckRequiredFields() == false)
                return;

            if (GetLoginUserInfo() == false)
                return;

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
        }

        private void idTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Utils.IsReturnKey(e) == true)
                loginButton_Click(null, null);
        }

        private void pwTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Utils.IsReturnKey(e) == true)
                loginButton_Click(null, null);
        }

        private bool GetLoginUserInfo()
        {
            string id = idTextBox.Text.Trim();
            string pw = pwTextBox.Text.Trim();

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectLoginUserInfo", id));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("아이디를 찾을 수 없습니다", ConstDefine.loginTitle);
                idTextBox.Focus();
                idTextBox.Text = "";
                return false;
            }

            if (pw != dataSet.Tables[0].Rows[0]["Password"].ToString())
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다", ConstDefine.loginTitle);
                pwTextBox.Focus();
                pwTextBox.Text = "";
                return false;
            }

            _parent._userId = id;
            _parent._userName = dataSet.Tables[0].Rows[0]["UserName"].ToString();
            _parent._securityCode = Convert.ToInt32(dataSet.Tables[0].Rows[0]["SecurityCode"].ToString());
            _parent._securityValue = dataSet.Tables[0].Rows[0]["securityValue"].ToString();



            DataSet dataSet2 = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectSecurityList", _parent._securityCode));
            if (dataSet2 == null || dataSet2.Tables.Count == 0 || dataSet2.Tables[0].Rows.Count == 0)
                return false;

            _parent._viewRealTime = dataSet2.Tables[0].Rows[0]["ViewRealTime"].ToString();
            _parent._viewSapUpdate = dataSet2.Tables[0].Rows[0]["ViewSapUpdate"].ToString();
            _parent._viewTjAsset = dataSet2.Tables[0].Rows[0]["ViewTjAsset"].ToString();
            _parent._viewLibrary = dataSet2.Tables[0].Rows[0]["ViewLibrary"].ToString();
            _parent._viewAdmin = dataSet2.Tables[0].Rows[0]["ViewAdmin"].ToString();

            return true;
        }

        private bool CheckRequiredFields()
        {
            if (idTextBox.Text.Trim() == "")
            {
                MessageBox.Show("아이디를 입력해 주십시오", ConstDefine.loginTitle);
                idTextBox.Focus();
                return false;
            }

            if (pwTextBox.Text.Trim() == "")
            {
                MessageBox.Show("비밀번호를 입력해 주십시오", ConstDefine.loginTitle);
                pwTextBox.Focus();
                return false;
            }

            return true;
        }

        private void settingTestAccount_Click(object sender, EventArgs e)
        {
            /*
            SettingTestAccount dlg = new SettingTestAccount(this);
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                DbHelper._ip = _ip;
                DbHelper._id = _id;
                DbHelper._pw = _pw;
                DbHelper.SetConnectionString();
            }
            */ 
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (false == GetConfig())
            {
                MessageBox.Show("환경정보를 읽어올 수 없습니다. 관리자에게 문의해 주십시오.");
                Close();
                return;
            }
            DbHelper.SetConnectionString(_dbPassword);
        }

        private bool GetConfig()
        {
            try
            {
                //string currentPath = System.AppDomain.CurrentDomain.BaseDirectory;
                string currentPath = Application.StartupPath;
                //currentPath = "C:\\HGFA\\ChangeManagement";
                string iniFilePath = currentPath + "\\Config\\Config.ini";

                _dbPassword = Utils.GetIniValue("DBMS", "PASSWORD", iniFilePath);

                if (_dbPassword.Equals(""))
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}
