using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TableTranslator.Abstract;
using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator
{
    internal class TranslationEngine : ITranslationEngine
    {
        public void Initialize(IEnumerable<InitializedTranslation> translations)
        {
            foreach (var trans in translations)
            {
                var table = new DataTable(trans.TranslationSettings.TranslationName);

                foreach (var colConfig in trans.ColumnConfigurations.OrderBy(x => x.Ordinal))
                {
                    table.Columns.Add(BuildDataColumn(trans, colConfig));
                }
                trans.Structure = table;
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

        private static DataColumn BuildDataColumn(TranslationBase translation, ColumnConfigurationBase colConfig)
        {
            // will get the "true" type for nullable types (e.g. int?, DateTime?, etc.), otherwise it will be null
            var underlyingTypeOfNullableType = Nullable.GetUnderlyingType(colConfig.OutputType);

            return new DataColumn(
                BuildFullColumnName(colConfig.ColumnName, translation),
                underlyingTypeOfNullableType ?? colConfig.OutputType)
            {
                // string can be null even though it doesn't implement Nullable<> and is a value type
                AllowDBNull = underlyingTypeOfNullableType != null || !colConfig.OutputType.IsValueType || colConfig.OutputType == typeof(string)
            };
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