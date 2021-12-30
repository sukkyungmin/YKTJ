using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace RealTimeRMT
{
    class DbHelper
    {
        // 시연 테스트
        static public string _ip = "(local)";
        static public string _id = "sa";
        static public string _pw = "hgfa7180";

        //static public string _connectionString = "Server=127.0.0.1;Database=RealTimeRMT;Uid=sa;Pwd=skekdns7;";
        //static public string _connectionString = "Server=127.0.0.1;Database=RealTimeRMT;Uid=sa;Pwd=hgfa7!@34;"; // HGFA 사내 테스트 노트북
        //static public string _connectionString = "Server=10.23.93.41;Database=RealTimeRMT;Uid=sa;Pwd=hgfa7180!@;";  // 유한킴벌리 테스트 서버
        static public string _connectionString = "Server=Kotjmfgapp3.mosaic.sys;Database=RealTimeRMT;Uid=KOTJRMTADM;Pwd=Admin@123456";  // 유한킴벌리 실서버


        static public void SetConnectionString(string dbPassword)
        {
            _connectionString = string.Format("Server={0};Database=RealTimeRMT;Uid={1};Pwd={2};", _ip, _id, _pw);
            //_connectionString = string.Format("Server=127.0.0.1;Database=RealTimeRMT;Uid=sa;Pwd={0}", dbPassword);  // 테스트 서버 

            //_connectionString = string.Format("Server=Kotjmfgapp3.mosaic.sys;Database=RealTimeRMT;Uid=KOTJRMTADM;Pwd={0}", dbPassword);  // 유한킴벌리 실서버
        }

        static public int ExecuteNonQuery(string query)
        {
            int retVal = -1;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    retVal = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.Write(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }
            return retVal;
        }


        static public int ExecuteNonQueryWithFileData(string query, SqlParameter fileParam)
        {
            int retVal = -1;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add(fileParam);
                    retVal = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.Write(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }
            return retVal;
        }

        static public DataSet SelectQuery(string query)
        {

            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(query, connection);
                    //adpt.SelectCommand.CommandTimeout = 120;
                    adpt.Fill(dataSet, "List");
                }
                catch (SqlException ex)
                {
                    Console.Write(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return dataSet;
        }

        static public string GetValue(string query, string field, string defaultValue)
        {
            string retVal = "";
            DataSet dataSet = DbHelper.SelectQuery(query);
            if (dataSet != null)
            {
                DataTableCollection collection = dataSet.Tables;
                if (collection.Count > 0)
                {
                    DataTable table = collection[0];
                    if (table.Rows.Count > 0)
                    {
                        DataRow dataRow = table.Rows[0];
                        retVal = (System.DBNull.Value == dataRow[field]) ? defaultValue : dataRow[field].ToString();
                    }
                    else
                    {
                        retVal = defaultValue;
                    }

                }
            }

            return retVal;
        }
    }
}
