using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_E_Commerce.Objects;

namespace Scenario_E_Commerce.Agents
{
    public class UPSDriver : Agent
    {
        public UPSDriver(ScenarioMain simulator)
            : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "DeliverPackage": // Delivery request from UPS
                    SendMessage(typeof(UPS), "DeliverSuccess", (Package)info[0]);
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
