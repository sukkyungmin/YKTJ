using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.Drawing;



namespace RealTimeRMT
{
    class SapUpdatePage
    {
        private MainForm _parent = null;
        private List<Schedule> _scheduleList = null;
        private List<Product> _productList = null;
        private List<Bom> _bomList = null;
        private List<Bom> _lengthList = null;
        public string ss;

        public SapUpdatePage(MainForm parent)
        {
            _parent = parent;
            _scheduleList = new List<Schedule>();
            _productList = new List<Product>();
            _bomList = new List<Bom>();
            _lengthList = new List<Bom>();

            CreateDirectory();
        }

        public void InitControls()
        {
            InitSapUpdateLogDataGridView();
            _parent.suSettingSideTeamTabControl.SelectedIndex = (int)ConstDefine.eTeamType.cc;
            _parent.suSettingTeamNameLabel.Text = "유아2팀  ";
            ChangeSuTjButtDataGridViewStyle();
            _parent.suTabControl.SelectedIndex = (int)ConstDefine.eSapUpdateTab.update;
            _parent.suCcDbUpdateButton.Checked = true;
            _parent.suTj01Button.Checked = true;
            _parent.suTj21Button.Checked = true;
        }

        private void CreateDirectory()
        {
            if (Directory.Exists(ConstDefine.sapSourceDir) == false)
                Directory.CreateDirectory(ConstDefine.sapSourceDir);
        }

        public void SelectSapSourceFile(int sapFileType)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ConstDefine.sapSourceDir;
            //openFileDialog.Filter = "Excel File (*.xls*)|*.xls*" ;
            openFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            //openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "RealTime Database 업데이트 작업파일(소스파일) 선택";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            // 파일 검증
            if (false == IsValidFileType(sapFileType, openFileDialog.SafeFileName))
                return;

            // 파일 경로, 상태 변경
            SetSelectedFileState(sapFileType, openFileDialog.FileName);
        }

        private bool IsValidFileType(int sapFileType, string safeFileName)
        {
            // 선택한 파일이름에서 ProductVersion, ProductBOM, Length 문자열을 검사, Schedule 파일이 기본
            string[] checkString = new string[] { "", "ProductVersion", "ProductBOM", "Length" };
            safeFileName = safeFileName.ToUpper();
            if (sapFileType != (int)ConstDefine.eSapFileType.schedule)
            {
                if (safeFileName.IndexOf(checkString[sapFileType].ToUpper()) == -1)
                {
                    MessageBox.Show("파일 이름에 -" + checkString[sapFileType] + "- 문자열이 없습니다. 다른 파일을 선택하거나 파일이름을 변경하십시오");
                    return false;
                }
            }
            else
            {
                for (int i = 1; i < checkString.Length; i++)
                {
                    if (safeFileName.IndexOf(checkString[i].ToUpper()) != -1)
                    {
                        MessageBox.Show("파일 이름에 -" + checkString[i] + "- 문자열이 포함되어 있습니다. 다른 파일을 선택하거나 파일이름을 변경하십시오");
                        return false;
                    }
                }
            }

            return true;
        }

        //
        // SAP 파일 선택 상태 변경
        //
        private void SetSelectedFileState(int sapFileType, string fileName)
        {
            if (sapFileType == (int)ConstDefine.eSapFileType.schedule)
            {
                _parent.suScheduleFilePathTextBox.Text = fileName;
                if (_parent.suScheduleFileStateImage.BackgroundImage != null)
                {
                    _parent.suScheduleFileStateImage.BackgroundImage.Dispose();
                    _parent.suScheduleFileStateImage.BackgroundImage = null;
                }
                _parent.suScheduleFileStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_on;
                _parent.suUpdateScheduleButton.Enabled = true;
            }
            else if (sapFileType == (int)ConstDefine.eSapFileType.product)
            {
                _parent.suProductFilePathTextBox.Text = fileName;
                if (_parent.suProductFileStateImage.BackgroundImage != null)
                {
                    _parent.suProductFileStateImage.BackgroundImage.Dispose();
                    _parent.suProductFileStateImage.BackgroundImage = null;
                }
                _parent.suProductFileStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_on;
                _parent.suUpdateProductButton.Enabled = true;
            }
            else if (sapFileType == (int)ConstDefine.eSapFileType.bom)
            {
                _parent.suBomFilePathTextBox.Text = fileName;
                if (_parent.suBomFileStateImage.BackgroundImage != null)
                {
                    _parent.suBomFileStateImage.BackgroundImage.Dispose();
                    _parent.suBomFileStateImage.BackgroundImage = null;
                }
                _parent.suBomFileStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_on;
                if (_parent.suBomFilePathTextBox.Text != "" && _parent.suLengthFilePathTextBox.Text != "")
                    _parent.suUpdateBomButton.Enabled = true;
            }

            else if (sapFileType == (int)ConstDefine.eSapFileType.length)
            {
                _parent.suLengthFilePathTextBox.Text = fileName;
                if (_parent.suLengthFileStateImage.BackgroundImage != null)
                {
                    _parent.suLengthFileStateImage.BackgroundImage.Dispose();
                    _parent.suLengthFileStateImage.BackgroundImage = null;
                }
                _parent.suLengthFileStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_on;
                if (_parent.suBomFilePathTextBox.Text != "" && _parent.suLengthFilePathTextBox.Text != "")
                    _parent.suUpdateBomButton.Enabled = true;
            }
            else
            {
                _parent.suScheduleFilePathTextBox.Text = "";
                if (_parent.suScheduleFileStateImage.BackgroundImage != null)
                {
                    _parent.suScheduleFileStateImage.BackgroundImage.Dispose();
                    _parent.suScheduleFileStateImage.BackgroundImage = null;
                }
                _parent.suScheduleFileStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_off;

                _parent.suProductFilePathTextBox.Text = "";
                if (_parent.suProductFileStateImage.BackgroundImage != null)
                {
                    _parent.suProductFileStateImage.BackgroundImage.Dispose();
                    _parent.suProductFileStateImage.BackgroundImage = null;
                }
                _parent.suProductFileStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_off;

                _parent.suBomFilePathTextBox.Text = "";
                if (_parent.suBomFileStateImage.BackgroundImage != null)
                {
                    _parent.suBomFileStateImage.BackgroundImage.Dispose();
                    _parent.suBomFileStateImage.BackgroundImage = null;
                }
                _parent.suBomFileStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_off;

                _parent.suLengthFilePathTextBox.Text = "";
                if (_parent.suLengthFileStateImage.BackgroundImage != null)
                {
                    _parent.suLengthFileStateImage.BackgroundImage.Dispose();
                    _parent.suLengthFileStateImage.BackgroundImage = null;
                }
                _parent.suLengthFileStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_off;

                _parent.suUpdateScheduleButton.Enabled = false;
                _parent.suUpdateProductButton.Enabled = false;
                _parent.suUpdateBomButton.Enabled = false;
                _parent.suUpdateBatchButton.Enabled = false;
            }

            // DB 업데이트 배치
            if (_parent.suUpdateScheduleButton.Enabled == true && _parent.suUpdateProductButton.Enabled == true &&
                _parent.suUpdateBomButton.Enabled == true)
                _parent.suUpdateBatchButton.Enabled = true;
        }

