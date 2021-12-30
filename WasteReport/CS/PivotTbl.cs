using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace WasteReport.CS
{
    public class PivotTbl
    {
        public DataTable Pivot(
       DataTable src,
       string VerticalColumnName,
       string HorizontalColumnName,
       string ValueColumnName1,
       string ValueColumnName2)
        {

            DataTable dst = new DataTable();
            if (src == null || src.Rows.Count == 0)
                return dst;

            // find all distinct names for column and row
            ArrayList ColumnValues = new ArrayList();
            ArrayList RowValues = new ArrayList();
            foreach (DataRow dr in src.Rows)
            {
                // find all column values
                object column = dr[VerticalColumnName];
                if (!ColumnValues.Contains(column))
                    ColumnValues.Add(column);

                //find all row values
                object row = dr[HorizontalColumnName];
                if (!RowValues.Contains(row))
                    RowValues.Add(row);
            }

            ColumnValues.Sort();
            RowValues.Sort();

            //create columns
            dst = new DataTable();
            dst.Columns.Add(VerticalColumnName, src.Columns[VerticalColumnName].DataType);
            Type t = src.Columns[ValueColumnName1].DataType;
            Type t2 = src.Columns[ValueColumnName2].DataType;
            foreach (object ColumnNameInRow in RowValues)
            {
                dst.Columns.Add(ColumnNameInRow.ToString());
                //dst.Columns.Add(ColumnNameInRow.ToString(), t);
                //dst.Columns.Add(ColumnNameInRow.ToString(), t2);
            }
            
            

            //create destination rows
            foreach (object RowName in ColumnValues)
            {
                DataRow NewRow = dst.NewRow();
                NewRow[VerticalColumnName] = RowName.ToString();
                dst.Rows.Add(NewRow);
            }

            //fill out pivot table
            foreach (DataRow drSource in src.Rows)
            {
                object key = drSource[VerticalColumnName];
                string ColumnNameInRow = Convert.ToString(drSource[HorizontalColumnName]);
                
                int index = ColumnValues.IndexOf(key);
                double value1, value2;
                
                //dst.Rows[index][ColumnNameInRow] = sum(dst.Rows[index][ColumnNameInRow], drSource[ValueColumnName1]);
                //dst.Rows[index][ColumnNameInRow] = sum(dst.Rows[index][ColumnNameInRow], drSource[ValueColumnName2]);
                value1 = sum(dst.Rows[index][ColumnNameInRow], drSource[ValueColumnName1]);
                value2 = sum(dst.Rows[index][ColumnNameInRow], drSource[ValueColumnName2]);

                //dst.Rows[index][ColumnNameInRow] = value1.ToString() + " / " + value2.ToString();
                //MessageBox.Show ((string.Format("{0} / {1}", value1, value2)).ToString());
                //dst.Rows[0][ColumnNameInRow] = ColumnNameInRow.Substring(0, 10);
                dst.Rows[index][ColumnNameInRow] = (string.Format("{0} / {1}", value1, value2)).ToString();
            }

            return dst;
        }

        dynamic sum(dynamic a, dynamic b)
        {
            if (a is DBNull && b is DBNull)
                return DBNull.Value;
            else if (a is DBNull && !(b is DBNull))
                return b;
            else if (!(a is DBNull) && b is DBNull)
                return a;
            else
                return a + b;
        }
    }
}
