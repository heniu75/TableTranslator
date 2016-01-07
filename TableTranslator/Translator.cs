using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using TableTranslator.Abstract;
using TableTranslator.Engines;
using TableTranslator.Exceptions;
using TableTranslator.Helpers;
using TableTranslator.Model;
using TableTranslator.Model.Settings;
using TableTranslator.Stores;

namespace TableTranslator
{
    /// <summary>
    /// Main entry point for TableTranslator interacting with profiles and translations
    /// </summary>
    public static class Translator
    {
        /// <summary>
        /// Determines if the translator has been initialized
        /// </summary>
        public static bool IsInitialized { get; private set; }

        private static readonly ITranslationStore _store = new TranslationStore();
        private static readonly List<TranslationEngine> _engines = new List<TranslationEngine>();

        /// <summary>
        /// Static constructor for set up before users call Initialize()
        /// </summary>
        static Translator()
        {
            AddEngine(new SimpleTranslationEngine());
            AddEngine(new DbParameterTranslationEngine());
        }

        /// <summary>
        /// Adds an engine to translator (private for now, but in the future we may allow users to create and add their own engines)
        /// </summary>
        /// <typeparam name="TEngine">Type of engine to add</typeparam>
        /// <param name="engine">Engine to add</param>
        private static void AddEngine<TEngine>(TEngine engine) where TEngine : TranslationEngine
        {
            _engines.Add(engine);
        }

        /// <summary>
        /// Initializes the translator with the supplied translation profiles and runs each profile's Configure() method.
        /// </summary>
        public static void Initialize()
        {
            if (IsInitialized) throw new TableTranslatorException("Table translator has already been initialized");
             _store.Initialize(_engines);
            IsInitialized = true;
        }

        /// <summary>
        /// Updates the translator with any profile or translation changes
        /// </summary>
        public static void ApplyUpdates()
        {
            if (!IsInitialized) throw new TableTranslatorException("You must initialize the translator before calling ApplyUpdates().");
            // currently just calls Initialize(), but separating it here for future enhancements (e.g. translation updates, column config updates, etc.)
            _store.Initialize(_engines);
        }

        /// <summary>
        /// Marks the translator as being not being initialzed and removes all profiles and translation 
        /// </summary>
        public static void Uninitialize()
        {
            _store.UnloadAll();
            IsInitialized = false;
        }

        #region add profiles

        /// <summary>
        /// Adds a profile to the translator
        /// </summary>
        /// <typeparam name="TProfile">Profile to add</typeparam>
        public static void AddProfile<TProfile>() where TProfile : TranslationProfile, new()
        {
            _store.AddProfile(new TProfile());
        }

        /// <summary>
        /// Adds a profile to the translator
        /// </summary>
        /// <param name="translationProfile">Profile to add</param>
        public static void AddProfile(TranslationProfile translationProfile)
        {
            _store.AddProfile(translationProfile);
        }

        /// <summary>
        /// Adds multiple profiles to the translator
        /// </summary>
        /// <param name="translationProfiles">Profiles to add</param>
        public static void AddProfiles(IEnumerable<TranslationProfile> translationProfiles)
        {
            if (translationProfiles == null) throw new ArgumentNullException(nameof(translationProfiles));
            var list = translationProfiles as TranslationProfile[] ?? translationProfiles.ToArray();
            if (list.Any(x => x == null)) throw new ArgumentException("Argument translationProfiles contained a item that was null.");
            foreach (var tp in list)
            {
                _store.AddProfile(tp);
            }
        }

        #endregion

        #region get profiles

        /// <summary>
        /// Gets all profiles in the translator
        /// </summary>
        /// <returns>All profiles in the translator</returns>
        public static IEnumerable<TranslationProfile> GetAllProfiles()
        {
            return _store.GetAllProfiles();
        }

        /// <summary>
        /// Filters profiles in the translator where the provided predicate returns true
        /// </summary>
        /// <param name="predicate">A function to test each profile in the translator for a condition</param>
        /// <returns>Profiles in the translator that match the provided predicate</returns>
        public static IEnumerable<TranslationProfile> GetProfiles(Func<TranslationProfile, bool> predicate)
        {
            return _store.GetProfiles(predicate);
        }

