using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_SmartHomeSystem.Abstract
{
    public class SmartHome_Agent : Agent
    {
        public SmartHome_Agent(ScenarioMain simulator)
            : base(simulator)
        {
            
        }

        public ScenarioMain Simulation
        {
            get
            {
                return base.Simulation<ScenarioMain>();
            }
        }
    }
}
