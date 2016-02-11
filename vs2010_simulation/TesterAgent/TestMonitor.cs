using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TesterAgent
{
    public delegate void TestMethod(string aa);

    public class TestMonitor
    {
        TestMethod _method;

        public TestMonitor(TestMethod aa)
        {
            _method = aa;
        }
    }
}
