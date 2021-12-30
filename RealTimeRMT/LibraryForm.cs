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
using System.Data.SqlClient;

namespace RealTimeRMT
{
    public partial class LibraryForm : Form
    {
        private bool _newFile = false;
        private LibraryPage _libraryPage = null; 
        private DataGridView _libraryListView = null;
        private string _fileName = ""; 
        int _libraryType = -1; 

        public LibraryForm(bool newFile, LibraryPage libraryPage, DataGridView libraryListView, int libraryType)
        {
            InitializeComponent();

            _newFile = newFile;
            _libraryPage = libraryPage;
            _libraryListView = libraryListView;
            _libraryType = libraryType;

            InitControls(); 
        }

        public void InitControls()
        {
            if (_newFile == false)
                SetLibraryFields();
            else
                InitLibraryFields(); 

            UpdateLibraryViewCount();

            if (_newFile == true)
            {
                deleteLibraryButton.Enabled = false;
                downloadFileButton.Enabled = false;

                deleteLibraryButton.BackColor = Color.DarkGray;
                downloadFileButton.BackColor = Color.DarkGray; 
            }

            if (_libraryType == (int)ConstDefine.eLibraryType.common && _libraryPage.GetCurrentSecurityCode() != ConstDefine.securityAdmin)
            {
                saveLibraryButton.Enabled = false; 
                deleteLibraryButton.Enabled = false;
                changeFileButton.Enabled = false;

                saveLibraryButton.BackColor = Color.DarkGray;
                deleteLibraryButton.BackColor = Color.DarkGray;
                changeFileButton.BackColor = Color.DarkGray; 
            }

            changeFileButton.Focus(); 
        }

        public void UpdateLibraryViewCount() 
        { 
            string no = noTextBox.Text.Trim(); 
            DbHelper.ExecuteNonQuery(string.Format("{0} {1}", "EXEC UpdateLibraryViewCount", no));
        }

        public void UpdateLibraryDownloadCount()
        {
            string no = noTextBox.Text.Trim();
            DbHelper.ExecuteNonQuery(string.Format("{0} {1}", "EXEC UpdateLibraryDownloadCount", no));
        } 


