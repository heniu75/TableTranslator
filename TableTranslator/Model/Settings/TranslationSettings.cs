using TableTranslator.Abstract;
using TableTranslator.Model.ColumnConfigurations.Identity;

namespace TableTranslator.Model.Settings
{
    /// <summary>
    /// Additional settings used to define a translation
    /// </summary>
    public sealed class TranslationSettings : ICloneable<TranslationSettings>
    {
        /// <summary>
        /// Name of the translation as stored in the translator
        /// </summary>
        public string TranslationName { get; set; }

        /// <summary>
        /// Prefix for all column names for all column configurations in the translation (overrides the translation profile level setting)
        /// </summary>
        public string ColumnNamePrefix { get; set; }

        /// <summary>
        /// Suffix for all column names for all column configurations in the translation (overrides the translation profile level setting)
        /// </summary>
        public string ColumnNameSuffix { get; set; }

        internal IdentityColumnConfiguration IdentityColumnConfiguration { get; set; }

        /// <summary>
        /// Creates an instance of TranslationSettings
        /// </summary>
        /// <param name="translationName">Name of the translation</param>
        /// <param name="columnNamePrefix">Prefix for all column names for all column configurations in the translation (overrides the translation profile level setting)</param>
        /// <param name="columnNameSuffix">Suffix for all column names for all column configurations in the translation (overrides the translation profile level setting)</param>
        public TranslationSettings(string translationName = "", string columnNamePrefix = "", string columnNameSuffix = "")
        {
            this.TranslationName = translationName;
            this.ColumnNamePrefix = columnNamePrefix;
            this.ColumnNameSuffix = columnNameSuffix;
        }

        /// <summary>
        /// Creates an instance of TranslationSettings with settings for an identity column that will be included with the translation
        /// </summary>
        /// <param name="identityColumnConfiguration">Settings for the identity column for the translation (e.g. Seeded, Guid, etc.)</param>
        /// <param name="translationName">Name of the translation</param>
        /// <param name="columnNamePrefix">Prefix for all column names for all column configurations in the translation (overrides the translation profile level setting)</param>
        /// <param name="columnNameSuffix">Suffix for all column names for all column configurations in the translation (overrides the translation profile level setting)</param>
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