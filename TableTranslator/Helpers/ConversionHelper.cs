using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Helpers
{
    internal static class ConversionHelper
    {
        internal static DbParameter WrapinDbParameter(this DataTable dataTable, DbParameterSettings dbParameterSettings)
        {
            if (dbParameterSettings == null)
            {
                throw new ArgumentNullException(nameof(dbParameterSettings));
            }

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
                    throw new NotImplementedException($"Database type {dbParameterSettings.DatabaseType} has not been implemented yet.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}