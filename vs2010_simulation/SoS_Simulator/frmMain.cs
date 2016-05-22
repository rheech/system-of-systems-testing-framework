using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestOracleGenerator;
using SoS_Simulator.Agents;
using TestOracleGenerator.Xml;
using System.Reflection;
using System.IO;

namespace SoS_Simulator
{
    public partial class frmMain : Form
    {
        private const string BASE_PATH = "Resources\\";

        TOGenerator _tcGenerator;
        Simulator s;

        public frmMain()
        {
            InitializeComponent();
            lstViewGoal.FullRowSelect = true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializeSimulator();

            EnableSimulator(false);
            LoadDefault();
        }

        private void LoadDefault()
        {
            LoadSimulator("Scenario_MCI.dll");
            LoadTestOracle(String.Format("{0}{1}", BASE_PATH, "Scenario_MCI.xml"));
        }

        private void EnableSimulator(bool bEnable)
        {
            lstViewGoal.Enabled = bEnable;
            btnOracleBrowse.Enabled = bEnable;
            tbChangeSpeed.Enabled = bEnable;
            btnReset.Enabled = bEnable;
            btnStart.Enabled = bEnable;
            lstGoals.Enabled = bEnable;
            lstViewGoal.Enabled = bEnable;
        }

        private void LoadSimulator(string simulatorFile)
        {
            bool isValidFile;
            Assembly simFile;

            isValidFile = false;

            try
            {
                simFile = Assembly.LoadFrom(simulatorFile);

                //Type[] mod = simFile.GetTypes();
                Type mainType;

                foreach (Type t in simFile.GetTypes())
                {
                    if (t.Name == "ScenarioMain")
                    {
                        mainType = t;

                        s = (Simulator)simFile.CreateInstance(mainType.FullName);
                        s.OnLogUpdate += Simulator_OnLogUpdate;
                        s.OnSimulationComplete += Simulator_OnSimulationComplete;
                        tsLabel.Text = "Ready";

                        isValidFile = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading simulator file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (isValidFile)
            {
                EnableSimulator(true);
            }
            else
            {
                MessageBox.Show("Error loading simulator file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTestOracle(string oracleFile)
        {
            string[] goalList = { "Communicate", "Triage", "Treatment", "MedComm", "Transportation" };
            //string[] goalList = { "SavePatient" };
            _tcGenerator = new TOGenerator(oracleFile);

            foreach (string s in goalList)
            {
                lstGoals.Items.Add(s);
            }
            //goalList = _tcGenerator.RetrieveGoalList();

            SetupGoalList(goalList);
        }

        private void updateTCResourceFile()
        {
            /*_tcGenerator = new TOGenerator(String.Format("{0}{1}", BASE_PATH, txtGoal.Text),
                                    String.Format("{0}{1}", BASE_PATH, txtRole.Text),
                                    String.Format("{0}{1}", BASE_PATH, txtAgent.Text));*/
            //_tcGenerator = new TOGenerator(String.Format("{0}{1}", BASE_PATH, "Scenario_MCI.xml"));
            // SavePatient, Communicate, MedicalCare, ReportIncident, LocatePatient, TreatPatient, TransferPatient
            //_tcGenerator.TaskModel = String.Format("{0}{1}", BASE_PATH, txtGoal.Text);
            //_tcGenerator.RoleModel = String.Format("{0}{1}", BASE_PATH, txtRole.Text);
            //_tcGenerator.AgentModel = String.Format("{0}{1}", BASE_PATH, txtAgent.Text);
            //_tcGenerator.ProtocolModel = String.Format("{0}{1}", BASE_PATH, txtProtocol.Text);
            
        }

        private void lstGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGoals.SelectedIndex != -1)
            {
                updateTCResourceFile();

                // Update visualization
                txtOutput.Text = _tcGenerator.GenerateTestOracle(lstGoals.SelectedItem.ToString()).ToString();
            }
        }

        private void InitializeSimulator()
        {
            /*simulationVisualizer1.StartVisualization();
            simulationVisualizer1.UpdateGraphics(s.UpdateVisualComponent());*/
            //lblStatus.Text = s.GetNumbers();

            // Reset listview
            //SetupGoalList();
        }

        private void SetupGoalList(string[] goalList)
        {
            // Reset listvies
            lstViewGoal.Items.Clear();

            //string[] goalList = { "Communicate", "Triage", "Treatment", "MedComm", "Transportation" };
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
                info = _tcGenerator.GenerateTestOracle(item.Text);

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
            //simulationVisualizer1.UpdateGraphics(s.UpdateVisualComponent());
            //lblStatus.Text = s.GetNumbers();

            /*if (Simulator.patients.Count == 0 && Simulator.savedPatients.Count == 10)
            {
                tmrSimulation.Enabled = false;
            }*/

            // Update goal listview (test pass/fail)
            updateTCResourceFile();
            UpdateGoalList(); // Test Oracle
        }

        private void tbChangeSpeed_Scroll(object sender, EventArgs e)
        {
            if (tbChangeSpeed.Value == 4)
            {
                tmrSimulation.Interval = 1000;
            }
            else
            {
                tmrSimulation.Interval = ((tbChangeSpeed.Maximum + 1) - tbChangeSpeed.Value) * 50;
            }
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
            s.InitializeSimulator();
            InitializeSimulator();
            txtSimOutput.Text = "";
        }

        private void tsBtnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdOpenFile = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            ofdOpenFile.Filter = "DLL Files (*.dll)|*.dll|All Files (*.*)|*.*";
            ofdOpenFile.FilterIndex = 1;
            ofdOpenFile.RestoreDirectory = true;

            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                LoadSimulator(ofdOpenFile.FileName);
            }
        }

        private void btnOracleBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdOpenFile = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            ofdOpenFile.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            ofdOpenFile.FilterIndex = 1;
            ofdOpenFile.RestoreDirectory = true;

            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                txtOraclePath.Text = ofdOpenFile.FileName;
                LoadTestOracle(ofdOpenFile.FileName);
            }
        }
    }
}
