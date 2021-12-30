using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WasteReport.CS;

namespace WasteReport.UserCons
{
    public partial class Con_Main : UserControl
    {
        MainForm mainForm;
        MDBmanager dcon_유아1부 = new MDBmanager("유아1부");
        WasteSearch ws = new WasteSearch();

        public Con_Main(MainForm mf)
        {
            InitializeComponent();

            mainForm = mf;

            string strSdate;
            DateTime dtSdate;
            strSdate = DateTime.Today.ToString("yyyy-MM-dd");
            dtSdate = Convert.ToDateTime(strSdate);
            dtp_start.Value = dtSdate;

            string strEdate;
            DateTime dtEdate;
            strEdate = DateTime.Today.ToString("yyyy-MM-dd");
            dtEdate = Convert.ToDateTime(strEdate);
            dtp_end.Value = dtEdate;

            dtp_start.ShowUpDown = true;
            dtp_end.ShowUpDown = true;

            cb_machines.SelectedIndex = 0;
            cb_shifts.SelectedIndex = 0;
            cb_dp.SelectedIndex = 2;
        }

        private void Con_Main_Load(object sender, EventArgs e)
        {
            SetWstGrpLb(dcon_유아1부);
        }

        

        //웨이스트 그룹 리스트 박스 세팅
        //private void SetWstGrpLb(MDBmanager dcon)
        //{
        //    lb_groups.Items.Clear();
        //    lb_groups.DisplayMember = "NAME";
        //    lb_groups.SelectedValue = "ID";
        //    ComboSetting("*", "*", lb_groups);

        //    DataTable dt_temp = new DataTable();

        //    dt_temp = dcon.GetWstGrp();

        //    if (dt_temp.Rows.Count > 0)
        //    {
        //        //DataTable dt = dbTable.Copy();
        //        DataView view = new DataView(dt_temp);
        //        DataTable distinctValues = view.ToTable(true, "WasteGroup");

        //        foreach (DataRow f_dr in distinctValues.Rows)
        //        {
        //            if (f_dr["WasteGroup"].ToString() != "")
        //            {
        //                ComboSetting(f_dr["WasteGroup"].ToString().Trim(), f_dr["WasteGroup"].ToString().Trim(), lb_groups);
        //            }
        //        }
        //    }

        //    lb_groups.SelectedIndex = 0;
        //}

        private void SetWstGrpLb(MDBmanager dcon)
        {
            lb_groups.Items.Clear();
            lb_groups.Items.Add("*");

            //DataTable dt_temp = new DataTable();

            //dt_temp = dcon.GetWstGrp();

            //if (dt_temp.Rows.Count > 0)
            //{
            //    //DataTable dt = dbTable.Copy();
            //    DataView view = new DataView(dt_temp);
            //    DataTable distinctValues = view.ToTable(true, "WasteGroup");

            //    foreach (DataRow f_dr in distinctValues.Rows)
            //    {
            //        if (f_dr["WasteGroup"].ToString() != "")
            //        {
            //            lb_groups.Items.Add(f_dr["WasteGroup"].ToString().Trim());
            //        }
            //    }
            //}

            lb_groups.SelectedIndex = 0;
        }

        //검색조건을 건네주기 위해 SearchClass 프로퍼티에 정보 할당
        private void SetSearchCond()
        {
            try
            {
                ws.bl_wasteCd = chb_wcTop.Checked;
                ws.bl_wasteGp = chb_wgTop.Checked;
                ws.bl_wgDdown = chb_wgDdown.Checked;

                ws.wcTop = cb_dp.SelectedItem.ToString();
                //ws.wgTop = cb_dp.SelectedItem.ToString();
                ws.machine = cb_machines.SelectedItem.ToString();
                //ws.group = ((ComboData)lb_groups.SelectedItem).NAME;
                ws.group = lb_groups.SelectedItem.ToString();
                ws.shift = cb_shifts.SelectedItem.ToString();
                ws.staDate = dtp_start.Value.ToShortDateString();
                ws.endDate = dtp_end.Value.ToShortDateString();
            }
            catch { }
        }


        //private void ComboSetting
        //       (string i_View, string i_value, ListBox i_cbBox)
        //{
        //    ComboData m_tempCd = new ComboData(i_View, i_value);
        //    i_cbBox.Items.Add(m_tempCd);

        //}

        //public class ComboData
        //{
        //    public string NAME { get; set; }
        //    public string ID { get; set; }
        //    public ComboData(string i_Id, string i_Name)
        //    {
        //        ID = i_Id;
        //        NAME = i_Name;

        //    }
        //}

        //Data Point 콤보 값 바뀌었을 때
        private void cb_dp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lbl_wcTop.Text = cb_dp.SelectedItem.ToString();
            //lbl_wgTop.Text = cb_dp.SelectedItem.ToString();
        }

