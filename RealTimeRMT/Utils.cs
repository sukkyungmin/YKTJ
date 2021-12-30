using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Runtime.InteropServices;


namespace RealTimeRMT
{
    class Utils
    {
        /**
         * 엔터키 체크
         */
        static public bool IsReturnKey(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                return true;
            return false;
        }

        /**
         * 지정한 파일이 사용중인지 확인한다. 
         */
        static public bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                if (file.Exists == true)
                {
                    stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is: 
                //still being written to 
                //or being processed by another thread 
                //or does not exist (has already been processed) 
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //file is not locked 
            return false;
        }

        /**
         * 주어진 문자열이 숫자인지 확인
         */
        static public bool IsDigit(string data)
        {
            long number = 0;
            return long.TryParse(data, out number);
        }

        /**
         * 주어진 문자열에서 0-9 까지의 숫자만 가져온다. 
         * 이름 변경 필요
         */
        static public string GetDigitFromString(string data)
        {
            return Regex.Replace(data, "[^0-9]", "", RegexOptions.Singleline);
        }

        /**
         * 주어진 문자열에서 0-9 까지의 숫자와 소수점만 가져온다.
         * 이름 변경 필요
         */
        static public string GetDecimalFromString(string data)
        {
            return Regex.Replace(data, "[^0-9.]", "", RegexOptions.Singleline);
        }

        /**
         * DBMS에서 허용되지 않는 문자를 변경한다. 
         * MySQL, SQLite, MSSQL에서 테스트 됐다. 
         * 점진적으로 개선 필요
         */
        static public string ReplaceSpecialChar(string data)
        {
            return data.Replace("'", "''");
        }

        /**
         * 세자리마다 콤마 삽입
         */
        static public string SetComma(string value)
        {
            if (value.Trim() == "")
                return value;

            string digit = value.Replace(",", "");
            Int64 i = 0;
            if (Int64.TryParse(digit, out i) == false)
                return value;

            return string.Format("{0:N0}", Convert.ToInt64(digit));
        }

        /**
         * 0~9까지의 숫자만 허용
         */
        static public void AcceptOnlyDigit(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /**
         * 소수만 허용
         */
        static public void AcceptOnlyDecimal(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !(char.IsDigit(e.KeyChar) || '.' == e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /**
         * 세자리마다 콤마 삽입
         * TextBox의 객체를 파리미터로 받아서 TextBox의 값 자체를 수정한다. 
         */
        static public void SetComma(object sender)
        {
            if ((sender as TextBox).Text.Trim() != "")
            {
                string digit = (sender as TextBox).Text.Replace(",", "");
                Int64 i = 0;
                if (Int64.TryParse(digit, out i) == false)
                    return;

                (sender as TextBox).Text = string.Format("{0:N0}", Convert.ToInt64(digit));
                (sender as TextBox).SelectionStart = (sender as TextBox).TextLength;
                (sender as TextBox).SelectionLength = 0;
            }
        }

        /** 
         * 지정한 소수 자리에서 반올림 한 값을 리턴한다. 
         */
        static public string GetValueRoundOff(string value, int decimalPlaces)
        {
            return string.Format("{0:F" + decimalPlaces.ToString() + "}", Convert.ToDouble(value));
        }

        /**
         * 입력된 문자열의 자리수만큼 * 문자로 변경한다.  
         */
        static public string ReplacePasswordToAsterisk(string data)
        {
            string asterisk = ""; 
            int len = data.Length; 
            //return Regex.Replace(data, "", "*", RegexOptions.Singleline);
            for (int i = 0; i < len; i++)
                asterisk += "*";

            return asterisk;
        }


        /**
         * Image to Byte Array
         * http://www.codeproject.com/Articles/15460/C-Image-to-Byte-Array-and-Byte-Array-to-Image-Conv
         */
        static public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        /**
         * Byte Array to Image
         * http://www.codeproject.com/Articles/15460/C-Image-to-Byte-Array-and-Byte-Array-to-Image-Conv
         */
        static public System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null)
                return null; 

            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        /**
         * 영문자만 허용
         */
        static public void AcceptOnlyAlphaChar(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122)) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        /**
         * 단순 스트링 반환, null 값에 대한 예외처리
         */
        static public string GetString(object value)
        {
            if (value == null)
                return "";
            else
                return value.ToString();
        }
        /**
                 * 입력된 값으로부터 double 값을 가져온다.
                 */
        static public double GetDoubleValue(string data)
        {
            string valueStr = GetDecimalString(data);
            double value = 0.0d;
            if (valueStr != "")
                value = Double.Parse(valueStr);

            return value;
        }
        static public double GetDoubleValue(object data)
        {
            if (data == null)
                return 0.0d;

            return GetDoubleValue(data.ToString());
        }

        /**
          * 주어진 문자열에서 0-9 까지의 숫자와 소수점만 가져온다.
          * 음수에 대한 보완이 필요하다. 일단 무식한 방법으로다. 
          * 1개 이상의 소수에 대한 보완이 필요하다. 
          */
        static public string GetDecimalString(string data)
        {
            if (data == null || data.Trim() == "")
                return "";

            string minus = "";
            if (data.Trim().Substring(0, 1) == "-")
                minus = "-";

            return (minus + Regex.Replace(data, "[^0-9.]", "", RegexOptions.Singleline));
        }

        /**
 * 주어진 문자열에서 int형을 리턴한다. 공백일 경우, 0을 리턴한다. 
 */
        static public int GetIntValue(string data)
        {
            string valueStr = Regex.Replace(data, "[^0-9]", "", RegexOptions.Singleline);
            int value = 0;
            if (valueStr != "")
                value = Int32.Parse(valueStr);

            return value;
        }

        /**
         * parent 영역 안에 target 영역에 커서가 있는지 검사한다.
         */
        static public bool InArea(Control target, Control parent)
        {
            Point cr = parent.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));

            //Console.WriteLine("parent : " + cr.X.ToString() + " " + cr.Y.ToString());
            //Console.WriteLine("target : " + target.Left.ToString() + " " + target.Top.ToString());
            if (target.Bounds.Contains(cr))
                return true;
            else
                return false; 

        }

        /**
       * Read ini
       */
        static public String GetIniValue(String Section, String Key, String iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
            return temp.ToString();
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
           string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
    } 
}
