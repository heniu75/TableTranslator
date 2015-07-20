using System;
using TableTranslator.Exceptions;
using TableTranslator.Model.Settings;

namespace TableTranslator.Model.ColumnConfigurations
{
    public sealed class DelegateColumnConfiguration : ColumnConfigurationBase
    {
        private readonly string _columnName;
        public DelegateSettings DelegateSettings { get; private set; }

        /// <summary>
        /// Builds a DelegateColumnConfiguration using the provided configuration settings
        /// </summary>
        /// <param name="delegateSettings">Settings describing the delegate to be called.</param>
        /// <param name="ordinal">Ordinal for the column.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="nullReplacement">Replacement value if the value returned by the delegate function is null. 
        /// Type must match the return type of the delegate in the delegateSettings.</param>
        public DelegateColumnConfiguration(DelegateSettings delegateSettings, int ordinal, string columnName, object nullReplacement) : base(ordinal, nullReplacement)
        {
            this.DelegateSettings = delegateSettings;
            this._columnName = columnName;

            ValidateInput();
        }

        public override string ColumnName
        {
            get { return !string.IsNullOrEmpty(this._columnName) ? this._columnName : base.DefaultColumnName; }
        }

        public override Type OutputType
        {
            get { return this.DelegateSettings.OutputType; }
        }

        public override object GetValueFromObject(object obj)
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

            if (this.NullReplacement != null && this.NullReplacement != DBNull.Value && this.DelegateSettings.OutputType != base.NullReplacement.GetType())
            {
                throw new TableTranslatorConfigurationException();
            }
        }
    }
}