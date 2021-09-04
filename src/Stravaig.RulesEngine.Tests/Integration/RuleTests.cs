using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
                new (nameof(SomeNumberIsGreaterThan100Test), new RuleSet(new RuleGroup[]
                {
                    new RuleGroup(BooleanOperator.And, true, 
                        new Rule("SomeNumber", ">", "100")),
                })),
            });
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SomeNumberIs100Test(bool isDebug)
        {
            RunTest(isDebug, 100);
        }
        
        [Test]
        [TestCase(true, 99)]
        [TestCase(false, 99)]
        [TestCase(true, 101)]
        [TestCase(false, 101)]
        public void SomeNumberIsNot100Test(bool isDebug, int someNumber)
        {
            RunTest(isDebug, someNumber);
        }
        
        [Test]
        [TestCase(true, 101)]
        [TestCase(false, 101)]
        public void SomeNumberIsGreaterThan100Test(bool isDebug, int someNumber)
        {
            RunTest(isDebug, someNumber);
        }

        private void RunTest(bool isDebug, int someNumber, [CallerMemberName]string methodName = null)
        {
            var context = new TheContext { SomeNumber = someNumber };
            var matches = FindMatches(context, isDebug);
            
            Console.WriteLine($"Matches found when SomeNumber={context.SomeNumber}:\n"+string.Join("\n", matches.Select(s => $" * {s}")));

            matches.Length.ShouldBeGreaterThanOrEqualTo(1);
            matches.ShouldContain(methodName);
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