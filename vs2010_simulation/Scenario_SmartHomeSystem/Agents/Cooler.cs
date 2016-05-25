using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_SmartHomeSystem.Agents
{
    public class Cooler : Agent
    {
        public Cooler()
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "Heat":
                    //SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.ReportDisaster);
                    SendMessage(typeof(Heater), "Heat");
                    break;
                case "Cool":
                    SendMessage(typeof(Cooler), "Cool");
                    break;
                case "DrawInMoist":
                    SendMessage(typeof(Dehumidifier), "DrawInMoist");
                    break;
                case "ReleaseMoist":
                    SendMessage(typeof(Hygrometer), "ReleaseMoist");
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
