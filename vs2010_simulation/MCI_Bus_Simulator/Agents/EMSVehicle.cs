using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCI_Bus_Simulator.Agents
{
    public class EMSVehicle : Agent
    {
        protected override void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            switch (msgType)
            {
                case MESSAGE_TYPE.NOTIFY:
                    SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.INCIDENT_REPORT, Simulator.patients.Count, _disaster.X);
                    break;
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
                        SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.RESCUE_COMPLETE, Simulator.patients.Count);
                    }
                    break;
                case MESSAGE_TYPE.RESCUE_COMPLETE:
                    SimulationComplete(false);
                    break;
                default:
                    break;
            }
        }
    }
}
