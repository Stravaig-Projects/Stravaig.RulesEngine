using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine.Compiler.OperatorBuilders;
using Stravaig.RulesEngine.Tests.DuplicateOperatorHandlerName;

namespace Stravaig.RulesEngine.Tests
{
    [TestFixture]
    public partial class OperatorHandlerServiceLocatorTests
    {
        [Test]
        public void DefaultInstantiationAllowsAccessToEqualsOperator()
        {
            var locator = new OperatorBuilderLocator();
            var handler = locator.GetBuilder("==", null);
            handler.ShouldBeOfType(typeof(EqualsOperatorBuilder));
        }
        
        [Test]
        public void InvalidOperatorNameThrowsException()
        {
            var locator = new OperatorBuilderLocator();
            var ex = Should.Throw<OperatorBuilderNotFoundException>(() => locator.GetBuilder("InvalidOperatorName", null));
            ex.OperatorName.ShouldBe("InvalidOperatorName");
        }

        // [Test]
        // public void DuplicateNameThrowsException()
        // {
        //     var locator = new OperatorBuilderLocator(typeof(DuplicateEqualsOperatorBuilder).Assembly);
        //     var ex = Should.Throw<OperatorBuilderWithNameAlreadyExistsException>(() => locator.GetBuilder("DoNotCare", null));
        //     ex.OperatorName.ShouldBe("==");
        //     ex.BuilderType.ShouldBe(typeof(DuplicateEqualsOperatorBuilder));
        //     ex.ExistingBuilderType.ShouldBe(typeof(EqualsOperatorBuilder));
        // }
        
    }
}