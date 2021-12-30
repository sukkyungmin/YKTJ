using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace WasteReport
{
    public enum DB_TYPE { MS_SQL, ORACLE, MDB };

    public class DB : IDisposable
    {
        #region 메모리 해제 함수
        private bool Disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool bManage)
        {
            if (Disposed == true) return;
            Disposed = true;

            if (bManage == true)
            {
            }

            try
            {
                if (m_Con != null)
                {
                    if (m_Con.State == ConnectionState.Open) m_Con.Close();
                    m_Con.Dispose();
                }
            }
            catch { }
        }
        ~DB()
        {
            Dispose(false);
        }
        #endregion 메모리 해제 함수

        ////멤버 변수 선언.
        public OleDbConnection m_Con = null;


        /// <summary>
        /// DB 접속 초기 설정 함수
        /// </summary>
        /// <param name="_db_type">MS_SQL 인가 ORACLE 인가</param>
        /// <param name="_ip">DB IP</param>
        /// <param name="_id">DB ID</param>
        /// <param name="_pass">DB 비밀번호</param>
        /// <param name="_catalog">MS_SQL 만 해당 카달로그</param>
        public void InitializeDB(DB_TYPE _db_type, string _ip, string _id, string _pass, string _catalog)
        {
            string conInfo = string.Empty;

            try
            {
                if (_db_type == DB_TYPE.MS_SQL)    //SQL
                {
                    conInfo = "Provider=SQLOLEDB.1;Password={0};Persist Security Info=True;User ID={1};Data Source={2};Initial Catalog={3}";
                    conInfo = string.Format(conInfo, _pass, _id, _ip, _catalog);
                }
                else if (_db_type == DB_TYPE.ORACLE)   //ORACLE
                {
                    //conInfo = "Provider=MSDAORA.1;Password={0};Persist Security Info=True;User ID={1};Data Source={2}";
                    conInfo = "Provider=OraOLEDB.Oracle.1;Password={0};Persist Security Info=True;User ID={1};Data Source={2}";
                    conInfo = string.Format(conInfo, _pass, _id, _ip);
                }
                else if (_db_type == DB_TYPE.MDB)
                {       //Provider=Microsoft.Jet.OLEDB.4.0;Data Source = 엑세스 파일위치
                    //conInfo = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = {0}";
                    conInfo = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = {0}";
                    conInfo = string.Format(conInfo, _catalog);
                }

                if (m_Con != null) m_Con.Dispose();

                m_Con = new OleDbConnection();

                m_Con.ConnectionString = conInfo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("DB 접속에러 MDB 파일을 확인해주세요 | 에러 원문 : {0}", ex.ToString()));
                LOG_Write(string.Format("함수명 InitDB 에러 {1} |원문 {2}|", DateTime.Now.ToString(), ex.Message, conInfo));
            }
        }


        /// <summary>
        /// DB 접속 시도 함수
        /// </summary>
        /// <returns>성공시 true 리턴 실패시 false 리턴</returns>
        public bool ConnectDB()
        {
            try
            {
                lock (this)
                {
                    if (GetDBState() != ConnectionState.Open)
                        m_Con.Open();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("DB 접속에러 MDB 경로를 확인해주세요 | 에러 원문 : {0}", ex.ToString()));
                LOG_Write(string.Format("함수명 DBConnect 에러 {1}", DateTime.Now.ToString(), ex.Message));

                return false;
            }
        }


        /// <summary>
        /// DB 종료 함수
        /// </summary>
        /// <returns>성공시 true 리턴 실패시 false 리턴</returns>
        public bool DisconnectDB()
        {
            try
            {
                lock (this)
                {
                    if (GetDBState() == ConnectionState.Open)
                        m_Con.Close();

                    m_Con.Dispose();

                    return true;
                }
            }
            catch (Exception ex)
            {
                LOG_Write(string.Format("함수명 DBDisConnect 에러 {1}", DateTime.Now.ToString(), ex.Message));

                m_Con.Dispose();

                return false;
            }
        }


        /// <summary>
        /// DB 상태를 읽어오는 함수
        /// </summary>
        /// <returns>DB 의 현재 상태</returns>
        public ConnectionState GetDBState()
        {
            try
            {
                lock (this)
                {
                    if (m_Con != null)
                        return m_Con.State;
                    else return ConnectionState.Closed;
                }
            }
            catch (Exception ex)
            {
                LOG_Write(string.Format("함수명 GetDBState 에러 {1}", DateTime.Now.ToString(), ex.Message));
                return ConnectionState.Closed;
            }
        }


        /// <summary>
        /// DB 에서 query 문을 실행해주는 함수
        /// </summary>
        /// <param name="query">실행해줄 쿼리 문장</param>
        /// <returns>성공시 true 리턴 실패시 false 리턴</returns>
        public bool Runquery(string query)
        {
            if ((m_Con.ConnectionString == null) || (m_Con.ConnectionString == string.Empty))
            {
                LOG_Write(string.Format("DB 접속 초기화가 없습니다.", DateTime.Now.ToString(), query));

                return false;
            }

            if (GetDBState() != ConnectionState.Open)
            {
                LOG_Write(string.Format("함수명 Runquery DB접속이 안되있습니다. |원문 {1}|", DateTime.Now.ToString(), query));

                return false;
            }

            lock (this)
            {
                for (int Err_cnt = 0; Err_cnt < 2; Err_cnt++)
                {
                    try
                    {
                        using (OleDbCommand Com = new OleDbCommand())
                        {
                            Com.Connection = m_Con;

                            Com.CommandType = CommandType.Text;

                            Com.CommandText = query;

                            Com.ExecuteNonQuery();

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LOG_Write(string.Format("함수명 Runquery 에러 {1} |원문 {2}|", DateTime.Now.ToString(), ex.Message, query));
                    }
                    LOG_Write(string.Format("쿼리 에러로 다시 실행", DateTime.Now.ToString()));

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
                }

                LOG_Write(string.Format("쿼리 모두다 실패", DateTime.Now.ToString()));
                return false;
            }
        }


        /// <summary>
        /// DB 에서 query 문을 실행해 DataTable 로 리턴해주는 함수
        /// </summary>
        /// <param name="dt">SELECT 된 값을 받을 데이터 테이블</param>
        /// <param name="query">실행해줄 쿼리 문장</param>
        /// <returns>성공시 true 리턴 실패시 false 리턴</returns>
        public bool Runquery(ref DataTable dt, string query)
        {
            if ((m_Con.ConnectionString == null) || (m_Con.ConnectionString == string.Empty))
            {
                LOG_Write(string.Format("DB 접속 초기화가 없습니다.", DateTime.Now.ToString(), query));

                return false;
            }

            if (GetDBState() != ConnectionState.Open)
            {
                LOG_Write(string.Format("함수명 Runquery DB접속이 안되있습니다. |원문 {1}|", DateTime.Now.ToString(), query));

                return false;
            }

            lock (this)
            {
                for (int Err_cnt = 0; Err_cnt < 2; Err_cnt++)
                {
                    try
                    {
                        using (OleDbCommand Com = new OleDbCommand())
                        {
                            Com.Connection = m_Con;

                            Com.CommandType = CommandType.Text;

                            Com.CommandText = query;

                            using (OleDbDataAdapter DB_ADT = new OleDbDataAdapter(Com))
                                DB_ADT.Fill(dt);

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LOG_Write(string.Format("함수명 Runquery 에러 {1} |원문 {2}|", DateTime.Now.ToString(), ex.Message, query));
                    }
                    LOG_Write(string.Format("쿼리 에러로 다시 실행", DateTime.Now.ToString()));

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
                }

                LOG_Write(string.Format("쿼리 모두다 실패", DateTime.Now.ToString()));
                return false;
            }
        }


        /// <summary>
        /// DB 에서 query 문을 실행해 string 으로 리턴해주는 함수
        /// </summary>
        /// <param name="query">실행 해줄 query 값</param>
        /// <returns>SELECT 된 문자 값</returns>
        public string Selectquery(string query)
        {
            if ((m_Con.ConnectionString == null) || (m_Con.ConnectionString == string.Empty))
            {
                LOG_Write(string.Format("DB 접속 초기화가 없습니다.", DateTime.Now.ToString(), query));

                return string.Empty;
            }

            if (GetDBState() != ConnectionState.Open)
            {
                LOG_Write(string.Format("함수명 Selectquery DB접속이 안되있습니다. |원문 {1}|", DateTime.Now.ToString(), query));

                return string.Empty;
            }

            lock (this)
            {
                for (int Err_cnt = 0; Err_cnt < 2; Err_cnt++)
                {
                    try
                    {
                        using (OleDbCommand Com = new OleDbCommand())
                        {
                            Com.Connection = m_Con;

                            Com.CommandType = CommandType.Text;

                            Com.CommandText = query;

                            if (Com.ExecuteScalar() == null)
                                return string.Empty;

                            return Com.ExecuteScalar().ToString().Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        LOG_Write(string.Format("함수명 Selectquery 에러 {1} |원문 {2}|", DateTime.Now.ToString(), ex.Message, query));
                    }
                    LOG_Write(string.Format("쿼리 에러로 다시 실행", DateTime.Now.ToString()));

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
                }

                LOG_Write(string.Format("쿼리 모두다 실패", DateTime.Now.ToString()));
                return string.Empty;
            }
        }


        public void RunProcedure(string procName, string[] columns, string[] value)
        {
            if ((m_Con.ConnectionString == null) || (m_Con.ConnectionString == string.Empty))
            {
                LOG_Write(string.Format("DB 접속 초기화가 없습니다.", DateTime.Now.ToString()));

                return;
            }

            if (GetDBState() != ConnectionState.Open)
            {
                LOG_Write(string.Format("함수명 RunProcedure DB접속이 안되있습니다. |원문 {1}|", DateTime.Now.ToString()));

                return;
            }

            lock (this)
            {
                for (int Err_cnt = 0; Err_cnt < 2; Err_cnt++)
                {
                    try
                    {
                        using (OleDbCommand Com = new OleDbCommand())
                        {
                            Com.Connection = m_Con;

                            Com.CommandType = CommandType.StoredProcedure;

                            Com.CommandText = procName;

                            for (int cnt = 0; cnt < value.Length; cnt++)
                            {
                                OleDbParameter pin = new OleDbParameter(columns[cnt], value[cnt]);
                                Com.Parameters.Add(pin);
                            }

                            Com.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        LOG_Write(string.Format("함수명 RunProcedure 에러 {1} |원문 {2}|", DateTime.Now.ToString(), ex.Message, procName));
                    }
                    LOG_Write(string.Format("쿼리 에러로 다시 실행", DateTime.Now.ToString()));

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
                }

                LOG_Write(string.Format("쿼리 모두다 실패", DateTime.Now.ToString()));
                return;
            }
        }

        public bool RunProcedure(ref DataTable dt, string procName, string[] columns, string[] value)
        {
            if ((m_Con.ConnectionString == null) || (m_Con.ConnectionString == string.Empty))
            {
                LOG_Write(string.Format("DB 접속 초기화가 없습니다.", DateTime.Now.ToString()));

                return false;
            }

            if (GetDBState() != ConnectionState.Open)
            {
                LOG_Write(string.Format("함수명 RunProcedure DB접속이 안되있습니다. |원문 {1}|", DateTime.Now.ToString()));

                return false;
            }

            lock (this)
            {
                for (int Err_cnt = 0; Err_cnt < 2; Err_cnt++)
                {
                    try
                    {
                        using (OleDbCommand Com = new OleDbCommand())
                        {
                            Com.Connection = m_Con;

                            Com.CommandType = CommandType.StoredProcedure;

                            Com.CommandText = procName;

                            for (int cnt = 0; cnt < value.Length; cnt++)
                            {
                                OleDbParameter pin = new OleDbParameter(columns[cnt], value[cnt]);
                                Com.Parameters.Add(pin);
                            }

                            using (OleDbDataAdapter DB_ADT = new OleDbDataAdapter(Com))
                                DB_ADT.Fill(dt);
                                return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LOG_Write(string.Format("함수명 RunProcedure 에러 {1} |원문 {2}|", DateTime.Now.ToString(), ex.Message, procName));
                    }
                    LOG_Write(string.Format("쿼리 에러로 다시 실행", DateTime.Now.ToString()));

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
                }

                LOG_Write(string.Format("쿼리 모두다 실패", DateTime.Now.ToString()));
                return false;
            }
        }


        private void LOG_Write(string _log)
        {
            string Location = string.Format("./LOG/{0}/{1}/", DateTime.Now.Year, DateTime.Now.Month);

            string FileInfo = Location + string.Format("{0}_DBErr.TXT", DateTime.Now.Day.ToString());

            if (!(Directory.Exists(Location)))
                Directory.CreateDirectory(Location);

            for (int cnt = 0; cnt < 2; cnt++)
            {
                lock (this)
                {
                    try
                    {
                        using (FileStream fs = new FileStream(FileInfo, FileMode.Append, FileAccess.Write))
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            _log = string.Format("{0}///{1}", DateTime.Now.ToString(), _log);

                            sw.WriteLine(_log);

                            sw.Close();
                            fs.Close();

                            return;
                        }
                    }
                    catch { }
                }

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
        }
    }
}
