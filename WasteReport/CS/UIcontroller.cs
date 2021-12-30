using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WasteReport.CS
{
    class UIcontroller
    {
        public delegate void D_CloseForm(Form frm);
        public void CloseForm(Form frm)
        {
            try
            {
                frm.Close();
            }
            catch (Exception ex) { ex.ToString(); }
        }
    }
}
