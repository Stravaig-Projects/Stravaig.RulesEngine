using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine.Compiler.OperatorBuilders;
using Stravaig.RulesEngine.Compiler.OperatorBuilders.General;

namespace Stravaig.RulesEngine.Tests
{
    public partial class OperatorBuilderServiceLocatorTests
    {
        public class StringEqualsOperatorBuilder : OperatorBuilder
        {
            public override string[] OperatorNames => new[] { "Equals", "==" };

            public StringEqualsOperatorBuilder()
                : base(typeof(string))
            {
            }

            public override Expression Build(Expression leftPropertyExpression, string rightValueAsString, Enum[] modifiers)
            {
                throw new System.NotImplementedException();
            }
        }
        
        public class DecimalEqualsOperatorBuilder : OperatorBuilder
        {
            public override string[] OperatorNames => new[] { "Equals", "==" };

            public DecimalEqualsOperatorBuilder()
                : base(typeof(decimal))
            {
            }

            public override Expression Build(Expression leftPropertyExpression, string rightValueAsString, Enum[] modifiers)
            {
                throw new System.NotImplementedException();
            }
        }
        
        [Test]
        public void StringTypeForEqualsOperatorResultsInStringSpecificBuilder()
        {
            var locator = new OperatorBuilderLocator(
                typeof(EqualsOperatorBuilder),
                typeof(DecimalEqualsOperatorBuilder),
                typeof(StringEqualsOperatorBuilder));

            var builder = locator.GetBuilder("==", typeof(string));
            builder.ShouldBeOfType(typeof(StringEqualsOperatorBuilder));
        }

        [Test]
        public void IntTypeForEqualsOperatorResultsInGeneralBuilder()
        {
            var locator = new OperatorBuilderLocator(
                typeof(EqualsOperatorBuilder),
                typeof(DecimalEqualsOperatorBuilder),
                typeof(StringEqualsOperatorBuilder));

            var builder = locator.GetBuilder("==", typeof(int));
            builder.ShouldBeOfType(typeof(EqualsOperatorBuilder));
        }
    }
}