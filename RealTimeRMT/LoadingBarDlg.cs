using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealTimeRMT
{
    public partial class LoadingBarDlg : Form
    {
        public MainForm _parent;
        public LoadingBarDlg(MainForm parent)
        {
            InitializeComponent();
            this.TopMost = true;

            _parent = parent;

            loadingBarTimer.Interval = 300;
            loadingBarTimer.Start();
        }

        private void loadingBarTimer_Tick(object sender, EventArgs e)
        {
            if (_parent._isClosedLoadingBar == true)
            {
                loadingBarTimer.Stop();
                Close();
            }
        }
    }
}