        //
        // DB 업데이트 상태 변경
        //
        public void SetUpdatedDbState(int sapFileType)
        {
            if (sapFileType == (int)ConstDefine.eSapFileType.schedule)
            {
                if (_parent.suScheduleUpdateStateImage.BackgroundImage != null)
                {
                    _parent.suScheduleUpdateStateImage.BackgroundImage.Dispose();
                    _parent.suScheduleUpdateStateImage.BackgroundImage = null;
                }
                _parent.suScheduleUpdateStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_on;
            }
            else if (sapFileType == (int)ConstDefine.eSapFileType.product)
            {
                if (_parent.suProductUpdateStateImage.BackgroundImage != null)
                {
                    _parent.suProductUpdateStateImage.BackgroundImage.Dispose();
                    _parent.suProductUpdateStateImage.BackgroundImage = null;
                }
                _parent.suProductUpdateStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_on;
            }
            else if (sapFileType == (int)ConstDefine.eSapFileType.bom)
            {
                if (_parent.suBomUpdateStateImage.BackgroundImage != null)
                {
                    _parent.suBomUpdateStateImage.BackgroundImage.Dispose();
                    _parent.suBomUpdateStateImage.BackgroundImage = null;
                }
                _parent.suBomUpdateStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_on;
            }
            else if (sapFileType == (int)ConstDefine.eSapFileType.batch)
            {
                if (_parent.suBatchUpdateStateImage.BackgroundImage != null)
                {
                    _parent.suBatchUpdateStateImage.BackgroundImage.Dispose();
                    _parent.suBatchUpdateStateImage.BackgroundImage = null;
                }
                _parent.suBatchUpdateStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_on;
            }
            else
            {
                if (_parent.suScheduleUpdateStateImage.BackgroundImage != null)
                {
                    _parent.suScheduleUpdateStateImage.BackgroundImage.Dispose();
                    _parent.suScheduleUpdateStateImage.BackgroundImage = null;
                }
                if (_parent.suProductUpdateStateImage.BackgroundImage != null)
                {
                    _parent.suProductUpdateStateImage.BackgroundImage.Dispose();
                    _parent.suProductUpdateStateImage.BackgroundImage = null;
                }
                if (_parent.suBomUpdateStateImage.BackgroundImage != null)
                {
                    _parent.suBomUpdateStateImage.BackgroundImage.Dispose();
                    _parent.suBomUpdateStateImage.BackgroundImage = null;
                }
                if (_parent.suBatchUpdateStateImage.BackgroundImage != null)
                {
                    _parent.suBatchUpdateStateImage.BackgroundImage.Dispose();
                    _parent.suBatchUpdateStateImage.BackgroundImage = null;
                }

                _parent.suScheduleUpdateStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_off;
                _parent.suProductUpdateStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_off;
                _parent.suBomUpdateStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_off;
                _parent.suBatchUpdateStateImage.BackgroundImage = RealTimeRMT.Properties.Resources.su_ok_off;
            }
        }

        public void InitUpdate()
        {
            SetSelectedFileState(-1, "");
            SetUpdatedDbState(-1);
        }
        public void InitSetting()
        {
            _parent.suTjProcessCodeTextBox.Text = "";
            _parent.suTjProcessNameTextBox.Text = "";
            _parent.suTjButtTextBox.Text = "";
            _parent.suTjButtValue1Button.Checked = false;
            _parent.suTjButtValue5Button.Checked = false;
            _parent.suTjButtValue10Button.Checked = false;
            _parent.suTjButtValue50Button.Checked = false;
            _parent.suTjButtValue100Button.Checked = false;
        }
        public bool UpdateSchedule(bool isBatch = false)
        {
            // 사용중인 파일 확인
            while (true == Utils.IsFileLocked(new FileInfo(_parent.suScheduleFilePathTextBox.Text)))
            {
                DialogResult result = MessageBox.Show("읽으려는 파일이 사용중입니다.", ConstDefine.updateTitle, MessageBoxButtons.RetryCancel);
                if (result == DialogResult.Cancel)
                    return false;
            }

            // HMI가 DB를 사용중인지 확인
            string isLocked = DbHelper.GetValue("EXEC SelectDbLockStatus 'S05'", "Status", "0");
            if (isLocked == "1")
            {
                MessageBox.Show("Retry after seconds...\nDataBase Edited by HMI.", ConstDefine.updateTitle);
                return false;
            }

            if (isBatch == false)
                _parent.OpenLoadingBar();

            // DB Lock
            DbHelper.ExecuteNonQuery("EXEC UpdateDbLockStatus 'S05', 0");

            // 스케줄 데이터 추출
            if (false == ExtractScheduleFromSapFile(_parent.suScheduleFilePathTextBox.Text))
                return false;

            // 스케줄 DB 업데이트 및 로그 저장
            string noChangedCount = "0";
            string insertedCount = "0";
            string updatedCount = "0";
            string updateItem = "Prod Schedule";
            string updateUser = _parent.GetCurrentUserName();

            UpdateScheduleDB(updateItem, updateUser, ref noChangedCount, ref insertedCount, ref updatedCount);

            // 로그 리스트뷰에 로그 추가
            string updatedDate = DbHelper.GetValue("EXEC SelectLastSapProdScheduleUpdatedDate " + GetSelectedTeamTypeValue().ToString(), "UpdatedDate", "");
            InsertSapLogListView(GetLastSapUpdateLogNoFromListView(), updateItem, updatedDate, updateUser, updatedCount, insertedCount, noChangedCount);

            // 상태 변경
            SetUpdatedDbState((int)ConstDefine.eSapFileType.schedule);

            // DB Unlock
            DbHelper.ExecuteNonQuery("EXEC UpdateDbLockStatus 'S05', 0");

            if (isBatch == false)
                _parent.CloseLoadingBar();

            return true;
        }

        private bool ExtractScheduleFromSapFile(string filePath)
        {
            _scheduleList.Clear();

            // 데이터 읽기
            int spaceRowCount = 100; // 빈줄이 연달아 100개이면 더이상 읽을 데이터가 없는 것으로 알고 빠져나간다. (잘못된 문서의 예외처리)
            const int codeCol = 0;
            const int codeLen = 8;
            int startTableRow = 0;
            const int dataStartCol = 2;
            const int dateRow = 0;
            const int unitCol = 1;
            int teamType = GetSelectedTeamTypeValue();

            int excelSheetIndex = teamType; // 첫번째 Sheet가 유아1팀, 두번째 Sheet가 유아2팀으로 가정한다. 
            //string csvFilePath = "";
            string version = "";
            string excelSheetName = "";

            try
            {
                /**
                 * Schedule은 excel에서 읽어오자. 
                 */
                GetExcelSheetName(filePath, excelSheetIndex, ref excelSheetName, ref version);
                DataTable dataTable = OleDbHelper.ImportExcel(filePath, excelSheetName, version);
                if (dataTable == null)
                {
                    MessageBox.Show(filePath + " 파일을 읽을 수 없습니다.");
                    return false;
                }

                string yearTwoFig = DateTime.Now.Year.ToString().Substring(0, 2);
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    //SetProgressValue(row); 
                    DataRow dataRow = dataTable.Rows[row];
                    // productCode 확인
                    string productCode = GetExcelData(dataRow[codeCol]);
                    // "" 이면 현재 테이블의 끝이거나 첫 date row이다. 
                    if (productCode == "")
                    {
                        if (--spaceRowCount == 0)
                            break;
                        startTableRow = row;
                        continue;
                    }
                    spaceRowCount = 100;

                    if (productCode.Length < codeLen)
                        continue;

                    productCode = productCode.Substring(0, codeLen);
                    //  숫자가 아닌 경우는 테이블 시작 후 두번째 row 까지다. 
                    if (Utils.IsDigit(productCode) == false)
                        continue;

                    // 데이터 확인 확인
                    for (int amountCol = dataStartCol; amountCol < dataTable.Columns.Count; amountCol++)
                    {
                        string amount = GetExcelData(dataRow[amountCol]);
                        if (amount == "")
                            continue;

                        Schedule schedule = new Schedule();
                        schedule.productCode = productCode;
                        schedule.amount = Utils.GetDigitFromString(amount);
                        string dateTime = GetExcelData(dataTable.Rows[dateRow][amountCol]);
                        schedule.dateTime = yearTwoFig + dateTime.Replace('.', '-');
                        string prodLine = GetExcelData(dataTable.Rows[startTableRow + 1][codeCol]);
                        schedule.prodLine = prodLine.Substring(0, prodLine.IndexOf('/')).Trim();
                        schedule.amtUnit = GetExcelData(dataRow[unitCol]);

                        _scheduleList.Add(schedule);
                    }
                }
            }
            finally
            {
                /*
                FileInfo csvfile = new FileInfo(csvFilePath);
                if (csvfile.Exists == true)
                    csvfile.Delete(); 
                 */
            }

