using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using SoS_Simulator.Objects;
using TestOracleGenerator.Oracle;

namespace SoS_Simulator
{
    /// <summary>
    /// An abstract class for System of Systems Simulator
    /// </summary>
    public abstract class Simulator
    {
        public delegate void LogUpdateHandler(string text);
        public delegate void SimulationCompleteHandler();
        public event LogUpdateHandler OnLogUpdate;
        public event SimulationCompleteHandler OnSimulationComplete;

        // Monitoring agent
        protected MonitorAgent _monitorAgent;

        /// <summary>
        /// Create an instance of Simulator
        /// </summary>
        public Simulator()
        {
            InitializeSimulator();
        }

        /// <summary>
        /// A private method for initializing simulator
        /// </summary>
        private void InitializeSimulator()
        {
            // Reset all event handlers for environmental objects & agents on previous simulator
            Agent.ResetEventHandler();
            SoS_Object.ResetEventHandler();

            // Keep in track of simulation input using MonitorAgent
            _monitorAgent = new MonitorAgent(this);
            _monitorAgent.OnTextUpdate += MonitorAgent_OnTextUpdate;
            _monitorAgent.OnSimulationFinished += MonitorAgent_OnSimulationFinished;

            // Initialize the derived environmental objects & agents
            Initialize();
        }

        /// <summary>
        /// Retrieve the global environmental values on simulation
        /// </summary>
        /// <returns></returns>
        public abstract string GetMonitoringText();

        /// <summary>
        /// An abstract initialization method for a derived Simulator
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// An internal method of starting simulator for a derived Simulator
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Start simulator
        /// </summary>
        public void RunSimulator()
        {
            Run();
        }

        /// <summary>
        /// Force complete the current simulation
        /// </summary>
        public void Finish()
        {
            if (OnSimulationComplete != null)
            {
                OnSimulationComplete();
            }
        }

        /// <summary>
        /// Allow the participating objects to update themselves in every tick
        /// </summary>
        public void Tick()
        {
            SoS_Object.RaiseTick();
        }

        /// <summary>
        /// Retrieve the simulation trace
        /// </summary>
        /// <returns>A list of simulation messages</returns>
        public MessageUnitList GetSimulationMessages()
        {
            return _monitorAgent.GetSimulationLog();
        }

        /// <summary>
        /// Simulation trace update event from MonitorAgent
        /// </summary>
        /// <param name="text"></param>
        private void MonitorAgent_OnTextUpdate(string text)
        {
            if (OnLogUpdate != null)
            {
                OnLogUpdate(text);
            }
        }

        /// <summary>
        /// Simulation complete event called from MonitorAgent
        /// </summary>
        private void MonitorAgent_OnSimulationFinished()
        {
            if (OnSimulationComplete != null)
            {
                OnSimulationComplete();
            }
        }
    }
}