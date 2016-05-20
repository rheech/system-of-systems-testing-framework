using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;
using System.Windows.Forms;
using TesterAgent;

namespace SoS_Simulator.Agents
{
    public enum MESSAGE_TYPE
    {
        ReportDisaster,
        DispatchCommand,
        DeclareMCI,
        AssignTriagePosition,
        ProvidePatientCount,
        AssignTreatmentPosition,
        AssignTransportationPosition,
        ProvidePatientTransportStatus,
        CoordinateTransport,
        ProvideAmbulanceDestination,
        RequestAmbulance,
        TransportComplete,
        CheckBedAvailability,
        ProvideBedAvailability,


        NOTIFY,
        INCIDENT_REPORT,
        ASSIGN_AMBULANCE,
        RESCUE_REQUEST,
        PATIENT_PICKEDUP,
        FIND_HOSPITAL,
        HOSPITAL_LOCATION,
        PATIENT_ARRIVAL,
        CHECK_MORE_PATIENTS,
        RESCUE_COMPLETE,
        SIMULATION_COMPLETE
    }

    public struct AgentMessage
    {
        public Type target;
        public MESSAGE_TYPE msgType;
        public object[] info;
    }

    public abstract class Agent : MCI_Object
    {
        private delegate void MessageEventHandler(object from, Type target, MESSAGE_TYPE msgType, params object[] info);
        private static event MessageEventHandler MessageReceived;
        private Queue<AgentMessage> _messageQueue;
        Random r;

        public Agent()
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
            AgentMessage msg;

            msg = new AgentMessage();
            msg.target = target;
            msg.msgType = msgType;
            msg.info = info;

            _messageQueue.Enqueue(msg);
        }

        private void OnMessageReceivedInternal(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            if (target == this.GetType() ||
                    target.Name == "Agent" ||
                    this.GetType().Name == "MonitorAgent") // if monitoring agent, receive all
            {
                // if (r.Next(1, 101) < 95) // 메시지가 90% 의 확률로 전달됨.
                {
                    OnMessageReceived(from, target, msgType, info);
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
                SendMessageInternal(this, msg.target, msg.msgType, msg.info);
            }
        }

        protected virtual void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
        }

        protected void SimulationComplete(bool error)
        {
            SendMessage(typeof(Agent), MESSAGE_TYPE.SIMULATION_COMPLETE);
        }
    }
}
