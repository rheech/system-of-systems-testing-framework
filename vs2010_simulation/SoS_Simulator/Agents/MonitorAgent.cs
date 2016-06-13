using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator;
using TestOracleGenerator.Oracle;

namespace SoS_Simulator.Agents
{
    /// <summary>
    /// A Monitoring Agent to keep track of interaction messages from simulation
    /// </summary>
    public class MonitorAgent : Agent
    {
        public delegate void TextUpdateHandler(string text);
        public delegate void SimulationFinished();

        public event TextUpdateHandler OnTextUpdate;
        public event SimulationFinished OnSimulationFinished;

        private MessageUnitList _simLog;
        private int _lastSimLogCount;
        private int _noMessageTick;

        /// <summary>
        /// Create an instance of MonitorAgent
        /// </summary>
        /// <param name="simulator">A simulator related to the MonitorAgent.</param>
        public MonitorAgent(Simulator simulator) : base(simulator)
        {
            _simLog = new MessageUnitList();
        }

        /// <summary>
        /// MessageReceived event to keep in track of every interaction among constituents in SoS
        /// </summary>
        /// <param name="from">A reference object to the sender of the message</param>
        /// <param name="target">A reference type to the receiver of the message</param>
        /// <param name="msgText"></param>
        /// <param name="info"></param>
        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            MessageUnit entry;
            entry = new MessageUnit();

            // Create MessageUnit using the received message
            entry.From = from.GetType().Name.ToString();
            entry.To = target.Name.ToString();
            entry.Message = msgText.ToString();

            // Add current message to the simulation log
            _simLog.Add(entry);

            // update visual text
            if (OnTextUpdate != null)
            {
                // Format message as follows:
                // (SenderAgent->ReceiverAgent).MessageText
                OnTextUpdate(entry.ToString());
            }
        }

        /// <summary>
        /// Retrieve the simulation log
        /// </summary>
        /// <returns></returns>
        public MessageUnitList GetSimulationLog()
        {
            return _simLog;
        }

        /// <summary>
        /// Monitor and terminate simulator if there is no message for 5 ticks.
        /// </summary>
        protected override void OnTick()
        {
            int simLogCount;

            simLogCount = 0;

            // prevent null exception
            if (_simLog != null)
            {
                simLogCount = _simLog.Count;
            }

            // count if there is no message
            if (_lastSimLogCount == _simLog.Count)
            {
                _noMessageTick++;
            }
            else
            {
                _lastSimLogCount = simLogCount;
                _noMessageTick = 0;
            }

            // if no message for 5 times, terminate simulation
            if (_noMessageTick > 4)
            {
                Simulation<Simulator>().Finish();
            }
        }
    }
}