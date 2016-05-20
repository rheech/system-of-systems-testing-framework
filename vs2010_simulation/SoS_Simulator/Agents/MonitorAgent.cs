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
        private List<MessageUnit> _simLog;

        public MonitorAgent()
        {
            _simLog = new List<MessageUnit>();
        }

        protected override void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
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
                case MESSAGE_TYPE.SIMULATION_COMPLETE:
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

        public MessageUnit[] GetSimulationLog()
        {
            return _simLog.ToArray();
        }

        protected override void OnTick()
        {
            
        }
    }
}
