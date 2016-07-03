using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_E_Commerce.Objects;

namespace Scenario_E_Commerce.Agents
{
    public class Visa : Agent
    {
        private struct CardInfo
        {
            public string Name;
            public string Number;
            public double CumulatedAmount;
            public double CreditLimit;
        }

        CardInfo[] _creditCards;

        public Visa(ScenarioMain simulator)
            : base(simulator)
        {
            CardInfo ci;

            ci = new CardInfo();
            ci.CreditLimit = 200.0;
            ci.CumulatedAmount = 0;
            ci.Name = "John Doe";
            ci.Number = "12345";

            _creditCards = new CardInfo[] { ci };
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "ChargeRequest": // Amazon requests the transaction for charging customer
                    ProcessPayment((Card)info[0], (double)info[1]);
                    SendMessage(typeof(Amazon), "ChargeComplete", (Package)info[2]);
                    break;
                default:
                    break;
            }
        }

        private void ProcessPayment(Card creditCard, double totalAmount)
        {
            // Check credit limit of the customer's card, and charge the payment
            for (int i = 0; i < _creditCards.Length; i++)
            {
                // If credit card found
                if (_creditCards[i].Name == creditCard.Name &&
                        _creditCards[i].Number == creditCard.Number)
                {
                    if (_creditCards[i].CreditLimit > (_creditCards[i].CumulatedAmount + totalAmount))
                    {
                        // Process payment using the provided credit card
                        _creditCards[i].CumulatedAmount += totalAmount;
                    }
                }
            }
        }

        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}
