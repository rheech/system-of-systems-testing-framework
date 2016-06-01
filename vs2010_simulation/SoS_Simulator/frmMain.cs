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
using BugTracker;

namespace SoS_Simulator
{
    public partial class frmMain : Form
    {
        private enum SIMULATION_STATUS
        {
            NOT_READY,
            READY,
            RUNNING,
            PAUSED,
            FINISHED
        }

        private const string BASE_PATH = "Resources\\";

        OracleGenerator _toGenerator;
        Simulator s;
        SIMULATION_STATUS _simulationStatus;
        FileInfo _fSimulator, _fOracle;

        public frmMain()
        {
            InitializeComponent();

            lstViewGoal.FullRowSelect = true;
            _simulationStatus = SIMULATION_STATUS.NOT_READY;
        }

        #region Event Handlers
        private void frmMain_Load(object sender, EventArgs e)
        {
            InitializeSimulatorDisplay();
            LoadDefault();
        }

        private void lstGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGoalsResult.SelectedIndex != -1)
            {
                // Update visualization
                //txtGoalOutput.Text = _toGenerator.GenerateTestOracle(lstGoalsResult.SelectedItem.ToString()).ToString();

                TestOracle oracle;
                oracle = _toGenerator.GenTestOracle(lstGoalsResult.SelectedItem.ToString());

                txtGoalOutput.Text = oracle.ToString();

            }
        }

        private void tmrSimulation_Tick(object sender, EventArgs e)
        {
            s.Tick();

            // Update goal listview (test pass/fail)
            UpdateGoalList(); // Test Oracle

            lblStatus.Text = String.Format("Simulation Output\r\n\r\n{0}", s.GetMonitoringText());
        }

        private void tbChangeSpeed_Scroll(object sender, EventArgs e)
        {
            int[] speed = { 2000, 1500, 1200, 1000, 500, 100, 50 };

            tmrSimulation.Interval = speed[tbChangeSpeed.Value - 1];
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            switch (_simulationStatus)
            {
                case SIMULATION_STATUS.READY:
                    StartSimulator();
                    break;
                case SIMULATION_STATUS.PAUSED:
                    ResumeSimulator();
                    break;
                case SIMULATION_STATUS.RUNNING:
                    PauseSimulator();
                    break;
                case SIMULATION_STATUS.FINISHED:
                    InitializeSimulator();
                    StartSimulator();
                    break;
                default:
                    throw new ApplicationException("Unhandled button");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitializeSimulator();
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
                InitializeSimulator();

                if (!LoadSimulatorLibrary(ofdOpenFile.FileName))
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

        private void TextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            string text;

            if (e.KeyData == (Keys.Control | Keys.C))
            {
                text = ((TextBox)sender).SelectedText;

                if (text != null && text != String.Empty)
                {
                    Clipboard.Clear();
                    Clipboard.SetText(text);
                }
            }
        }
        #endregion

        #region Initialization

        private void InitializeSimulatorDisplay()
        {
            EnableSimulator(false);
            EnableOracle(false);
        }

        private void LoadDefault()
        {
            if (LoadSimulatorLibrary("Scenario_MCI.dll"))
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

        private bool LoadSimulator()
        {
            if (_fSimulator.Exists)
            {
                return LoadSimulatorLibrary(_fSimulator);
            }

            throw new ApplicationException("Simulator is not initialized");
        }

        private bool LoadSimulatorLibrary(string simulatorfile)
        {
            return LoadSimulatorLibrary(new FileInfo(simulatorfile));
        }

        private bool LoadSimulatorLibrary(FileInfo simulatorFile)
        {
            Assembly simFile;

            try
            {
                simFile = Assembly.LoadFrom(simulatorFile.FullName);

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

                        _fSimulator = simulatorFile;
                        EnableSimulator(true);
                        _simulationStatus = SIMULATION_STATUS.READY;

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

        private bool LoadTestOracle()
        {
            if (_fOracle.Exists)
            {
                return LoadTestOracle(_fOracle);
            }

            throw new ApplicationException("Simulator is not initialized");
        }

        private bool LoadTestOracle(string oracleFile)
        {
            return LoadTestOracle(new FileInfo(oracleFile));
        }

        private bool LoadTestOracle(FileInfo oracleFile)
        {
            //string[] goalList = { "Communicate", "Triage", "Treatment", "MedComm", "Transportation" };
            string[] goalList;

            try
            {
                _toGenerator = new OracleGenerator(oracleFile.FullName);
                goalList = _toGenerator.RetrieveGoalList();

                lstGoalsResult.Items.Clear();

                foreach (string s in goalList)
                {
                    lstGoalsResult.Items.Add(s);
                }
                //goalList = _tcGenerator.RetrieveGoalList();

                EnableOracle(true);
                _fOracle = oracleFile;
                txtOraclePath.Text = _fOracle.FullName;
                SetupGoalList(goalList);

                return true;
            }
            catch (Exception)
            {
                // Error loading xml file
            }

            return false;
        }

        #endregion

        #region Simulator
        private void EnableSimulator(bool bEnable)
        {
            lstViewGoal.Enabled = bEnable;
            btnOracleBrowse.Enabled = bEnable;
            tbChangeSpeed.Enabled = bEnable;
            btnReset.Enabled = bEnable;
            btnStart.Enabled = bEnable;
            lstViewGoal.Enabled = bEnable;
        }

        private void EnableOracle(bool bEnable)
        {
            lstGoalsResult.Enabled = bEnable;
            txtGoalOutput.Enabled = bEnable;
        }

        private void InitializeSimulator()
        {
            tmrSimulation.Enabled = false;
            lstGoalsResult.Items.Clear();
            txtSimOutput.Text = "";

            // If simulator is previously loaded (reset)
            if (_fSimulator != null && _fOracle != null)
            {
                LoadSimulator();
                LoadTestOracle();

                tsLabel.Text = "Ready";
                btnStart.Text = "&Start";

                _simulationStatus = SIMULATION_STATUS.READY;
            }
            else
            {
                _simulationStatus = SIMULATION_STATUS.NOT_READY;
            }
        }

        private void StartSimulator()
        {
            s.RunSimulator();
            tmrSimulation.Enabled = true;
            tsLabel.Text = "Simulating...";
            btnStart.Text = "Pau&se";
            _simulationStatus = SIMULATION_STATUS.RUNNING;
        }

        private void PauseSimulator()
        {
            tmrSimulation.Enabled = false;
            tsLabel.Text = "Paused";
            btnStart.Text = "Re&sume";
            _simulationStatus = SIMULATION_STATUS.PAUSED;
        }

        private void ResumeSimulator()
        {
            tmrSimulation.Enabled = true;
            tsLabel.Text = "Simulating...";
            btnStart.Text = "Pau&se";
            _simulationStatus = SIMULATION_STATUS.RUNNING;
        }

        private void FinishSimulator()
        {
            tmrSimulation.Enabled = false;
            tsLabel.Text = "Simulation complete.";
            btnStart.Text = "Re&start";
            _simulationStatus = SIMULATION_STATUS.FINISHED;
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
            FinishSimulator();
        }
        #endregion

        #region Test Oracle
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
            MessageUnit[] msgUnit;

            msgUnit = s.GetSimulationMessages();

            //Console.WriteLine(_toGenerator.CompareOutput("Transportation", unit, 0));

            /*foreach (ListViewItem item in lstViewGoal.Items)
            {
                if (_toGenerator.CompareOutput(item.Text, msgUnit))
                {
                    item.SubItems[1].Text = "Pass";
                    item.ForeColor = Color.Green;
                }
                else
                {
                    item.SubItems[1].Text = "Fail";
                    item.ForeColor = Color.Red;
                }
            }*/
        }
        #endregion
    }
}
