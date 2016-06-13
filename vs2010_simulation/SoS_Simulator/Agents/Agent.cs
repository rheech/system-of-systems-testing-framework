using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;
using System.Windows.Forms;

namespace SoS_Simulator.Agents
{
    public struct AgentMessage
    {
        public Type target;
        public string msgText;
        public object[] info;
    }

    public abstract class Agent : SoS_Object
    {
        private delegate void MessageEventHandler(object from, Type target, string msgText, params object[] info);
        private static event MessageEventHandler MessageReceived;
        private Queue<AgentMessage> _messageQueue;
        private Random r;

        public Agent(Simulator simulator) : base(simulator)
        {
            r = new Random();
            _messageQueue = new Queue<AgentMessage>();
            Agent.MessageReceived += this.OnMessageReceivedInternal;
        }

        ~Agent()
        {
            Agent.MessageReceived -= this.OnMessageReceivedInternal;
        }

        public static new void ResetEventHandler()
        {
            MessageReceived = null;
            SoS_Object.ResetEventHandler();
        }

        private static void SendMessageInternal(object from, Type target, string msgText, params object[] info)
        {
            if (MessageReceived != null)
            {
                MessageReceived(from, target, msgText, info);
            }
        }

        public void SendMessage(Type target, string msgText, params object[] info)
        {
            AgentMessage msg;

            msg = new AgentMessage();
            msg.target = target;
            msg.msgText = msgText;
            msg.info = info;

            _messageQueue.Enqueue(msg);
        }

        private void OnMessageReceivedInternal(object from, Type target, string msgText, params object[] info)
        {
            if (target == this.GetType() ||
                    target.Name == "Agent" ||
                    this.GetType().Name == "MonitorAgent") // if monitoring agent, receive all
            {
                // if (r.Next(1, 101) < 95) // 메시지가 90% 의 확률로 전달됨.
                {
                    OnMessageReceived(from, target, msgText, info);
                }
                //else
                {
                    //SimulationComplete(true);
                }
            }
        }

        protected override void OnTick()
        {
            AgentMessage msg;

            if (_messageQueue.Count > 0)
            {
                msg = _messageQueue.Dequeue();
                SendMessageInternal(this, msg.target, msg.msgText, msg.info);
            }
        }

        protected virtual void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
        }

        protected void SimulationComplete(bool error)
        {
            SendMessage(typeof(Agent), "SIMULATION_COMPLETE");
        }
    }
}