        /// <summary>
        /// Gets the profile in the translator with the provided name
        /// </summary>
        /// <param name="profileName">Name of the profile</param>
        /// <returns>Profile in the translator that has the provided name</returns>
        public static TranslationProfile GetProfile(string profileName)
        {
            return string.IsNullOrEmpty(profileName) ? null : _store.GetProfile(profileName);
        }

        #endregion

        #region remove profiles

        /// <summary>
        /// Removes a profile from the translator
        /// </summary>
        /// <typeparam name="TProfile">Profle to remove</typeparam>
        public static void RemoveProfile<TProfile>() where TProfile : TranslationProfile, new()
        {
            _store.RemoveProfile(new TProfile().ProfileName);
        }

        /// <summary>
        /// Removes a profile from the translator
        /// </summary>
        /// <param name="translationProfile">Profile to remove</param>
        public static void RemoveProfile(TranslationProfile translationProfile)
        {
            if (translationProfile == null) return;
            _store.RemoveProfile(translationProfile.ProfileName);
        }

        /// <summary>
        /// Removes a profile from the translator
        /// </summary>
        /// <param name="profileName">Name of the profile to remove</param>
        public static void RemoveProfile(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return;
            _store.RemoveProfile(profileName);
        }

        /// <summary>
        /// Removes all profiles from the translator
        /// </summary>
        public static void RemoveAllProfiles()
        {
            _store.RemoveAllProfiles();
        }

        /// <summary>
        /// Removes profiles where the provided predicate returns true
        /// </summary>
        /// <param name="predicate">A function to test each profile in the translator for a condition</param>
        public static void RemoveProfiles(Func<TranslationProfile, bool> predicate)
        {
            _store.RemoveProfiles(predicate);
        }

        #endregion

        #region get translations

        /// <summary>
        /// Gets all translations in the translator
        /// </summary>
        /// <returns>All translations in the translator</returns>
        public static IEnumerable<TranslationBase> GetAllTranslations()
        {
            return _store.GetAllTranslations();
        }

        /// <summary>
        /// Filters translations in the translator where the provided predicate returns true
        /// </summary>
        /// <param name="predicate">A function to test each translation in the translator for a condition</param>
        /// <returns>Translations in the translator that match the provided predicate</returns>
        public static IEnumerable<TranslationBase> GetTranslations(Func<Translation, bool> predicate)
        {
            return _store.GetTranslations(predicate);
        }

        /// <summary>
        /// Gets all translations in the translator for a given profile
        /// </summary>
        /// <typeparam name="TProfile">Type of a translation profile</typeparam>
        /// <returns>The translations in the translator for the profile provided</returns>
        public static IEnumerable<TranslationBase> GetProfileTranslations<TProfile>() where TProfile : TranslationProfile, new()
        {
            return _store.GetProfileTranslations<TProfile>();
        }

        /// <summary>
        /// Gets all translations in the translator for a specific type for a given profile
        /// </summary>
        /// <typeparam name="TProfile">Type of a translation profile</typeparam>
        /// <typeparam name="KTranslationType">Type the translation is for</typeparam>
        /// <returns>The translations in the translator for the given type in the profile provided</returns>
        public static IEnumerable<TranslationBase> GetProfileTranslationsForType<TProfile, KTranslationType>() where TProfile : TranslationProfile, new()
        {
            return _store.GetProfileTranslationsForType<TProfile, KTranslationType, Translation>();
        }

        /// <summary>
        /// Gets all translations in the translator for a specific type for a given profile that has a specific name
        /// </summary>
        /// <typeparam name="TProfile">Type of a translation profile</typeparam>
        /// <typeparam name="KTranslationType">Type the translation is for</typeparam>
        /// <param name="translationName">Name of the translation</param>
        /// <returns>The translations in the translator for the given type in the profile provided that matches the name provided</returns>
        public static TranslationBase GetProfileTranslation<TProfile, KTranslationType>(string translationName) where TProfile : TranslationProfile, new()
        {
            return _store.GetProfileTranslation<TProfile, KTranslationType, Translation>(translationName);
        }

