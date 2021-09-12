using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.IsContainedIn
{
    public abstract class IsContainedInOperatorBuilder<T> : OperatorBuilder
    {
        private char SeparatorCharacter = '|';
        protected override string OperatorName => "IsContainedIn";

        protected IsContainedInOperatorBuilder()
            : base(typeof(T))
        {
        }

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            var items = GetSetOfItems(rightValueAsString);
            var type = items.GetType();
            var containsMethod = type.GetMethod(
                nameof(items.Contains),
                new[]
                {
                    typeof(T)
                });
            if (containsMethod == null)
                throw new InvalidOperationException($"HashSet<{typeof(T).Name}>.Contains({typeof(T).Name}) was expected to exist.");

            var itemsExpression = Expression.Constant(items);           
            
            var result = Expression.Call(
                itemsExpression, // Hash Set containing rightValueAsString
                containsMethod, // Contains(object) method
                leftPropertyExpression); // left property expression arg0

            return result;

        }

        private HashSet<T> GetSetOfItems(string rightValueAsString)
        {
            string[] stringItems = ExtractRightValueInToStrings(rightValueAsString);
            T[] items = ConvertStringsToDesiredType(stringItems);
            return new HashSet<T>(items);
        }

        private T[] ConvertStringsToDesiredType(string[] stringItems)
        {
            T[] items = new T[stringItems.Length];
            for (int i = 0; i < stringItems.Length; i++)
            {
                items[i] = (T)Convert.ChangeType(stringItems[i], typeof(T));
            }

            return items;
        }

        private string[] ExtractRightValueInToStrings(string rightValueAsString)
        {
            var stringItems = rightValueAsString.Split(SeparatorCharacter)
                              ?? Array.Empty<string>();
            return stringItems;
        }
    }
}