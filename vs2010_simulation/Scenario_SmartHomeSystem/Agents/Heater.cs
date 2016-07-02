using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_SmartHomeSystem.Abstract;

namespace Scenario_SmartHomeSystem.Agents
{
    public class Heater : SmartHome_Agent
    {
        public Heater(ScenarioMain simulator)
            : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "Heat":
                    //SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.ReportDisaster);
                    //SendMessage(typeof(Heater), "Heat");
                    Simulation.room.temperature += 0.05;
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
