namespace TableTranslator.Model.Settings
{
    public class DbParameterSettings
    {
        private readonly string _parameterName;
        private readonly string _databaseObjectName;
        private readonly DatabaseType _databaseType;

        public DbParameterSettings(string parameterName, string databaseObjectName, DatabaseType databaseType)
        {
            this._databaseObjectName = databaseObjectName;
            this._databaseType = databaseType;
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

        public DatabaseType DatabaseType
        {
            get { return this._databaseType; }
        }
    }
}