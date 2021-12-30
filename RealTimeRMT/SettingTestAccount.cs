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
    public partial class SettingTestAccount : Form
    {
        public LoginForm _parent = null;
        
        public SettingTestAccount(LoginForm parent)
        {
            InitializeComponent();

            _parent = parent;
            ip.Text = DbHelper._ip; 
            id.Text = DbHelper._id; 
            pw.Text = DbHelper._pw; 

        }

        private void confirm_Click(object sender, EventArgs e)
        {
            if (ip.Text.Trim() == "" ||
                id.Text.Trim() == "" ||
                pw.Text.Trim() == "")
            {
                DialogResult result = MessageBox.Show("모든 정보를 입력하지 않았습니다. 기본 정보로 DB에 연결하시겠습니까?", "DB 연결정보 입력", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
                else
                    return; 
            }

            _parent._ip = ip.Text.Trim();       
            _parent._id = id.Text.Trim();       
            _parent._pw = pw.Text.Trim();
            DialogResult = DialogResult.OK; 
            Close();
        }
    }
}