        private void changeFileNameButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _fileName = openFileDialog.FileName;
                fileNameTextBox.Text = openFileDialog.SafeFileName;  
            }
        }

        private void saveLibraryButton_Click(object sender, EventArgs e)
        {
            if (true == SaveLibrary())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void deleteLibraryButton_Click(object sender, EventArgs e)
        {
            if (true == DeleteLibrary())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void listLibraryButton_Click(object sender, EventArgs e)
        {
            CancelLibrary(); 
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void downloadFileButton_Click(object sender, EventArgs e)
        {
            string no = noTextBox.Text.Trim();
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectLibraryFile", no));
            if (dataSet == null || dataSet.Tables.Count == 0)
            {
                MessageBox.Show("라이브러리 파일을 다운로드 할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.libraryTitle);
                return; 
            } 

            if(DBNull.Value.Equals(dataSet.Tables[0].Rows[0]["FileData"]) == true) 
            {
                MessageBox.Show("다운로드 할 라이브러리 파일이 없습니다.", ConstDefine.libraryTitle);
                return;
            }

            SaveFileDialog saveFileDialog= new SaveFileDialog();

            saveFileDialog.Filter = "All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = dataSet.Tables[0].Rows[0]["FileName"].ToString(); 

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog.FileName, (Byte[])dataSet.Tables[0].Rows[0]["FileData"]);
                UpdateLibraryDownloadCount();
                MessageBox.Show("다운로드가 완료되었습니다.", ConstDefine.libraryTitle);
            }
        }

        public bool SaveLibrary()
        {
            if (CheckRequiredField() == false)
            {
                MessageBox.Show("라이브리러 파일을 선택해 주십시오.", ConstDefine.libraryTitle);
                return false;
            }

            if (_newFile == true && IsExistedFile() == false)
            {
                MessageBox.Show("라이브러리 파일이 디스크에 존재하는지 확인해 주십시오.", ConstDefine.libraryTitle);
                return false;
            }

            string no = noTextBox.Text.Trim(); 
            string fileName = fileNameTextBox.Text.Trim();
            // fileData
            string fileDescription = fileDescriptionTextBox.Text.Trim();
            string viewCount = viewCountTextBox.Text.Trim(); 
            string downloadCount = downloadCountTextBox.Text.Trim(); 
            string writer = writerTextBox.Text.Trim();
            string userId = _libraryPage._parent.GetCurrentUserId(); 
            string createdDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string libraryType = (_libraryType == (int)ConstDefine.eLibraryType.common) ? "COMMON" : "PERSONAL";
         

            SqlParameter fileParam = new SqlParameter();
            fileParam.SqlDbType = SqlDbType.VarBinary;
            fileParam.ParameterName = "FileData";
            fileParam.Value = (IsExistedFile() == false) ? null : File.ReadAllBytes(_fileName); 

            int retVal = 0;
            if (_newFile == true)
            {
                retVal = DbHelper.ExecuteNonQueryWithFileData(string.Format("{0} '{1}', @FileData, '{2}', '{3}', {4}, {5}, '{6}', '{7}'",
                    "EXEC InsertLibrary", Utils.ReplaceSpecialChar(fileName), Utils.ReplaceSpecialChar(fileDescription), 
                    libraryType, viewCount, downloadCount, userId, createdDate), fileParam);
            }
            else
            {
                if (fileParam.Value != null)
                    retVal = DbHelper.ExecuteNonQueryWithFileData(string.Format("{0} {1}, '{2}', @FileData, '{3}'",
                        "EXEC UpdateLibrary", no, Utils.ReplaceSpecialChar(fileName), Utils.ReplaceSpecialChar(fileDescription)), fileParam);
                else
                    retVal = DbHelper.ExecuteNonQuery(string.Format("{0} {1}, '', NULL, '{2}'",
                        "EXEC UpdateLibrary", no, Utils.ReplaceSpecialChar(fileDescription))); 
            }

            if (retVal < 1)
            {
                MessageBox.Show("라이브러리를 저장할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.libraryTitle);
                return false;
            }
                /*
            else
            {
                _libraryPage.LoadLibraryList(_libraryType);
                _libraryPage.ResetStatusBar(_libraryType);
            }
            */ 

            InitLibraryFields();

            return true;
        }

        public bool CheckRequiredField()
        {
            if (fileNameTextBox.Text.Trim() == "")
            {
                changeFileButton.Focus();
                return false;
            }
            return true;
        }

        public void SetLibraryFields()
        {
            if (_libraryListView.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = _libraryListView.SelectedRows[0];

            noTextBox.Text = row.Cells[(int)ConstDefine.eLibraryListView.no].Value.ToString();
            fileNameTextBox.Text = row.Cells[(int)ConstDefine.eLibraryListView.fileName].Value.ToString();
            fileDescriptionTextBox.Text = row.Cells[(int)ConstDefine.eLibraryListView.fileDescription].Value.ToString();
            viewCountTextBox.Text = row.Cells[(int)ConstDefine.eLibraryListView.viewCount].Value.ToString();
            downloadCountTextBox.Text = row.Cells[(int)ConstDefine.eLibraryListView.downloadCount].Value.ToString();
            writerTextBox.Text = row.Cells[(int)ConstDefine.eLibraryListView.writer].Value.ToString();
        }

        public void InitLibraryFields()
        {
            noTextBox.Text = "";
            fileNameTextBox.Text = "";
            fileDescriptionTextBox.Text = "";
            viewCountTextBox.Text = "0";
            downloadCountTextBox.Text = "0";
            writerTextBox.Text = _libraryPage._parent.GetCurrentUserName();
        }

        public bool DeleteLibrary()
        {
            if (_libraryListView.SelectedRows.Count == 0)
                return false;  

            int retVal = DbHelper.ExecuteNonQuery(string.Format("{0} '{1}'",
                "EXEC DeleteLibrary",
                _libraryListView.SelectedRows[0].Cells[(int)ConstDefine.eLibraryListView.no].Value.ToString().Trim()));

            if (retVal < 0)
            {
                MessageBox.Show("라이브러리를 삭제할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.libraryTitle);
                return false;
            }
                /*
            else
            {
                _libraryPage.LoadLibraryList(_libraryType); 
                _libraryPage.ResetStatusBar(_libraryType);
            }
            */ 

            InitLibraryFields();

            return true;
        }

        public void CancelLibrary()
        {
            /*
                
             * _libraryPage.LoadLibraryList(_libraryType); 
                _libraryPage.ResetStatusBar(_libraryType);
             */
        }

        public bool IsExistedFile()
        {
            if (_fileName == "")
                return false;

            if (File.Exists(_fileName) == false)
                return false;

            return true; 
        }

        private void LibraryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _libraryPage.LoadLibraryList(_libraryType);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            TextBox[] textBoxArray = { fileNameTextBox, fileDescriptionTextBox };

            Pen pen = new Pen(Color.FromArgb(249, 249, 250));
            for (int i = 0; i < textBoxArray.Length; i++)
            {
                Graphics g = e.Graphics;
                g.DrawRectangle(pen, textBoxArray[i].Left - 1, textBoxArray[i].Top - 1, textBoxArray[i].Width + 2, textBoxArray[i].Height + 2);
            }
        }
    }
}
