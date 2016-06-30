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
        Product product;

        Amazon _amazon;
        Customer _customer;
        BarnesAndNoble _dell;
        UPS _ups;
        Visa _visa;

        protected override void Initialize()
        {
            _amazon = new Amazon(this);
            _customer = new Customer(this);
            _dell = new BarnesAndNoble(this);
            _ups = new UPS(this);
            _visa = new Visa(this);

            
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
