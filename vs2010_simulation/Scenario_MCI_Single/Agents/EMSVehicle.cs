using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_MCI_Single.Objects;
using Scenario_MCI_Single.Abstract;

namespace Scenario_MCI_Single.Agents
{
    public class EMSVehicle : MCI_Agent
    {
        Patient[] patients;
        int numOfAvailableBeds;

        public EMSVehicle(ScenarioMain simulator) : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "AssignTreatmentPosition":
                    SendMessage(typeof(EmergencyCallCenter), "ProvidePatientTransportStatus");
                    break;
                /*case MESSAGE_TYPE.AssignTreatmentPosition:

                    break;*/
                case "AssignTransportationPosition":
                    SendMessage(typeof(EMSVehicle), "CoordinateTransport");
                    break;
                case "CoordinateTransport":
                    SendMessage(typeof(Hospital), "CheckBedAvailability");
                    break;//SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.ProvideAmbulanceDestination);
                case "ProvideBedAvailability":
                    numOfAvailableBeds = (int)info[0];

                    // if number of beds available
                    if (numOfAvailableBeds > 0)
                    {
                        // Provide ambulance destination with patient info
                        SendMessage(typeof(EMSVehicle), "ProvideAmbulanceDestination");
                    }

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
