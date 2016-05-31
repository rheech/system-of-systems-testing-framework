using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;

namespace Scenario_SmartHomeSystem.Abstract
{
    public class SmartHome_Object : SoS_Object
    {
        public SmartHome_Object(ScenarioMain simulator)
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
