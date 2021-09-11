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
                new(nameof(StringOrdinalEquals), new RuleSet(new Rule("SomeString", "OrdinalEquals", "abc"))),
                new(nameof(StringOrdinalNotEquals), new RuleSet(new Rule("SomeString", "OrdinalNotEquals", "def"))),
                new(nameof(InvariantCultureEquals), new RuleSet(new Rule("SomeString", "InvariantCultureEquals", "ghi"))),
                new(nameof(InvariantCultureNotEquals), new RuleSet(new Rule("SomeString", "InvariantCultureNotEquals", "klm"))),
                new(nameof(CurrentCultureEquals), new RuleSet(new Rule("SomeString", "CurrentCultureEquals", "nop"))),
                new(nameof(CurrentCultureNotEquals), new RuleSet(new Rule("SomeString", "CurrentCultureNotEquals", "qrs"))),

                new(nameof(StringOrdinalIgnoreCaseEquals), new RuleSet(new Rule("SomeString", "OrdinalIgnoreCaseEquals", "abc"))),
                new(nameof(StringOrdinalIgnoreCaseNotEquals), new RuleSet(new Rule("SomeString", "OrdinalIgnoreCaseNotEquals", "def"))),
                new(nameof(InvariantCultureIgnoreCaseEquals), new RuleSet(new Rule("SomeString", "InvariantCultureIgnoreCaseEquals", "ghi"))),
                new(nameof(InvariantCultureIgnoreCaseNotEquals), new RuleSet(new Rule("SomeString", "InvariantCultureIgnoreCaseNotEquals", "klm"))),
                new(nameof(CurrentCultureIgnoreCaseEquals), new RuleSet(new Rule("SomeString", "CurrentCultureIgnoreCaseEquals", "nop"))),
                new(nameof(CurrentCultureIgnoreCaseNotEquals), new RuleSet(new Rule("SomeString", "CurrentCultureIgnoreCaseNotEquals", "qrs"))),
            };
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void StringOrdinalEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "abc");
            ShouldNotMatchRule(isDebug, someString: "ABC");
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void StringOrdinalIgnoreCaseEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "abc");
            ShouldMatchRule(isDebug, someString: "ABC");
            ShouldNotMatchRule(isDebug, someString: "123");
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void StringOrdinalNotEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "DEF");
            ShouldNotMatchRule(isDebug, someString: "def");
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void StringOrdinalIgnoreCaseNotEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "123");
            ShouldNotMatchRule(isDebug, someString: "DEF");
            ShouldNotMatchRule(isDebug, someString: "def");
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void InvariantCultureEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "ghi");
            ShouldNotMatchRule(isDebug, someString: "GHI");
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void InvariantCultureIgnoreCaseEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "ghi");
            ShouldMatchRule(isDebug, someString: "GHI");
            ShouldNotMatchRule(isDebug, someString: "123");
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void InvariantCultureNotEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "KLM");
            ShouldNotMatchRule(isDebug, someString: "klm");
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void InvariantCultureIgnoreCaseNotEquals(bool isDebug)
        {
            ShouldMatchRule(isDebug, someString: "123");
            ShouldNotMatchRule(isDebug, someString: "klm");
            ShouldNotMatchRule(isDebug, someString: "KLM");
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
                ShouldNotMatchRule(isDebug, someString: "NOP");
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
        public void CurrentCultureIgnoreCaseEquals(bool isDebug)
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
        [TestCase(true)]
        [TestCase(false)]
        [NonParallelizable]
        public void CurrentCultureNotEquals(bool isDebug)
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
        [TestCase(true)]
        [TestCase(false)]
        [NonParallelizable]
        public void CurrentCultureIgnoreCaseNotEquals(bool isDebug)
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