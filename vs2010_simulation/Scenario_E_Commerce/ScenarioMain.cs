using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator;
using Scenario_E_Commerce.Agents;

namespace Scenario_E_Commerce
{
    public class ScenarioMain : Simulator
    {
        Amazon _amazon;
        Customer _customer;
        Dell _dell;
        UPS _ups;
        Visa _visa;

        protected override void Initialize()
        {
            _amazon = new Amazon(this);
            _customer = new Customer(this);
            _dell = new Dell(this);
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
