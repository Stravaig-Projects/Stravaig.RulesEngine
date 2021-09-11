using System.Collections.Generic;
using NUnit.Framework;

namespace Stravaig.RulesEngine.Tests.Integration
{
    public partial class RuleTests
    {
        private static KeyValuePair<string, RuleSet>[] SingleRuleTestSets =>
            new KeyValuePair<string, RuleSet>[]
            {
                new(nameof(SomeNumberIs100Test), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.And, true,
                        new Rule("SomeNumber", "==", "100")),
                })),
                new(nameof(SomeNumberIsNot100Test), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.And, true,
                        new Rule("SomeNumber", "!=", "100")),
                })),
                new(nameof(SomeNumberIsGreaterThan100Test), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.And, true,
                        new Rule("SomeNumber", ">", "100")),
                })),
                new(nameof(SomeNumberIsGreaterThanOrEqualTo100Test), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.And, true,
                        new Rule("SomeNumber", ">=", "100")),
                })),
                new(nameof(SomeNumberIsLessThan100Test), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.And, true,
                        new Rule("SomeNumber", "<", "100")),
                })),
                new(nameof(SomeNumberIsLessThanOrEqualTo100Test), new RuleSet(new[]
                {
                    new RuleGroup(BooleanOperator.And, true,
                        new Rule("SomeNumber", "<=", "100")),
                })),
            };
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SomeNumberIs100Test(bool isDebug)
        {
            ShouldMatchRule(isDebug, 100);
        }
        
        [Test]
        [TestCase(true, 99)]
        [TestCase(false, 99)]
        [TestCase(true, 101)]
        [TestCase(false, 101)]
        public void SomeNumberIsNot100Test(bool isDebug, int someNumber)
        {
            ShouldMatchRule(isDebug, someNumber);
        }
        
        [Test]
        [TestCase(true, 101)]
        [TestCase(false, 101)]
        public void SomeNumberIsGreaterThan100Test(bool isDebug, int someNumber)
        {
            ShouldMatchRule(isDebug, someNumber);
        }

        [Test]
        [TestCase(true, 100)]
        [TestCase(false, 100)]
        [TestCase(true, 101)]
        [TestCase(false, 101)]
        public void SomeNumberIsGreaterThanOrEqualTo100Test(bool isDebug, int someNumber)
        {
            ShouldMatchRule(isDebug, someNumber);
        }
        
        [Test]
        [TestCase(true, 99)]
        [TestCase(false, 99)]
        public void SomeNumberIsLessThan100Test(bool isDebug, int someNumber)
        {
            ShouldMatchRule(isDebug, someNumber);
        }
        
        [Test]
        [TestCase(true, 99)]
        [TestCase(false, 99)]
        [TestCase(true, 100)]
        [TestCase(false, 100)]
        public void SomeNumberIsLessThanOrEqualTo100Test(bool isDebug, int someNumber)
        {
            ShouldMatchRule(isDebug, someNumber);
        }

    }
}