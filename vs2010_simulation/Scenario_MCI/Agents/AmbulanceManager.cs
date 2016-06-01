using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_MCI.Abstract;
using Scenario_MCI.Objects;

namespace Scenario_MCI.Agents
{
    public class AmbulanceManager : MCI_Agent
    {
        Patient _currentPatient;

        public AmbulanceManager(ScenarioMain simulator) : base(simulator)
        {
        }

        public void PickupPatient(Patient pt)
        {
            _currentPatient = pt;
        }

        public Patient ReleasePatient(Patient pt)
        {
            Patient rtnPatient;
            rtnPatient = _currentPatient;
            
            _currentPatient = null;

            return rtnPatient;
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "RequestAmbulance":
                    SendMessage(typeof(EMS_Manager), "TransportComplete");
                    Console.WriteLine(Simulation.disaster);
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
