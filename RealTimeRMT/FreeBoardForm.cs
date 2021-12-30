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
    public partial class FreeBoardForm : Form
    {
        private bool _newBoard = false;
        private LibraryPage _libraryPage = null; 
        private DataGridView _boardListView = null;
        private string _fileName = ""; 

        public FreeBoardForm(bool newFile, LibraryPage libraryPage, DataGridView boardListView)
        {
            InitializeComponent();

            _newBoard = newFile;
            _libraryPage = libraryPage;
            _boardListView = boardListView;

            InitControls(); 
        }

        public void InitControls()
        {
            if (_newBoard == false)
                SetBoardFields();
            else
                InitBoardFields();

            UpdateBoardViewCount(); 

            if (_newBoard == true)
            {
                deleteBoardButton.Enabled = false;
                downloadFileButton.Enabled = false;

                deleteBoardButton.BackColor = Color.DarkGray;
                downloadFileButton.BackColor = Color.DarkGray; 
            }
        }

        public void UpdateBoardViewCount() 
        { 
            string no = noTextBox.Text.Trim(); 
            DbHelper.ExecuteNonQuery(string.Format("{0} {1}", "EXEC UpdateFreeBoardViewCount", no));
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

        private void saveBoardButton_Click(object sender, EventArgs e)
        {
            if (true == SaveBoard())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void deleteBoardButton_Click(object sender, EventArgs e)
        {
            if (true == DeleteBoard())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cancelBoardButton_Click(object sender, EventArgs e)
        {
            CancelBoard(); 
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void downloadFileButton_Click(object sender, EventArgs e)
        {
            string no = noTextBox.Text.Trim(); 
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectFreeBoardFile", no));
            if (dataSet == null || dataSet.Tables.Count == 0)
            {
                MessageBox.Show("파일을 다운로드 할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.freeBoardTitle);
                return; 
            } 

            if(DBNull.Value.Equals(dataSet.Tables[0].Rows[0]["FileData"]) == true) 
            {
                MessageBox.Show("다운로드 할 파일이 없습니다.", ConstDefine.freeBoardTitle);
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
                MessageBox.Show("다운로드가 완료되었습니다.", ConstDefine.freeBoardTitle);
            }
        }

        public bool SaveBoard()
        {
            if (CheckRequiredField() == false)
            {
                MessageBox.Show("필수 항목을 모두 입력해 주십시오.", ConstDefine.freeBoardTitle);
                return false;
            }

            string no = noTextBox.Text.Trim(); 
            string title = titleTextBox.Text.Trim(); 
            string contents = contentsTextBox.Text.Trim(); 
            string fileName = fileNameTextBox.Text.Trim();
            string viewCount = viewCountTextBox.Text.Trim(); 
            string writer = writerTextBox.Text.Trim();
            string userId = _libraryPage._parent.GetCurrentUserId(); 
            string createdDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
         

            SqlParameter fileParam = new SqlParameter();
            fileParam.SqlDbType = SqlDbType.VarBinary;
            fileParam.ParameterName = "FileData";
            fileParam.Value = (IsExistedFile() == false) ? null : File.ReadAllBytes(_fileName); 

            int retVal = 0;
            if (_newBoard == true)
            {
                if (fileParam.Value != null)
                    retVal = DbHelper.ExecuteNonQueryWithFileData(string.Format("{0} '{1}', '{2}', {3}, '{4}', @FileData, '{5}', '{6}'",
                        "EXEC InsertFreeBoard", Utils.ReplaceSpecialChar(title), Utils.ReplaceSpecialChar(contents), viewCount, 
                        Utils.ReplaceSpecialChar(fileName), userId, createdDate), fileParam);
                else
                    retVal = DbHelper.ExecuteNonQuery(string.Format("{0} '{1}', '{2}', {3}, '', NULL, '{4}', '{5}'",
                        "EXEC InsertFreeBoard", Utils.ReplaceSpecialChar(title), Utils.ReplaceSpecialChar(contents), viewCount, userId, createdDate));
            }
            else
            {
                if (fileParam.Value != null)
                    retVal = DbHelper.ExecuteNonQueryWithFileData(string.Format("{0} {1}, '{2}', '{3}', '{4}', @FileData",
                        "EXEC UpdateFreeBoard", no, Utils.ReplaceSpecialChar(title), Utils.ReplaceSpecialChar(contents), 
                        Utils.ReplaceSpecialChar(fileName)), fileParam);
                else
                    retVal = DbHelper.ExecuteNonQuery(string.Format("{0} {1}, '{2}', '{3}', '', NULL",
                        "EXEC UpdateFreeBoard", no, Utils.ReplaceSpecialChar(title), Utils.ReplaceSpecialChar(contents))); 
            }

            if (retVal < 1)
            {
                MessageBox.Show("게시글을 저장할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.freeBoardTitle);
                return false;
            }

            InitBoardFields();
            return true;
        }

        public bool CheckRequiredField()
        {
            if (titleTextBox.Text.Trim() == "")
            {
                titleTextBox.Focus();
                return false;
            }
            if (contentsTextBox.Text.Trim() == "")
            {
                contentsTextBox.Focus();
                return false;
            }
            return true;
        }

        public void SetBoardFields()
        {
            if (_boardListView.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = _boardListView.SelectedRows[0];

            noTextBox.Text = row.Cells[(int)ConstDefine.eBoardListView.no].Value.ToString();
            titleTextBox.Text = row.Cells[(int)ConstDefine.eBoardListView.title].Value.ToString();
            viewCountTextBox.Text = row.Cells[(int)ConstDefine.eBoardListView.viewCount].Value.ToString();
            writerTextBox.Text = row.Cells[(int)ConstDefine.eBoardListView.writer].Value.ToString();
            createdDateTextBox.Text = row.Cells[(int)ConstDefine.eBoardListView.createdDate].Value.ToString();
            contentsTextBox.Text = GetContents(); 
            fileNameTextBox.Text = row.Cells[(int)ConstDefine.eBoardListView.fileName].Value.ToString();
        }

        public void InitBoardFields()
        {
            noTextBox.Text = "";
            titleTextBox.Text = "";
            contentsTextBox.Text = "";
            viewCountTextBox.Text = "0";
            writerTextBox.Text = _libraryPage._parent.GetCurrentUserName();
            createdDateTextBox.Text = ""; 
        }

        public bool DeleteBoard()
        {
            if (_boardListView.SelectedRows.Count == 0)
                return false;  

            int retVal = DbHelper.ExecuteNonQuery(string.Format("{0} '{1}'",
                "EXEC DeleteFreeBoard",
                _boardListView.SelectedRows[0].Cells[(int)ConstDefine.eBoardListView.no].Value.ToString().Trim()));

            if (retVal < 0)
            {
                MessageBox.Show("게시글을 삭제할 수 없습니다. 관리자에게 문의해 주십시오.", ConstDefine.freeBoardTitle);
                return false;
            }
            InitBoardFields();
            return true;
        }

        public void CancelBoard()
        {
        }

        public bool IsExistedFile()
        {
            if (_fileName == "")
                return false;

            if (File.Exists(_fileName) == false)
                return false;

            return true; 
        }

        private void FreeBoardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _libraryPage.LoadFreeBoardList();
        }

        private string GetContents()
        {
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectFreeBoardContents", noTextBox.Text.Trim()));  
             if (dataSet == null || dataSet.Tables.Count == 0)
                return "";
                
            if(DBNull.Value.Equals(dataSet.Tables[0].Rows[0]["Contents"]) == true)
                return ""; 
            else 
               return dataSet.Tables[0].Rows[0]["Contents"].ToString(); 
        }

        private void freeBoardPanel_Paint(object sender, PaintEventArgs e)
        {
            TextBox[] textBoxArray = { titleTextBox, contentsTextBox, fileNameTextBox };

            Pen pen = new Pen(Color.FromArgb(249, 249, 250));
            for (int i = 0; i < textBoxArray.Length; i++)
            {
                Graphics g = e.Graphics;
                g.DrawRectangle(pen, textBoxArray[i].Left - 1, textBoxArray[i].Top - 1, textBoxArray[i].Width + 2, textBoxArray[i].Height + 2);
            }
        }
    }
}
