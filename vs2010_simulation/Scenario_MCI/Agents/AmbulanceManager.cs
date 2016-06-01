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
        bool isDispatched;

        public AmbulanceManager(ScenarioMain simulator)
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
                    // Request ambulance to standby in the staging area
                    SendMessage(typeof(Ambulance), "RequestStandBy");
                    break;
                case "RequestAmbulance":
                    // Dispatch ambulance to the field
                    SendMessage(typeof(Ambulance), "DispatchAmbulance");
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
