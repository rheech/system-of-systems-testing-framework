using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_MCI.Agents
{
    public class RescueVehicle : Agent
    {
        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "DispatchCommand":
                    SendMessage(typeof(EmergencyCallCenter), "DeclareMCI");

                    SendMessage(typeof(RescueVehicle), "AssignTriagePosition");
                    SendMessage(typeof(EMSVehicle), "AssignTreatmentPosition");
                    SendMessage(typeof(EMSVehicle), "AssignTransportationPosition");
                    break;
                case "AssignTriagePosition":
                    SendMessage(typeof(RescueVehicle), "ProvidePatientCount");
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
