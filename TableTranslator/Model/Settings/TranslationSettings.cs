using TableTranslator.Abstract;

namespace TableTranslator.Model.Settings
{
    public sealed class TranslationSettings : ICloneable<TranslationSettings>
    {
        public string TranslationName { get; internal set; }
        public string ColumnNamePrefix { get; internal set; }
        public string ColumnNameSuffix { get; internal set; }

        public TranslationSettings(string translationName = "", string columnNamePrefix = "", string columnNameSuffix = "")
        {
            this.TranslationName = translationName;
            this.ColumnNamePrefix = columnNamePrefix;
            this.ColumnNameSuffix = columnNameSuffix;
        }

        public TranslationSettings ShallowClone()
        {
            return this.MemberwiseClone() as TranslationSettings;
        }

        public TranslationSettings DeepClone()
        {
            return ShallowClone();
        }
    }
}