using System.Collections.Generic;
using System.Data;
using TableTranslator.Helpers;
using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations;

namespace TableTranslator.Engines
{
    public abstract class TranslationEngine
    {
        internal string Name => this.GetType().Name;

        internal abstract DataTable BuildDataTableStructure(InitializedTranslation translation);
        internal abstract DataTable FillDataTable<T>(InitializedTranslation translation, IEnumerable<T> data) where T : new();
        internal abstract object GetColumnValue<T>(T data, NonIdentityColumnConfiguration colConfig) where T : new();

        protected static DataColumn BuildDataColumn(TranslationBase translation, NonIdentityColumnConfiguration colConfig)
        {
            return new DataColumn(
                BuildFullColumnName(colConfig.ColumnName, translation),
                colConfig.OutputType.GetPureType()) // will get the "underlying" type for nullable valye types (e.g. int?) if needed
            {
                AllowDBNull = colConfig.OutputType.IsNullAssignable()
            };
        }

        protected static string BuildFullColumnName(string columnName, TranslationBase translation)
        {
            return string.Format("{0}{1}{2}",
                !string.IsNullOrEmpty(translation.TranslationSettings.ColumnNamePrefix) ? translation.TranslationSettings.ColumnNamePrefix : translation.TranslationProfile.ColumnNamePrefix,
                columnName,
                !string.IsNullOrEmpty(translation.TranslationSettings.ColumnNameSuffix) ? translation.TranslationSettings.ColumnNameSuffix : translation.TranslationProfile.ColumnNameSuffix);
        }
    }
}