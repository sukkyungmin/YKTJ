using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace WasteReport
{
    public class TextRW
    {
        /// <summary>
        /// 파일을 읽어서 리턴해준다.
        /// </summary>
        /// <param name="Location">파일 경로</param>
        /// <param name="FileName">파일 이름</param>
        /// <returns>리턴값이 "NOT FOLDER" = 폴더가 없다, "NOT FILE" = 파일이 없다, "ERROR" = 오류</returns>
        public string ReadTxt(string Location, string FileName)
        {
            lock (this)
            {
                string FileInfo = Location + FileName;

                if (!(Directory.Exists(Location))) return "NOT FOLDER";
                if (!(File.Exists(FileInfo))) return "NOT FILE";

                string Data = string.Empty;

                try
                {
                    using (FileStream fs = new FileStream(FileInfo, FileMode.Open, FileAccess.Read))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        Data = sr.ReadToEnd();

                        sr.Close();
                        fs.Close();

                        return Data;
                    }
                }
                catch
                {
                    Data = "ERROR";

                    return Data;
                }
            }
        }


        /// <summary>
        /// 파일을 읽어서 테이블에 써준다.
        /// </summary>
        /// <param name="Location">파일 경로</param>
        /// <param name="FileName">파일 이름</param>
        /// <param name="dt">값을 써줄 테이블</param>
        /// <param name="sub">구분자</param>
        /// <returns>성공시 true, 실패시 false</returns>
        public bool ReadTxtTable(string Location, string FileName, ref DataTable dt, char sub)
        {
            lock (this)
            {
                string FileInfo = Location + FileName;

                if (!(Directory.Exists(Location))) return false;
                if (!(File.Exists(FileInfo))) return false;

                try
                {
                    using (FileStream fs = new FileStream(FileInfo, FileMode.Open, FileAccess.Read))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        for (int cntRow = 0; !sr.EndOfStream; cntRow++)
                        {
                            string[] data = sr.ReadLine().Split(sub);

                            if (data.Length == 0 && data[0].Trim() == string.Empty)
                                break;

                            if (cntRow == 0)    //컬럼 생성
                            {
                                for (int cnt = 0; cnt < data.Length; cnt++) dt.Columns.Add(cnt.ToString());
                            }

                            dt.Rows.Add();

                            for (int cntCol = 0; cntCol < dt.Columns.Count; cntCol++) dt.Rows[cntRow][cntCol] = data[cntCol];
                        }
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 파일에 내용을 써준다.
        /// </summary>
        /// <param name="Location">파일 경로</param>
        /// <param name="FileName">파일 이름</param>
        /// <param name="Data">써줄 내용</param>
        /// <param name="Overwrite">이전 내용을 지우고 쓰는가 뒤에 이어서 쓰는가</param>
        /// <returns>성공시 true, 실패시 false</returns>
        public bool WriteTxt(string Location, string FileName, string Data, bool Overwrite)
        {
            lock (this)
            {
                string FileInfo = Location + FileName;

                if (!(Directory.Exists(Location)))
                    Directory.CreateDirectory(Location);

                FileStream fs = null;

                try
                {
                    if (Overwrite) fs = new FileStream(FileInfo, FileMode.Create, FileAccess.ReadWrite);
                    else fs = new FileStream(FileInfo, FileMode.Append, FileAccess.Write);

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(Data);

                        sw.Close();
                        fs.Close();

                        fs.Dispose();

                        return true;
                    }
                }
                catch
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }

                    return false;
                }
            }
        }


        /// <summary>
        /// 파일에 데이터 테이블 내용을 써준다.
        /// </summary>
        /// <param name="Location">파일 경로</param>
        /// <param name="FileName">파일 이름</param>
        /// <param name="Data">써줄 내용</param>
        /// <param name="sub">컬럼별 구분자</param>
        /// <param name="Overwrite">이전 내용을 지우고 쓰는가 뒤에 이어서 쓰는가</param>
        /// <returns>성공시 true, 실패시 false</returns>
        public bool WriteTxt(string Location, string FileName, DataTable Data, char sub, bool Overwrite)
        {
            lock (this)
            {
                string FileInfo = Location + FileName;

                if (!(Directory.Exists(Location)))
                    Directory.CreateDirectory(Location);

                FileStream fs = null;

                try
                {
                    if (Overwrite) fs = new FileStream(FileInfo, FileMode.Create, FileAccess.ReadWrite);
                    else fs = new FileStream(FileInfo, FileMode.Append, FileAccess.Write);

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        string PlusData = string.Empty;

                        for (int row = 0; row < Data.Rows.Count; row++)
                        {
                            for (int column = 0; column < Data.Columns.Count; column++)
                            {
                                PlusData += Data.Rows[row][column].ToString().Trim();
                                if (column != Data.Columns.Count - 1) PlusData += sub;
                                else PlusData += Environment.NewLine;
                            }
                            sw.Write(PlusData);
                            PlusData = string.Empty;
                        }

                        sw.Close();
                        fs.Close();

                        fs.Dispose();

                        return true;
                    }
                }
                catch
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }

                    return false;
                }
            }
        }
    }
}
