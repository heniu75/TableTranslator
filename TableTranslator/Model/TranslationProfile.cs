using TableTranslator.Abstract;
using TableTranslator.Exceptions;
using TableTranslator.Helpers;
using TableTranslator.Model.Settings;

namespace TableTranslator.Model
{
    public abstract class TranslationProfile : ICloneable<TranslationProfile>
    {
        public bool IsInitialized { get; private set; }
        public TranslationProfileState TranslationProfileState { get; internal set; }

        private ITranslationStore _translationStore;
        protected internal abstract void Configure();

        public virtual string ProfileName
        {
            get { return this.GetType().GetFormattedName(); }
        }
        protected internal virtual string ColumnNamePrefix { get { return string.Empty; } }
        protected internal virtual string ColumnNameSuffix { get { return string.Empty; } }

        internal void Initialize(ITranslationStore translationStore)
        {
            this._translationStore = translationStore;
            Configure();
            this.TranslationProfileState = TranslationProfileState.Initialized;
            this.IsInitialized = true;
        }

        protected internal TranslationExpression<T> AddTranslation<T>() where T : new()
        {
            return AddTranslation<T>(new TranslationSettings());
        }

        protected internal TranslationExpression<T> AddTranslation<T>(TranslationSettings translationSettings) where T : new()
        {
            try
            {
                return this._translationStore.AddTranslation<T>(this.ProfileName, translationSettings);
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