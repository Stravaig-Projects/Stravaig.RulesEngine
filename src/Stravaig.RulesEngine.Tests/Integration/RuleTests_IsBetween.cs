using System;
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
                new(nameof(DateTimeIsBetween), new RuleSet(new Rule("SomeDate", "IsBetween", "2021-09-01|2021-09-10"))), 
                new(nameof(DateTimeIsNotBetween), new RuleSet(new Rule("SomeDate", "IsNotBetween", "2021-09-11|2021-09-22"))), 
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
        
        [Test]
        public void DateTimeIsBetween([Values]bool isDebug, [Values(1,5,10)]int dayOfMonth, [Values]ShouldMatch _)
        {
            ShouldMatchRule(isDebug, someDate: new DateTime(2021, 9, dayOfMonth));
        }

        [Test]
        public void DateTimeIsBetween([Values]bool isDebug, [Values(11, 12)]int dayOfMonth, [Values]ShouldNotMatch _)
        {
            ShouldNotMatchRule(isDebug,someDate: new DateTime(2021, 9, dayOfMonth));
        }
        
        [Test]
        public void DateTimeIsNotBetween([Values]bool isDebug, [Values(11, 19, 22)]int dayOfMonth, [Values]ShouldNotMatch _)
        {
            ShouldNotMatchRule(isDebug, someDate: new DateTime(2021, 9, dayOfMonth));
        }

        [Test]
        public void DateTimeIsNotBetween([Values]bool isDebug, [Values(1, 10, 23, 30)]int dayOfMonth, [Values]ShouldMatch _)
        {
            ShouldMatchRule(isDebug, someDate: new DateTime(2021, 9, dayOfMonth));
        }

    }
}