using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Stravaig.RulesEngine.Tests.Integration
{
    public partial class RuleTests
    {
        private static KeyValuePair<string, RuleSet>[] MultipleRuleGroupTestSets =>
            new KeyValuePair<string, RuleSet>[]
            {
                new(nameof(TwoGroupsOredTogether), new RuleSet(new[]
                {
                    new RuleGroup(
                        BooleanOperator.Or,
                        true,
                        new RuleGroup(BooleanOperator.And, true,
                            new Rule("SomeNumber", "==", "100"),
                            new Rule("SomeDate", "==", "2021-09-04")),
                        new RuleGroup(BooleanOperator.And, true,
                            new Rule("SomeNumber", "==", "50"),
                            new Rule("SomeDate", "==", "2000-09-04"))),
                })),
                // new(nameof(NumberAndDateNotEqualsTest), new RuleSet(new[]
                // {
                //     new RuleGroup(BooleanOperator.And, true,
                //         new Rule("SomeNumber", "!=", "100"),
                //         new Rule("SomeDate", "!=", "2021-09-04")),
                // })),
                // new(nameof(NumberOrDateEqualsTest), new RuleSet(new[]
                // {
                //     new RuleGroup(BooleanOperator.Or, true,
                //         new Rule("SomeNumber", "==", "100"),
                //         new Rule("SomeDate", "==", "2021-09-04")),
                // })),
                // new(nameof(NotNumberOrDateEqualsTest), new RuleSet(new[]
                // {
                //     new RuleGroup(BooleanOperator.Or, false,
                //         new Rule("SomeNumber", "==", "100"),
                //         new Rule("SomeDate", "==", "2021-09-04")),
                // })),
            };
        
        [Test]
        [TestCase(true, 100, 2021, true)]
        [TestCase(false, 100, 2021, true)]
        [TestCase(true, 50, 2000, true)]
        [TestCase(false, 50, 2000, true)]
        [TestCase(true, 50, 2021, false)]
        [TestCase(false, 50, 2021, false)]
        [TestCase(true, 100, 2000, false)]
        [TestCase(false, 100, 2000, false)]
        public void TwoGroupsOredTogether(bool isDebug, int someNumber, int year, bool isMatch)
        {
            // {
            //   [
            //     {
            //       "BooleanExpression": "And",
            //       "Rules" :[ 
            //         { PropertyPath : "A.B.C", Operator: ">=", Value: "123" },
            //         { PropertyPath : "A.B.D", Operator: "==", Value: "ABC" },
            //     },
            //     {}
            //   ]
            // }
            //
            // (
            //     (someNumber == 100 && year == 2021) == true ||
            //     (someNumber == 50 && year == 2000) == true
            // ) == true
            if (isMatch)
                ShouldMatchRule(isDebug, someNumber, new DateTime(year, 09, 04));
            else
                ShouldNotMatchRule(isDebug, someNumber, new DateTime(year, 09, 04));
        }
    }
}