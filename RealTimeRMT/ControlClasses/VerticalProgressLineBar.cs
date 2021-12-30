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
    public partial class VerticalProgressLineBar : ProgressBar
    {
        /*
        private System.Timers.Timer _progressTimer = null;
        delegate void ProgressTimerDelegate();
        const int FRMAE_PER_SEC = 32;
        private int _sizePerFrame = 0;
        private int _progressValue = 0;
        */

        Brush barColor = new SolidBrush(Color.FromArgb(43, 157, 184));
        Pen barPen = new Pen(new SolidBrush(Color.FromArgb(55, 63, 71)));

        public int lineValue = 0;

        public VerticalProgressLineBar()
        {
            InitializeComponent();

            /*
            _progressTimer = new System.Timers.Timer();
            _progressTimer.Interval = 1000 / FRMAE_PER_SEC; // 32프레임/sec
            _sizePerFrame = this.Maximum / (FRMAE_PER_SEC / 2);
            _progressTimer.Elapsed += new System.Timers.ElapsedEventHandler(progressTimer_ElapsedEventHandler);
            InitProgressBar();
            t
            */

            this.SetStyle(ControlStyles.UserPaint, true);

            barPen.Width = 2F;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            /*
            Rectangle rec = pe.ClipRectangle;
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(pe.Graphics, pe.ClipRectangle);
            rec.Height = rec.Height - 4;
            pe.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(43, 157, 184)), 2, 2, rec.Width, rec.Height);
            */


            // 수직으로 처리하도록
            Rectangle rec = pe.ClipRectangle;
            //rec.Width = rec.Width - 4; 
            rec.Width = rec.Width;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(pe.Graphics, pe.ClipRectangle);
            //rec.Height = (int)(rec.Height * ((double)Value / Maximum)) - 4;
            rec.Height = (int)(rec.Height * ((double)Value / Maximum));
            pe.Graphics.FillRectangle(barColor, 0, pe.ClipRectangle.Height - rec.Height, rec.Width, rec.Height);

            // Line 그리기
            rec = pe.ClipRectangle;
            int realLineValue = (int)(rec.Height * ((double)lineValue / Maximum));
            if (realLineValue > 0 && realLineValue <= pe.ClipRectangle.Height)
            {
                pe.Graphics.DrawLine(barPen,
                    1, pe.ClipRectangle.Height - realLineValue,
                    rec.Width - 1, pe.ClipRectangle.Height - realLineValue);
            }
        }


        /*
        protected override CreateParams CreateParams
        {
            get
            {
                //return base.CreateParams;

                CreateParams createParams = base.CreateParams;
                createParams.Style |= 0x04;
                return createParams;
            }
        }
        */


        /*
    private void progressTimer_ElapsedEventHandler(object sender, System.Timers.ElapsedEventArgs e)
    {
        BeginInvoke(new ProgressTimerDelegate(progressTimer_DoWork));
    }

    private void progressTimer_DoWork()
    {
        if (this.Value <= (_progressValue + _sizePerFrame))
        {
            _progressTimer.Stop();
            _progressValue = this.Value;
        }
        else
        {
            _progressValue += _sizePerFrame;
        }

        Graphics g = this.CreateGraphics();

        Rectangle rec = this.ClientRectangle;
        //rec.Width = rec.Width - 4; 
        rec.Width = rec.Width;
        if (ProgressBarRenderer.IsSupported)
            ProgressBarRenderer.DrawHorizontalBar(g, this.ClientRectangle);
        //rec.Height = (int)(rec.Height * ((double)Value / Maximum)) - 4;
        rec.Height = (int)(rec.Height * ((double)Value / Maximum));
        g.FillRectangle(new SolidBrush(Color.FromArgb(43, 157, 184)), 0, this.ClientRectangle.Height - _progressValue, rec.Width, _progressValue);
    }

    private void InitProgressBar()
    {
        Graphics g = this.CreateGraphics();
        g.Clear(_backgroundColor);
        this.BackColor = _backgroundColor;
        int arcGap = _barWidth / 2;
        Pen backgroundPen = new Pen(_backgroundBarColor, _barWidth);
        g.DrawArc(backgroundPen, arcGap, arcGap, this.Width - (arcGap * 2), (this.Height * 2) - (arcGap * 2), 180, 180);
    }

    public void StartAnimation(int value)
    {
        this.Value = value;
        _progressTimer.Start(); 
    }
    */
    }
}
