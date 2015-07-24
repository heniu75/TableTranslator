using TableTranslator.Abstract;

namespace TableTranslator.Model
{
    internal class InitializedTranslationProfile
    {
        private string ColumnNamePrefix { get; set; }
        private string ColumnNameSuffix { get; set; }
        private string ProfileName { get; set; }

        public InitializedTranslationProfile(ICloneable<TranslationProfile> profile)
        {
            var clone = profile.DeepClone();
            this.ColumnNamePrefix = clone.ColumnNamePrefix;
            this.ColumnNameSuffix = clone.ColumnNameSuffix;
            this.ProfileName = clone.ProfileName;
        }
    }
}