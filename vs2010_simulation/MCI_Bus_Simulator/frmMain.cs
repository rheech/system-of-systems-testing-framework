using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestCaseGenerator;

namespace MCI_Bus_Simulator
{
    public partial class frmMain : Form
    {
        TCGenerator _tcGenerator;

        private const string BASE_PATH = "Resources\\";

        public frmMain()
        {
            InitializeComponent();
            _tcGenerator = new TCGenerator();
        }

        private void updateTCResourceFile()
        {
            _tcGenerator.GoalModel = String.Format("{0}{1}", BASE_PATH, txtGoal.Text);
            _tcGenerator.AgentModel = String.Format("{0}{1}", BASE_PATH, txtAgent.Text);
            _tcGenerator.RoleModel = String.Format("{0}{1}", BASE_PATH, txtRole.Text);
            _tcGenerator.ProtocolModel = String.Format("{0}{1}", BASE_PATH, txtProtocol.Text);
        }

        private void Main()
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

        }

        private void lstGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGoals.SelectedIndex != -1)
            {
                updateTCResourceFile();

                txtOutput.Text = _tcGenerator.GenerateTestCase(lstGoals.SelectedItem.ToString());
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
