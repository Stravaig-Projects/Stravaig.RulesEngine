using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Stravaig.RulesEngine.Tests.Integration
{
    public partial class RuleTests //_IsContainedIn
    {
        private static KeyValuePair<string, RuleSet>[] IsContainedInRuleSets =>
            new KeyValuePair<string, RuleSet>[]
            {
                new(nameof(IntIsContainedIn), new RuleSet(new Rule("SomeNumber", "IsContainedIn", "1|3|5"))), 
                new(nameof(IntIsNotContainedIn), new RuleSet(new Rule("SomeNumber", "IsNotContainedIn", "8|6|4"))), 
                new(nameof(DateIsContainedIn), new RuleSet(new Rule("SomeDate", "IsContainedIn", "2021-08-01|2021-09-12|2021-10-31"))), 
                new(nameof(DateIsNotContainedIn), new RuleSet(new Rule("SomeDate", "IsNotContainedIn", "1999-05-06|2003-05-01|2007-05-03|2011-05-05|2016-05-05|2021-05-06"))), 
            };
        
        [Test]
        [TestCase(true, 1, true)]
        [TestCase(true, 2, false)]
        [TestCase(true, 3, true)]
        [TestCase(true, 4, false)]
        [TestCase(true, 5, true)]
        [TestCase(true, 6, false)]
        [TestCase(false, 1, true)]
        [TestCase(false, 2, false)]
        [TestCase(false, 3, true)]
        [TestCase(false, 4, false)]
        [TestCase(false, 5, true)]
        [TestCase(false, 6, false)]
        public void IntIsContainedIn(bool isDebug, int number, bool isMatch)
        {
            if (isMatch)
                ShouldMatchRule(isDebug, someNumber: number);
            else
                ShouldNotMatchRule(isDebug, someNumber: number);
        }
        
        [Test]
        [TestCase(true, 4, false)]
        [TestCase(true, 5, true)]
        [TestCase(true, 6, false)]
        [TestCase(true, 7, true)]
        [TestCase(true, 8, false)]
        [TestCase(true, 9, true)]
        [TestCase(false, 4, false)]
        [TestCase(false, 5, true)]
        [TestCase(false, 6, false)]
        [TestCase(false, 7, true)]
        [TestCase(false, 8, false)]
        [TestCase(false, 9, true)]
        public void IntIsNotContainedIn(bool isDebug, int number, bool isMatch)
        {
            if (isMatch)
                ShouldMatchRule(isDebug, someNumber: number);
            else
                ShouldNotMatchRule(isDebug, someNumber: number);
        }

        [Test]
        public void DateIsContainedIn([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someDate: new DateTime(2021, 09, 12));
        }
        
        [Test]
        public void DateIsNotContainedIn([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someDate: new DateTime(2021, 09, 12));
        }
    }
}