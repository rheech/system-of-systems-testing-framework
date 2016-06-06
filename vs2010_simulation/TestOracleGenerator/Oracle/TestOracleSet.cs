using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestOracleGenerator.Oracle
{
    public class TestOracleSet : List<TestOracle>
    {
        private static Predicate<TestOracle> ByGoalName(string goalName)
        {
            return delegate(TestOracle testOracle)
            {
                return testOracle.GoalName == goalName;
            };
        }

        public TestOracle FindTestOracleByGoal(string goalName)
        {
            return this.Find(ByGoalName(goalName));
        }
    }
}
