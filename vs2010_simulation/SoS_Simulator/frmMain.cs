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
using SoS_Simulator.UtilityFunc;
using System.Diagnostics;

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

        ComparingEngine _toGenerator;
        Simulator s;
        SIMULATION_STATUS _simulationStatus;
        FileInfo _fSimulator, _fOracle;
        UtilityFuncLib _util;
        Stopwatch _simTimeWatch;
        TimeSpan _simTickTime;

        public frmMain()
        {
            InitializeComponent();

            lstViewGoal.FullRowSelect = true;
            _simulationStatus = SIMULATION_STATUS.NOT_READY;
            _util = null;
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

        int k = 0;

        private void tmrSimulation_Tick(object sender, EventArgs e)
        {
            k++;
            _simTickTime += TimeSpan.FromMilliseconds(tmrSimulation.Interval);
            s.Tick();

            // Update goal listview (test pass/fail)
            UpdateGoalList(); // Test Oracle

            PrintSimulationStatus(s.GetMonitoringText());
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

                ClearTestOracle();

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

        private void btnOracleBrowse_Paint(object sender, PaintEventArgs e)
        {
            int arrowX = btnOracleBrowse.ClientRectangle.Width - 14;
            int arrowY = btnOracleBrowse.ClientRectangle.Height / 2 - 1;

            Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
            Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
            e.Graphics.FillPolygon(brush, arrows);
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
            if (LoadSimulatorLibrary("Scenario_E_Commerce.dll"))
            {
                //if (!LoadTestOracle(String.Format("{0}{1}", BASE_PATH, "Scenario_MCI.xml")))
                {
                    //MessageBox.Show("Error loading test oracles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        PrintSimulationStatus(s.GetMonitoringText());

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

        private void ClearTestOracle()
        {
            txtOraclePath.Text = "";
            _fOracle = null;
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
            string[] goalList;
            string utilFile;

            try
            {
                _toGenerator = new ComparingEngine(oracleFile.FullName);
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

                utilFile = UtilityFunc.UtilityFuncLib.GetUtilityFuncLibrary(oracleFile.FullName);

                if (utilFile != null)
                {
                    _util = UtilityFunc.UtilityFuncLib.LoadUtilityFuncLib(utilFile, s);
                }

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
            if (_fSimulator != null)
            {
                LoadSimulator();

                if (_fOracle != null)
                {
                    LoadTestOracle();
                }

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
            _simTickTime = new TimeSpan();
            _simTimeWatch = new Stopwatch();
            _simTimeWatch.Start();
            s.RunSimulator();
            tmrSimulation.Enabled = true;
            tsLabel.Text = "Simulating...";
            btnStart.Text = "Pau&se";
            _simulationStatus = SIMULATION_STATUS.RUNNING;
        }

        private void PauseSimulator()
        {
            tmrSimulation.Enabled = false;
            _simTimeWatch.Stop();
            tsLabel.Text = "Paused";
            btnStart.Text = "Re&sume";
            _simulationStatus = SIMULATION_STATUS.PAUSED;
        }

        private void ResumeSimulator()
        {
            _simTimeWatch.Start();
            tmrSimulation.Enabled = true;
            tsLabel.Text = "Simulating...";
            btnStart.Text = "Pau&se";
            _simulationStatus = SIMULATION_STATUS.RUNNING;
        }

        private void FinishSimulator()
        {
            tmrSimulation.Enabled = false;
            _simTimeWatch.Stop();
            tsLabel.Text = String.Format("Simulation complete. (Duration: {0:0.00} seconds, {1:0.00} seconds delayed.)",
                                        _simTimeWatch.Elapsed.TotalSeconds, (_simTimeWatch.Elapsed - _simTickTime).TotalSeconds);
            _simTickTime = TimeSpan.Zero;
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
            UpdateGoalList(); // Test Oracle
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
                item.UseItemStyleForSubItems = false;
                item.SubItems.Add("");
                item.SubItems.Add("");

                lstViewGoal.Items.Add(item);
            }
        }

        private void UpdateGoalList()
        {
            MessageUnitList msgUnitList;

            msgUnitList = s.GetSimulationMessages();

            //Console.WriteLine(_toGenerator.CompareOutput("Transportation", unit, 0));

            foreach (ListViewItem item in lstViewGoal.Items)
            {
                //item.UseItemStyleForSubItems = false;

                if (_toGenerator.CompareOutput(item.Text, msgUnitList))
                {
                    item.SubItems[1].Text = "Pass";
                    item.SubItems[1].ForeColor = Color.Green;
                }
                else
                {
                    item.SubItems[1].Text = "Fail";
                    item.SubItems[1].ForeColor = Color.Red;
                }

                // Check pass/fail using utility function
                if (_util != null)
                {
                    if (_util.CheckGoalAccomplishment(item.Text))
                    {
                        item.SubItems[2].Text = "Pass";
                        item.SubItems[2].ForeColor = Color.Green;
                    }
                    else
                    {
                        item.SubItems[2].Text = "Fail";
                        item.SubItems[2].ForeColor = Color.Red;
                    }
                }
            }
        }
        #endregion

        private void oracleClear_Click(object sender, EventArgs e)
        {
            ClearTestOracle();
        }

        private void PrintSimulationStatus(string format, params object[] args)
        {
            lblStatus.Text = String.Format(format, args);
        }

        private void tsCompLv1_Click(object sender, EventArgs e)
        {
            _toGenerator.ComparisonLevel = COMPARISON_LEVEL.LEVEL1;

            tsCompLv1.Checked = true;
            tsCompLv2.Checked = false;
            tsCompLv3.Checked = false;

            groupBox3.Text = "Goal (Comparison Lv.1)";
        }

        private void tsCompLv2_Click(object sender, EventArgs e)
        {
            _toGenerator.ComparisonLevel = COMPARISON_LEVEL.LEVEL2;

            tsCompLv1.Checked = false;
            tsCompLv2.Checked = true;
            tsCompLv3.Checked = false;

            groupBox3.Text = "Goal (Comparison Lv.2)";
        }

        private void tsCompLv3_Click(object sender, EventArgs e)
        {
            _toGenerator.ComparisonLevel = COMPARISON_LEVEL.LEVEL3;

            tsCompLv1.Checked = false;
            tsCompLv2.Checked = false;
            tsCompLv3.Checked = true;

            groupBox3.Text = "Goal (Comparison Lv.3)";
        }
    }
}
