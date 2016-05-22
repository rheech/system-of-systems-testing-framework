using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_MCI.Agents
{
    public class EMSVehicle : Agent
    {
        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "AssignTreatmentPosition":
                    SendMessage(typeof(EmergencyCallCenter), "ProvidePatientTransportStatus");
                    break;
                /*case MESSAGE_TYPE.AssignTreatmentPosition:

                    break;*/
                case "AssignTransportationPosition":
                    SendMessage(typeof(EMSVehicle), "CoordinateTransport");

                    SendMessage(typeof(EMSVehicle), "ProvidePatientTransportStatus");
                    break;
                case "CoordinateTransport":
                    SendMessage(typeof(Hospital), "CheckBedAvailability");
                    break;//SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.ProvideAmbulanceDestination);
                case "ProvideBedAvailability":
                    SendMessage(typeof(EMSVehicle), "ProvideAmbulanceDestination");
                    break;
                case "ProvideAmbulanceDestination":
                    SendMessage(typeof(Ambulance), "RequestAmbulance");
                    break;
                case "TransportComplete":
                    SendMessage(typeof(Agent), "SIMULATION_COMPLETE");
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
