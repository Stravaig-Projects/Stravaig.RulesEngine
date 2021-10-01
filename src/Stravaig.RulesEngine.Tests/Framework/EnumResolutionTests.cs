using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace Stravaig.RulesEngine.Tests.Framework
{
    [TestFixture]
    public class EnumResolutionTests
    {
        [Test]
        public void CheckType()
        {
            Enum stringComparison = StringComparison.OrdinalIgnoreCase;

            stringComparison.ShouldBeOfType<StringComparison>();
        }

        [Test]
        public void ResolveOneIntoConcrete()
        {
            Enum[] enums = new Enum[]
            {
                StringComparison.OrdinalIgnoreCase,
                DateTimeKind.Local,
                DateTimeStyles.RoundtripKind,
            };

            var resolved = enums
                .Where(e => e.GetType() == typeof(StringComparison))
                .Cast<StringComparison>()
                .FirstOrDefault();

            resolved.ShouldNotBe(default(Enum));
            resolved.ShouldBeOfType<StringComparison>();
        }
    }
}