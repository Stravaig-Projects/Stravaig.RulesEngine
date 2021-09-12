using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine.Compiler.OperatorBuilders;
using Stravaig.RulesEngine.Compiler.OperatorBuilders.General;
using Stravaig.RulesEngine.Tests.DuplicateOperatorHandlerName;

namespace Stravaig.RulesEngine.Tests
{
    [TestFixture]
    public partial class OperatorBuilderServiceLocatorTests
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

        [Test]
        public void ShowDebugOutput()
        {
            // Not a test, but a quick way to visually inspect the debug info
            // string that would be displayed to a developer in the debugger.
            var locator = new OperatorBuilderLocator();
            string debugInfo = locator.DEBUG_AvailableBuilders;
            Console.WriteLine(debugInfo);
        }
    }
}