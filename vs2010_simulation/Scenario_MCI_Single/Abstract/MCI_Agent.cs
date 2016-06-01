using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_MCI_Single.Abstract
{
    public abstract class MCI_Agent : Agent
    {
        public MCI_Agent(ScenarioMain simulator) : base(simulator)
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
