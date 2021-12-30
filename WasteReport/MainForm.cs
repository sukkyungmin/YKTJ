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
using WasteReport.CS;

namespace WasteReport
{
    public partial class MainForm : Form
    {
        Con_Main con_main;
        Con_Daily con_daily;
        Con_Sub con_sub;
        Con_Chart con_chart;
        Con_Prod con_prod;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            con_main = new Con_Main(this);

            tlp_main.Controls.Add(con_main, 0, 0);
            con_main.Dock = DockStyle.Fill;

            con_main.Visible = true;
        }

        public void GoToMain()
        {
            con_main.Visible = true;

            if(con_daily != null)
            {
                con_daily.Visible = false;
            }

            if (con_sub != null)
            {
                con_sub.Visible = false;
            }

            if (con_chart != null)
            {
                con_chart.Visible = false;
            }

            if (con_prod != null)
            {
                con_prod.Visible = false;
            }
        }

        public void GoToDaily(WasteSearch ws)
        {
            con_daily = new Con_Daily(this, ws);
            tlp_main.Controls.Add(con_daily, 0, 0);
            con_daily.Dock = DockStyle.Fill;

            con_main.Visible = false;
            if(con_sub != null)
            {
                con_sub.Visible = false;
            }
            if (con_chart != null)
            {
                con_chart.Visible = false;
            }
            if (con_prod != null)
            {
                con_prod.Visible = false;
            }
            con_daily.Visible = true;
        }

        public void GoToSub(WasteSearch ws)
        {
            con_sub = new Con_Sub(this, ws);
            tlp_main.Controls.Add(con_sub, 0, 0);
            con_sub.Dock = DockStyle.Fill;

            con_main.Visible = false;
            if (con_daily != null)
            {
                con_daily.Visible = false;
            }
            if (con_chart != null)
            {
                con_chart.Visible = false;
            }
            if (con_prod != null)
            {
                con_prod.Visible = false;
            }
            con_sub.Visible = true;
        }

        public void GoToChart(WasteSearch ws)
        {
            con_chart = new Con_Chart(this, ws);
            tlp_main.Controls.Add(con_chart, 0, 0);
            con_chart.Dock = DockStyle.Fill;

            con_main.Visible = false;
            if (con_daily != null)
            {
                con_daily.Visible = false;
            }
            if (con_sub != null)
            {
                con_sub.Visible = false;
            }
            if (con_prod != null)
            {
                con_prod.Visible = false;
            }
            con_chart.Visible = true;
        }

        public void GoToProd(WasteSearch ws)
        {
            con_prod = new Con_Prod(this, ws);
            tlp_main.Controls.Add(con_prod, 0, 0);
            con_prod.Dock = DockStyle.Fill;

            con_main.Visible = false;
            if (con_daily != null)
            {
                con_daily.Visible = false;
            }
            if (con_chart != null)
            {
                con_chart.Visible = false;
            }
            if (con_sub != null)
            {
                con_sub.Visible = false;
            }
            con_prod.Visible = true;
        }
        private void OUT()
        {
            try
            {
                String[] args = Environment.GetCommandLineArgs();

                // args[0] is the program name and, args[1] is the first argument.
                // Test for a command-line argument.
                if (args.Length > 1)
                {

                    // Parse the argument. If successful, exit with the parsed code.
                    try
                    {
                        int exitCode = int.Parse(args[1]);

                        Environment.Exit(exitCode);
                    }
                    // If the parse fails, you fall out of the program.
                    catch
                    {
                        Environment.Exit(0);
                        this.Close();
                    }
                }

                int exit1Code = int.Parse(args[1]);

                Environment.Exit(exit1Code);

                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("종료 하시겠습니까?", "종료 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                OUT();
            }
            else
            {
                e.Cancel = true;
            }
        }

    }
}
