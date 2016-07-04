using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.UtilityFunc;
using SoS_Simulator;
using Scenario_E_Commerce;

namespace Utility_E_Commerce
{
    public class UtilityFuncMain : UtilityFuncLib
    {
        public UtilityFuncMain(Simulator simulator)
            : base(simulator)
        {
            if (_simulator.GetType() != typeof(ScenarioMain))
            {
                throw new ApplicationException("This library is not an E-Commerce simulator.");
            }
        }

        public override bool CheckGoalAccomplishment(string goalName)
        {
            switch (goalName)
            {
                case "E-Commerce":
                case "DeliverProduct":
                    return CheckTotalOrdered();
                default:
                    break;
            }

            return true;
        }

        public bool CheckTotalOrdered()
        {
            return (Simulation.ReceivedProducts == Simulation.TotalOrdered);
        }

        public ScenarioMain Simulation
        {
            get
            {
                return (ScenarioMain)_simulator;
            }
        }
    }
}
