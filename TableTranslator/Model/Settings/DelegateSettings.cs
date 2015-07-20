using System;
using System.Linq;
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

            var delegateParameters = delegateFunction.Method.GetParameters();
            var delegateReturnType = delegateFunction.Method.ReturnType;

            if (delegateParameters.Count() != 1)
            {
                throw new TableTranslatorConfigurationException("Delegate must have one parameter.");
            }

            if (delegateReturnType == typeof(void))
            {
                throw new TableTranslatorConfigurationException("Delegate must return data.");
            }

            this.DelegateFunction = delegateFunction;
            this.InputType = delegateParameters[0].ParameterType; // checking above that it has only one parameter
            this.OutputType = delegateReturnType;
        }
    }
}