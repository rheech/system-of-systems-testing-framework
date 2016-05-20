using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoS_Simulator.Agents
{
    public class EMSVehicle : Agent
    {
        protected override void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            switch (msgType)
            {
                case MESSAGE_TYPE.AssignTreatmentPosition:
                    SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.ProvidePatientTransportStatus);
                    break;
                /*case MESSAGE_TYPE.AssignTreatmentPosition:

                    break;*/
                case MESSAGE_TYPE.AssignTransportationPosition:
                    SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.CoordinateTransport);

                    SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.ProvidePatientTransportStatus);
                    break;
                case MESSAGE_TYPE.CoordinateTransport:
                    SendMessage(typeof(Hospital), MESSAGE_TYPE.CheckBedAvailability);
                    break;//SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.ProvideAmbulanceDestination);
                case MESSAGE_TYPE.ProvideBedAvailability:
                    SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.ProvideAmbulanceDestination);
                    break;
                case MESSAGE_TYPE.ProvideAmbulanceDestination:
                    SendMessage(typeof(Ambulance), MESSAGE_TYPE.RequestAmbulance);
                    break;
                case MESSAGE_TYPE.TransportComplete:
                    SendMessage(typeof(Agent), MESSAGE_TYPE.SIMULATION_COMPLETE);
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
