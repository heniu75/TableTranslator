using System;

namespace TableTranslator.Model.Settings
{
    /// <summary>
    /// Additional settings used to define a database parameter
    /// </summary>
    public class DbParameterSettings
    {
        /// <summary>
        /// Creates an instance of DbParameterSettings
        /// </summary>
        /// <param name="parameterName">Name of the database parameter</param>
        /// <param name="databaseObjectName">Name of the database object (most likley a table value parameter type name)</param>
        public DbParameterSettings(string parameterName, string databaseObjectName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentNullException(nameof(parameterName));
            if (string.IsNullOrWhiteSpace(databaseObjectName)) throw new ArgumentNullException(nameof(databaseObjectName));

            this.DatabaseObjectName = databaseObjectName;
            this.DatabaseType = DatabaseType.Sql;
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Name of the database parameter
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Name of the database object (most likley a table value parameter type name)
        /// </summary>
        public string DatabaseObjectName { get; }

        internal DatabaseType DatabaseType { get; }
    }
}