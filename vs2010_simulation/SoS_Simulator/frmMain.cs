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
using TestOracleGenerator.Oracle;

namespace SoS_Simulator
{
    public partial class frmMain : Form
    {
        private enum SIMULATION_STATUS
        {
            READY,
            RUNNING,
            PAUSED,
            FINISHED
        }

        private const string BASE_PATH = "Resources\\";

        TOGenerator _toGenerator;
        Simulator s;
        SIMULATION_STATUS simulationStatus;
        FileInfo fSimulator, fOracle;

        public frmMain()
        {
            InitializeComponent();
            lstViewGoal.FullRowSelect = true;
            simulationStatus = SIMULATION_STATUS.READY;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializeSimulator();

            EnableSimulator(false);
            LoadDefault();
        }

        private void LoadDefault()
        {
            if (LoadSimulator("Scenario_MCI.dll"))
            {
                if (!LoadTestOracle(String.Format("{0}{1}", BASE_PATH, "Scenario_MCI.xml")))
                {
                    MessageBox.Show("Error loading test oracles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Error loading simulation library.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private bool LoadSimulator(string simulatorFile)
        {
            Assembly simFile;

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

                        fSimulator = new FileInfo(simulatorFile);
                        EnableSimulator(true);

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // Error loading simulator file
            }

            return false;
        }

        private bool LoadTestOracle(string oracleFile)
        {
            //string[] goalList = { "Communicate", "Triage", "Treatment", "MedComm", "Transportation" };
            string[] goalList;

            try
            {
                _toGenerator = new TOGenerator(oracleFile);
                goalList = _toGenerator.RetrieveGoalList();

                foreach (string s in goalList)
                {
                    lstGoals.Items.Add(s);
                }
                //goalList = _tcGenerator.RetrieveGoalList();

                fOracle = new FileInfo(oracleFile);
                txtOraclePath.Text = fOracle.FullName;
                SetupGoalList(goalList);

                return true;
            }
            catch (Exception)
            {
                // Error loading xml file
            }

            return false;
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
                txtOutput.Text = _toGenerator.GenerateTestOracle(lstGoals.SelectedItem.ToString()).ToString();
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

            MessageUnit[] msgUnit;
            COMPARISON_INFO comparisonInfo;

            msgUnit = s.GetSimulationMessages();
            
            //Console.WriteLine(_toGenerator.CompareOutput("Transportation", unit, 0));

            foreach (ListViewItem item in lstViewGoal.Items)
            {
                comparisonInfo = new COMPARISON_INFO();
                comparisonInfo.Result = true;
                comparisonInfo.CurrentIndex = 0;

                info = _toGenerator.GenerateTestOracle(item.Text);

                //if (s.CompareResult(info))
                comparisonInfo = _toGenerator.CompareOutput(item.Text, msgUnit, comparisonInfo);

                if (comparisonInfo.Result)
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
            simulationStatus = SIMULATION_STATUS.FINISHED;
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
            switch (simulationStatus)
            {
                case SIMULATION_STATUS.READY:
                    s.RunSimulator();
                    tmrSimulation.Enabled = true;
                    tsLabel.Text = "Simulating...";
                    btnStart.Text = "Pau&se";
                    simulationStatus = SIMULATION_STATUS.RUNNING;
                    break;
                case SIMULATION_STATUS.RUNNING:
                    tmrSimulation.Enabled = false;
                    tsLabel.Text = "Paused";
                    btnStart.Text = "Re&sume";
                    simulationStatus = SIMULATION_STATUS.PAUSED;
                    break;
                case SIMULATION_STATUS.PAUSED:
                    tmrSimulation.Enabled = true;
                    tsLabel.Text = "Simulating...";
                    btnStart.Text = "Pau&se";
                    simulationStatus = SIMULATION_STATUS.RUNNING;
                    break;
                case SIMULATION_STATUS.FINISHED:
                    s.RunSimulator();
                    tsLabel.Text = "Simulating...";
                    btnStart.Text = "Pau&se";
                    simulationStatus = SIMULATION_STATUS.RUNNING;
                    break;
                default:
                    break;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tmrSimulation.Enabled = false;
            simulationStatus = SIMULATION_STATUS.READY;
            btnStart.Text = "&Start";
            lstGoals.Items.Clear();
            s.InitializeSimulator();
            InitializeSimulator();
            LoadSimulator(fSimulator.FullName);
            LoadTestOracle(fOracle.FullName);
            txtSimOutput.Text = "";
        }

        private void tsBtnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdOpenFile = new OpenFileDialog();

            ofdOpenFile.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            ofdOpenFile.Filter = "DLL Files (*.dll)|*.dll|All Files (*.*)|*.*";
            ofdOpenFile.FilterIndex = 1;
            ofdOpenFile.RestoreDirectory = true;

            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                if (!LoadSimulator(ofdOpenFile.FileName))
                {
                    MessageBox.Show("Error loading simulation library.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnOracleBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdOpenFile = new OpenFileDialog();

            ofdOpenFile.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            ofdOpenFile.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            ofdOpenFile.FilterIndex = 1;
            ofdOpenFile.RestoreDirectory = true;

            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                if (!LoadTestOracle(ofdOpenFile.FileName))
                {
                    MessageBox.Show("Error loading test oracles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
