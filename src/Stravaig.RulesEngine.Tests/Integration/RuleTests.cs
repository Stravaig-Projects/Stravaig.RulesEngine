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
        public class SingleRuleContext
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
            });
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SomeNumberIs100Test(bool isDebug)
        {
            var context = new SingleRuleContext()
            {
                SomeNumber = 100,
            };
            var session = _ruleRepository.StartSession<SingleRuleContext>();
            var matches = FindMatches(context, session, isDebug);
            
            matches.Length.ShouldBe(1);
            matches[0].ShouldBe(nameof(SomeNumberIs100Test));
        }

        private string[] FindMatches<TContext>(TContext context, RulesEngineSession<string, TContext> session, bool isDebug)
        {
            return isDebug
                ? session.DEBUG_FindMatches(context).ToArray()
                : session.FindMatches(context).ToArray();
        }
    }
}