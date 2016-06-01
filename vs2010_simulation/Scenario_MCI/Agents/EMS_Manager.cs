using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_MCI.Objects;
using Scenario_MCI.Abstract;

namespace Scenario_MCI.Agents
{
    public class EMS_Manager : MCI_Agent
    {
        bool isDispatched;

        public EMS_Manager(ScenarioMain simulator)
            : base(simulator)
        {
            isDispatched = false;
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "DispatchCommand":
                    isDispatched = true;
                    break;
                case "RequestPatientTreatment":
                    // check bed availability, then send patients to hospital...
                    CheckBedAvailability();
                    break;
                case "ProvideBedAvailability":
                    RequestAmbulance((int)info[0]);
                    break;
                case "FieldArrivalReport": // Ambulance is arrived
                    LoadPatientToAmbulance();
                    break;
                default:
                    break;
            }
        }

        private void CheckBedAvailability()
        {
            SendMessage(typeof(Hospital), "RequestBedAvailability");
        }

        private void RequestAmbulance(int bedAvailability)
        {
            if (Simulation.patients.Count > 0)
            {
                // if there bed available
                if (bedAvailability > 0)
                {
                    SendMessage(typeof(AmbulanceManager), "RequestAmbulance");

                    // if last patient
                    if (Simulation.patients.Count == 1)
                    {
                        // if no more patient, report complete
                        SendMessage(typeof(RescueVehicle), "TreatmentComplete");
                    }
                }
            }
        }

        private void LoadPatientToAmbulance()
        {
            Patient p = Simulation.patients[0];
            Simulation.patients.Remove(p);

            SendMessage(typeof(Ambulance), "RequestPatientTransfer", p);

	        // if more patients, perform medical treatment again
            if (Simulation.patients.Count > 0)
            {
                CheckBedAvailability();
            }
            else
            {
                // if no more patient, declare completion
                SendMessage(typeof(RescueVehicle), "TransportComplete");
            }
        }

        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}
