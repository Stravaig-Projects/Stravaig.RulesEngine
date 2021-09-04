using System;
using System.Collections.Generic;
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
        }

        private RuleRepository<string> _ruleRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _ruleRepository = new RuleRepository<string>();
            _ruleRepository.Load(
                SingleRuleTestSets
                    .Union(MultipleRuleTestSets));
        }


        private void ShouldMatchRule(bool isDebug, int someNumber, DateTime someDate = default, [CallerMemberName]string methodName = null)
        {
            string[] matches = RunTest(isDebug, someNumber, someDate);

            matches.Length.ShouldBeGreaterThanOrEqualTo(1);
            matches.ShouldContain(methodName);
        }

        private void ShouldNotMatchRule(bool isDebug, int someNumber, DateTime someDate = default, [CallerMemberName]string methodName = null)
        {
            string[] matches = RunTest(isDebug, someNumber, someDate);

            matches.ShouldNotContain(methodName);
        }

        private string[] RunTest(bool isDebug, int someNumber, DateTime someDate)
        {
            var context = new TheContext { SomeNumber = someNumber, SomeDate = someDate };
            var matches = FindMatches(context, isDebug);

            Console.WriteLine($"Matches found when SomeNumber={context.SomeNumber}; SomeDate={someDate:O}:\n" + string.Join("\n", matches.Select(s => $" * {s}")));
            return matches;
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