using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator;
using TestOracleGenerator.Oracle;

namespace SoS_Simulator.Agents
{
    public struct SimulationEntry
    {
        public string agent;
        public string arrow;

        public override string ToString()
        {
            return String.Format("{0}.{1}", agent, arrow);
        }
    }

    public class MonitorAgent : Agent
    {
        public delegate void TextUpdateHandler(string text);
        public event TextUpdateHandler OnTextUpdate;
        public delegate void SimulationFinished();
        public event SimulationFinished OnSimulationFinished;
        private MessageUnitList _simLog;
        private int _lastSimLogCount;
        private int _noMessageTick;

        public MonitorAgent(Simulator simulator) : base(simulator)
        {
            _simLog = new MessageUnitList();
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            MessageUnit entry;
            entry = new MessageUnit();

            // print debug log
            //Debug.Print(msgType.ToString());

            entry.From = from.GetType().Name.ToString();
            entry.To = target.Name.ToString();
            entry.Message = msgType.ToString();

            _simLog.Add(entry);

            // get msg type
            switch (msgType)
            {
                case "SIMULATION_COMPLETE":
                    if (OnSimulationFinished != null)
                    {
                        OnSimulationFinished();
                    }
                    break;
                default:
                    break;
            }

            // update visual text
            if (OnTextUpdate != null)
            {
                //OnTextUpdate(from.GetType().Name.ToString() + "->" + target.Name.ToString() + "." + msgType.ToString());
                OnTextUpdate(entry.ToString());
            }
        }

        public MessageUnitList GetSimulationLog()
        {
            return _simLog;
        }

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

            // if no message for 5 times, finish simulation
            if (_noMessageTick > 4)
            {
                Simulation<Simulator>().Finish();
            }
        }
    }
}
