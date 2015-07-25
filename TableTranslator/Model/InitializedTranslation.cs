using System.Collections.Generic;
using System.Data;
using System.Linq;
using TableTranslator.Abstract;

namespace TableTranslator.Model
{
    public class InitializedTranslation : TranslationBase
    {
        internal readonly Dictionary<string, DataTable> Structures = new Dictionary<string, DataTable>();

        internal static InitializedTranslation CreateInstance(ICloneable<TranslationBase> translation, IEnumerable<TranslationEngine> engines)
        {
            var initializedTranslation = new InitializedTranslation(translation);
            engines.ToList().ForEach(e => initializedTranslation.Structures.Add(e.Name, e.BuildDataTableStructure(initializedTranslation)));
            return initializedTranslation;
        }

        private InitializedTranslation(ICloneable<TranslationBase> translation)
        {
            var clone = translation.DeepClone();
            this.TranslationUniqueIdentifier = clone.TranslationUniqueIdentifier;
            this.TraversedGenericArguments = clone.TraversedGenericArguments;
            this.TranslationProfile = clone.TranslationProfile;
            this.TranslationSettings = clone.TranslationSettings;
            this.ColumnConfigurations = clone.ColumnConfigurations;
        }
    }
}