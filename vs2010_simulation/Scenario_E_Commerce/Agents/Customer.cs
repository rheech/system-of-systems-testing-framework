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

        public Customer(ScenarioMain simulator)
            : base(simulator)
        {

        }

        public void BuyProduct(Product[] productsToBuy, Card creditCard)
        {
            _productToBuy = new List<Product>(productsToBuy);
            _creditCard = creditCard;

            CheckNextProductToBuy();
        }

        public void OneClickOrder(Product productToBuy, Card creditCard)
        {
            _creditCard = creditCard;

            SendMessage(typeof(Amazon), "OneClickOrder", productToBuy, creditCard);
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "SearchProductReturn":
                    // If book is found (with the book info), add to cart
                    if (info != null)
                    {
                        AddToCart((Product)info[0]);
                    }
                    break;
                case "AddCartSucceed":
                    // If adding cart is succeed, check for another one or proceed to checkout
                    if (_productToBuy.Count > 0)
                    {
                        // Search for the next product to buy
                        CheckNextProductToBuy();
                    }
                    else // if everything is found
                    {
                        // Proceed to checkout (pay the price)
                        SendMessage(typeof(Amazon), "MakePayment", _creditCard);
                    }
                    break;
                case "OrderCharged":
                case "OrderShipped":
                case "OrderDelivered":
                    // Order is complete. Nothing to do for now.
                    break;
                default:
                    break;
            }
        }

        private void AddToCart(Product product)
        {
            // if the product is the one the customer was looking for
            if (product == _lastProduct)
            {
                SendMessage(typeof(Amazon), "AddCart", product);
            }
        }

        private void CheckNextProductToBuy()
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
