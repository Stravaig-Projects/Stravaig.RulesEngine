using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine;

namespace Stravaig_RulesEngine.Tests
{
    [TestFixture]
    public class ExpressionBuilderTests
    {
        private class TestContext
        {
            public string SomeStringProperty { get; set; }
        }

        public void SinglePropertyEqualityExpression()
        {
            ExpressionBuilder builder = new ExpressionBuilder();
            var func = builder.Build<TestContext>(nameof(TestContext.SomeStringProperty), "==", "abc");

            func.ShouldNotBeNull();

            var testContext = new TestContext { SomeStringProperty = "abc" };
            func(testContext).ShouldBeTrue();
            
            testContext = new TestContext { SomeStringProperty = "def" };
            func(testContext).ShouldBeFalse();
        }
    }
}