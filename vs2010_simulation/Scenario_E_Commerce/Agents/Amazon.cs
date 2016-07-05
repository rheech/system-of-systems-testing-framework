using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_E_Commerce.Objects;

namespace Scenario_E_Commerce.Agents
{
    public class Amazon : Agent
    {
        List<Product> _stock_barnesAndNoble;
        Package _user_cart;

        public Amazon(ScenarioMain simulator) : base(simulator)
        {
            Product tempProduct;
            _stock_barnesAndNoble = new List<Product>();
            _user_cart = new Package();

            // We assume that Amazon already have the stock information of the vendor (Barnes and Noble)
            tempProduct = new Product();
            tempProduct.Name = "Software Engineering (10th Edition)";
            tempProduct.Price = 165.20;
            _stock_barnesAndNoble.Add(tempProduct);

            tempProduct = new Product();
            tempProduct.Name = "Introduction to Programming Using C#";
            tempProduct.Price = 24.95;
            _stock_barnesAndNoble.Add(tempProduct);

            tempProduct = new Product();
            tempProduct.Name = "Introduction to Programming Using C++";
            tempProduct.Price = 30.99;
            _stock_barnesAndNoble.Add(tempProduct);
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "SearchProduct": // Search the product that the customer has requested
                    SearchProduct((Product)info[0]);
                    break;
                case "AddCart": // Add cart request from the customer
                    AddToCart((Product)info[0]);
                    // Add to cart and return success
                    SendMessage(typeof(Customer), "AddCartSucceed");
                    break;
                case "OneClickOrder": // One click order
                    ProcessOneClickOrder((Product)info[0], (Card)info[1]);
                    break;
                case "MakePayment": // Customer requests payment for processing order
                    MakePayment((Card)info[0], new Package(_user_cart));
                    // Clear the cart after the order
                    _user_cart.Clear();
                    break;
                case "ChargeComplete": // Payment success from Visa
                    // Send order to Barnes and Noble
                    ProcessOrderToVendor((Package)info[0]);
                    break;
                case "DeliveryProcessed": // The book company shipped the package
                    // Notify to the customer that the product is shipped
                    SendMessage(typeof(Customer), "OrderShipped");
                    break;
                case "Delivered": // Package delivered
                    // Notify customer
                    SendMessage(typeof(Customer), "OrderDelivered", (Package)info[0]);
                    break;
                default:
                    break;
            }
        }

        private void SearchProduct(Product product)
        {
            // Find the requested product in barnes and noble (hard coded)
            foreach (Product p in _stock_barnesAndNoble)
            {
                if (p == product)
                {
                    // if found, return the found book
                    SendMessage(typeof(Customer), "SearchProductReturn", p);
                    return;
                }
            }

            // if not found, return null
            SendMessage(typeof(Customer), "SearchProductReturn", null);
        }

        private void AddToCart(Product product)
        {
            _user_cart.Add(product);
        }

        private void ProcessOneClickOrder(Product product, Card creditCard)
        {
            Package package;

            SendMessage(typeof(Customer), "ProcessingOneClickOrder");

            package = new Package();
            package.Add(product);

            SendMessage(typeof(Visa), "ChargeRequest", creditCard, product.Price, package);
        }

        private void MakePayment(Card creditCard, Package package)
        {
            double totalAmount;

            totalAmount = 0;

            // Sum up all prices
            foreach (Product p in _user_cart)
            {
                totalAmount += p.Price;
            }

            // Send payment info to the credit card company
            SendMessage(typeof(Visa), "ChargeRequest", creditCard, totalAmount, package);
        }

        private void ProcessOrderToVendor(Package package)
        {
            // Send order to Barnes and Noble (book company)
            SendMessage(typeof(BarnesAndNoble), "OrderRequest", package);

            // Notify customer that the order is charged successfully
            SendMessage(typeof(Customer), "OrderCharged");
        }

        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}
