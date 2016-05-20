using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoS_Simulator.Agents
{
    public class RescueVehicle : Agent
    {
        protected override void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            switch (msgType)
            {
                case MESSAGE_TYPE.DispatchCommand:
                    SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.DeclareMCI);

                    SendMessage(typeof(RescueVehicle), MESSAGE_TYPE.AssignTriagePosition);
                    SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.AssignTreatmentPosition);
                    SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.AssignTransportationPosition);
                    break;
                case MESSAGE_TYPE.AssignTriagePosition:
                    SendMessage(typeof(RescueVehicle), MESSAGE_TYPE.ProvidePatientCount);
                    break;
                default:
                    break;
            }
        }

        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}
