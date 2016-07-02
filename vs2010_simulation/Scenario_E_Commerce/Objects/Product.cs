using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scenario_E_Commerce.Objects
{
    public class Product
    {
        public string Name;
        public double Price;

        public static bool operator ==(Product a, Product b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Product a, Product b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool bEquals;
            Product prd;

            if (obj is Product)
            {
                prd = (Product)obj;
                bEquals = (Name == prd.Name && Price == prd.Price);

                return bEquals;
            }

            return base.Equals(obj);
        }
    }
}
