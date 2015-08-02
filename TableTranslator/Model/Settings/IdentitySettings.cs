using System;
using System.Data;

namespace TableTranslator.Model.Settings
{
    public abstract class IdentitySettings
    {
        private string _providedColumnName { get; set; }
        protected internal abstract Type Type { get; }
        protected internal abstract bool IsAutoGenerated { get; }

        protected IdentitySettings()
        {
        }

        protected IdentitySettings(string columnName)
        {
            this._providedColumnName = columnName;
        }

        protected internal string ColumnName => !string.IsNullOrWhiteSpace(this._providedColumnName)
            ? this._providedColumnName
            : string.Format("Column0");

        protected internal virtual DataColumn GenerateIdentityColumn() => 
            new DataColumn(this.ColumnName, this.Type)
            {
                Unique = true
            };

        protected internal abstract object GetValue(object previousValue);
    }
}