        #endregion

        #region adhoc

        /// <summary>
        /// Create an adhoc translation (not contained in a TranslationProfile) for type TTranslationType
        /// </summary>
        /// <typeparam name="TTranslationType">Type that the translation is for</typeparam>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public static TranslationExpression<TTranslationType> CreateAdhocTranslation<TTranslationType>()
           where TTranslationType : new()
        {
            return CreateAdhocTranslation<TTranslationType>(new TranslationSettings());
        }

        /// <summary>
        /// Create an adhoc translation (not contained in a TranslationProfile) for type TTranslationType
        /// </summary>
        /// <typeparam name="TTranslationType">Type that the translation is for</typeparam>
        /// <param name="translationSettings">Additional configuration settings for the translation</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public static TranslationExpression<TTranslationType> CreateAdhocTranslation<TTranslationType>(TranslationSettings translationSettings)
            where TTranslationType : new()
        {
            var translation = new Translation(typeof(TTranslationType), new AdhocProfile(), translationSettings);
            return new TranslationExpression<TTranslationType>(translation);
        }

        /// <summary>
        /// Translates a list of objects to data table using an adhoc translation (not contained in a TranslationProfile)
        /// </summary>
        /// <typeparam name="KTranslationDataType">Type of object to translate</typeparam>
        /// <param name="translationExpression">Translation expression to use in the translation</param>
        /// <param name="source">The data to translate to a data table</param>
        /// <returns>Translated data table</returns>
        public static DataTable AdhocTranslate<KTranslationDataType>(TranslationExpression<KTranslationDataType> translationExpression,
            IEnumerable<KTranslationDataType> source)
            where KTranslationDataType : new()
        {
            var tmpEngine = new SimpleTranslationEngine();
            var initialzedTranslation =
                InitializedTranslation.CreateInstance(translationExpression.Translation, new List<TranslationEngine> { tmpEngine });
            return tmpEngine.FillDataTable(initialzedTranslation, source);
        }

        /// <summary>
        /// Translates a list of objects to a database parameter using an adhoc translation (not contained in a TranslationProfile)
        /// </summary>
        /// <typeparam name="KTranslationDataType">Type of object to translate</typeparam>
        /// <param name="translationExpression">Translation expression to use in the translation</param>
        /// <param name="source">The data to translate to a data table</param>
        /// <param name="dbParameterSettings">Additional settings for generating the database parameter</param>
        /// <returns>Translated database parameter</returns>
        public static DbParameter AdhocTranslateToDbParameter<KTranslationDataType>(TranslationExpression<KTranslationDataType> translationExpression,
            IEnumerable<KTranslationDataType> source, DbParameterSettings dbParameterSettings)
            where KTranslationDataType : new()
        {
            var tmpEngine = new DbParameterTranslationEngine();
            var initialzedTranslation =
                InitializedTranslation.CreateInstance(translationExpression.Translation, new List<TranslationEngine> { tmpEngine });
            return tmpEngine.WrapinDbParameter(tmpEngine.FillDataTable(initialzedTranslation, source), dbParameterSettings);
        }

        #endregion

        #region translators

        /// <summary>
        /// Translates a list of objects to data table
        /// </summary>
        /// <typeparam name="TProfile">Type of a translation profile</typeparam>
        /// <typeparam name="KTranslationDataType">Type of object to translate</typeparam>
        /// <param name="source">The data to translate to a data table</param>
        /// <returns>Translated data table</returns>
        public static DataTable Translate<TProfile, KTranslationDataType>(IEnumerable<KTranslationDataType> source) 
            where TProfile : TranslationProfile, new()
            where KTranslationDataType : new()
        {
            PreTranslateValidation();
            return _engines.GetEngine<SimpleTranslationEngine>().FillDataTable(
                _store.SingleInitializedTranslation<TProfile, KTranslationDataType>(), source);
        }

