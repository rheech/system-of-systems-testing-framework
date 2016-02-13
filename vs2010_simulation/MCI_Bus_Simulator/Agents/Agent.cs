using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCI_Bus_Simulator.Objects;
using System.Windows.Forms;
using TesterAgent;

namespace MCI_Bus_Simulator.Agents
{
    public enum MESSAGE_TYPE
    {
        DISASTER_REPORT,
        ASSIGN_AMBULANCE,
        PATIENT_PICKEDUP,
        HOSPITAL_LOCATION,
        PATIENT_ARRIVAL,
        CHECK_MORE_PATIENTS,
        REMAINING_PATIENTS,
        RESCUE_COMPLETE
    }

    public abstract class Agent : MCI_Object
    {
        private delegate void MessageEventHandler(object from, Type target, MESSAGE_TYPE msgType, params object[] info);
        private static event MessageEventHandler MessageReceived;
        Random r;

        public Agent()
        {
            r = new Random();
            Agent.MessageReceived += this.OnMessageReceivedInternal;
        }

        ~Agent()
        {
            Agent.MessageReceived -= this.OnMessageReceivedInternal;
        }


        public static new void ResetEventHandler()
        {
            MessageReceived = null;
            MCI_Object.ResetEventHandler();
        }

        private static void SendMessageInternal(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            if (MessageReceived != null)
            {
                MessageReceived(from, target, msgType, info);
            }
        }

        public void SendMessage(Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            SendMessageInternal(this, target, msgType, info);
        }

        private void OnMessageReceivedInternal(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            if (target == this.GetType() ||
                    target.Name == "Agent" ||
                    this.GetType().Name == "MonitorAgent") // if monitoring agent, receive all
            {
                //if (r.Next(0, 100) < 90) // 메시지가 90% 의 확률로 전달됨.
                {
                    OnMessageReceived(from, target, msgType, info);
                }
            }
        }

        protected virtual void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
        }
    }
}
