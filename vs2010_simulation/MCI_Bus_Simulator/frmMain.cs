using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestCaseGenerator;
using MCI_Bus_Simulator.Agents;

namespace MCI_Bus_Simulator
{
    public partial class frmMain : Form
    {
        private const string BASE_PATH = "Resources\\";

        TCGenerator _tcGenerator;
        Simulator s = new Simulator();

        public frmMain()
        {
            InitializeComponent();
            _tcGenerator = new TCGenerator();
        }

        private void MonitorAgent_OnTextUpdate(string text)
        {
            StringBuilder sb = new StringBuilder();

            if (textBox1.Text != "")
            {
                sb.AppendFormat("{0}\r\n{1}", textBox1.Text, text);
            }
            else
            {
                sb.Append(text);
            }

            textBox1.Text = sb.ToString();

            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void updateTCResourceFile()
        {
            _tcGenerator.GoalModel = String.Format("{0}{1}", BASE_PATH, txtGoal.Text);
            _tcGenerator.AgentModel = String.Format("{0}{1}", BASE_PATH, txtAgent.Text);
            _tcGenerator.RoleModel = String.Format("{0}{1}", BASE_PATH, txtRole.Text);
            _tcGenerator.ProtocolModel = String.Format("{0}{1}", BASE_PATH, txtProtocol.Text);
        }

        private void lstGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGoals.SelectedIndex != -1)
            {
                updateTCResourceFile();

                txtOutput.Text = _tcGenerator.GenerateTestCase(lstGoals.SelectedItem.ToString());
            }
        }

        private void InitializeSimulator()
        {
            simulationVisualizer1.StartVisualization();
            simulationVisualizer1.UpdateGraphics(s.UpdateVisualComponent());
            lblStatus.Text = s.GetNumbers();
            s.MonitorAgent.OnTextUpdate += MonitorAgent_OnTextUpdate;
        }
        
        private void btnReset_Click(object sender, EventArgs e)
        {
            tmrSimulation.Enabled = false;
            s.InitializeSimulation();
            InitializeSimulator();
            textBox1.Text = "";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            s.RunSimulator();
            tmrSimulation.Enabled = true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializeSimulator();
        }

        private void simulationVisualizer1_Click(object sender, EventArgs e)
        {

        }

        private void tmrSimulation_Tick(object sender, EventArgs e)
        {
            s.Tick();
            simulationVisualizer1.UpdateGraphics(s.UpdateVisualComponent());
            lblStatus.Text = s.GetNumbers();

            /*if (Simulator.patients.Count == 0 && Simulator.savedPatients.Count == 10)
            {
                tmrSimulation.Enabled = false;
            }*/
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tmrSimulation.Interval = trackBar1.Value * 50;
        }
    }
}
