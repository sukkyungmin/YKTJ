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
    public partial class UserForm : Form
    {
        private bool _newUser = false;
        private AdminPage _adminPage = null;
        private DataGridView _userGridView = null;
        private int _rowIndex = -1; 


        public UserForm(bool newUser, AdminPage adminPage, DataGridView userGridView, int rowIndex)
        {
            InitializeComponent();

            _newUser = newUser;
            _adminPage = adminPage;
            _userGridView = userGridView;
            _rowIndex = rowIndex; 

            InitControls(); 
        }

        public void InitControls()
        {
            userIdTextBox.ReadOnly = !_newUser;

            LoadSecurityValueList();
            LoadPositionValueList();
            LoadTeamTypeValueList();
            LoadTjTypeValueList();

            if (_newUser == false)
                SetUserFields();

            if (_newUser == true)
            {
                deleteUserButton.Enabled = false;
                deleteUserButton.BackColor = Color.DarkGray;

                userIdTextBox.BackColor = Color.White;
            }
            
        }

        public void LoadSecurityValueList()
        {
            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectSecurityList -1");
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            securityComboBox.Items.Clear();
            securityComboBox.BeginUpdate();

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                securityComboBox.Items.Add(new ComboBoxItem(dataRow["SecurityValue"].ToString(), dataRow["SecurityCode"].ToString()));
            }

            securityComboBox.EndUpdate();
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

        private void deleteUserButton_Click(object sender, EventArgs e)
        {
            if (true == DeleteUser())
            {
                DialogResult = DialogResult.OK;
                Close(); 
            }
        }


        public bool SaveUser()
        {
            if (CheckRequiredField() == false)
            {
                MessageBox.Show("필수 항목을 모두 입력해 주십시오.", ConstDefine.adminTitle);
                return false;
            }

            if (IsExistedId() == true)
            {
                MessageBox.Show("기존에 존재하는 ID입니다. 다른 ID를 입력해 주십시오.", ConstDefine.adminTitle);
                return false; 
            }

            if (IsMatchedPassword() == false)
            {
                MessageBox.Show("패스워드가 잘못 입력되었습니다.", ConstDefine.adminTitle);
                return false;
            }

            string userName = userNameTextBox.Text.Trim();
            string userId = userIdTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();
            string passwordRetry = passwordRetryTextBox.Text.Trim();
            string securityCode = (securityComboBox.SelectedItem as ComboBoxItem).Value.ToString();
            string securityValue = (securityComboBox.SelectedItem as ComboBoxItem).Text;
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
            if (_newUser == true)
            {
                if (imageParam.Value != null)
                    retVal = DbHelper.ExecuteNonQueryWithFileData(string.Format("{0} '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', {8}, {9}, {10}, @ProfilePicture",
                        "EXEC InsertUser", userId, Utils.ReplaceSpecialChar(userName), password, securityCode, phoneNumber, mobileNumber, email, positionCode, teamTypeCode, tjTypeCode),
                        imageParam);
                else
                    retVal = DbHelper.ExecuteNonQuery(string.Format("{0} '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', {8}, {9}, {10}, {11}",
                        "EXEC InsertUser", userId, Utils.ReplaceSpecialChar(userName), password, securityCode, phoneNumber, mobileNumber, email, positionCode, teamTypeCode, tjTypeCode, "NULL"));
            }
            else
            {
                if (IsLastAdmin("MODIFY") == true)
                {
                    MessageBox.Show("마지막 Admin 계정의 권한은 수정할 수 없습니다.", ConstDefine.adminTitle);
                    return false;
                }

                if (imageParam.Value != null)
                    retVal = DbHelper.ExecuteNonQueryWithFileData(string.Format("{0} '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', {8}, {9}, {10}, @ProfilePicture",
                        "EXEC UpdateUser", userId, Utils.ReplaceSpecialChar(userName), password, securityCode, phoneNumber, mobileNumber, email, positionCode, teamTypeCode, tjTypeCode),
                        imageParam); 
                else
                    retVal = DbHelper.ExecuteNonQuery(string.Format("{0} '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', {8}, {9}, {10}, {11}",
                        "EXEC UpdateUser", userId, Utils.ReplaceSpecialChar(userName), password, securityCode, phoneNumber, mobileNumber, email, positionCode, teamTypeCode, tjTypeCode, "NULL")); 
            }

            if (retVal < 1)
            {
                MessageBox.Show("사용자 정보를 저장할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.adminTitle);
                return false;
            }
            else
            {
                /*
                if (_newUser == true)
                {
                    _adminPage.InsertUserListViewItem((profilePicture.Image.Clone() as Image), userName, userId, password, securityValue, phoneNumber, mobileNumber, email, positionValue, teamTypeValue, tjTypeValue,
                        securityCode, positionCode, teamTypeCode, tjTypeCode);
                    _adminPage.ResetStatusBar();
                }
                else
                {
                    _adminPage.UpdateUserListViewItem(_rowIndex, (profilePicture.Image.Clone() as Image), userName, password, securityValue, phoneNumber, mobileNumber, email, positionValue, teamTypeValue, tjTypeValue,
                        securityCode, positionCode, teamTypeCode, tjTypeCode);
                }
                */

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
            else if (securityComboBox.SelectedIndex == -1)
            {
                securityComboBox.Focus();
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


        public bool IsExistedId()
        {
            int count = _userGridView.Rows.Count;

            for (int i = 0; i < count; i++)
            {
                if (_rowIndex == i)
                    continue;

                if (_userGridView.Rows[i].Cells[(int)ConstDefine.eUserListView.userId].Value.ToString() == userIdTextBox.Text.Trim())
                    return true;
            }

            return false;
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
            securityComboBox.Text = ""; 
            securityComboBox.SelectedIndex = -1; 
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

        public void SetUserFields()
        {
            if (_rowIndex == -1)
                return; 

            if (profilePicture.Image != null)
            {
                profilePicture.Image.Dispose();
                profilePicture.Image = null;
            }
            profilePicture.Image = (_userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.profilePicture].Value as Image);
            userNameTextBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.userName].Value.ToString();
            userIdTextBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.userId].Value.ToString();
            passwordTextBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.planePassword].Value.ToString();
            passwordRetryTextBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.planePassword].Value.ToString();
            securityComboBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.securityValue].Value.ToString();
            phoneNumberTextBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.phoneNumber].Value.ToString();
            mobileNumberTextBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.mobileNumber].Value.ToString();
            emailTextBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.email].Value.ToString();
            positionComboBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.positionValue].Value.ToString();
            teamTypeComboBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.teamTypeValue].Value.ToString();
            tjTypeComboBox.Text = _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.tjTypeValue].Value.ToString(); 
        }

        public bool IsLastAdmin(string command)
        {
            if (_rowIndex == -1)
                return false;

            // 수정/삭제하려는 대상이 admin이 아니면 리턴
            if (_userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.securityCode].Value.ToString() != ConstDefine.securityAdmin.ToString())
                return false;

            // 수정인 경우, admin 권한을 유지 또는 admin으로 수정하는 경우 리턴
            if (command == "MODIFY")
            {
                if ((securityComboBox.SelectedItem as ComboBoxItem).Value.ToString() == ConstDefine.securityAdmin.ToString())
                    return false;
            }

            int count = _userGridView.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                if (_rowIndex == i)
                    continue;

                if (_userGridView.Rows[i].Cells[(int)ConstDefine.eUserListView.securityCode].Value.ToString() == ConstDefine.securityAdmin.ToString())
                    return false;
            }

            return true;
        }

        public bool DeleteUser()
        {
            if (_rowIndex == -1)
                return false;

            if (IsLastAdmin("DELETE") == true)
            {
                MessageBox.Show("마지막 Admin 계정은 삭제할 수 없습니다.", ConstDefine.adminTitle);
                return false; 
            }

            int retVal = DbHelper.ExecuteNonQuery(string.Format("{0} '{1}'",
                "EXEC DeleteUser",
                _userGridView.Rows[_rowIndex].Cells[(int)ConstDefine.eUserListView.userId].Value.ToString().Trim()));

            if (retVal < 0)
            {
                MessageBox.Show("사용자를 삭제할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.adminTitle);
                return false; 
            }
            else
            {
                /*
                _userGridView.Rows.RemoveAt(_rowIndex); 
                _adminPage.ResetUserListNo();
                _adminPage.ResetStatusBar();
                */
            }

            InitUserFields();

            return true; 
        }

        public void CancelUser()
        {
        }

        private void UserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _adminPage.LoadUserList();
        }
    }
}