        /// <summary>
        /// Translates a list of objects to data table
        /// </summary>
        /// <typeparam name="TProfile">Type of a translation profile</typeparam>
        /// <typeparam name="KTranslationDataType">Type of object to translate</typeparam>
        /// <param name="source">The data to translate to a data table</param>
        /// <param name="translationName">Name of the translation to use (must be specified if more than one translation is in the provided profile for the given type)</param>
        /// <returns>Translated data table</returns>
        public static DataTable Translate<TProfile, KTranslationDataType>(IEnumerable<KTranslationDataType> source, string translationName)
            where TProfile : TranslationProfile, new()
            where KTranslationDataType : new()
        {
            PreTranslateValidation();
            return _engines.GetEngine<SimpleTranslationEngine>().FillDataTable(
                _store.SingleInitializedTranslation<TProfile, KTranslationDataType>(translationName), source);
        }


        /// <summary>
        /// Translates a list of objects to a database parameter
        /// </summary>
        /// <typeparam name="TProfile">Type of a translation profile</typeparam>
        /// <typeparam name="KTranslationDataType">Type of object to translate</typeparam>
        /// <param name="source">The data to translate to a data table</param>
        /// <param name="dbParameterSettings">Additional settings for generating the database parameter</param>
        /// <returns>Translated database parameter</returns>
        public static DbParameter TranslateToDbParameter<TProfile, KTranslationDataType>(IEnumerable<KTranslationDataType> source, DbParameterSettings dbParameterSettings)
            where TProfile : TranslationProfile, new()
            where KTranslationDataType : new()
        {
            PreTranslateValidation();
            var engine = _engines.GetEngine<DbParameterTranslationEngine>();
            return engine.WrapinDbParameter(engine.FillDataTable(
                _store.SingleInitializedTranslation<TProfile, KTranslationDataType>(), source), dbParameterSettings);
        }

        /// <summary>
        /// Translates a list of objects to a database parameter
        /// </summary>
        /// <typeparam name="TProfile">Type of a translation profile</typeparam>
        /// <typeparam name="KTranslationDataType">Type of object to translate</typeparam>
        /// <param name="source">The data to translate to a data table</param>
        /// <param name="dbParameterSettings">Additional settings for generating the database parameter</param>
        /// <param name="translationName">Name of the translation to use (must be specified if more than one translation is in the provided profile for the given type)</param>
        /// <returns></returns>
        public static DbParameter TranslateToDbParameter<TProfile, KTranslationDataType>(IEnumerable<KTranslationDataType> source, DbParameterSettings dbParameterSettings, string translationName)
            where TProfile : TranslationProfile, new()
            where KTranslationDataType : new()
        {
            PreTranslateValidation();
            var engine = _engines.GetEngine<DbParameterTranslationEngine>();
            return engine.WrapinDbParameter(engine.FillDataTable(
                _store.SingleInitializedTranslation<TProfile, KTranslationDataType>(translationName), source), dbParameterSettings);
        }

        #endregion

        #region reverse translators

        //public static IEnumerable<ObjectResult<KTranslationDataType>> ReverseTranslate<TProfile, KTranslationDataType>(DataTable source)
        //    where TProfile : TranslationProfile, new()
        //    where KTranslationDataType : new()
        //{
        //    PreTranslateValidation();
        //    return _engines.GetEngine<SimpleTranslationEngine>().FillObjectResult<KTranslationDataType>(
        //        _store.SingleInitializedTranslation<TProfile, KTranslationDataType>(), source);
        //}

        //public static IEnumerable<ObjectResult<KTranslationDataType>> ReverseTranslate<TProfile, KTranslationDataType>(DataTable source, string translationName)
        //    where TProfile : TranslationProfile, new()
        //    where KTranslationDataType : new()
        //{
        //    PreTranslateValidation();
        //    return null;
        //}

        #endregion

        private static void PreTranslateValidation()
        {
            if (!IsInitialized) throw new TableTranslatorException("You must initialize the translator by calling 'Translator.Initialize()' before perfoming a translation.");
        }
    }
}