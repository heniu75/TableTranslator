using System;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.ColumnConfigurations.Identity
{
    public abstract class ProviderIdentityColumnConfiguration : IdentityColumnConfiguration
    {
        protected ProviderIdentityColumnConfiguration(Type type) : base(type)
        {
            this.Validate();
        }

        protected ProviderIdentityColumnConfiguration(Type type, string columnName) : base(type, columnName)
        {
            this.Validate();
        }

        private void Validate()
        {
            var value = GetValue(null);
            if (value == null)
            {
                throw new TableTranslatorConfigurationException(
                    "The GetValue() method for identity column configurations cannot return null values");
            }

            if (value.GetType() != this.DataType)
            {
                throw new TableTranslatorConfigurationException(
                    "The return type of 'GetValue()' and the 'DataType' property must be of the same type for identity column configurations. " +
                    $"Currently the return type of 'GetValue()' is '{value.GetType()}' and the type of the 'DataType' property is '{this.DataType}'");
            }
        }
    }
}