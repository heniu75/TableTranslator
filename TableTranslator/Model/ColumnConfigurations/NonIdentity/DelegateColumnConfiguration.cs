using System;
using TableTranslator.Exceptions;
using TableTranslator.Model.Settings;

namespace TableTranslator.Model.ColumnConfigurations.NonIdentity
{
    internal sealed class DelegateColumnConfiguration : NonIdentityColumnConfiguration
    {
        private readonly string _columnName;
        internal DelegateSettings DelegateSettings { get; }

        internal DelegateColumnConfiguration(DelegateSettings delegateSettings, int ordinal, string columnName, object nullReplacement) : base(ordinal, nullReplacement)
        {
            this.DelegateSettings = delegateSettings;
            this._columnName = columnName;

            ValidateInput();
        }

        public override string ColumnName => !string.IsNullOrEmpty(this._columnName) ? this._columnName : base.OrdinalColumnName;
        public override Type ColumnDataType => this.DelegateSettings.ReturnType;

        internal override object GetValueFromObject(object obj)
        {
            // call the delegate function and get the return value
            return this.DelegateSettings.DelegateFunction.DynamicInvoke(obj);
        }

        internal override void ValidateInput()
        {
            if (this.DelegateSettings == null)
            {
                throw new ArgumentNullException("delegateSettings");
            }

            if (this.NullReplacement != null && this.NullReplacement != DBNull.Value && this.DelegateSettings.ReturnType != base.NullReplacement.GetType())
            {
                throw new TableTranslatorConfigurationException("Null replacement for delegate must be either of the same type as the delegate's return type, null, or DBNull.");
            }
        }
    }
}