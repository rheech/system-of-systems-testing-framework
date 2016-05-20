using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoS_Simulator.Objects
{
    public abstract class SoS_Object
    {
        private delegate void TickEventHandler();
        private static event TickEventHandler Tick;

        public SoS_Object()
        {
            SoS_Object.Tick += this.OnTickInternal;
        }

        ~SoS_Object()
        {
            SoS_Object.Tick -= this.OnTickInternal;
        }

        public SoS_Object(int positionX)
        {
            
        }

        public static void ResetEventHandler()
        {
            Tick = null;
        }

        public static void RaiseTick()
        {
            if (Tick != null)
            {
                Tick();
            }
        }

        private void OnTickInternal()
        {
            OnTick();
        }

        protected virtual void OnTick()
        {
        }
    }
}
