using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCI_Bus_Simulator.Objects;

namespace MCI_Bus_Simulator.Agents
{
    public class EmergencyCallCenter : Agent
    {
        private int _reportedNumOfPatients;
        private int _savedPatients;
        private Disaster _disaster;

        public EmergencyCallCenter()
        {
        }

        protected override void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            switch (msgType)
            {
                case MESSAGE_TYPE.INCIDENT_REPORT:
                    SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.ASSIGN_AMBULANCE, Simulator.patients.Count, _disaster.X);
                    break;
                case MESSAGE_TYPE.ASSIGN_AMBULANCE:
                case MESSAGE_TYPE.CHECK_MORE_PATIENTS:
                    if (Simulator.patients.Count > 0)
                    {
                        SendMessage(typeof(Ambulance), MESSAGE_TYPE.RESCUE_REQUEST, Simulator.patients.Count, _disaster.X);
                    }
                    else
                    {
                        SendMessage(typeof(Agent), MESSAGE_TYPE.RESCUE_COMPLETE, Simulator.patients.Count);
                    }
                    break;
                default:
                    break;
            }
        }

        public void ReportDisaster(Disaster disaster)
        {
            _disaster = disaster;
            SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.INCIDENT_REPORT, _disaster.X);
        }
    }
}