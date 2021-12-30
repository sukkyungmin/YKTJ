using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb; 
using System.Data;
using System.IO;

namespace RealTimeRMT
{
    class OleDbHelper
    {
        static public DataTable ImportExcel(string excelFilePath, string excelSheetName, string version) 
        {
            string connectionString = ""; 
            if (float.Parse(version) >= 12.0)
            {
                // 2007이상
                connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=No;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'", excelFilePath); 
            }
            else
            {
                // 97-2003
                connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=No'", excelFilePath); 
            }

            DataTable dataTable = null; 
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM [" + excelSheetName + "$]";
                OleDbDataReader dataReader = command.ExecuteReader();
                if (dataReader != null)
                {
                    dataTable = new DataTable();
                    dataTable.Load(dataReader); 
                }
                connection.Close(); 
            }
            return dataTable;
        }


        static public DataTable ImportCsv(string csvFilePath)
        {
            //OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + file.DirectoryName + "\";Extended Properties='text;HDR=No;FMT=Delimited(,)';"))
            OleDbConnectionStringBuilder connectionStringBuilder = new OleDbConnectionStringBuilder();
            connectionStringBuilder["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            connectionStringBuilder["Data Source"] = Path.GetDirectoryName(csvFilePath);
            connectionStringBuilder["Extended Properties"] = "Text;HDR=NO";

            DataTable dataTable = null;
            using (OleDbConnection connection = new OleDbConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(string.Format("Select * from {0}", Path.GetFileName(csvFilePath)), connection);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
                if (dataAdapter != null)
                {
                    dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                }
                connection.Close();
            }
            return dataTable; 
        }
    }
}
