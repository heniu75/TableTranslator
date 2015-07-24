using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Abstract
{
    internal interface ITranslationEngine
    {
        void Initialize(IEnumerable<InitializedTranslation> translations);
        DataTable FillDataTable<T>(InitializedTranslation translation, IEnumerable<T> data, bool isForDbParameter = false) where T : new();
        DbParameter BuildDbParameter<T>(InitializedTranslation translation, IEnumerable<T> data, DbParameterSettings dbParameterSettings) where T : new();
    }
}