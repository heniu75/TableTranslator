using System.Data;
using TableTranslator.Abstract;

namespace TableTranslator.Model
{
    public class InitializedTranslation : TranslationBase
    {
        internal DataTable Structure { get; set; }

        internal static InitializedTranslation CreateInstance(ICloneable<TranslationBase> translation)
        {
            return new InitializedTranslation(translation);
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