using System;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.ColumnConfigurations
{
    public sealed class SimpleValueColumnConfiguration : ColumnConfigurationBase
    {
        private readonly string _columnName;
        private readonly object _value;
        private readonly Type _outputType;

        public SimpleValueColumnConfiguration(object value, Type outputType, int ordinal, string columnName, object nullReplacement)
            : base(ordinal, nullReplacement)
        {
            this._value = value;
            this._columnName = columnName;
            this._outputType = outputType;

            ValidateInput();
        }

        public override string ColumnName
        {
            get { return !string.IsNullOrEmpty(this._columnName) ? this._columnName : base.DefaultColumnName; }
        }

        public override Type OutputType
        {
            get { return this._outputType; }
        }

        public override object GetValueFromObject(object obj)
        {
            return this._value;
        }

        internal override void ValidateInput()
        {
            if (this._outputType == null)
            {
                throw new ArgumentNullException("outputType");
            }

            if (this._value != null && this._value.GetType() != this._outputType)
            {
                throw new TableTranslatorConfigurationException();
            }

            if (base.NullReplacement != null && base.NullReplacement != DBNull.Value && base.NullReplacement.GetType() != this._outputType)
            {
                throw new TableTranslatorConfigurationException();
            }
        }
    }
}