using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.Settings
{
    public sealed class DelegateSettings
    {
        public Delegate DelegateFunction { get; private set; }
        public Type InputType { get; private set; }
        public Type OutputType { get; private set; }

        public DelegateSettings(Delegate delegateFunction)
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
            this.OutputType = delegateReturnType;
        }
    }
}