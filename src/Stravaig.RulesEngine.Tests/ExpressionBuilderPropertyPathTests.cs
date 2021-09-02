using System;
using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine.Compiler;

namespace Stravaig.RulesEngine.Tests
{
    [TestFixture]
    public class ExpressionBuilderPropertyPathTests
    {
        public class SubContext
        {
            public string SubStringProperty { get; set; }
            
            private string PrivateGetter { get; set; }
        }

        private class TestContext
        {
            private string _setterOnlyProperty;

            public string SomeStringProperty { get; set; }
            public SubContext SubContextProperty { get; set; }

            public string SetterOnlyProperty
            {
                set => _setterOnlyProperty = value;
            }
        }

        [Test]
        public void MissingExpressionThrowsException()
        {
            ExpressionBuilder builder = new ExpressionBuilder();
            Should.Throw<ArgumentNullException>(() => builder.Build<TestContext>(null!, null!, null!));
        }
        
        [Test]
        public void MissingPropertyPathThrowsException()
        {
            ExpressionBuilder builder = new ExpressionBuilder();
            Should.Throw<ArgumentNullException>(() => builder.Build<TestContext>("Some.Property.Path", null!, null!));
        }

        [Test]
        public void MissingValueThrowsException()
        {
            ExpressionBuilder builder = new ExpressionBuilder();
            Should.Throw<ArgumentNullException>(() => builder.Build<TestContext>("Some.Property.Path", "==", null!));
        }

        
        [Test]
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
        
        [Test]
        public void PropertyPathWithEqualityExpression()
        {
            ExpressionBuilder builder = new ExpressionBuilder();
            var func = builder.Build<TestContext>(
                $"{nameof(TestContext.SubContextProperty)}.{nameof(SubContext.SubStringProperty)}",
                "==",
                "abc");

            func.ShouldNotBeNull();

            var testContext = new TestContext { SubContextProperty = new SubContext{ SubStringProperty = "abc"} };
            func(testContext).ShouldBeTrue();
            
            testContext = new TestContext { SubContextProperty = new SubContext{ SubStringProperty = "def"} };
            func(testContext).ShouldBeFalse();
        }
        
        [Test]
        public void BuildWithoutGetterPropertyThrowsException()
        {
            const string propertyPath = nameof(TestContext.SetterOnlyProperty);
            ExpressionBuilder builder = new ExpressionBuilder();
            var ex = Should.Throw<PropertyGetterRequiredException>(
                () => builder.Build<TestContext>(
                    propertyPath,
                    "==",
                    "abc"));

            ex.FailingNode.ShouldBe(propertyPath);
            ex.PropertyPath.ShouldBe(propertyPath);
            ex.ContextType.ShouldBe(typeof(TestContext));
        }

        [Test]
        public void BuildWithInaccessibleGetterPropertyThrowsException()
        {
            const string propertyPath = nameof(TestContext.SubContextProperty) + ".PrivateGetter";
            ExpressionBuilder builder = new ExpressionBuilder();
            var ex = Should.Throw<PublicPropertyGetterRequiredException>(
                () => builder.Build<TestContext>(
                    propertyPath,
                    "==",
                    "abc"));

            ex.FailingNode.ShouldBe(propertyPath);
            ex.PropertyPath.ShouldBe(propertyPath);
            ex.ContextType.ShouldBe(typeof(TestContext));
        }
    }
}