        //Machines 콤보 값 바뀌었을 때
        private void cb_machines_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cb_machines.SelectedItem.ToString() == "유아1부")
            //{
            //    SetWstGrpLb(dcon_유아1부);
            //}
            //else
            //{
            //    SetWstGrpLb(dcon_TJ02);
            //}
            switch (cb_machines.SelectedItem.ToString())
            {
                case "유아1부":
                    SetWstGrpLb(dcon_유아1부);
                    break;
            }
        }

        private void lbl_wcTop_Click(object sender, EventArgs e)
        {
            if (chb_wcTop.Checked)
            {
                chb_wcTop.Checked = false;
            }
            else
            {
                chb_wcTop.Checked = true;
            }
        }

        private void lbl_wgTop_Click(object sender, EventArgs e)
        {
            if (chb_wgTop.Checked)
            {
                chb_wgTop.Checked = false;
            }
            else
            {
                chb_wgTop.Checked = true;
            }
        }

        //데일리 버튼 클릭
        private void btn_daily_Click(object sender, EventArgs e)
        {
            if(lb_groups.SelectedIndex == 0 )
            {
                MessageBox.Show("해당 모드는 Groups에서 모듈을 선택해주시기 바랍니다","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                lb_groups.Focus();
            }
            else
            {
                tlp_main.Focus();
                SetSearchCond();
                mainForm.GoToDaily(ws);
            }
        }

        //조회 버튼 클릭
        private void btn_search_Click(object sender, EventArgs e)
        {
            tlp_main.Focus();
            SetSearchCond();
            mainForm.GoToSub(ws);
        }

        //차트 버튼 클릭시
        private void btn_chart_Click(object sender, EventArgs e)
        {
            tlp_main.Focus();
            SetSearchCond();
            mainForm.GoToChart(ws);
        }

        //리셋 버튼 클릭시
        private void btn_reset_Click(object sender, EventArgs e)
        {
           
            chb_wcTop.Checked = false;
            chb_wgDdown.Checked = false;
            chb_wgTop.Checked = false;

            lb_groups.SelectedIndex = 0;
            cb_dp.SelectedIndex = 2;
            cb_shifts.SelectedIndex = 0;

            string strNdate;
            DateTime dtNdate;
            strNdate = DateTime.Today.ToString("yyyy-MM-dd");
            dtNdate = Convert.ToDateTime(strNdate);
            dtp_start.Value = dtNdate;
            dtp_end.Value = dtNdate;
        }

        private void btn_production_Click(object sender, EventArgs e)
        {
            tlp_main.Focus();
            SetSearchCond();
            mainForm.GoToProd(ws);
        }

        private void lb_groups_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;
            Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ?
                          new SolidBrush(Color.FromArgb(190, 187, 180)) : new SolidBrush(e.BackColor);
            g.FillRectangle(brush, e.Bounds);
            e.Graphics.DrawString(lb_groups.Items[e.Index].ToString(), e.Font,
                     new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();            

        }

        private void pbtn_reset_MouseEnter(object sender, EventArgs e)
        {
            pbtn_reset.BackgroundImage = Properties.Resources.main_btn_reset_on;
        }

        private void pbtn_reset_MouseLeave(object sender, EventArgs e)
        {
            pbtn_reset.BackgroundImage = Properties.Resources.main_btn_reset_off;
        }

        private void pbtn_daily_MouseEnter(object sender, EventArgs e)
        {
            pbtn_daily.BackgroundImage = Properties.Resources.main_btn_daily_on;
        }

        private void pbtn_daily_MouseLeave(object sender, EventArgs e)
        {
            pbtn_daily.BackgroundImage = Properties.Resources.main_btn_daily_off;
        }

        private void pbtn_submit_MouseEnter(object sender, EventArgs e)
        {
            pbtn_submit.BackgroundImage = Properties.Resources.main_btn_daily_on;
        }

        private void pbtn_submit_MouseLeave(object sender, EventArgs e)
        {
            pbtn_submit.BackgroundImage = Properties.Resources.main_btn_daily_off;
        }

        private void pbtn_chart_MouseEnter(object sender, EventArgs e)
        {
            pbtn_chart.BackgroundImage = Properties.Resources.main_btn_chart_on;
        }

        private void pbtn_chart_MouseLeave(object sender, EventArgs e)
        {
            pbtn_chart.BackgroundImage = Properties.Resources.main_btn_chart_off;
        }

        private void pbtn_production_MouseEnter(object sender, EventArgs e)
        {
            pbtn_production.BackgroundImage = Properties.Resources.main_btn_production_on;
        }

        private void pbtn_production_MouseLeave(object sender, EventArgs e)
        {
            pbtn_production.BackgroundImage = Properties.Resources.main_btn_production_off;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }


        private void dtp_start_KeyUp(object sender, KeyEventArgs e)
        {
           // tlp_main.Focus();
        }

        private void dtp_end_KeyUp(object sender, KeyEventArgs e)
        {
           // tlp_main.Focus();
        }

        


        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var parms = base.CreateParams;
        //        parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
        //        return parms;
        //    }
        //}

    }
}
