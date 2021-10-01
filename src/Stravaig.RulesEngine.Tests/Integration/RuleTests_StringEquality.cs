using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace Stravaig.RulesEngine.Tests.Integration
{
    public partial class RuleTests
    {
        private static KeyValuePair<string, RuleSet>[] StringEqualityRuleSets =>
            new KeyValuePair<string, RuleSet>[]
            {
                new(nameof(StringOrdinalEquals), new RuleSet(new Rule("SomeString", "Equals", "abc"))),
                new(nameof(StringOrdinalNotEquals), new RuleSet(new Rule("SomeString", "NotEquals", "def"))),
                new(nameof(InvariantCultureEquals), new RuleSet(new Rule("SomeString", "Equals", "ghi", StringComparison.InvariantCulture))),
                new(nameof(InvariantCultureNotEquals), new RuleSet(new Rule("SomeString", "NotEquals", "klm", StringComparison.InvariantCulture))),
                new(nameof(CurrentCultureEquals), new RuleSet(new Rule("SomeString", "Equals", "nop", StringComparison.CurrentCulture))),
                new(nameof(CurrentCultureNotEquals), new RuleSet(new Rule("SomeString", "NotEquals", "qrs", StringComparison.CurrentCulture))),

                new(nameof(StringOrdinalIgnoreCaseEquals), new RuleSet(new Rule("SomeString", "Equals", "abc", StringComparison.OrdinalIgnoreCase))),
                new(nameof(StringOrdinalIgnoreCaseNotEquals), new RuleSet(new Rule("SomeString", "NotEquals", "def", StringComparison.OrdinalIgnoreCase))),
                new(nameof(InvariantCultureIgnoreCaseEquals), new RuleSet(new Rule("SomeString", "Equals", "ghi", StringComparison.InvariantCultureIgnoreCase))),
                new(nameof(InvariantCultureIgnoreCaseNotEquals), new RuleSet(new Rule("SomeString", "NotEquals", "klm", StringComparison.InvariantCultureIgnoreCase))),
                new(nameof(CurrentCultureIgnoreCaseEquals), new RuleSet(new Rule("SomeString", "Equals", "nop", StringComparison.CurrentCultureIgnoreCase))),
                new(nameof(CurrentCultureIgnoreCaseNotEquals), new RuleSet(new Rule("SomeString", "NotEquals", "qrs", StringComparison.CurrentCultureIgnoreCase))),
            };
        
        [Test]
        public void StringOrdinalEquals([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "abc");
            ShouldNotMatchRule(isDebug, someString: "ABC");
        }
        
        [Test]
        public void StringOrdinalIgnoreCaseEquals([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "abc");
            ShouldMatchRule(isDebug, someString: "ABC");
            ShouldNotMatchRule(isDebug, someString: "123");
        }

        [Test]
        public void StringOrdinalNotEquals([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "DEF");
            ShouldNotMatchRule(isDebug, someString: "def");
        }

        [Test]
        public void StringOrdinalIgnoreCaseNotEquals([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "123");
            ShouldNotMatchRule(isDebug, someString: "DEF");
            ShouldNotMatchRule(isDebug, someString: "def");
        }

        [Test]
        public void InvariantCultureEquals([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "ghi");
            ShouldNotMatchRule(isDebug, someString: "GHI");
        }
        
        [Test]
        public void InvariantCultureIgnoreCaseEquals([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "ghi");
            ShouldMatchRule(isDebug, someString: "GHI");
            ShouldNotMatchRule(isDebug, someString: "123");
        }

        [Test]
        public void InvariantCultureNotEquals([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "KLM");
            ShouldNotMatchRule(isDebug, someString: "klm");
        }
        
        [Test]
        public void InvariantCultureIgnoreCaseNotEquals([Values] bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "123");
            ShouldNotMatchRule(isDebug, someString: "klm");
            ShouldNotMatchRule(isDebug, someString: "KLM");
        }
        
        [Test]
        [NonParallelizable]
        public void CurrentCultureEquals([Values] bool isDebug)
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                Console.WriteLine("THe current culture is "+Thread.CurrentThread.CurrentCulture);
                ShouldMatchRule(isDebug, someString: "nop");
                ShouldNotMatchRule(isDebug, someString: "NOP");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
        
        [Test]
        [NonParallelizable]
        public void CurrentCultureIgnoreCaseEquals([Values] bool isDebug)
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                Console.WriteLine("THe current culture is "+Thread.CurrentThread.CurrentCulture);
                ShouldMatchRule(isDebug, someString: "nop");
                ShouldMatchRule(isDebug, someString: "NOP");
                ShouldNotMatchRule(isDebug, someString: "123");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Test]
        [NonParallelizable]
        public void CurrentCultureNotEquals([Values] bool isDebug)
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("DE-de");
                ShouldMatchRule(isDebug, someString: "QRS");
                ShouldNotMatchRule(isDebug, someString: "qrs");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
        
        [Test]
        [NonParallelizable]
        public void CurrentCultureIgnoreCaseNotEquals([Values] bool isDebug)
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("DE-de");
                ShouldMatchRule(isDebug, someString: "123");
                ShouldNotMatchRule(isDebug, someString: "qrs");
                ShouldNotMatchRule(isDebug, someString: "QRS");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
    }
}