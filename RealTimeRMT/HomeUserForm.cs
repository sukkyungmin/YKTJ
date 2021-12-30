using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RealTimeRMT
{
    public partial class HomeUserForm : Form
    {
        private HomePage _homePage = null;
        private string _userId = "";
        private string _securityCode = "";
        private string _messageTitle = "개인정보변경"; 

        public HomeUserForm(HomePage homePage, string userId)
        {
            InitializeComponent();

            _homePage = homePage;
            _userId = userId; 

            InitControls(); 
        }

        public void InitControls()
        {
            LoadPositionValueList();
            LoadTeamTypeValueList();
            LoadTjTypeValueList();

            LoadUserInfo();
        }

        public void LoadPositionValueList()
        {
            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectPositionList");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            positionComboBox.Items.Clear();
            positionComboBox.BeginUpdate();

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                positionComboBox.Items.Add(new ComboBoxItem(dataRow["PositionValue"].ToString(), dataRow["PositionCode"].ToString()));
            }

            positionComboBox.EndUpdate();
        }

        public void LoadTeamTypeValueList()
        {
            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectTeamTypeList");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            teamTypeComboBox.Items.Clear();
            teamTypeComboBox.BeginUpdate();

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                teamTypeComboBox.Items.Add(new ComboBoxItem(dataRow["TeamTypeValue"].ToString(), dataRow["TeamTypeCode"].ToString()));
            }

            teamTypeComboBox.EndUpdate();
        }

        public void LoadTjTypeValueList()
        {
            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectTjTypeList");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            tjTypeComboBox.Items.Clear();
            tjTypeComboBox.BeginUpdate();

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                tjTypeComboBox.Items.Add(new ComboBoxItem(dataRow["TjTypeValue"].ToString(), dataRow["TjTypeCode"].ToString()));
            }

            tjTypeComboBox.EndUpdate();
        }

        private void changeProfilePictureButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (profilePicture.Image != null)
                {
                    profilePicture.Image.Dispose();
                    profilePicture.Image = null; 
                }
                profilePicture.Image = Image.FromFile(openFileDialog.FileName);
                profilePicture.Refresh(); 
            }
        }

        private void deleteProfilePictureButton_Click(object sender, EventArgs e)
        {
            if(profilePicture.Image != null) 
            {
                profilePicture.Image.Dispose();
                profilePicture.Image = null;
                profilePicture.Refresh(); 
            }

            profilePicture.Image = RealTimeRMT.Properties.Resources.no_person; 
            profilePicture.Refresh(); 
        }

        private void saveUserButton_Click(object sender, EventArgs e)
        {
            if (true == SaveUser())
            {
                DialogResult = DialogResult.OK;
                Close(); 
            }
        }

        private void cancelUserButton_Click(object sender, EventArgs e)
        {
            CancelUser(); 
            DialogResult = DialogResult.Cancel;
            Close(); 
        }

        public bool SaveUser()
        {
            if (CheckRequiredField() == false)
            {
                MessageBox.Show("필수 항목을 모두 입력해 주십시오.", _messageTitle);
                return false;
            }

            if (IsMatchedPassword() == false)
            {
                MessageBox.Show("패스워드가 잘못 입력되었습니다.", _messageTitle);
                return false;
            }

            string userName = userNameTextBox.Text.Trim();
            string userId = userIdTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();
            string passwordRetry = passwordRetryTextBox.Text.Trim();
            string phoneNumber = phoneNumberTextBox.Text.Trim();
            string mobileNumber = mobileNumberTextBox.Text.Trim();
            string email = emailTextBox.Text.Trim();
            string positionCode = (positionComboBox.SelectedItem as ComboBoxItem).Value.ToString();
            string positionValue = (positionComboBox.SelectedItem as ComboBoxItem).Text;
            string teamTypeCode = (teamTypeComboBox.SelectedItem as ComboBoxItem).Value.ToString();
            string teamTypeValue = (teamTypeComboBox.SelectedItem as ComboBoxItem).Text;
            string tjTypeCode = (tjTypeComboBox.SelectedItem as ComboBoxItem).Value.ToString();
            string tjTypeValue = (tjTypeComboBox.SelectedItem as ComboBoxItem).Text;

            SqlParameter imageParam = new SqlParameter();
            imageParam.SqlDbType = SqlDbType.Image;
            imageParam.ParameterName = "ProfilePicture";
            imageParam.Value = (profilePicture.Image == null) ? null : Utils.ImageToByteArray(profilePicture.Image); 

            int retVal = 0;

            if (imageParam.Value != null)
                retVal = DbHelper.ExecuteNonQueryWithFileData(string.Format("{0} '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', {8}, {9}, {10}, @ProfilePicture",
                    "EXEC UpdateUser", userId, Utils.ReplaceSpecialChar(userName), password, _securityCode, phoneNumber, mobileNumber, email, positionCode, teamTypeCode, tjTypeCode),
                    imageParam); 
            else
                retVal = DbHelper.ExecuteNonQuery(string.Format("{0} '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', {8}, {9}, {10}, {11}",
                    "EXEC UpdateUser", userId, Utils.ReplaceSpecialChar(userName), password, _securityCode, phoneNumber, mobileNumber, email, positionCode, teamTypeCode, tjTypeCode, "NULL")); 

            if (retVal < 1)
            {
                MessageBox.Show("개인정보를 변경할 수 없습니다. 관리자에게 문의해 주십시오.", "개인정보변경");
                return false;
            }
            else
            {
                _homePage.SetCurretUserInfo(userName); 
            }

            InitUserFields();

            return true; 
        }


        public bool CheckRequiredField()
        {
            if (userNameTextBox.Text.Trim() == "")
            {
                userNameTextBox.Focus();
                return false;
            }
            else if (userIdTextBox.Text.Trim() == "")
            {
                userIdTextBox.Focus();
                return false;
            }
            else if (passwordTextBox.Text.Trim() == "")
            {
                passwordTextBox.Focus();
                return false;
            }
            else if (passwordRetryTextBox.Text.Trim() == "")
            {
                passwordRetryTextBox.Focus();
                return false;
            }
            else if (_securityCode == "")
            {
                return false;
            }
            else if (positionComboBox.SelectedIndex == -1)
            {
                positionComboBox.Focus();
                return false;
            }
            else if (teamTypeComboBox.SelectedIndex == -1)
            {
                teamTypeComboBox.Focus();
                return false;
            }
            else if (tjTypeComboBox.SelectedIndex == -1)
            {
                tjTypeComboBox.Focus();
                return false;
            }
            return true;
        }

        public bool IsMatchedPassword()
        {
            if (passwordTextBox.Text.Trim() == passwordRetryTextBox.Text.Trim())
            {
                return true;
            }
            else
            {
                passwordTextBox.Focus();
                return false;
            }
        }

        public void InitUserFields()
        {
            if (profilePicture.Image != null)
            {
                profilePicture.Image.Dispose();
                profilePicture.Image = null;
                profilePicture.Refresh(); 
            }

            userNameTextBox.Text = ""; 
            userIdTextBox.Text = ""; 
            passwordTextBox.Text = "";;
            passwordRetryTextBox.Text = "";
            securityTextBox.Text = ""; 
            phoneNumberTextBox.Text = "";
            mobileNumberTextBox.Text = ""; 
            emailTextBox.Text = ""; 
            positionComboBox.Text = ""; 
            positionComboBox.SelectedIndex = -1; 
            teamTypeComboBox.Text = ""; 
            teamTypeComboBox.SelectedIndex = -1;
            tjTypeComboBox.Text = "";
            tjTypeComboBox.SelectedIndex = -1;
            //profilePicturePictureBox.Image.Dispose();
        }

        public void LoadUserInfo()
        {
            InitUserFields();

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} '{1}', '', '', '', '', ''", "EXEC SelectUserList",  _userId));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            profilePicture.Image = Utils.ByteArrayToImage((Byte[])dataSet.Tables[0].Rows[0]["ProfilePicture"]); 
            userNameTextBox.Text = dataSet.Tables[0].Rows[0]["UserName"].ToString(); 
            userIdTextBox.Text = dataSet.Tables[0].Rows[0]["UserId"].ToString(); 
            passwordTextBox.Text = dataSet.Tables[0].Rows[0]["Password"].ToString(); 
            passwordRetryTextBox.Text = dataSet.Tables[0].Rows[0]["Password"].ToString(); 
            securityTextBox.Text = dataSet.Tables[0].Rows[0]["SecurityValue"].ToString(); 
            phoneNumberTextBox.Text = dataSet.Tables[0].Rows[0]["PhoneNumber"].ToString(); 
            mobileNumberTextBox.Text = dataSet.Tables[0].Rows[0]["MobileNumber"].ToString(); 
            emailTextBox.Text = dataSet.Tables[0].Rows[0]["Email"].ToString(); 
            positionComboBox.Text = dataSet.Tables[0].Rows[0]["PositionValue"].ToString(); 
            teamTypeComboBox.Text = dataSet.Tables[0].Rows[0]["TeamTypeValue"].ToString(); 
            tjTypeComboBox.Text = dataSet.Tables[0].Rows[0]["TjTypeValue"].ToString(); 
            _securityCode = dataSet.Tables[0].Rows[0]["SecurityCode"].ToString();  
        }

        public void CancelUser()
        {
        }
    }
}
