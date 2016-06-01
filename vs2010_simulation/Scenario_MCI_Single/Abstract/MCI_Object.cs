using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;

namespace Scenario_MCI_Single.Abstract
{
    public abstract class MCI_Object : SoS_Object
    {
        public MCI_Object(ScenarioMain simulator) : base(simulator)
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
