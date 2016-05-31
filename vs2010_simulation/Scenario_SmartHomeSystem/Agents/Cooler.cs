using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_SmartHomeSystem.Abstract;

namespace Scenario_SmartHomeSystem.Agents
{
    public class Cooler : SmartHome_Agent
    {
        public Cooler(ScenarioMain simulator)
            : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "Heat":
                    //SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.ReportDisaster);
                    SendMessage(typeof(Heater), "Heat");
                    Simulation.room.temperature += 0.05;
                    break;
                case "Cool":
                    SendMessage(typeof(Cooler), "Cool");
                    Simulation.room.temperature -= 0.05;
                    break;
                case "DrawInMoist":
                    SendMessage(typeof(Dehumidifier), "DrawInMoist");
                    Simulation.room.humidity -= 0.05;
                    break;
                case "ReleaseMoist":
                    SendMessage(typeof(Hygrometer), "ReleaseMoist");
                    Simulation.room.humidity += 0.05;
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
