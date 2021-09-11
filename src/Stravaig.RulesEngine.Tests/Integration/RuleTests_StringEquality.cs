using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace Stravaig.RulesEngine.Tests.Integration
{
    public partial class RuleTests
    {
        private KeyValuePair<string, RuleSet>[] StringEqualityRuleSets =>
            new KeyValuePair<string, RuleSet>[]
            {
                new(nameof(StringOrdinalEquals), new RuleSet(new Rule("SomeString", "OrdinalEquals", "abc"))),
                new(nameof(StringOrdinalNotEquals), new RuleSet(new Rule("SomeString", "OrdinalNotEquals", "def"))),
                new(nameof(InvariantCultureEquals), new RuleSet(new Rule("SomeString", "InvariantCultureEquals", "ghi"))),
                new(nameof(InvariantCultureNotEquals), new RuleSet(new Rule("SomeString", "InvariantCultureNotEquals", "klm"))),
                new(nameof(CurrentCultureEquals), new RuleSet(new Rule("SomeString", "CurrentCultureEquals", "nop"))),
                new(nameof(CurrentCultureNotEquals), new RuleSet(new Rule("SomeString", "CurrentCultureNotEquals", "qrs"))),
            };
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void StringOrdinalEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "abc");
            ShouldNotMatchRule(isDebug, someString: "123");
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void StringOrdinalNotEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "123");
            ShouldNotMatchRule(isDebug, someString: "def");
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void InvariantCultureEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "ghi");
            ShouldNotMatchRule(isDebug, someString: "123");
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void InvariantCultureNotEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "123");
            ShouldNotMatchRule(isDebug, someString: "klm");
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        [NonParallelizable]
        public void CurrentCultureEquals(bool isDebug)
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                Console.WriteLine("THe current culture is "+Thread.CurrentThread.CurrentCulture);
                ShouldMatchRule(isDebug, someString: "nop");
                ShouldNotMatchRule(isDebug, someString: "123");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        [NonParallelizable]
        public void CurrentCultureNotEquals(bool isDebug)
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("DE-de");
                ShouldMatchRule(isDebug, someString: "123");
                ShouldNotMatchRule(isDebug, someString: "qrs");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
    }
}