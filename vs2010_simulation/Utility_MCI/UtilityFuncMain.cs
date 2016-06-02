using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator;
using SoS_Simulator.UtilityFunc;
using Scenario_MCI;

namespace Utility_MCI
{
    public class UtilityFuncMain : UtilityFuncLib
    {
        public UtilityFuncMain(Simulator simulator)
            : base(simulator)
        {
            if (_simulator.GetType() != typeof(ScenarioMain))
            {
                throw new ApplicationException("This library is not an MCI simulator.");
            }
        }

        public override bool CheckGoalAccomplishment(string goalName)
        {
            switch (goalName)
            {
                case "RescuePatient":
                    return RescuePatient();
                default:
                    break;
            }

            return false;
        }

        private bool RescuePatient()
        {
            if (Simulation.patients != null)
            {
                return (Simulation.patients.Count != 0);
            }

            return false;
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
