using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TesterAgent
{
    public delegate void TestMethod(string aa);

    public class TestMonitor
    {
        public TestMonitor()
        {
        }

        public void TestProgress(object sender, string methodName)
        {
            Console.WriteLine("{0}.{1} called.", sender.GetType().Name, methodName);
        }
    }
}
