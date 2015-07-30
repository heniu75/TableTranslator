using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TableTranslator.Abstract;
using TableTranslator.Engines;
using TableTranslator.Exceptions;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator
{
    public static class Translator
    {
        public static bool IsInitialized { get; private set; }
        private static readonly ITranslationStore _store = new TranslationStore();

        private static readonly List<TranslationEngine> _engines = new List<TranslationEngine>
        {
            new SimpleTranslationEngine(),
            new DbParameterTranslationEngine()
        };

        public static void Initialize()
        {
            if (IsInitialized) throw new TableTranslatorException("Table translator has already been initialized");
             _store.Initialize(_engines);
            IsInitialized = true;
        }

        public static void ApplyUpdates()
        {
            if (!IsInitialized) throw new TableTranslatorException("You must initialize the translator before calling ApplyUpdates().");
            // currently just calls Initialize(), but separating it here for future enhancements (e.g. translation updates, column config updates, etc.)
            _store.Initialize(_engines);
        }

        public static void UnloadAll()
        {
            _store.UnloadAll();
            IsInitialized = false;
        }

        #region add profiles

        public static void AddProfile<T>() where T : TranslationProfile, new()
        {
            _store.AddProfile(new T());
        }

        public static void AddProfile(TranslationProfile translationProfile)
        {
            _store.AddProfile(translationProfile);
        }

        public static void AddProfiles(IEnumerable<TranslationProfile> translationProfiles)
        {
            if (translationProfiles == null) throw new ArgumentNullException("translationProfiles");
            var list = translationProfiles as TranslationProfile[] ?? translationProfiles.ToArray();
            if (list.Any(x => x == null)) throw new ArgumentException("Argument translationProfiles contained a item that was null.");
            foreach (var tp in list)
            {
                _store.AddProfile(tp);
            }
        }

        #endregion

        #region get profiles

        public static IEnumerable<TranslationProfile> GetAllProfiles()
        {
            return _store.GetAllProfiles();
        }

        public static IEnumerable<TranslationProfile> GetProfiles(Func<TranslationProfile, bool> predicate)
        {
            return _store.GetProfiles(predicate);
        }

        public static TranslationProfile GetProfile(string profileName)
        {
            return string.IsNullOrEmpty(profileName) ? null : _store.GetProfile(profileName);
        }

        #endregion

        #region remove profiles

        public static void RemoveProfile<T>() where T : TranslationProfile, new()
        {
            _store.RemoveProfile(new T().ProfileName);
        }

        public static void RemoveProfile(TranslationProfile translationProfile)
        {
            if (translationProfile == null) return;
            _store.RemoveProfile(translationProfile.ProfileName);
        }

        public static void RemoveProfile(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return;
            _store.RemoveProfile(profileName);
        }

        public static void RemoveAllProfiles()
        {
            _store.RemoveAllProfiles();
        }

        public static void RemoveProfiles(Func<TranslationProfile, bool> predicate)
        {
            _store.RemoveProfiles(predicate);
        }

        #endregion

        #region get translations

        public static IEnumerable<TranslationBase> GetAllTranslations()
        {
            return _store.GetAllTranslations();
        }

        public static IEnumerable<TranslationBase> GetTranslations(Func<Translation, bool> predicate)
        {
            return _store.GetTranslations(predicate);
        }

        public static IEnumerable<TranslationBase> GetProfileTranslations<T>() where T : TranslationProfile, new()
        {
            return _store.GetProfileTranslations<T>();
        }

        public static IEnumerable<TranslationBase> GetProfileTranslationsForType<T, K>() where T : TranslationProfile, new()
        {
            return _store.GetProfileTranslationsForType<T, K, Translation>();
        }

        public static TranslationBase GetProfileTranslation<T, K>(string translationName) where T : TranslationProfile, new()
        {
            return _store.GetProfileTranslation<T, K, Translation>(translationName);
        }

        #endregion

        #region translators

        public static DataTable Translate<T, K>(IEnumerable<K> source) 
            where T : TranslationProfile, new()
            where K : new()
        {
            if (!IsInitialized) throw new TableTranslatorException("You must initialize the translator before calling TranslateToDataTable().");
            return _engines.Single(x => x.GetType() == typeof(SimpleTranslationEngine)).FillDataTable(_store.SingleInitializedTranslation<T, K>(), source);
        }

        public static DataTable Translate<T, K>(IEnumerable<K> source, string translationName)
            where T : TranslationProfile, new()
            where K : new()
        {
            if (!IsInitialized) throw new TableTranslatorException("You must initialize the translator before calling TranslateToDataTable().");
            return _engines.Single(x => x.GetType() == typeof(SimpleTranslationEngine)).FillDataTable(_store.SingleInitializedTranslation<T, K>(translationName), source);
        }

        public static DbParameter TranslateToDbParameter<T, K>(IEnumerable<K> source, DbParameterSettings dbParameterSettings)
            where T : TranslationProfile, new()
            where K : new()
        {
            if (!IsInitialized) throw new TableTranslatorException("You must initialize the translator before calling TranslateToDbParameter().");
            var table = _engines.Single(x => x.GetType() == typeof(DbParameterTranslationEngine)).FillDataTable(_store.SingleInitializedTranslation<T, K>(), source);
            return WrapinDbParameter(table, dbParameterSettings);
        }

        public static DbParameter TranslateToDbParameter<T, K>(IEnumerable<K> source, string translationName, DbParameterSettings dbParameterSettings)
            where T : TranslationProfile, new()
            where K : new()
        {
            if (!IsInitialized) throw new TableTranslatorException("You must initialize the translator before calling TranslateToDbParameter().");
            var table = _engines.Single(x => x.GetType() == typeof(SimpleTranslationEngine)).FillDataTable(_store.SingleInitializedTranslation<T, K>(translationName), source);
            return WrapinDbParameter(table, dbParameterSettings);
        }

        private static DbParameter WrapinDbParameter(DataTable dataTable, DbParameterSettings dbParameterSettings)
        {
            if (dbParameterSettings == null)
            {
                throw new ArgumentNullException("dbParameterSettings");
            }

            dataTable.TableName = dbParameterSettings.DatabaseObjectName;

            switch (dbParameterSettings.DatabaseType)
            {
                case DatabaseType.Sql:
                    return new SqlParameter(dbParameterSettings.ParameterName, SqlDbType.Structured)
                    {
                        Value = dataTable,
                        TypeName = dbParameterSettings.DatabaseObjectName
                    };
                case DatabaseType.Oracle:
                case DatabaseType.MySql:
                    throw new NotImplementedException(string.Format("Database type {0} has not been implemented yet.", dbParameterSettings.DatabaseType));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}