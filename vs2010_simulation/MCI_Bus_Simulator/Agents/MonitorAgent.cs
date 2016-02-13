using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCI_Bus_Simulator.Agents
{
    public class MonitorAgent : Agent
    {
        public delegate void TextUpdateHandler(string text);
        public event TextUpdateHandler OnTextUpdate;

        public MonitorAgent()
        {
        }

        protected override void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            Console.WriteLine(msgType.ToString());

            if (OnTextUpdate != null)
            {
                OnTextUpdate(from.GetType().Name.ToString() + "->" + target.Name.ToString() + "." + msgType.ToString());
            }
        }
    }
}
