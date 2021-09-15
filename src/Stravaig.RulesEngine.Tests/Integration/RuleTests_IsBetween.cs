using System.Collections.Generic;
using NUnit.Framework;

namespace Stravaig.RulesEngine.Tests.Integration
{
    public partial class RuleTests // _IsBetween
    {
        private static KeyValuePair<string, RuleSet>[] IsBetweenRuleSets =>
            new KeyValuePair<string, RuleSet>[]
            {
                new(nameof(IntIsBetween), new RuleSet(new Rule("SomeNumber", "IsBetween", "1|10"))), 
                new(nameof(IntIsNotBetween), new RuleSet(new Rule("SomeNumber", "IsNotBetween", "111|222"))), 
            };

        [Test]
        public void IntIsBetween([Values]bool isDebug, [Values(1,5,10)]int someNumber, [Values]ShouldMatch _)
        {
            ShouldMatchRule(isDebug, someNumber: someNumber);
        }

        [Test]
        public void IntIsBetween([Values]bool isDebug, [Values(0,11)]int someNumber, [Values]ShouldNotMatch _)
        {
            ShouldNotMatchRule(isDebug, someNumber: someNumber);
        }
        
        [Test]
        public void IntIsNotBetween([Values]bool isDebug, [Values(111, 199, 222)]int someNumber, [Values]ShouldNotMatch _)
        {
            ShouldNotMatchRule(isDebug, someNumber: someNumber);
        }

        [Test]
        public void IntIsNotBetween([Values]bool isDebug, [Values(0, 110, 223, 300)]int someNumber, [Values]ShouldMatch _)
        {
            ShouldMatchRule(isDebug, someNumber: someNumber);
        }
    }
}