using System;
using TableTranslator.Model.ColumnConfigurations;

namespace TableTranslator.Engines
{
    internal class DbParameterTranslationEngine : SimpleTranslationEngine
    {
        internal override object GetColumnValue<T>(T data, ColumnConfigurationBase colConfig)
        {
            return colConfig.GetValueFromObject(data) ?? colConfig.NullReplacement ?? DBNull.Value;
        }
    }
}