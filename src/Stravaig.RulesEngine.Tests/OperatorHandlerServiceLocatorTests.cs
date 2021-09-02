using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine.Compiler.OperatorBuilders;
using Stravaig.RulesEngine.Tests.DuplicateOperatorHandlerName;

namespace Stravaig.RulesEngine.Tests
{
    [TestFixture]
    public class OperatorHandlerServiceLocatorTests
    {
        [Test]
        public void DefaultInstantiationAllowsAccessToEqualsOperator()
        {
            var locator = new OperatorBuilderLocator();
            var handler = locator.GetBuilder("==");
            handler.ShouldBeOfType(typeof(EqualsOperatorBuilder));
        }
        
        [Test]
        public void InvalidOperatorNameThrowsException()
        {
            var locator = new OperatorBuilderLocator();
            var ex = Should.Throw<OperatorBuilderNotFoundException>(() => locator.GetBuilder("InvalidOperatorName"));
            ex.OperatorName.ShouldBe("InvalidOperatorName");
        }

        [Test]
        public void DuplicateNameThrowsException()
        {
            var locator = new OperatorBuilderLocator(typeof(DuplicateEqualsOperatorBuilder).Assembly);
            var ex = Should.Throw<OperatorBuilderWithNameAlreadyExistsException>(() => locator.GetBuilder("DoNotCare"));
            ex.OperatorName.ShouldBe("==");
            ex.BuilderType.ShouldBe(typeof(DuplicateEqualsOperatorBuilder));
            ex.ExistingBuilderType.ShouldBe(typeof(EqualsOperatorBuilder));
        }
    }
}