using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TableTranslator.Abstract;
using TableTranslator.Helpers;
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

        public DataTable FillDataTable<T>(InitializedTranslation translation, IEnumerable<T> data, bool isForDbParameter) where T : new()
        {
            var dataTable = translation.Structure.Clone();
            if (data == null) return dataTable;
            
            foreach (var x in data)
            {
                /* ReSharper disable once CompareNonConstrainedGenericWithNull
                I have Jon Skeet's express permission to ignore this warning :) (http://stackoverflow.com/questions/5340817/what-should-i-do-about-possible-compare-of-value-type-with-null) */
                if (x == null) continue;

                var row = dataTable.NewRow();
                foreach (var colConfig in translation.ColumnConfigurations)
                {
                    var tmp = colConfig.GetValueFromObject(x);
                    row[BuildFullColumnName(colConfig.ColumnName, translation)] = 
                        !isForDbParameter 
                            ? tmp ?? colConfig.NullReplacement
                            : tmp ?? colConfig.NullReplacement ?? DBNull.Value; // we want to use DBNull is this destined for a DB
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public DbParameter BuildDbParameter<T>(InitializedTranslation translation, IEnumerable<T> data, DbParameterSettings dbParameterSettings)
             where T : new()
        {
            var dataTable = FillDataTable(translation, data, true);
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
            return new DataColumn(
                BuildFullColumnName(colConfig.ColumnName, translation),
                colConfig.OutputType.GetPureType()) // will get the "underlying" type for nullable valye types (e.g. int?) if needed
            {
                AllowDBNull = colConfig.OutputType.IsNullAssignable()
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