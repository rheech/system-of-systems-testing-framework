using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator;
using Scenario_E_Commerce.Agents;
using Scenario_E_Commerce.Objects;

namespace Scenario_E_Commerce
{
    public class ScenarioMain : Simulator
    {
        Amazon _amazon;
        Customer _customer;
        BarnesAndNoble _dell;
        UPS _ups;
        Visa _visa;

        protected override void Initialize()
        {
            List<Product> productsToBuy;
            Product tempProduct;
            Card creditCard;

            _amazon = new Amazon(this);
            _customer = new Customer(this);
            _dell = new BarnesAndNoble(this);
            _ups = new UPS(this);
            _visa = new Visa(this);

            productsToBuy = new List<Product>();

            // Add books
            tempProduct = new Product();
            tempProduct.Name = "Software Engineering (10th Edition)";
            tempProduct.Price = 165.20;
            productsToBuy.Add(tempProduct);

            tempProduct = new Product();
            tempProduct.Name = "Introduction to Programming Using C#";
            tempProduct.Price = 24.95;
            productsToBuy.Add(tempProduct);

            // Add credit card
            creditCard = new Card();
            creditCard.Number = "12345";
            creditCard.Name = "John Doe";
            creditCard.CreditLimit = 200.00;

        }

        protected override void Run()
        {
            _customer.RequestOrder();
        }

        public override string GetMonitoringText()
        {
            return "Test";
        }
    }
}
