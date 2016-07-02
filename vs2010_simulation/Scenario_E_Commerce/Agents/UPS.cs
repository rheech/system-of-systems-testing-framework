using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_E_Commerce.Objects;

namespace Scenario_E_Commerce.Agents
{
    public class UPS : Agent
    {
        public UPS(ScenarioMain simulator)
            : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "RequestDelivery": // Delivery requested from vendor
                    ProcessDelivery((Package)info[0]);
                    break;
                case "DeliverSuccess": // Package delivered (notified by the UPS driver)
                    SendMessage(typeof(BarnesAndNoble), "DeliverFinished", (Package)info[0]);
                    break;
                default:
                    break;
            }
        }

        private void ProcessDelivery(Package package)
        {
            // Send package information to the UPS driver
            SendMessage(typeof(UPSDriver), "DeliverPackage", package);
            // Notify shipment
            SendMessage(typeof(BarnesAndNoble), "DeliverStarted", package);
        }

        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}
