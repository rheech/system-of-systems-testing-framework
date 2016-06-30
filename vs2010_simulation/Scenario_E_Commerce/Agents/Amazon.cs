using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_E_Commerce.Agents
{
    public class Amazon : Agent
    {
        public Amazon(ScenarioMain simulator) : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "SearchProduct":
                    SendMessage(typeof(Customer), "SearchProductReturn");
                    break;
                case "ViewProduct":
                    SendMessage(typeof(Customer), "ViewProductReturn");
                    break;
                case "MakePayment":
                    SendMessage(typeof(Visa), "ProcessPayment");
                    break;
                case "PaymentComplete":
                    SendMessage(typeof(BarnesAndNoble), "OrderRequest");
                    SendMessage(typeof(Customer), "OrderComplete");
                    break;
                case "DeliverStarted":
                    SendMessage(typeof(Customer), "OrderShipped");
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
