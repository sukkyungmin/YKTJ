using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealTimeRMT.ControlClasses
{
    public partial class CustomProgressBar : ProgressBar
    {
        Brush barColor = new SolidBrush(Color.FromArgb(43, 157, 184)); 
        public CustomProgressBar()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Rectangle rec = pe.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(pe.Graphics, pe.ClipRectangle);
            rec.Height = rec.Height - 4;
            pe.Graphics.FillRectangle(barColor, 2, 2, rec.Width, rec.Height);
        }
    }
}
