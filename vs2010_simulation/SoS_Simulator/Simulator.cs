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
        protected MonitorAgent _monitorAgent;

        public Simulator()
        {
            InitializeSimulator();
        }

        private void InitializeSimulator()
        {
            Agent.ResetEventHandler();
            SoS_Object.ResetEventHandler();

            // simulation input
            _monitorAgent = new MonitorAgent(this);
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

        public MessageUnit[] GetSimulationMessages()
        {
            return _monitorAgent.GetSimulationLog();
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
