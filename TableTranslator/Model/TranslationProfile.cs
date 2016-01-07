using TableTranslator.Abstract;
using TableTranslator.Exceptions;
using TableTranslator.Helpers;
using TableTranslator.Model.Settings;

namespace TableTranslator.Model
{
    public abstract class TranslationProfile : ICloneable<TranslationProfile>
    {
        private ITranslationStore _translationStore;
        protected internal abstract void Configure();

        /// <summary>
        /// Determined is the profile has been initialized by the translator
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// The current state of the profile in the translator
        /// </summary>
        public TranslationProfileState TranslationProfileState { get; internal set; }

        /// <summary>
        /// Name of the profile as registered with the translator
        /// </summary>
        public virtual string ProfileName => this.GetType().BuildFormattedName();

        /// <summary>
        /// Prefix for all column names for all column configurations in the profile
        /// </summary>
        protected internal virtual string ColumnNamePrefix => string.Empty;

        /// <summary>
        /// Suffix for all column names for all column configurations in the profile
        /// </summary>
        protected internal virtual string ColumnNameSuffix => string.Empty;

        internal void Initialize(ITranslationStore translationStore)
        {
            this._translationStore = translationStore;
            Configure();
            this.TranslationProfileState = TranslationProfileState.Initialized;
            this.IsInitialized = true;
        }

        /// <summary>
        /// Adds a translation for type TTranslationType to the translation profile
        /// </summary>
        /// <typeparam name="TTranslationType">Type that the translation is for</typeparam>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<TTranslationType> AddTranslation<TTranslationType>() where TTranslationType : new()
        {
            return AddTranslation<TTranslationType>(new TranslationSettings());
        }

        /// <summary>
        /// Adds a translation for type TTranslationType to the translation profile
        /// </summary>
        /// <typeparam name="TTranslationType">Type that the translation is for</typeparam>
        /// <param name="translationSettings">Additional configuration settings for the translation</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<TTranslationType> AddTranslation<TTranslationType>(TranslationSettings translationSettings) 
            where TTranslationType : new()
        {
            try
            {
                return this._translationStore.AddTranslation<TTranslationType>(this.ProfileName, translationSettings);
            }
            catch (TableTranslatorConfigurationException ex)
            {
                ex.TranslationProfile = this;
                throw;
            }
        }

        public TranslationProfile ShallowClone()
        {
            return this.MemberwiseClone() as TranslationProfile;
        }

        public TranslationProfile DeepClone()
        {
            return ShallowClone();
        }
    }
}