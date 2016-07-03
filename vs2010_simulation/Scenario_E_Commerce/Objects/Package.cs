using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scenario_E_Commerce.Objects
{
    public class Package : List<Product>
    {
        public Package()
            : base()
        {
        }

        public Package(Package package)
            : base(package.ToArray())
        {
        }
    }
}
