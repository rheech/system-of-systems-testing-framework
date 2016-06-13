using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_SmartHomeSystem.Abstract;

namespace Scenario_SmartHomeSystem.Agents
{
    public class ControlCenter : SmartHome_Agent
    {
        private double goalTemp, goalHumid;

        public ControlCenter(ScenarioMain simulator)
            : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
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

        public void CheckTemperature()
        {
            SendMessage(typeof(Thermometer), "CheckTemp");
        }

        public void CheckHumidity()
        {
            SendMessage(typeof(Hygrometer), "CheckHumid");
        }

        protected override void OnTick()
        {
            base.OnTick();

            CheckTemperature();
            //CheckHumidity();
        }
    }
}
