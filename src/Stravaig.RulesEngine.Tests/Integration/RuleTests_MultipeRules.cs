using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Stravaig.RulesEngine.Tests.Integration
{
    public partial class RuleTests
    {
        private KeyValuePair<string, RuleSet>[] MultipleRuleTestSets =>
            new KeyValuePair<string, RuleSet>[]
            {
                new(nameof(NumberAndDateEqualityTest), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.And, true,
                        new Rule("SomeNumber", "==", "100"),
                        new Rule("SomeDate", "==", "2021-09-04")),
                })),
                new(nameof(NumberAndDateNotEqualsTest), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.And, true,
                        new Rule("SomeNumber", "!=", "100"),
                        new Rule("SomeDate", "!=", "2021-09-04")),
                })),
                new(nameof(NumberOrDateEqualsTest), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.Or, true,
                        new Rule("SomeNumber", "==", "100"),
                        new Rule("SomeDate", "==", "2021-09-04")),
                })),
                new(nameof(NotNumberOrDateEqualsTest), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.Or, false,
                        new Rule("SomeNumber", "==", "100"),
                        new Rule("SomeDate", "==", "2021-09-04")),
                })),
            };
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void NumberAndDateEqualityTest(bool isDebug)
        {
            ShouldMatchRule(isDebug, 100, new DateTime(2021, 09, 04));
        }
        
        [Test]
        [TestCase(true, 99, 2020, true)]
        [TestCase(false, 99, 2020, true)]
        [TestCase(true, 101, 2022, true)]
        [TestCase(false, 101, 2022, true)]
        [TestCase(true, 99, 2021, false)]
        [TestCase(false, 99, 2021, false)]
        [TestCase(true, 101, 2021, false)]
        [TestCase(false, 101, 2021, false)]
        [TestCase(true, 100, 2020, false)]
        [TestCase(false, 100, 2020, false)]
        [TestCase(true, 100, 2022, false)]
        [TestCase(false, 100, 2022, false)]
        public void NumberAndDateNotEqualsTest(bool isDebug, int someNumber, int year, bool isMatch)
        {
            // someNumber != 100 && year != 2021
            if (isMatch)
                ShouldMatchRule(isDebug, someNumber, new DateTime(year, 09, 04));
            else
                ShouldNotMatchRule(isDebug, someNumber, new DateTime(year, 09, 04));
        }
        
        [Test]
        [TestCase(true, 100, 2021, true)]
        [TestCase(false, 100, 2021, true)]
        [TestCase(true, 99, 2021, true)]
        [TestCase(false, 99, 2021, true)]
        [TestCase(true, 100, 2020, true)]
        [TestCase(false, 100, 2020, true)]
        [TestCase(true, 99, 2020, false)]
        [TestCase(false, 99, 2020, false)]
        public void NumberOrDateEqualsTest(bool isDebug, int someNumber, int year, bool isMatch)
        {
            // someNumber == 100 || year == 2021
            if (isMatch)
                ShouldMatchRule(isDebug, someNumber, new DateTime(year, 09, 04));
            else
                ShouldNotMatchRule(isDebug, someNumber, new DateTime(year, 09, 04));
        }

        [Test]
        [TestCase(true, 100, 2021, false)]
        [TestCase(false, 100, 2021, false)]
        [TestCase(true, 99, 2021, false)]
        [TestCase(false, 99, 2021, false)]
        [TestCase(true, 100, 2020, false)]
        [TestCase(false, 100, 2020, false)]
        [TestCase(true, 99, 2020, true)]
        [TestCase(false, 99, 2020, true)]
        public void NotNumberOrDateEqualsTest(bool isDebug, int someNumber, int year, bool isMatch)
        {
            // (someNumber == 100 || year == 2021) == false
            // or
            // !(someNumber == 100 || year == 2021)
            if (isMatch)
                ShouldMatchRule(isDebug, someNumber, new DateTime(year, 09, 04));
            else
                ShouldNotMatchRule(isDebug, someNumber, new DateTime(year, 09, 04));
        }
    }
}