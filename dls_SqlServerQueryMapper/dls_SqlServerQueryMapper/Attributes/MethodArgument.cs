using System;
using System.Collections.Generic;
using System.Linq;

namespace dls_SqlServerQueryMapper.Attributes
{
    /// <summary>
    /// Concrete method argument with argument number, type and value
    /// </summary>
    internal class MethodArgument
    {
        /// <summary>
        /// Number of argument on method
        /// </summary>
        public int ArgNumber { get; }

        /// <summary>
        /// Type of argument 
        /// </summary>
        public Type ArgType { get; }

        /// <summary>
        /// Value of argument
        /// </summary>
        public object ArgValue { get; }

        /// <summary>
        /// Creates a new method argument with specified parameters
        /// </summary>
        /// <param name="argNumber">argument number on method (first will have number 0)</param>
        /// <param name="argType">argument type</param>
        /// <param name="argValue">argument value</param>
        internal MethodArgument(int argNumber, Type argType, object argValue)
        {
            ArgNumber = argNumber;
            ArgType = argType;
            ArgValue = argValue;
        }

        /// <summary>
        /// Collection checking: is not null, has all numbers from 0 to (Size - 1), all items get type not null and value can be casted to its type.
        /// </summary>
        /// <param name="methodArguments">collection to validate</param>
        internal static void CheckIsValid(IList<MethodArgument> methodArguments)
        {
            var errorSufix = $"error on {typeof(MethodArgument).Name}.CheckIsValid() method: ";
            if (methodArguments == null)
            {
                throw new Exception(errorSufix + "method argument collection is null!!!");
            }

            var processed = new List<int>();
            for (int i = 0; i < methodArguments.Count; i++)
            {
                // Check arg number i was not processed
                if (processed.Contains(i))
                    throw new Exception(errorSufix + $"method argument with number {i} is duplicated");

                processed.Add(i);

                // Check argument number i exists
                var methodArgument = methodArguments.Where(a => a.ArgNumber == i).FirstOrDefault();
                if (methodArgument == default)
                    throw new Exception(errorSufix + $"method argument with number {i} is not present");

                // Check type not null
                if (methodArgument?.ArgType == null)
                    throw new Exception(errorSufix + $"method argument with number {i} has not type");

                // Check that value is coherent with type
                try
                {
                    var x = Convert.ChangeType(methodArgument.ArgValue, methodArgument.ArgType);
                }
                catch (Exception ex)
                {
                    throw new Exception(errorSufix + $"method argument with number {i} has a value not valid", ex);
                }
            }
        }
    }
}
