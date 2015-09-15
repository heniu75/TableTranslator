using System;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.ColumnConfigurations.NonIdentity
{
    internal sealed class SimpleValueColumnConfiguration : NonIdentityColumnConfiguration
    {
        private readonly string _columnName;
        private readonly object _value;
        private readonly Type _columnDataType;

        internal SimpleValueColumnConfiguration(object value, Type columnDataType, int ordinal, string columnName, object nullReplacement)
            : base(ordinal, nullReplacement)
        {
            this._value = value;
            this._columnName = columnName;
            this._columnDataType = columnDataType;

            ValidateInput();
        }

        public override string ColumnName => !string.IsNullOrEmpty(this._columnName) ? this._columnName : base.OrdinalColumnName;
        public override Type ColumnDataType => this._columnDataType;

        internal override object GetValueFromObject(object obj)
        {
            return this._value;
        }

        internal override void ValidateInput()
        {
            if (this._columnDataType == null)
            {
                throw new ArgumentNullException("outputType");
            }

            if (this._value != null && this._value.GetType() != this._columnDataType)
            {
                throw new TableTranslatorConfigurationException("The output type must be of the same type as the value provided.");
            }

            if (base.NullReplacement != null && base.NullReplacement != DBNull.Value && base.NullReplacement.GetType() != this._columnDataType)
            {
                throw new TableTranslatorConfigurationException("Null replacement for simple value must be either of the same type as the value provided, null, or DBNull.");
            }
        }
    }
}