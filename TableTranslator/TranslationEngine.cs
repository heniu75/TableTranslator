using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TableTranslator.Abstract;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator
{
    internal class TranslationEngine : ITranslationEngine
    {
        public void Initialize(IEnumerable<InitializedTranslation> translations)
        {
            foreach (var t in translations)
            {
                var table = new DataTable(t.TranslationSettings.TranslationName);

                foreach (var config in t.ColumnConfigurations.OrderBy(x => x.Ordinal))
                {
                    var underlyingNullableType = Nullable.GetUnderlyingType(config.OutputType);
                    var column = new DataColumn(
                        BuildFullColumnName(config.ColumnName, t), 
                        underlyingNullableType ?? config.OutputType)
                    {
                        // string can be null even though it doesn't implement Nullable<> and is a value type
                        AllowDBNull = underlyingNullableType != null || !config.OutputType.IsValueType || config.OutputType == typeof(string)
                    };
                    table.Columns.Add(column);
                }
                t.Structure = table;
            }
        }

        public DataTable TranslateToDataTable<T>(InitializedTranslation translation, IEnumerable<T> data, bool isForDbParameter) where T : new()
        {
            var dataTable = translation.Structure.Clone();
            if (data == null) return dataTable;
            
            foreach (var x in data)
            {
                if (x == null) continue;

                var row = dataTable.NewRow();
                foreach (var colConfig in translation.ColumnConfigurations)
                {
                    var tmp = colConfig.GetValueFromObject(x);
                    row[BuildFullColumnName(colConfig.ColumnName, translation)] = 
                        !isForDbParameter 
                            ? tmp ?? colConfig.NullReplacement
                            : tmp ?? colConfig.NullReplacement ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public DbParameter TranslateToDbParameter<T>(InitializedTranslation translation, IEnumerable<T> data, DbParameterSettings dbParameterSettings)
             where T : new()
        {
            var dataTable = TranslateToDataTable(translation, data, true);
            dataTable.TableName = dbParameterSettings.DatabaseObjectName;

            switch (dbParameterSettings.DatabaseType)
            {
                case DatabaseType.Sql:
                    return new SqlParameter(dbParameterSettings.ParameterName, SqlDbType.Structured)
                    {
                        Value = dataTable,
                        TypeName = dbParameterSettings.DatabaseObjectName
                    };
                case DatabaseType.Oracle:
                case DatabaseType.MySql:
                    throw new NotImplementedException(string.Format("Database type {0} has not been implemented yet.", dbParameterSettings.DatabaseType));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string BuildFullColumnName(string columnName, TranslationBase translation)
        {
            return string.Format("{0}{1}{2}",
                !string.IsNullOrEmpty(translation.TranslationSettings.ColumnNamePrefix) ? translation.TranslationSettings.ColumnNamePrefix : translation.TranslationProfile.ColumnNamePrefix,
                columnName,
                !string.IsNullOrEmpty(translation.TranslationSettings.ColumnNameSuffix) ? translation.TranslationSettings.ColumnNameSuffix : translation.TranslationProfile.ColumnNameSuffix);
        }
    }
}