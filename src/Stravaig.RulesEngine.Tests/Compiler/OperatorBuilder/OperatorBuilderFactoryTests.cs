using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine.Compiler;

namespace Stravaig.RulesEngine.Tests.Compiler.OperatorBuilder
{
    [TestFixture]
    public class OperatorBuilderFactoryTests
    {
        public class SimpleOperatorBuilder : RulesEngine.Compiler.OperatorBuilders.OperatorBuilder
        {
            public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
            {
                return Expression.Constant(true);
            }
        }

        public class GenericOperatorBuilder<T> : RulesEngine.Compiler.OperatorBuilders.OperatorBuilder
        {
            public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
            {
                return Expression.Constant(true);
            }
        }

        [Test]
        public void CanCreateSimpleOperatorBuilder()
        {
            var factory = new OperatorBuilderFactory();
            var result = factory.Create(typeof(SimpleOperatorBuilder));

            result.ShouldBeOfType(typeof(SimpleOperatorBuilder));
        }

        [Test]
        public void CanCreateGenericOperatorBuilder()
        {
            var factory = new OperatorBuilderFactory();
            var result = factory.Create(typeof(GenericOperatorBuilder<double>));
            result.ShouldBeOfType(typeof(GenericOperatorBuilder<double>));
            
        }
    }
}