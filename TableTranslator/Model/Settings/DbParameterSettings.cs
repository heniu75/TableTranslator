using System;

namespace TableTranslator.Model.Settings
{
    public class DbParameterSettings
    {
        private readonly string _parameterName;
        private readonly string _databaseObjectName;
        private readonly DatabaseType _databaseType;

        public DbParameterSettings(string parameterName, string databaseObjectName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentNullException("parameterName");
            if (string.IsNullOrWhiteSpace(databaseObjectName)) throw new ArgumentNullException("databaseObjectName");

            this._databaseObjectName = databaseObjectName;
            this._databaseType = DatabaseType.Sql;
            this._parameterName = parameterName;
        }

        public string ParameterName
        {
            get { return this._parameterName; }
        }

        public string DatabaseObjectName
        {
            get { return this._databaseObjectName; }
        }

        internal DatabaseType DatabaseType
        {
            get { return this._databaseType; }
        }
    }
}