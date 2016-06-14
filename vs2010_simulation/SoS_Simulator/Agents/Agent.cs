using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;
using System.Windows.Forms;

namespace SoS_Simulator.Agents
{
    /// <summary>
    /// 
    /// </summary>
    public struct AgentMessage
    {
        public Type target;
        public string msgText;
        public object[] info;
    }

    /// <summary>
    /// An Abstract autonomous Agent class to simulate constituent systems in SoS
    /// </summary>
    public abstract class Agent : SoS_Object
    {
        private delegate void MessageEventHandler(object from, Type target, string msgText, params object[] info);
        private static event MessageEventHandler MessageReceived;
        private Queue<AgentMessage> _messageQueue;

        /// <summary>
        /// Create an instance of Agent
        /// </summary>
        /// <param name="simulator">A simulator related to the Agent</param>
        public Agent(Simulator simulator) : base(simulator)
        {
            _messageQueue = new Queue<AgentMessage>();
            Agent.MessageReceived += this.OnMessageReceivedInternal;
        }

        /// <summary>
        /// 
        /// </summary>
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
                OnMessageReceived(from, target, msgText, info);
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
    }
}
