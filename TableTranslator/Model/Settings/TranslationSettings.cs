using TableTranslator.Abstract;

namespace TableTranslator.Model.Settings
{
    public sealed class TranslationSettings : ICloneable<TranslationSettings>
    {
        public IdentitySettings IdentitySettings { get; private set; }
        public string TranslationName { get; internal set; }
        public string ColumnNamePrefix { get; private set; }
        public string ColumnNameSuffix { get; private set; }

        public TranslationSettings(string translationName = "", string columnNamePrefix = "", string columnNameSuffix = "")
        {
            this.TranslationName = translationName;
            this.ColumnNamePrefix = columnNamePrefix;
            this.ColumnNameSuffix = columnNameSuffix;
        }

        public TranslationSettings(IdentitySettings identitySettings, string translationName = "", string columnNamePrefix = "", string columnNameSuffix = "")
        {
            this.IdentitySettings = identitySettings;
            this.TranslationName = translationName;
            this.ColumnNamePrefix = columnNamePrefix;
            this.ColumnNameSuffix = columnNameSuffix;
        }

        public TranslationSettings ShallowClone() => this.MemberwiseClone() as TranslationSettings;
        public TranslationSettings DeepClone() => ShallowClone();
    }
}