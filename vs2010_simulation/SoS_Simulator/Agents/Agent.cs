using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;
using System.Windows.Forms;

namespace SoS_Simulator.Agents
{
    /// <summary>
    /// AgentMessage structure
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
        /// Destructor for Agent class
        /// </summary>
        ~Agent()
        {
            // Remove from message event handler
            Agent.MessageReceived -= this.OnMessageReceivedInternal;
        }

        /// <summary>
        /// Reset all event handlers (overrides the base method)
        /// </summary>
        public static new void ResetEventHandler()
        {
            MessageReceived = null;
            SoS_Object.ResetEventHandler();
        }

        /// <summary>
        /// Send message to agents
        /// </summary>
        /// <param name="target">Receiver agent</param>
        /// <param name="msgText">Message text in string</param>
        /// <param name="info">Additional parameter</param>
        public void SendMessage(Type target, string msgText, params object[] info)
        {
            AgentMessage msg;

            msg = new AgentMessage();
            msg.target = target;
            msg.msgText = msgText;
            msg.info = info;

            // Put message to message queue
            _messageQueue.Enqueue(msg);
        }

        /// <summary>
        /// An internal method of SendMessage based on queue
        /// </summary>
        /// <param name="from">Transmitter agent</param>
        /// <param name="target">Receiver agent type</param>
        /// <param name="msgText">Message text in string</param>
        /// <param name="info">Additional parameter</param>
        private static void SendMessageInternal(object from, Type target, string msgText, params object[] info)
        {
            if (MessageReceived != null)
            {
                MessageReceived(from, target, msgText, info);
            }
        }


        /// <summary>
        /// Message receive event handler
        /// </summary>
        /// <param name="from">Transmitter agent</param>
        /// <param name="target">Receiver agent type</param>
        /// <param name="msgText">Message text in string</param>
        /// <param name="info">Additional parameter</param>
        private void OnMessageReceivedInternal(object from, Type target, string msgText, params object[] info)
        {
            if (target == this.GetType() ||
                    target.Name == "Agent" ||
                    this.GetType().Name == "MonitorAgent") // if monitoring agent, receive all
            {
                OnMessageReceived(from, target, msgText, info);
            }
        }

        /// <summary>
        /// Execute agent in accordance to the instruction cycle
        /// </summary>
        protected override void OnTick()
        {
            AgentMessage msg;

            // Process message queue
            if (_messageQueue.Count > 0)
            {
                msg = _messageQueue.Dequeue();
                SendMessageInternal(this, msg.target, msg.msgText, msg.info);
            }
        }

        /// <summary>
        /// Overridable function of receive message event for derived agents
        /// </summary>
        /// <param name="from">Transmitter agent</param>
        /// <param name="target">Receiver agent type</param>
        /// <param name="msgText">Message text in string</param>
        /// <param name="info">Additional parameter</param>
        protected virtual void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
        }
    }
}
