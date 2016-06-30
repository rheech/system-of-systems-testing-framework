using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_E_Commerce.Agents
{
    public class Customer : Agent
    {
        public Customer(ScenarioMain simulator)
            : base(simulator)
        {
        }

        public void RequestOrder()
        {
            SendMessage(typeof(Amazon), "SearchProduct");
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "SearchProductReturn":
                    SendMessage(typeof(Amazon), "ViewProduct");
                    break;
                case "ViewProductReturn":
                    SendMessage(typeof(Amazon), "MakePayment");
                    break;
                case "OrderComplete":
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
