using System;
using System.Collections.Generic;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    public class OperatorBuilderTypeComparer : IComparer<Type?>
    {
        private readonly Type _desiredLeftType;

        public OperatorBuilderTypeComparer(Type desiredLeftType)
        {
            _desiredLeftType = desiredLeftType;
        }

        public int Compare(Type? x, Type? y)
        {
            // 1 = X is greater than Y
            // 0 = X is the same as Y
            // -1 = X is less than Y.
            
            if (x == y) // Includes when x == y == null
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;
            
            // Highest priority of things is the specific type.
            if (x == _desiredLeftType)
                return 1;
            
            if (y == _desiredLeftType)
                return -1;

            // Next highest priority of things is _desiredLeftType being a
            // base class
            if (x.IsSubclassOf(_desiredLeftType))
                return 1;

            if (y.IsSubclassOf(_desiredLeftType))
                return -1;
            
            // Anything else...
            return 0;
        }
    }
}