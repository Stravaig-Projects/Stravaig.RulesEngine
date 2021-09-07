using System.Collections.Generic;
using NUnit.Framework;

namespace Stravaig.RulesEngine.Tests.Integration
{
    public partial class RuleTests
    {
        private KeyValuePair<string, RuleSet>[] StringEqualityRuleSets =>
            new KeyValuePair<string, RuleSet>[]
            {
                new(nameof(StringOrdinalEquals), new RuleSet(new Rule("SomeString", "OrdinalEquals", "encyclopædia"))),
                new(nameof(StringOrdinalNotEquals), new RuleSet(new Rule("SomeString", "OrdinalNotEquals", "encyclopædia"))),
            };
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void StringOrdinalEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "encyclopædia");
            ShouldNotMatchRule(isDebug, someString: "encyclopaedia");
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void StringOrdinalNotEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "encyclopaedia");
            ShouldNotMatchRule(isDebug, someString: "encyclopædia");
        }

    }
}