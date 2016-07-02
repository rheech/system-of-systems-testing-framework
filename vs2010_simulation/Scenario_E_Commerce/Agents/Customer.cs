using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_E_Commerce.Objects;

namespace Scenario_E_Commerce.Agents
{
    public class Customer : Agent
    {
        List<Product> _productToBuy;
        Card _creditCard;
        Product _lastProduct;

        public Customer(ScenarioMain simulator, Product[] productsToBuy, Card creditCard)
            : base(simulator)
        {
            _productToBuy = new List<Product>(productsToBuy);
            _creditCard = creditCard;
        }

        public void BuyProduct()
        {
            // If there is a product to purchase
            if (_productToBuy.Count > 0)
            {
                // Take it, and search it using Amazon
                _lastProduct = _productToBuy[0];
                _productToBuy.RemoveAt(0);
                SendMessage(typeof(Amazon), "SearchProduct", _lastProduct);
            }
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

        public Card CreditCard
        {
            get
            {
                return _creditCard;
            }
        }

        public Product[] ProductsToBuy
        {
            get
            {
                if (_productToBuy != null)
                {
                    return _productToBuy.ToArray();
                }

                return null;
            }
        }
    }
}
