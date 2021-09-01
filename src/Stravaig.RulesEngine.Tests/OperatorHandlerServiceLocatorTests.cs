using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine.OperatorHandlers;
using Stravaig.RulesEngine.Tests.DuplicateOperatorHandlerName;

namespace Stravaig.RulesEngine.Tests
{
    [TestFixture]
    public class OperatorHandlerServiceLocatorTests
    {
        [Test]
        public void DefaultInstantiationAllowsAccessToEqualsOperator()
        {
            var locator = new OperatorHandlerServiceLocator();
            var handler = locator.GetHandlerFromName("==");
            handler.ShouldBeOfType(typeof(EqualsOperatorHandler));
        }
        
        [Test]
        public void InvalidOperatorNameThrowsException()
        {
            var locator = new OperatorHandlerServiceLocator();
            var ex = Should.Throw<OperatorHandlerNotFoundException>(() => locator.GetHandlerFromName("InvalidOperatorName"));
            ex.Name.ShouldBe("InvalidOperatorName");
        }

        [Test]
        public void DuplicateNameThrowsException()
        {
            var locator = new OperatorHandlerServiceLocator(typeof(DuplicateEqualsOperatorHandler).Assembly);
            var ex = Should.Throw<OperatorHandlerWithNameAlreadyExistsException>(() => locator.GetHandlerFromName("DoNotCare"));
            ex.HandlerName.ShouldBe("==");
            ex.HandlerType.ShouldBe(typeof(DuplicateEqualsOperatorHandler));
            ex.ExistingHandlerType.ShouldBe(typeof(EqualsOperatorHandler));
        }
    }
}