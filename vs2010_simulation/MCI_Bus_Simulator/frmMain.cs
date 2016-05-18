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
using TestCaseGenerator.Xml;

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
            lstViewGoal.FullRowSelect = true;
            _tcGenerator = new TCGenerator();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializeSimulator();
            s.OnLogUpdate += Simulator_OnLogUpdate;
            s.OnSimulationComplete += Simulator_OnSimulationComplete;
        }

        private void updateTCResourceFile()
        {
            // SavePatient, Communicate, MedicalCare, ReportIncident, LocatePatient, TreatPatient, TransferPatient
            _tcGenerator.TaskModel = String.Format("{0}{1}", BASE_PATH, txtGoal.Text);
            _tcGenerator.RoleModel = String.Format("{0}{1}", BASE_PATH, txtRole.Text);
            _tcGenerator.AgentModel = String.Format("{0}{1}", BASE_PATH, txtAgent.Text);
            //_tcGenerator.ProtocolModel = String.Format("{0}{1}", BASE_PATH, txtProtocol.Text);
        }

        private void lstGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGoals.SelectedIndex != -1)
            {
                updateTCResourceFile();

                // Update visualization
                txtOutput.Text = _tcGenerator.GenerateTestCase(lstGoals.SelectedItem.ToString()).ToString();
            }
        }

        private void InitializeSimulator()
        {
            simulationVisualizer1.StartVisualization();
            simulationVisualizer1.UpdateGraphics(s.UpdateVisualComponent());
            lblStatus.Text = s.GetNumbers();

            // Reset listview
            SetupGoalList();
        }

        private void SetupGoalList()
        {
            // Reset listvies
            lstViewGoal.Items.Clear();

            string[] goalList = { "SavePatient", "Communicate", "MedicalCare", "ReportIncident", "LocatePatient", "TreatPatient", "TransferPatient" };
            ListViewItem item;

            foreach (string ss in goalList)
            {
                item = new ListViewItem(ss);
                item.SubItems.Add("");

                lstViewGoal.Items.Add(item);
            }
        }

        private void UpdateGoalList()
        {
            updateTCResourceFile();
            TestInfo info;

            foreach (ListViewItem item in lstViewGoal.Items)
            {
                info = _tcGenerator.GenerateTestCase(item.Text);

                if (s.CompareResult(info))
                {
                    item.SubItems[1].Text = "Pass";
                    item.ForeColor = Color.Green;
                }
                else
                {
                    item.SubItems[1].Text = "Fail";
                    item.ForeColor = Color.Red;
                }
            }
        }


        private void Simulator_OnLogUpdate(string text)
        {
            StringBuilder sb = new StringBuilder();

            if (txtSimOutput.Text != "")
            {
                sb.AppendFormat("{0}\r\n{1}", txtSimOutput.Text, text);
            }
            else
            {
                sb.Append(text);
            }

            txtSimOutput.Text = sb.ToString();

            txtSimOutput.SelectionStart = txtSimOutput.Text.Length;
            txtSimOutput.ScrollToCaret();
        }

        private void Simulator_OnSimulationComplete()
        {
            tmrSimulation.Enabled = false;
            tsLabel.Text = "Simulation complete.";
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

            // Update goal listview (test pass/fail)
            updateTCResourceFile();
            UpdateGoalList();
        }

        private void tbChangeSpeed_Scroll(object sender, EventArgs e)
        {
            tmrSimulation.Interval = ((tbChangeSpeed.Maximum + 1) - tbChangeSpeed.Value) * 50;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            s.RunSimulator();
            tmrSimulation.Enabled = true;
            tsLabel.Text = "Simulating...";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tmrSimulation.Enabled = false;
            s.InitializeSimulation();
            InitializeSimulator();
            txtSimOutput.Text = "";
        }
    }
}
