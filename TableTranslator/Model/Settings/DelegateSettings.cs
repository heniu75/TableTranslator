using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.Settings
{
    /// <summary>
    /// Additional settings used to define a delegate
    /// </summary>
    internal sealed class DelegateSettings
    {
        /// <summary>
        /// Delegate used in retrieving column value 
        /// </summary>
        internal Delegate DelegateFunction { get; set; }

        /// <summary>
        /// Input parameter type of the delegate
        /// </summary>
        internal Type InputType { get; set; }

        /// <summary>
        /// Return value type of the delegate
        /// </summary>
        internal Type ReturnType { get; set; }

        /// <summary>
        /// Creates an instance of DelegateSettings
        /// </summary>
        /// <param name="delegateFunction">Delegate function to be used in retrieving column value</param>
        internal DelegateSettings(Delegate delegateFunction)
        {
            if (delegateFunction == null)
            {
                throw new ArgumentNullException();
            }

            var delegateParameters = delegateFunction.Method.GetParameters().Where(x => x.ParameterType != typeof(Closure)).ToList();
            var delegateReturnType = delegateFunction.Method.ReturnType;

            if (delegateParameters.Count() != 1)
            {
                throw new TableTranslatorConfigurationException("Delegate must take exactly one parameter.");
            }

            if (delegateReturnType == typeof(void))
            {
                throw new TableTranslatorConfigurationException("Delegate must return a value (cannot be void).");
            }

            this.DelegateFunction = delegateFunction;
            this.InputType = delegateParameters.First().ParameterType; // checking above that it has only one parameter
            this.ReturnType = delegateReturnType;
        }
    }
}