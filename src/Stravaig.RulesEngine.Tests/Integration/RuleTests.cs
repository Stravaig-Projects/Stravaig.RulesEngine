using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace Stravaig.RulesEngine.Tests.Integration
{
    [TestFixture]
    public class RuleTests
    {
        private class TheContext
        {
            public int SomeNumber { get; set; }
        }

        private RuleRepository<string> _ruleRepository;

        [SetUp]
        public void SetUp()
        {
            _ruleRepository = new RuleRepository<string>();
            
            _ruleRepository.Load(new KeyValuePair<string, RuleSet>[]
            {
                new (nameof(SomeNumberIs100Test), new RuleSet(new RuleGroup[]
                {
                    new RuleGroup(BooleanOperator.And, true, 
                        new Rule("SomeNumber", "==", "100")),
                })),
                new (nameof(SomeNumberIsNot100Test), new RuleSet(new RuleGroup[]
                {
                    new RuleGroup(BooleanOperator.And, true, 
                        new Rule("SomeNumber", "!=", "100")),
                })),
            });
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SomeNumberIs100Test(bool isDebug)
        {
            var context = new TheContext
            {
                SomeNumber = 100,
            };
            var matches = FindMatches(context, isDebug);
            
            matches.Length.ShouldBe(1);
            matches[0].ShouldBe(nameof(SomeNumberIs100Test));
        }
        
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SomeNumberIsNot100Test(bool isDebug)
        {
            var context = new TheContext
            {
                SomeNumber = 101,
            };
            var matches = FindMatches(context, isDebug);
            
            matches.Length.ShouldBe(1);
            matches[0].ShouldBe(nameof(SomeNumberIsNot100Test));
        }

        private string[] FindMatches<TContext>(TContext context, bool isDebug)
        {
            var session = _ruleRepository.StartSession<TContext>();
            return isDebug
                ? session.DEBUG_FindMatches(context).ToArray()
                : session.FindMatches(context).ToArray();
        }
    }
}