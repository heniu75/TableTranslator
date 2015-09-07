using System;

namespace TableTranslator.Model.Settings
{
    public class DbParameterSettings
    {
        public DbParameterSettings(string parameterName, string databaseObjectName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentNullException(nameof(parameterName));
            if (string.IsNullOrWhiteSpace(databaseObjectName)) throw new ArgumentNullException(nameof(databaseObjectName));

            this.DatabaseObjectName = databaseObjectName;
            this.DatabaseType = DatabaseType.Sql;
            this.ParameterName = parameterName;
        }

        public string ParameterName { get; }
        public string DatabaseObjectName { get; }
        internal DatabaseType DatabaseType { get; }
    }
}