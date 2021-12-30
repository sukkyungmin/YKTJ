using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WasteReport.UserCons;
using System.Threading;
using WasteReport.CS;

namespace WasteReport
{
    public partial class LoadingForm : Form
    {
        Con_Prod con_prod = null;
        private Thread thrd_check = null;
        private bool bl_done = false;

        public LoadingForm(Con_Prod _conProd)
        {
            InitializeComponent();

            con_prod = _conProd;
        }

        public void StartThrd()
        {
            thrd_check = new Thread(new ThreadStart(RunCheckAndClose));
            thrd_check.Start();
        }

        private void RunCheckAndClose()
        {
            while (!bl_done)
            {
                if (con_prod.bl_finish)
                {
                    Invoke(new UIcontroller.D_CloseForm(new UIcontroller().CloseForm), new object[] { this });
                    bl_done = true;
                }

                Thread.Sleep(300);
            }
        }

       private void LoadingForm_FormClosed(object sender, FormClosedEventArgs e)
       {
           thrd_check.Abort();
       }

    }
}
