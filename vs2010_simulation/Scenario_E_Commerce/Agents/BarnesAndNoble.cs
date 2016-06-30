using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_E_Commerce.Agents
{
    public class BarnesAndNoble : Agent
    {
        public BarnesAndNoble(ScenarioMain simulator)
            : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "OrderRequest":
                    SendMessage(typeof(UPS), "RequestDelivery");
                    break;
                case "DeliverStarted":
                    SendMessage(typeof(Amazon), "DeliveryProcessed");
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
