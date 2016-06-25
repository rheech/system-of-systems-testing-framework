using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_MCI.Objects;
using Scenario_MCI.Abstract;

namespace Scenario_MCI.Agents
{
    /// <summary>
    /// Emergency Medical Service (EMS) Vehicle
    /// </summary>
    public class EMSVehicle : MCI_Agent
    {
        bool isDispatched;

        public EMSVehicle(ScenarioMain simulator)
            : base(simulator)
        {
            isDispatched = false;
        }

        /// <summary>
        /// Receive message from other agents
        /// </summary>
        /// <param name="from">Transmitter agent</param>
        /// <param name="target">Receiver agent type</param>
        /// <param name="msgText">Message text in string</param>
        /// <param name="info">Additional parameter</param>
        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "DispatchEMSVehicle":
                    isDispatched = true;
                    break;
                case "RequestPatientTreatment":
                    // check bed availability, then send patients to hospital.
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

        /// <summary>
        /// Check bed availability to hospital
        /// </summary>
        private void CheckBedAvailability()
        {
            SendMessage(typeof(Hospital), "RequestBedAvailability");
        }

        /// <summary>
        /// Request Ambulance to Ambulance Manager
        /// </summary>
        /// <param name="bedAvailability">Number of available beds acquired from Hospital</param>
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

        /// <summary>
        /// Send patient to ambulance for transportation
        /// </summary>
        private void LoadPatientToAmbulance()
        {
            // Remove patient from the global variable, and send to ambulance
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

        /// <summary>
        /// Tick function derived from the parent
        /// </summary>
        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}