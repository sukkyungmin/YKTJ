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
    public partial class RealTimeSearchConditionForm : Form
    {
        private HomePage _homePage = null; 
        private string _logNo = ""; 

        public RealTimeSearchConditionForm(HomePage homePage)
        {
            InitializeComponent();
            _homePage = homePage; 
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        public void SetLogNo(string logNo)
        {
            _logNo = logNo;
        }

        private void RealTimeSearchConditionForm_Load(object sender, EventArgs e)
        {
            LoadCondition(); 
        }

        private void LoadCondition()
        {
            if (_logNo == "")
                return;

            DataSet dataSet = DbHelper.SelectQuery(string.Format("{0} {1}", "SelectRealTimeSearchCondition", _logNo));
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return;

            string teamType = dataSet.Tables[0].Rows[0]["TeamType"].ToString();
            string tjType = dataSet.Tables[0].Rows[0]["TjType"].ToString();

            if (teamType == "1")
                mergeLabel.Text = "BC 통합"; 
            else 
                mergeLabel.Text = "CC 통합"; 

            teamTypeTextBox.Text = "유아" + teamType + "팀";
            tjTypeTextBox.Text = _homePage.GetTjNameFromTjType(Convert.ToInt32(tjType)); 
            periodTextBox.Text = dataSet.Tables[0].Rows[0]["StartDate"].ToString() + " ~ " + dataSet.Tables[0].Rows[0]["EndDate"].ToString(); 
            reportNameTextBox.Text = dataSet.Tables[0].Rows[0]["ReportName"].ToString(); 

            sapCodeTextBox.Text = dataSet.Tables[0].Rows[0]["SapCodeCd"].ToString(); 
            sapDescTextBox.Text = dataSet.Tables[0].Rows[0]["SapDescCd"].ToString(); 
            procEnemTextBox.Text = dataSet.Tables[0].Rows[0]["ProcMnemCd"].ToString(); 
            eventTypeTextBox.Text = dataSet.Tables[0].Rows[0]["EventTypeCd"].ToString(); 
            wasteLossTextBox.Text = dataSet.Tables[0].Rows[0]["WasteLossCd"].ToString(); 
            setDataTextBox.Text = dataSet.Tables[0].Rows[0]["SetDataCd"].ToString();
            mergeTextBox.Text = (dataSet.Tables[0].Rows[0]["IsMerge"].ToString() == "1") ? "통합" : "하지 않음"; 
            createdDateTextBox.Text = dataSet.Tables[0].Rows[0]["CreatedDate"].ToString(); 
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(new Pen(Color.Red), teamTypeTextBox.Left - 1, teamTypeTextBox.Top - 1, teamTypeTextBox.Width + 2, teamTypeTextBox.Height + 2);

        }

        private void basicConditionPanel_Paint(object sender, PaintEventArgs e)
        {
            TextBox[] textBoxArray = { teamTypeTextBox, tjTypeTextBox, periodTextBox, reportNameTextBox };

            Pen pen = new Pen(Color.FromArgb(249, 249, 250));
            for (int i = 0; i < textBoxArray.Length; i++)
            {
                Graphics g = e.Graphics;
                g.DrawRectangle(pen, textBoxArray[i].Left - 1, textBoxArray[i].Top - 1, textBoxArray[i].Width + 2, textBoxArray[i].Height + 2);
            }
        }

        private void detailConditionPanel_Paint(object sender, PaintEventArgs e)
        {
            TextBox[] textBoxArray = { sapCodeTextBox, sapDescTextBox, procEnemTextBox, eventTypeTextBox, wasteLossTextBox , wasteLossTextBox, setDataTextBox, mergeTextBox, createdDateTextBox};

            Pen pen = new Pen(Color.FromArgb(249, 249, 250));
            for (int i = 0; i < textBoxArray.Length; i++)
            {
                Graphics g = e.Graphics;
                g.DrawRectangle(pen, textBoxArray[i].Left - 1, textBoxArray[i].Top - 1, textBoxArray[i].Width + 2, textBoxArray[i].Height + 2);
            }
        }
    }
}
