using TableTranslator.Abstract;

namespace TableTranslator.Model
{
    internal class InitializedTranslationProfile
    {
        internal string ColumnNamePrefix { get; set; }
        internal string ColumnNameSuffix { get; set; }
        internal string ProfileName { get; set; }

        public InitializedTranslationProfile(ICloneable<TranslationProfile> profile)
        {
            var clone = profile.DeepClone();
            this.ColumnNamePrefix = clone.ColumnNamePrefix;
            this.ColumnNameSuffix = clone.ColumnNameSuffix;
            this.ProfileName = clone.ProfileName;
        }
    }
}