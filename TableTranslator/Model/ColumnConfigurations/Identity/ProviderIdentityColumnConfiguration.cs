using System;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.ColumnConfigurations.Identity
{
    /// <summary>
    /// Base class for identity column configurations where the implementation provides the values for the column
    /// </summary>
    public abstract class ProviderIdentityColumnConfiguration : IdentityColumnConfiguration
    {
        /// <summary>
        /// Creates an instance of LongSeededIdentityColumnConfiguration
        /// </summary>
        /// <param name="type">Data type for the identity column</param>
        protected ProviderIdentityColumnConfiguration(Type type) : base(type)
        {
            this.Validate();
        }

        /// <summary>
        /// Creates an instance of LongSeededIdentityColumnConfiguration
        /// </summary>
        /// <param name="type">Data type for the identity column</param>
        /// <param name="columnName">Name of the identity column</param>
        protected ProviderIdentityColumnConfiguration(Type type, string columnName) : base(type, columnName)
        {
            this.Validate();
        }

        private void Validate()
        {
            var value = GetNextValue(null);
            if (value == null)
            {
                throw new TableTranslatorConfigurationException(
                    "The GetNextValue() method for identity column configurations cannot return null values");
            }

            if (value.GetType() != this.ColumnDataType)
            {
                throw new TableTranslatorConfigurationException(
                    "The return type of 'GetNextValue()' and the 'DataType' property must be of the same type for identity column configurations. " +
                    $"Currently the return type of 'GetNextValue()' is '{value.GetType()}' and the type of the 'DataType' property is '{this.ColumnDataType}'");
            }
        }
    }
}