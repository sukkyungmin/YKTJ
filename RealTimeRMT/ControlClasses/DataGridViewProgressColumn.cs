using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

/**
 * 아래 링크 참조
 * http://stackoverflow.com/questions/4646920/populating-a-datagridview-with-text-and-progressbars
 */

namespace RealTimeRMT.ControlClasses
{
    public class DataGridViewProgressColumn : DataGridViewImageColumn
    {
        public DataGridViewProgressColumn()
        {
            CellTemplate = new DataGridViewProgressCell();
        }
    }
}

namespace RealTimeRMT.ControlClasses
{
    class DataGridViewProgressCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        static Image emptyImage;
        static DataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public DataGridViewProgressCell()
        {
            this.ValueType = typeof(int);
        }
        // Method required to make the Progress Cell consistent with the default Image Cell. 
        // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
        protected override object GetFormattedValue(object value,
                            int rowIndex, ref DataGridViewCellStyle cellStyle,
                            TypeConverter valueTypeConverter,
                            TypeConverter formattedValueTypeConverter,
                            DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }
        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            try
            {
                //int progressBgValue = 100;
                int progressVal = (int)value;
                float percentage = ((float)progressVal / 100.0f); // Need to convert to float before division; otherwise C# returns int which is 0 for anything but 100%.
                //Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
                //Brush backColorBrush = new SolidBrush(Color.FromArgb(215, 215, 214));
                Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
                // Draws the cell grid
                base.Paint(g, clipBounds, cellBounds,
                 rowIndex, cellState, value, formattedValue, errorText,
                 cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));


                int topBottomGap = 10;
                //int textWidth = 50;
                int textWidth = 0; // 별도 컬럼으로 뺌
                int progressBarWidth = Convert.ToInt32((1.0 * (cellBounds.Width - textWidth) - 4));
                g.FillRectangle(new SolidBrush(Color.FromArgb(215, 215, 214)), cellBounds.X + 2, cellBounds.Y + topBottomGap, progressBarWidth, cellBounds.Height - (topBottomGap * 2));
                 

                if (percentage > 0.0)
                {
                    // Draw the progress bar and the text
                    /*
                    g.FillRectangle(new SolidBrush(Color.FromArgb(42, 159, 184)), cellBounds.X + 2, cellBounds.Y + 8, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 16);
                    g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + (cellBounds.Width / 2) - 5, cellBounds.Y + 2);
                    */ 

                    /*
                    int topBottomGap = 10;
                    int textWidth = 50; 
                    */
                    progressBarWidth = Convert.ToInt32((percentage * (cellBounds.Width - textWidth) - 4));
                    g.FillRectangle(new SolidBrush(Color.FromArgb(42, 159, 184)), cellBounds.X + 2, cellBounds.Y + topBottomGap, progressBarWidth, cellBounds.Height - (topBottomGap * 2));
                    //g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + cellBounds.Width - (textWidth - 2), cellBounds.Y + topBottomGap + 2);

                }
                else
                {
                    // draw the text
                    /*
                    if (this.DataGridView.CurrentRow.Index == rowIndex)
                        g.DrawString(progressVal.ToString() + "%", cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6, cellBounds.Y + 2);
                    else
                        g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
                        */
                    /*
                    int topBottomGap = 10;
                    int textWidth = 50; 
                    */
                    //g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + cellBounds.Width - (textWidth - 2), cellBounds.Y + topBottomGap + 2);
                     
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }
    }
}