            return true;
        }

        private bool UpdateScheduleDB(string updateItem, string updateUser, ref string noChangedCount, ref string insertedCount, ref string updatedCount)
        {
            bool result = true;
            int teamType = GetSelectedTeamTypeValue();
            using (SqlConnection connection = new SqlConnection(DbHelper._connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("UpdateScheduleDBTransaction");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = string.Format("{0} {1}", "EXEC DeleteAllSapProdSchedule", teamType);
                    command.ExecuteNonQuery();

                    for (int si = 0; si < _scheduleList.Count; si++)
                    {
                        Schedule schedule = _scheduleList[si];
                        command.CommandText = string.Format("{0} {1}, '{2}', '{3}', '{4}', {5}, '{6}'",
                            "EXEC InsertSapProdSchedule",
                            teamType,
                            schedule.productCode,
                            schedule.dateTime,
                            schedule.prodLine,
                            schedule.amount,
                            Utils.ReplaceSpecialChar(schedule.amtUnit));

                        command.ExecuteNonQuery();
                    }

                    // 
                    // Insert Sap Update Log
                    noChangedCount = "0";
                    insertedCount = _scheduleList.Count.ToString();
                    updatedCount = "0";
                    command.CommandText = string.Format("{0} {1}, '{2}', '{3}', {4}, {5}, {6}",
                        "EXEC InsertSapUpdateLog",
                        teamType,
                        updateItem,
                        updateUser,
                        noChangedCount,
                        insertedCount,
                        updatedCount);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result = false;
                    Console.Write(ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
                finally
                {
                    connection.Close();
                    _scheduleList.Clear();
                }
            }
            return result;
        }


        public bool UpdateProduct(bool isBatch = false)
        {
            // 사용중인 파일 확인
            while (true == Utils.IsFileLocked(new FileInfo(_parent.suProductFilePathTextBox.Text)))
            {
                DialogResult result = MessageBox.Show("읽으려는 파일이 사용중입니다.", ConstDefine.updateTitle, MessageBoxButtons.RetryCancel);
                if (result == DialogResult.Cancel)
                    return false;
            }

            // HMI가 DB를 사용중인지 확인
            string isLocked = DbHelper.GetValue("EXEC SelectDbLockStatus 'S05'", "Status", "0");
            if (isLocked == "1")
            {
                MessageBox.Show("Retry after seconds...\nDataBase Edited by HMI.", ConstDefine.updateTitle);
                return false;
            }

            if (isBatch == false)
                _parent.OpenLoadingBar();

            // DB Lock
            DbHelper.ExecuteNonQuery("EXEC UpdateDbLockStatus 'S05', 0"); ;

            // Product 데이터 추출
            //if (false == ExtractProductFromSapFile(teamType, productPaths[teamType].Text))
            if (false == ExtractProductFromSapFile(_parent.suProductFilePathTextBox.Text))
                return false;

            // Product DB 업데이트 및 로그 저장
            string noChangedCount = "0";
            string insertedCount = "0";
            string updatedCount = "0";
            string updateItem = "Prod Version";
            string updateUser = _parent.GetCurrentUserName();
            UpdateProductDB(updateItem, updateUser, ref noChangedCount, ref insertedCount, ref updatedCount);

            // 로그 리스트뷰에 로그 추가
            string updatedDate = DbHelper.GetValue("EXEC SelectLastSapProductVersionUpdatedDate " + GetSelectedTeamTypeValue().ToString(), "UpdatedDate", "");
            InsertSapLogListView(GetLastSapUpdateLogNoFromListView(), updateItem, updatedDate, updateUser, updatedCount, insertedCount, noChangedCount);

            // 상태 변경
            SetUpdatedDbState((int)ConstDefine.eSapFileType.product);

            // DB Unlock
            DbHelper.ExecuteNonQuery("EXEC UpdateDbLockStatus 'S05', 0");

            if (isBatch == false)
                _parent.CloseLoadingBar();

            return true;
        }

        private bool ExtractProductFromSapFile(string filePath)
        {
            _productList.Clear();

            // 데이터 읽기
            int spaceRowCount = 100; // 빈줄이 연달아 100개이면 더이상 읽을 데이터가 없는 것으로 알고 빠져나간다. (잘못된 문서의 예외처리)
            const int prodCodeCol = 4;
            const int prodLineCol = 21;
            const int descriptionCol = 15;
            const int dataStartRow = 6;

            int excelSheetIndex = 1; // 항상 첫번째 Sheet를 읽는다고 가정
            //string csvFilePath = "";
            string version = "";
            string excelSheetName = "";

            try
            {
                //GetCsvFilPath(filePath, excelSheetIndex, ref csvFilePath);
                //DataTable dataTable = OleDbHelper.ImportCsv(csvFilePath); 
                GetExcelSheetName(filePath, excelSheetIndex, ref excelSheetName, ref version);
                DataTable dataTable = OleDbHelper.ImportExcel(filePath, excelSheetName, version);

                if (dataTable == null)
                {
                    MessageBox.Show(filePath + " 파일을 읽을 수 없습니다.");
                    return false;
                }

                string prodVerDate = dataTable.Rows[0][0].ToString();
                prodVerDate = prodVerDate.Substring(0, 10);
                string prodUpDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                for (int row = dataStartRow; row < dataTable.Rows.Count; row++)
                {
                    DataRow dataRow = dataTable.Rows[row];
                    string productCode = GetExcelData(dataRow[prodCodeCol]);
                    if (productCode == "")
                    {
                        if (--spaceRowCount == 0)
                            break;
                        continue;
                    }
                    spaceRowCount = 100;

                    Product product = new Product();
                    product.productCode = productCode;
                    product.prodLine = GetExcelData(dataRow[prodLineCol]);
                    product.prodVerDate = prodVerDate;
                    product.prodUpDate = prodUpDate;
                    product.description = GetExcelData(dataRow[descriptionCol]);

                    // 나머지 정보는 description에서 추출해 온다. 
                    if (true == ExtractProductExtraInfoFromDesc(ref product))
                        _productList.Add(product);
                }
            }
            finally
            {
                /*
                FileInfo csvfile = new FileInfo(csvFilePath);
                if (csvfile.Exists == true)
                    csvfile.Delete(); 
                */
            }

            return true;
        }

        private bool ExtractProductExtraInfoFromDesc(ref Product product)
        {
            try
            {
                string description = product.description;
                ss = description;
                string pattern = "[0-9][남여공]|[0-9] BOY|[0-9] GIRL|[0-9] NORMAL|[0-9] UNI|[0-9][BGN]|[0-9] [남여공]|[0-9] [BGN]|[남여공] 소형|[남여공] 중형|[남여공] 대형|소형 [남여공]|중형 [남여공]|대형 [남여공]|"
                                                    + "[LM][0-9][BG]|[X]L[0-9][BG]|[X][XL]L[0-9][BG]";
                Match match = Regex.Match(description, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (match.Success)
                {
                    //Console.WriteLine("Found '{0}' at position {1}", match.Value, match.Index);
                    product.prodName = description.Substring(0, match.Index).Trim();

                    string tempProdGs = Regex.Replace(match.Value.Trim(), "소형|중형|대형", "2", RegexOptions.Singleline); // 소형, 중형, 대형 사이즈는 항상 2, 확인 필요
                    string tempProdSize = tempProdGs;
                    tempProdGs = Regex.Replace(tempProdGs, "남|B|BOY|[LM][0-9]B|[X][XL][L][0-9]B", "B", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    tempProdGs = Regex.Replace(tempProdGs, "여|G|GIRL|[LM][0-9]G|[X][XL][L][0-9]G", "G", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    tempProdGs = Regex.Replace(tempProdGs, "공|N|NORMAL|UNI", "N", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    product.prodGender = Regex.Replace(tempProdGs, "[0-9]", "", RegexOptions.Singleline);
                    product.prodSize = "STEP-" + Regex.Replace(tempProdSize, "[^0-9]", "", RegexOptions.Singleline);

                    product.domestic = "DOMESTIC";
                    product.country = "KOR";
                    product.prodCountPerBag = "0";
                    product.bagCountPerCase = "0";
                    product.prodTotal = "0";
                    string tempExtra = "";
                    string tempPb = description.Substring(match.Index + match.Value.Length).Trim();
                    tempPb = Regex.Replace(tempPb, "-|'", "", RegexOptions.Singleline);
                    int tempPbPos = tempPb.IndexOf(' ');
                    if (tempPbPos != -1)
                    {
                        tempExtra = tempPb.Substring(tempPbPos).Trim();
                        tempPb = tempPb.Substring(0, tempPbPos);
                    }

                    //string[] tempPbArray = tempPb.Split('/');
                    string[] tempPbArray = Regex.Split(tempPb, "[/]|[(]|[xe]|[X]");
                    product.prodCountPerBag = Regex.Replace(tempPbArray[0], "[^0-9+]", "", RegexOptions.Singleline);
                    product.bagCountPerCase = Regex.Replace(tempPbArray[1], "[^0-9]", "", RegexOptions.Singleline);
                    int plusPos = product.prodCountPerBag.IndexOf('+');
                    if (plusPos != -1)
                    {
                        product.prodCountPerBag = (Convert.ToInt32(product.prodCountPerBag.Substring(0, plusPos).Trim()) +
                            Convert.ToInt32(product.prodCountPerBag.Substring(plusPos + 1).Trim())).ToString();
                    }
                    product.prodTotal = (Convert.ToInt32(product.prodCountPerBag) * Convert.ToInt32(product.bagCountPerCase)).ToString();

                    if (tempExtra != "")
                    {
                        //Match matchContry = Regex.Match(tempExtra, "HK|TW|S.A.|SG|CN|VN|JP|TH|IT|UK|MG|RE", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        Match matchContry = Regex.Match(tempExtra, "HK|TW|S.A.|SG|CN|VN|JP|TH|IT|UK|MG", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        if (matchContry.Success)
                        {
                            product.domestic = "EXPORT";
                            product.country = matchContry.Value;
                        }
                    }

                    return true;
                }


                pattern = "소형 [0-9][0-9]$|중형 [0-9][0-9]$|대형 [0-9][0-9]$";
                match = Regex.Match(description, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (match.Success)
                {
                    //Console.WriteLine("Found '{0}' at position {1}", match.Value, match.Index);
                    product.prodName = description.Substring(0, match.Index).Trim();
                    product.prodGender = "N";
                    product.prodSize = "STEP-" + match.Value.Substring(3, 1);
                    product.domestic = "DOMESTIC";
                    product.country = "KOR";
                    product.prodCountPerBag = match.Value.Substring(3, 2);
                    product.bagCountPerCase = "1";
                    product.prodTotal = product.prodCountPerBag;

                    return true;
                }

                //pattern = " [0-9]-| [0-9]U-| RE [0-9]U | [0-9]U ";
                pattern = " [0-9]-| [0-9]+| [0-9]U-| RE [0-9]U | [0-9]U | S[0-9]";
                match = Regex.Match(description, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (match.Success)
                {
                    //Console.WriteLine("Found '{0}' at position {1}", match.Value, match.Index);
                    product.prodName = description.Substring(0, match.Index).Trim();
                    product.prodGender = "N";
                    product.prodSize = "STEP-" + Regex.Replace(match.Value, "[^0-9]", "", RegexOptions.Singleline);
                    product.domestic = "DOMESTIC";
                    product.country = "KOR";

                    string tempPb = description.Substring(match.Index + match.Value.Length).Trim();
                    ////
                    // 하기스매직팬티(17) 3+공46'/4 패턴에 대한 예외처리
                    if (tempPb.IndexOf('+') == 0)
                    {
                        tempPb = tempPb.Substring(1);
                    }
                    ////

                    Match match2 = Regex.Match(tempPb, "AU", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    tempPb = (!match2.Success) ? tempPb.Replace("U", "") : tempPb;
                    string[] tempPbArray = Regex.Split(tempPb, "[/]|[(]|[x]|[X]");
                    product.prodCountPerBag = Regex.Replace(tempPbArray[0], "[^0-9+]", "", RegexOptions.Singleline);
                    product.bagCountPerCase = Regex.Replace(tempPbArray[1], "[^0-9]", "", RegexOptions.Singleline);
                    int plusPos = product.prodCountPerBag.IndexOf('+');
                    if (plusPos != -1)
                    {
                        product.prodCountPerBag = (Convert.ToInt32(product.prodCountPerBag.Substring(0, plusPos).Trim()) +
                            Convert.ToInt32(product.prodCountPerBag.Substring(plusPos + 1).Trim())).ToString();
                    }
                    product.prodTotal = (Convert.ToInt32(product.prodCountPerBag) * Convert.ToInt32(product.bagCountPerCase)).ToString();

                    string tempExtra = tempPb;
                    if (tempExtra != "")
                    {
                        //Match matchContry = Regex.Match(tempExtra, "HK|TW|S.A.|SG|CN|VN|JP|TH|IT|UK|MG|RE", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        Match matchContry = Regex.Match(tempExtra, "HK|TW|S.A.|SG|CN|VN|JP|TH|IT|UK|MG|AU", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        if (matchContry.Success)
                        {
                            product.domestic = "EXPORT";
                            product.country = matchContry.Value;
                        }
                    }

                    return true;
                }

                Console.WriteLine("Not Found '{0}'", description);
                return false;
            }
            catch (Exception ex2)
            {
                string filePath = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                  System.Windows.Forms.Application.StartupPath,
                  Path.DirectorySeparatorChar,
                  "LOG",
                  Path.DirectorySeparatorChar,
                  "QTRS-LOG-",
                  DateTime.Now.ToString("yyyy-MM-dd"),
                  ".txt");


                string dirPath = filePath.Substring(0, filePath.LastIndexOf(Path.DirectorySeparatorChar));
                if (Directory.Exists(dirPath) == false)
                    Directory.CreateDirectory(dirPath);

                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.Write(ss);
                    writer.Write("\r\n");
                }

                return false;
            }


        }


        private bool UpdateProductDB(string updateItem, string updateUser, ref string noChangedCount, ref string insertedCount, ref string updatedCount)
        {
            bool result = true;
            int teamType = GetSelectedTeamTypeValue();

            using (SqlConnection connection = new SqlConnection(DbHelper._connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("UpdateProductDBTransaction");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    for (int pi = 0; pi < _productList.Count; pi++)
                    {
                        Product product = _productList[pi];
                        command.CommandText = string.Format("{0} {1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', {12}, {13}, {14}",
                            "EXEC UpdateInsertSapProductVersion",
                            teamType,
                            product.productCode,    // 2
                            product.prodLine,       // 3
                            product.prodVerDate,    // 4
                            product.prodUpDate,     // 5
                            Utils.ReplaceSpecialChar(product.description),    // 6
                            Utils.ReplaceSpecialChar(product.prodName),       // 7
                            product.prodGender,     // 8
                            product.prodSize,       // 9
                            product.domestic,       // 10
                            product.country,        // 11
                            product.prodCountPerBag, // 12
                            product.bagCountPerCase, // 13
                            product.prodTotal);      // 14

                        command.ExecuteNonQuery();
                    }

                    // 
                    // Insert Sap Update Log
                    DataSet dataSet = new DataSet();
                    command.CommandText = "EXEC SelectCurrentSapProductVersionStatus " + teamType.ToString();
                    SqlDataAdapter adpt = new SqlDataAdapter(command);
                    adpt.Fill(dataSet);
                    if (dataSet != null)
                    {
                        DataTableCollection collection = dataSet.Tables;
                        if (collection.Count > 0)
                        {
                            DataTable table = collection[0];
                            if (table.Rows.Count > 0)
                            {
                                DataRow dataRow = table.Rows[0];
                                noChangedCount = dataRow["NoChangedCount"].ToString();
                                insertedCount = dataRow["insertedCount"].ToString();
                                updatedCount = dataRow["updatedCount"].ToString();
                            }
                        }
                    }
                    command.CommandText = string.Format("{0} {1}, '{2}', '{3}', {4}, {5}, {6}",
                        "EXEC InsertSapUpdateLog",
                        teamType,
                        updateItem,
                        updateUser,
                        noChangedCount,
                        insertedCount,
                        updatedCount);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result = false;
                    Console.Write(ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
                finally
                {
                    connection.Close();
                    _productList.Clear();
                }
            }
            return result;
        }

        public bool UpdateBom(bool isBatch = false)
        {
            // 엑셀 파일이 사용중인 파일 확인
            while (true == Utils.IsFileLocked(new FileInfo(_parent.suBomFilePathTextBox.Text)) &&
                true == Utils.IsFileLocked(new FileInfo(_parent.suLengthFilePathTextBox.Text)))
            {
                DialogResult result = MessageBox.Show("읽으려는 파일이 사용중입니다.", ConstDefine.updateTitle, MessageBoxButtons.RetryCancel);
                if (result == DialogResult.Cancel)
                    return false;
            }

            // HMI가 DB를 사용중인지 확인
            string isLocked = DbHelper.GetValue("EXEC SelectDbLockStatus 'S05'", "Status", "0");
            if (isLocked == "1")
            {
                MessageBox.Show("Retry after seconds...\nDataBase Edited by HMI.", ConstDefine.updateTitle);
                return false;
            }

            if (isBatch == false)
                _parent.OpenLoadingBar();

            // DB Lock
            DbHelper.ExecuteNonQuery("EXEC UpdateDbLockStatus 'S05', 0");
            DbHelper.ExecuteNonQuery("EXEC UpdateDbLockStatus 'S06', 1");

            // Length, Bom 데이터 추출
            if (false == ExtractLengthFromSapFile(_parent.suLengthFilePathTextBox.Text) || false == ExtractBomFromSapFile(_parent.suBomFilePathTextBox.Text))
                return false;

            // Bom DB 업데이트 및 로그 저장
            string noChangedCount = "0";
            string insertedCount = "0";
            string updatedCount = "0";
            string updateItem = "Prod BOM";
            string updateUser = _parent.GetCurrentUserName();
            UpdateBomDB(updateItem, updateUser, ref noChangedCount, ref insertedCount, ref updatedCount);

            // 로그 리스트뷰에 로그 추가
            string updatedDate = DbHelper.GetValue("EXEC SelectLastSapBomUpdatedDate " + GetSelectedTeamTypeValue().ToString(), "UpdatedDate", "");
            InsertSapLogListView(GetLastSapUpdateLogNoFromListView(), updateItem, updatedDate, updateUser, updatedCount, insertedCount, noChangedCount);

            // 상태 변경
            SetUpdatedDbState((int)ConstDefine.eSapFileType.bom);

            // DB Unlock
            DbHelper.ExecuteNonQuery("EXEC UpdateDbLockStatus 'S06', 0");
            DbHelper.ExecuteNonQuery("EXEC UpdateDbLockStatus 'S05', 0");

            if (isBatch == false)
                _parent.CloseLoadingBar();

            return true;
        }

        private bool ExtractLengthFromSapFile(string filePath)
        {
            _lengthList.Clear();

            // 데이터 읽기
            int spaceRowCount = 100; // 빈줄이 연달아 100개이면 더이상 읽을 데이터가 없는 것으로 알고 빠져나간다. (잘못된 문서의 예외처리)
            const int compCodeCol = 1;
            const int unitQuantityCol = 3;
            const int priceOfRollCol = 4;
            const int setDiaTarCol = 5;
            const int spliceDiaTarCol = 6;
            const int vendorCol = 7;
            const int dataStartRow = 2;

            int excelSheetIndex = 1; // 항상 첫번째 Sheet를 읽는다고 가정
            //string csvFilePath = "";
            string version = "";
            string excelSheetName = "";

            try
            {
                //GetCsvFilPath(filePath, excelSheetIndex, ref csvFilePath);
                //DataTable dataTable = OleDbHelper.ImportCsv(csvFilePath); 
                GetExcelSheetName(filePath, excelSheetIndex, ref excelSheetName, ref version);
                DataTable dataTable = OleDbHelper.ImportExcel(filePath, excelSheetName, version);

                if (dataTable == null)
                {
                    MessageBox.Show(filePath + " 파일을 읽을 수 없습니다.");
                    return false;
                }

                for (int row = dataStartRow; row < dataTable.Rows.Count; row++)
                {
                    DataRow dataRow = dataTable.Rows[row];

                    string compCode = GetExcelData(dataRow[compCodeCol]);
                    if (compCode == "")
                    {
                        if (--spaceRowCount == 0)
                            break;
                        continue;
                    }
                    spaceRowCount = 100;

                    Bom length = new Bom();
                    length.compCode = compCode;
                    length.unitQuantity = Utils.GetDigitFromString(GetExcelData(dataRow[unitQuantityCol]));
                    length.priceOfRoll = Utils.GetDecimalFromString(GetExcelData(dataRow[priceOfRollCol]));
                    length.setDiaTar = Utils.GetDecimalFromString(GetExcelData(dataRow[setDiaTarCol]));
                    length.spliceDiaTar = Utils.GetDecimalFromString(GetExcelData(dataRow[spliceDiaTarCol]));
                    length.vendor = GetExcelData(dataRow[vendorCol]);

                    _lengthList.Add(length);
                }
            }
            finally
            {
                /*
                FileInfo csvfile = new FileInfo(csvFilePath);
                if (csvfile.Exists == true)
                    csvfile.Delete(); 
                */
            }

            return true;
        }


        private bool ExtractBomFromSapFile(string filePath)
        {
            _bomList.Clear();

            // 데이터 읽기
            int spaceRowCount = 100; // 빈줄이 연달아 100개이면 더이상 읽을 데이터가 없는 것으로 알고 빠져나간다. (잘못된 문서의 예외처리)

            /*
            const int compCodeCol = 12;
            const int productCodeCol = 1;
            const int itemNumCol = 9;
            const int bomLevelCol = 10;
            const int itemCategoryCol = 11;
            const int altBomCol = 4;
            const int quantityCol = 16;
            const int resultQuantityCol = 20;
            const int unitCol = 17;
            const int descriptionCol = 13;
            const int dataStartRow = 2; 
            */
            // G(6)번째 컬럼에 신규 컬럼 추가, 사용하지는 않음
            const int compCodeCol = 13;
            const int productCodeCol = 1;
            const int itemNumCol = 10;
            const int bomLevelCol = 11;
            const int itemCategoryCol = 12;
            const int altBomCol = 4;
            const int quantityCol = 17;
            const int resultQuantityCol = 21;
            const int unitCol = 18;
            const int descriptionCol = 14;
            const int dataStartRow = 2;
            ////


            int excelSheetIndex = 1; // 항상 첫번째 Sheet를 읽는다고 가정
            //string csvFilePath = "";
            string version = "";
            string excelSheetName = "";

            try
            {
                //GetCsvFilPath(filePath, excelSheetIndex, ref csvFilePath);
                //DataTable dataTable = OleDbHelper.ImportCsv(csvFilePath); 
                GetExcelSheetName(filePath, excelSheetIndex, ref excelSheetName, ref version);
                DataTable dataTable = OleDbHelper.ImportExcel(filePath, excelSheetName, version);
                if (dataTable == null)
                {
                    MessageBox.Show(filePath + " 파일을 읽을 수 없습니다.");
                    return false;
                }

                for (int row = dataStartRow; row < dataTable.Rows.Count; row++)
                {
                    DataRow dataRow = dataTable.Rows[row];

                    string compCode = GetExcelData(dataRow[compCodeCol]);
                    if (compCode == "")
                    {
                        if (--spaceRowCount == 0)
                            break;
                        continue;
                    }
                    spaceRowCount = 100;

                    Bom bom = new Bom();
                    bom.compCode = compCode;
                    bom.productCode = GetExcelData(dataRow[productCodeCol]);
                    bom.itemNum = Utils.GetDigitFromString(GetExcelData(dataRow[itemNumCol]));
                    bom.bomLevel = GetExcelData(dataRow[bomLevelCol]);
                    if (bom.bomLevel != "1" && bom.bomLevel != "1.1")
                        bom.bomLevel = bom.bomLevel.Replace(".", "");
                    bom.itemCategory = GetExcelData(dataRow[itemCategoryCol]);
                    bom.altBom = GetExcelData(dataRow[altBomCol]);
                    bom.quantity = Utils.GetDecimalFromString(GetExcelData(dataRow[quantityCol]));
                    bom.resultQuantity = Utils.GetDecimalFromString(GetExcelData(dataRow[resultQuantityCol]));
                    bom.unit = GetExcelData(dataRow[unitCol]);
                    bom.description = GetExcelData(dataRow[descriptionCol]);

                    // 나머지 정보는 _lengthList에서 추출해 온다.
                    bool isFound = false;
                    for (int li = 0; li < _lengthList.Count; li++)
                    {
                        Bom length = _lengthList[li];
                        if (bom.compCode == length.compCode)
                        {
                            bom.unitQuantity = (length.unitQuantity == "") ? "0" : length.unitQuantity;
                            bom.priceOfRoll = (length.priceOfRoll == "") ? "0" : length.priceOfRoll;
                            bom.setDiaTar = (length.setDiaTar == "") ? "0" : length.setDiaTar;
                            bom.spliceDiaTar = (length.spliceDiaTar == "") ? "0" : length.spliceDiaTar;
                            bom.vendor = length.vendor;
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound == false)
                    {
                        bom.unitQuantity = "0";
                        bom.priceOfRoll = "0";
                        bom.setDiaTar = "0";
                        bom.spliceDiaTar = "0";
                        bom.vendor = "NA";
                    }

                    _bomList.Add(bom);
                }
            }
            finally
            {
                _lengthList.Clear();

                /*
                FileInfo csvfile = new FileInfo(csvFilePath);
                if (csvfile.Exists == true)
                    csvfile.Delete();
                */
            }

            return true;
        }

        private bool UpdateBomDB(string updateItem, string updateUser, ref string noChangedCount, ref string insertedCount, ref string updatedCount)
        {
            bool result = true;
            int teamType = GetSelectedTeamTypeValue();

            using (SqlConnection connection = new SqlConnection(DbHelper._connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("UpdateBomDBTransaction");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = string.Format("{0} {1}", "EXEC DeleteAllSapBom", teamType);
                    command.ExecuteNonQuery();

                    for (int bi = 0; bi < _bomList.Count; bi++)
                    {
                        Bom bom = _bomList[bi];
                        command.CommandText = string.Format("{0} {1}, '{2}', '{3}', {4}, {5}, '{6}', {7}, {8}, {9}, '{10}', '{11}', {12}, {13}, {14}, {15}, '{16}'",
                            "EXEC InsertSapBom",
                            teamType,
                            bom.compCode,           // 2
                            bom.productCode,        // 3
                            bom.itemNum,            // 4
                            bom.bomLevel,           // 5
                            bom.itemCategory,       // 6
                            bom.altBom,             // 7
                            bom.quantity,           // 8
                            bom.resultQuantity,     // 9
                            bom.unit,               // 10
                            Utils.ReplaceSpecialChar(bom.description),        // 11
                            bom.unitQuantity,       // 12
                            bom.priceOfRoll,        // 13
                            bom.setDiaTar,          // 14
                            bom.spliceDiaTar,       // 15
                            Utils.ReplaceSpecialChar(bom.vendor));            // 16

                        command.ExecuteNonQuery();
                    }

                    // 
                    // Insert Sap Update Log
                    DataSet dataSet = new DataSet();
                    command.CommandText = "EXEC SelectCurrentSapBomStatus " + teamType.ToString();
                    SqlDataAdapter adpt = new SqlDataAdapter(command);
                    adpt.Fill(dataSet);
                    if (dataSet != null)
                    {
                        DataTableCollection collection = dataSet.Tables;
                        if (collection.Count > 0)
                        {
                            DataTable table = collection[0];
                            if (table.Rows.Count > 0)
                            {
                                DataRow dataRow = table.Rows[0];
                                noChangedCount = dataRow["NoChangedCount"].ToString();
                                insertedCount = dataRow["insertedCount"].ToString();
                                updatedCount = dataRow["updatedCount"].ToString();
                            }
                        }
                    }
                    command.CommandText = string.Format("{0} {1}, '{2}', '{3}', {4}, {5}, {6}",
                        "EXEC InsertSapUpdateLog",
                        teamType,
                        updateItem,
                        updateUser,
                        noChangedCount,
                        insertedCount,
                        updatedCount);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result = false;
                    Console.Write(ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
                finally
                {
                    connection.Close();
                    _bomList.Clear();
                }
            }
            return result;
        }

        public void LoadSapUpdateLog()
        {
            int teamType = GetSelectedTeamTypeValue();
            _parent.suSapUpdateLogDataGridView.Rows.Clear();

            DataSet dataSet = DbHelper.SelectQuery("EXEC SelectSapUpdateLog " + teamType.ToString());
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            int no = 1;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                string updateItem = dataRow["UpdateItem"].ToString();
                string updatedDate = dataRow["UpdateDate"].ToString();
                string updateUser = dataRow["UpdateUser"].ToString();
                string updatedCount = dataRow["CountUpdate"].ToString();
                string insertedCount = dataRow["CountInsert"].ToString();
                string noChangeedCount = dataRow["CountNoChg"].ToString();
                InsertSapLogListView(no.ToString(), updateItem, updatedDate, updateUser, updatedCount, insertedCount, noChangeedCount);
                no++;
            }
            _parent.suSapUpdateLogDataGridView.ClearSelection();
        }

        public void LoadButtInfo()
        {
            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "EXEC SelectButtInfo", GetSelectedTjTypeValue()));
            if (dataSet == null || dataSet.Tables.Count == 0)
                return;

            _parent.suTjButtDataGridView.Rows.Clear();
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                _parent.suTjButtDataGridView.Rows.Add(dataRow["ProcCode"].ToString(), dataRow["ProcMnem"].ToString(), dataRow["GainReal2"].ToString());
            }
        }

        private void InsertSapLogListView(string no, string updateItem, string updatedDate, string updateUser,
            string updatedCount, string insertedCount, string noChangeedCount)
        {
            _parent.suSapUpdateLogDataGridView.Rows.Insert(0, no, updateItem, updatedDate, updateUser, updatedCount, insertedCount, noChangeedCount);
        }

        private string GetLastSapUpdateLogNoFromListView()
        {
            string no = "0";
            if (_parent.suSapUpdateLogDataGridView.Rows.Count > 0)
            {
                no = _parent.suSapUpdateLogDataGridView.Rows[0].Cells[(int)ConstDefine.eSapUpdateLogListView.no].Value.ToString();
                no = (Convert.ToInt32(no) + 1).ToString();
            }
            return no;
        }

        public void SetButtInfo()
        {
            DataGridViewRow row = _parent.suTjButtDataGridView.SelectedRows[0];
            _parent.suTjProcessCodeTextBox.Text = row.Cells[(int)ConstDefine.eSapButtListview.processCode].Value.ToString();
            _parent.suTjProcessNameTextBox.Text = row.Cells[(int)ConstDefine.eSapButtListview.processName].Value.ToString();
            _parent.suTjButtTextBox.Text = row.Cells[(int)ConstDefine.eSapButtListview.butt].Value.ToString();
        }

        public void SetButtValue(string value)
        {
            _parent.suTjButtTextBox.Text = value;
        }

        public void UpdateButtValue()
        {
            string processCode = _parent.suTjProcessCodeTextBox.Text.Trim();
            string butt = Utils.GetDigitFromString(_parent.suTjButtTextBox.Text.Trim());

            if (processCode == "")
            {
                MessageBox.Show("수정할 항목을 선택해 주십시오.", ConstDefine.updateTitle);
                return;
            }

            if (butt == "")
            {
                MessageBox.Show("Butt 값을 입력해 주십시오.", ConstDefine.updateTitle);
                _parent.suTjButtTextBox.Focus();
                return;
            }

            DbHelper.ExecuteNonQuery(string.Format("{0} {1}, '{2}', {3}", "EXEC UpdateButtValue", GetSelectedTjTypeValue(), processCode, butt));
            _parent.suTjButtDataGridView.SelectedRows[0].Cells[(int)ConstDefine.eSapButtListview.butt].Value = butt;
        }

        private void GetExcelSheetName(string excelFilePath, int excelSheetIndex, ref string excelSheetName, ref string version)
        {
            Excel.Application application = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Excel 파일 열기
                application = new Excel.Application();
                application.Visible = false;
                workbook = application.Workbooks.Open(excelFilePath);
                worksheet = workbook.Worksheets.get_Item(excelSheetIndex) as Excel.Worksheet;
                worksheet.Select();
                version = application.Version;
                excelSheetName = worksheet.Name;
            }
            finally
            {
                workbook.Close(false);
                application.Quit();
                ReleaseExcelObject(worksheet);
                ReleaseExcelObject(workbook);
                ReleaseExcelObject(application);
            }
        }

        private void GetCsvFilPath(string excelFilePath, int excelSheetIndex, ref string csvFilePath)
        {
            Excel.Application application = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                string tempPath = Path.GetTempPath();
                int i = 0;
                FileInfo file = null;
                do
                {
                    csvFilePath = tempPath + "realtimermtcvstemp" + i.ToString() + ".csv";
                    file = new FileInfo(csvFilePath);
                    i++;
                } while (file.Exists == true);

                // Excel 파일 열기
                application = new Excel.Application();
                application.Visible = false;
                workbook = application.Workbooks.Open(excelFilePath);
                worksheet = workbook.Worksheets.get_Item(excelSheetIndex) as Excel.Worksheet;
                worksheet.Select();
                //workbook.SaveAs(csvFilePath);
                workbook.SaveAs(csvFilePath, Excel.XlFileFormat.xlCSV);
                //workbook.SaveAs(csvFilePath, Excel.XlFileFormat.xlCSVWindows); 
                //workbook.SaveAs(csvFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlCSVWindows, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, false, Type.Missing, Type.Missing, Type.Missing);
            }
            finally
            {
                workbook.Close(false);
                application.Quit();
                ReleaseExcelObject(worksheet);
                ReleaseExcelObject(workbook);
                ReleaseExcelObject(application);
            }
        }

        private void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        /*
        private string GetExcelData(Excel.Range range)
        {
            return range.Value2 == null ? "" : range.Value2.ToString().Trim(); 
        }
        */

        private string GetExcelData(object dataObject)
        {
            return dataObject == null ? "" : dataObject.ToString().Trim();
        }


        public int GetSelectedTeamTypeValue()
        {
            int teamTypeValue = 2;

            if (_parent.suBcDbUpdateButton.Checked == true)
                teamTypeValue = 1;
            else if (_parent.suCcDbUpdateButton.Checked == true)
                teamTypeValue = 2;

            return teamTypeValue;
        }

        public int GetSelectedTjTypeValue()
        {
            int tjTypeValue = 21;

            if (_parent.suSettingSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc)
            {
                if (_parent.suTj01Button.Checked == true)
                    tjTypeValue = 1;
                else if (_parent.suTj02Button.Checked == true)
                    tjTypeValue = 2;
                else if (_parent.suTj03Button.Checked == true)
                    tjTypeValue = 3;
                else if (_parent.suTj04Button.Checked == true)
                    tjTypeValue = 4;
                else if (_parent.suTj05Button.Checked == true)
                    tjTypeValue = 5;
                else
                    tjTypeValue = 1;
            }
            else
            {
                if (_parent.suTj21Button.Checked == true)
                    tjTypeValue = 21;
                else if (_parent.suTj22Button.Checked == true)
                    tjTypeValue = 22;
                else if (_parent.suTj23Button.Checked == true)
                    tjTypeValue = 23;
                else
                    tjTypeValue = 21;
            }

            return tjTypeValue;
        }

        public void ResetTjButtons(string currentName)
        {
            CustomControls.ButtonEx01[] team1ButtonItems = { _parent.suTj01Button, _parent.suTj02Button, _parent.suTj03Button, _parent.suTj04Button, _parent.suTj05Button };
            CustomControls.ButtonEx01[] team2ButtonItems = { _parent.suTj21Button, _parent.suTj22Button, _parent.suTj23Button };

            CustomControls.ButtonEx01[] buttonItems = _parent.suSettingSideTeamTabControl.SelectedIndex == (int)ConstDefine.eTeamType.bc ? team1ButtonItems : team2ButtonItems;
            for (int i = 0; i < buttonItems.Length; i++)
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;
        }

        public void ResetTeamButtons(string currentName)
        {
            CheckBox[] buttonItems = { _parent.suBcDbUpdateButton, _parent.suCcDbUpdateButton };

            for (int i = 0; i < buttonItems.Length; i++)
            {
                buttonItems[i].Checked = (buttonItems[i].Name == currentName) ? true : false;
            }
        }
        public void ResizeControls()
        {
            _parent.suTabControl.Left = 0;
            _parent.suTabControl.Top = 0;
            _parent.suTabControl.Width = _parent.mainTabControl.Width;
            _parent.suTabControl.Height = _parent.mainTabControl.Height;

            _parent.suUpdateAsidePanel.Height = _parent.suTabControl.Height;
            _parent.ssDBUpdateLogPanel.Left = _parent.suUpdateAsidePanel.Width + ConstDefine.defaultGap;
            _parent.ssDBUpdateLogPanel.Top = _parent.suDBUpdatePanel.Top + _parent.suDBUpdatePanel.Height + 10;
            _parent.ssDBUpdateLogPanel.Width = _parent.suTabControl.Width - (_parent.suUpdateAsidePanel.Width + (ConstDefine.defaultGap * 2));
            _parent.ssDBUpdateLogPanel.Height = _parent.suTabControl.Height - (_parent.ssDBUpdateLogPanel.Top + ConstDefine.defaultGap);
            _parent.suSapUpdateLogDataGridView.Left = 0;
            _parent.suSapUpdateLogDataGridView.Top = 0;
            _parent.suSapUpdateLogDataGridView.Width = _parent.ssDBUpdateLogPanel.Width;
            _parent.suSapUpdateLogDataGridView.Height = _parent.ssDBUpdateLogPanel.Height;


            _parent.suSettingAsidePanel.Height = _parent.suUpdateAsidePanel.Height;


            int gridWidth = _parent.suSapUpdateLogDataGridView.Width - ConstDefine.scrollSize;
            int colWidth = gridWidth / _parent.suSapUpdateLogDataGridView.Columns.Count;
            int totalColWidth = 0;
            for (int i = 0; i < _parent.suSapUpdateLogDataGridView.Columns.Count; i++)
            {
                if ((int)ConstDefine.eSapUpdateLogListView.no == i)
                    _parent.suSapUpdateLogDataGridView.Columns[i].Width = 50;
                else
                    _parent.suSapUpdateLogDataGridView.Columns[i].Width = colWidth;

                totalColWidth += _parent.suSapUpdateLogDataGridView.Columns[i].Width;
            }
            if (gridWidth > totalColWidth)
            {
                _parent.suSapUpdateLogDataGridView.Columns[(int)ConstDefine.eSapUpdateLogListView.updateItem].Width += (gridWidth - totalColWidth) / 2;
                _parent.suSapUpdateLogDataGridView.Columns[(int)ConstDefine.eSapUpdateLogListView.updatedDate].Width += (gridWidth - totalColWidth) / 2;
            }


        }
        public void LoadSaData()
        {
            if (_parent.suTabControl.SelectedIndex == (int)ConstDefine.eSapUpdateTab.update)
            {
                InitUpdate();
                LoadSapUpdateLog();
            }
            else if (_parent.suTabControl.SelectedIndex == (int)ConstDefine.eSapUpdateTab.setting)
            {
                InitSetting();
                LoadButtInfo();
            }
        }

        private void ChangeSuTjButtDataGridViewStyle()
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
            _parent.suTjButtDataGridView.ColumnHeadersVisible = true;
            _parent.suTjButtDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.suTjButtDataGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.suTjButtDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.suTjButtDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.suTjButtDataGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.suTjButtDataGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.suTjButtDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.suTjButtDataGridView.AllowUserToResizeColumns = true;
            _parent.suTjButtDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.suTjButtDataGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.suTjButtDataGridView.RowHeadersVisible = false;

            // Default row style
            _parent.suTjButtDataGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.suTjButtDataGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.suTjButtDataGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.suTjButtDataGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.suTjButtDataGridView.AllowUserToResizeRows = false;
            //_parent.suTjButtDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.suTjButtDataGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.suTjButtDataGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.suTjButtDataGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.suTjButtDataGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.suTjButtDataGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.suTjButtDataGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.suTjButtDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.suTjButtDataGridView.GridColor = GRID_COLOR;
            _parent.suTjButtDataGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.suTjButtDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.suTjButtDataGridView.MultiSelect = false;
            _parent.suTjButtDataGridView.ReadOnly = true;
            _parent.suTjButtDataGridView.ScrollBars = ScrollBars.None;


            _parent.suTjButtDataGridView.Height = GRID_COLUMN_HEIGHT + (GRID_ROW_HEIGHT * 13);
            int colWidth = _parent.suTjButtDataGridView.Width / 3;
            _parent.suTjButtDataGridView.Columns[(int)ConstDefine.eSapButtListview.processCode].Width = colWidth;
            _parent.suTjButtDataGridView.Columns[(int)ConstDefine.eSapButtListview.processName].Width = colWidth;
            _parent.suTjButtDataGridView.Columns[(int)ConstDefine.eSapButtListview.butt].Width = _parent.suTjButtDataGridView.Width - (colWidth * 2);

            _parent.suTjButtDataGridView.Refresh();
        }

        private void InitSapUpdateLogDataGridView()
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
            _parent.suSapUpdateLogDataGridView.ColumnHeadersVisible = true;
            _parent.suSapUpdateLogDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _parent.suSapUpdateLogDataGridView.ColumnHeadersHeight = GRID_COLUMN_HEIGHT;
            _parent.suSapUpdateLogDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.suSapUpdateLogDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = GRID_COLUMN_FORE_COLOR;
            _parent.suSapUpdateLogDataGridView.ColumnHeadersDefaultCellStyle.BackColor = GRID_COLUMN_BACK_COLOR;
            _parent.suSapUpdateLogDataGridView.EnableHeadersVisualStyles = false; // 헤더 백컬러 변경을 위해 필요
            _parent.suSapUpdateLogDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.suSapUpdateLogDataGridView.AllowUserToResizeColumns = true;
            _parent.suSapUpdateLogDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //_parent.suTjButtListView.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.suSapUpdateLogDataGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Default row header style
            _parent.suSapUpdateLogDataGridView.RowHeadersVisible = false;

            // Default row style
            _parent.suSapUpdateLogDataGridView.RowsDefaultCellStyle.Font = new Font("BareunDotum 1", 11, FontStyle.Regular);
            _parent.suSapUpdateLogDataGridView.RowsDefaultCellStyle.ForeColor = GRID_ROW_FORE_COLOR;
            _parent.suSapUpdateLogDataGridView.RowsDefaultCellStyle.BackColor = GRID_ROW_BACK_COLOR;
            _parent.suSapUpdateLogDataGridView.RowTemplate.Height = GRID_ROW_HEIGHT;
            _parent.suSapUpdateLogDataGridView.AllowUserToResizeRows = false;
            //_parent.suSapUpdateLogDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _parent.suSapUpdateLogDataGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _parent.suSapUpdateLogDataGridView.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            _parent.suSapUpdateLogDataGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            _parent.suSapUpdateLogDataGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            _parent.suSapUpdateLogDataGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            //_parent.suSapUpdateLogDataGridView.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Each column style 
            //_parent.suTjButtListView.Columns[(int)eDailyTest_parent.suTjButtListView.ID].Visible = false;

            // Common style
            _parent.suSapUpdateLogDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _parent.suSapUpdateLogDataGridView.GridColor = GRID_COLOR;
            _parent.suSapUpdateLogDataGridView.BackgroundColor = GRID_BACK_COLOR;         // BackgroundColor 
            _parent.suSapUpdateLogDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // SelectionMode
            _parent.suSapUpdateLogDataGridView.MultiSelect = false;
            _parent.suSapUpdateLogDataGridView.ReadOnly = true;
            _parent.suSapUpdateLogDataGridView.ScrollBars = ScrollBars.Vertical;


            _parent.suSapUpdateLogDataGridView.Refresh();
        }
    }
}
