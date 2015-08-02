using TableTranslator.Abstract;
using TableTranslator.Model.ColumnConfigurations;

namespace TableTranslator.Model.Settings
{
    public sealed class TranslationSettings : ICloneable<TranslationSettings>
    {
        public IdentityColumnConfiguration IdentityColumnConfiguration { get; private set; }
        public string TranslationName { get; internal set; }
        public string ColumnNamePrefix { get; private set; }
        public string ColumnNameSuffix { get; private set; }

        public TranslationSettings(string translationName = "", string columnNamePrefix = "", string columnNameSuffix = "")
        {
            this.TranslationName = translationName;
            this.ColumnNamePrefix = columnNamePrefix;
            this.ColumnNameSuffix = columnNameSuffix;
        }

        public TranslationSettings(IdentityColumnConfiguration identityColumnConfiguration, string translationName = "", string columnNamePrefix = "", string columnNameSuffix = "")
        {
            this.IdentityColumnConfiguration = identityColumnConfiguration;
            this.TranslationName = translationName;
            this.ColumnNamePrefix = columnNamePrefix;
            this.ColumnNameSuffix = columnNameSuffix;
        }

        public TranslationSettings ShallowClone() => this.MemberwiseClone() as TranslationSettings;
        public TranslationSettings DeepClone() => ShallowClone();
    }
}