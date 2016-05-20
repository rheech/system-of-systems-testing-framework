using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using SoS_Simulator.Objects;
using TestOracleGenerator;
using TestOracleGenerator.Oracle;

namespace SoS_Simulator
{
    public abstract class Simulator
    {
        public delegate void LogUpdateHandler(string text);
        public delegate void SimulationCompleteHandler();
        public event LogUpdateHandler OnLogUpdate;
        public event SimulationCompleteHandler OnSimulationComplete;

        // Monitoring agent
        MonitorAgent _monitorAgent;

        public Simulator()
        {
            InitializeSimulator();
        }

        public void InitializeSimulator()
        {
            Agent.ResetEventHandler();
            SoS_Object.ResetEventHandler();

            // simulation input
            _monitorAgent = new MonitorAgent();
            _monitorAgent.OnTextUpdate += MonitorAgent_OnTextUpdate;
            _monitorAgent.OnSimulationFinished += MonitorAgent_OnSimulationFinished;

            Initialize();
        }

        protected abstract void Initialize();

        public void RunSimulator()
        {
            Run();
        }

        protected abstract void Run();

        public void Tick()
        {
            SoS_Object.RaiseTick();
        }

        public bool CompareResult(TestInfo tcResult)
        {
            MessageUnit[] simResult;
            string output;
            StringBuilder sb = new StringBuilder();
            bool bExists = true;

            simResult = _monitorAgent.GetSimulationLog();
            
            for (int i = 0; i < simResult.Length; i++)
            {
                sb.AppendFormat("{0}\n", simResult[i].ToString());
            }

            output = sb.ToString();

            for (int i = 0; i < tcResult.oracle.Length; i++)
            {
                for (int j = 0; j < tcResult.oracle[i].Length; j++)
                {
                    //Console.WriteLine(tcResult.oracle[i][j].ToString());
                    bExists &= (output.IndexOf(tcResult.oracle[i][j].ToString()) != -1);
                }
            }

            /*foreach (Arrow arrow in tcResult.sequence)
            {
                bExists &= (output.IndexOf(arrow.ToString()) != -1);
            }*/

            return bExists;
        }

        private void MonitorAgent_OnTextUpdate(string text)
        {
            if (OnLogUpdate != null)
            {
                OnLogUpdate(text);
            }
        }

        private void MonitorAgent_OnSimulationFinished()
        {
            if (OnSimulationComplete != null)
            {
                OnSimulationComplete();
            }
        }
    }
}
