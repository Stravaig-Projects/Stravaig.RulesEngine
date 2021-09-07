using System;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Shouldly;

namespace Stravaig.RulesEngine.Tests.Integration
{
    [TestFixture]
    public partial class RuleTests
    {
        private class TheContext
        {
            public int SomeNumber { get; set; }
            public DateTime SomeDate { get; set; }
            public string SomeString { get; set; }
        }

        private RuleRepository<string> _ruleRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _ruleRepository = new RuleRepository<string>();
            _ruleRepository.Load(
                SingleRuleTestSets
                    .Union(MultipleRuleTestSets)
                    .Union(StringEqualityRuleSets)
                    .Union(MultipleRuleGroupTestSets));
        }
        
        private void ShouldMatchRule(bool isDebug, int someNumber = default, DateTime someDate = default, string someString = default, [CallerMemberName]string methodName = null)
        {
            // Check rule repository contains the rule we're testing for.
            _ruleRepository.RuleSetKeys.ShouldContain(methodName);
            string[] matches = RunTest(isDebug, someNumber, someDate, someString);

            matches.Length.ShouldBeGreaterThanOrEqualTo(1);
            matches.ShouldContain(methodName);
        }

        private void ShouldNotMatchRule(bool isDebug, int someNumber = default, DateTime someDate = default, string someString = default, [CallerMemberName]string methodName = null)
        {
            // Check rule repository contains the rule we're testing for.
            _ruleRepository.RuleSetKeys.ShouldContain(methodName);
            string[] matches = RunTest(isDebug, someNumber, someDate, someString);

            matches.ShouldNotContain(methodName);
        }

        private string[] RunTest(bool isDebug, int someNumber, DateTime someDate, string someString)
        {
            try
            {
                var context = new TheContext { SomeNumber = someNumber, SomeDate = someDate, SomeString = someString};
                var matches = FindMatches(context, isDebug);

                Console.WriteLine($"Matches found when SomeNumber={context.SomeNumber}; SomeDate={context.SomeDate:O}; SomeString=\"{context.SomeString}\":\n" + string.Join("\n", matches.Select(s => $" * {s}")));
                return matches;
            }
            catch (Exception e)
            {
                Console.WriteLine("Available Operators:");
                Console.WriteLine(_ruleRepository.DEBUG_AvailableBuilders);
                throw;
            }
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