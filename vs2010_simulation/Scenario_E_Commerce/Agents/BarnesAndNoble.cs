using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_E_Commerce.Objects;

namespace Scenario_E_Commerce.Agents
{
    public class BarnesAndNoble : Agent
    {
        List<Product> _books;

        public BarnesAndNoble(ScenarioMain simulator)
            : base(simulator)
        {
            Product tempProduct;

            _books = new List<Product>();

            tempProduct = new Product();
            tempProduct.Name = "Software Engineering (10th Edition)";
            tempProduct.Price = 165.20;
            _books.Add(tempProduct);

            tempProduct = new Product();
            tempProduct.Name = "Introduction to Programming Using C#";
            tempProduct.Price = 24.95;
            _books.Add(tempProduct);

            tempProduct = new Product();
            tempProduct.Name = "Introduction to Programming Using C++";
            tempProduct.Price = 30.99;
            _books.Add(tempProduct);
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "OrderRequest": // Shipment request from Amazon
                    // Request delivery to UPS
                    SendMessage(typeof(UPS), "RequestDelivery", (Package)info[0]);
                    break;
                case "DeliverStarted": // Delivery start notice from UPS
                    // Notify Amazon that the delivery has started
                    SendMessage(typeof(Amazon), "DeliveryProcessed");
                    break;
                case "DeliverFinished": // Delivery finishe notice from UPS
                    // Notify Amazon
                    SendMessage(typeof(Amazon), "Delivered");
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
