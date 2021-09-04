using System;
using NUnit.Framework;
using Shouldly;

namespace Stravaig.RulesEngine.Tests
{
    [TestFixture]
    public class RuleSetTests
    {
        [Test]
        public void RejectNoElementConstructor()
        {
            Should.Throw<ArgumentException>(() => new RuleSet())
                .Message.ShouldStartWith("Value cannot be an empty collection.");
        }
        
        [Test]
        public void RejectEmptyConstructor()
        {
            Should.Throw<ArgumentException>(() => new RuleSet(Array.Empty<RuleGroup>()))
                .Message.ShouldStartWith("Value cannot be an empty collection.");
        }
    }
}