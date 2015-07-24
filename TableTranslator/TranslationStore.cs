using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TableTranslator.Abstract;
using TableTranslator.Exceptions;
using TableTranslator.Helpers;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator
{
    internal class TranslationStore : ITranslationStore
    {
        private readonly ObservableDictionary<string, InitializedTranslationProfile> _initializedProfiles = new ObservableDictionary<string, InitializedTranslationProfile>();
        private readonly Dictionary<TranslationUniqueIdentifier, InitializedTranslation> _initializedTranslations =
            new Dictionary<TranslationUniqueIdentifier, InitializedTranslation>(new TranslationUniqueIdentifierComparer());

        private readonly ObservableDictionary<string, TranslationProfile> _profiles = new ObservableDictionary<string, TranslationProfile>();
        private readonly Dictionary<TranslationUniqueIdentifier, Translation> _translations = 
            new Dictionary<TranslationUniqueIdentifier, Translation>(new TranslationUniqueIdentifierComparer());


        public TranslationStore()
        {
            this._profiles.CollectionChanged += CascadeProfileChanges;
            this._initializedProfiles.CollectionChanged += CascadeInitializedProfileChanges;
        }

        public IEnumerable<InitializedTranslation> Initialize(bool fullReset)
        {
            ClearProfilesAndTranslations(false);

            this._profiles.ToList().ForEach(x =>
            {
                x.Value.Initialize(this);
                this._initializedProfiles.Add(x.Key, new InitializedTranslationProfile(x.Value));
            });

            this._translations.ToList().ForEach(x => this._initializedTranslations.Add(x.Key.ShallowClone(), InitializedTranslation.CreateInstance(x.Value)));

            return this._initializedTranslations.Values;
        }

        public void UnloadAll()
        {
            ClearProfilesAndTranslations(true);
        }

        private void ClearProfilesAndTranslations(bool fullReset)
        {
            this._initializedProfiles.Clear();
            this._translations.Clear();
            this._profiles.RemoveAll((s, p) => fullReset || p.TranslationProfileState == TranslationProfileState.RemovalPending);
        }

        public void AddProfile(TranslationProfile translationProfile)
        {
            if (translationProfile == null)
            {
                throw new ArgumentNullException("translationProfile");
            }
            if (string.IsNullOrEmpty(translationProfile.ProfileName))
            {
                throw new TableTranslatorConfigurationException("Translation profile must have a name.");
            }
            if (GetProfile(translationProfile.ProfileName) != null)
            {
                throw new TableTranslatorConfigurationException(string.Format("This translation profile ({0}) already added.", translationProfile.ProfileName));
            }

            this._profiles.Add(translationProfile.ProfileName, translationProfile);
        }

        public IEnumerable<TranslationProfile> GetAllProfiles()
        {
            return this._profiles.Values;
        }

        public IEnumerable<TranslationProfile> GetProfiles(Func<TranslationProfile, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return this._profiles.Values.Where(predicate);
        }

        public TranslationProfile GetProfile(string profileName)
        {
            var profile = this._profiles.FirstOrDefault(x => x.Key == profileName);
            return profile.Value;
        }

        public void RemoveAllProfiles()
        {
            foreach (var p in this._profiles)
            {
                p.Value.TranslationProfileState = TranslationProfileState.RemovalPending;
            }
        }

        public void RemoveProfiles(Func<TranslationProfile, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            var profiles = this._profiles.Where(x => predicate(x.Value)).ToList(); // ToList() so we do not defer the query
            foreach (var p in profiles)
            {
                p.Value.TranslationProfileState = TranslationProfileState.RemovalPending;
            }
        }

        public void RemoveProfile(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return;

            var profile = this._profiles.FirstOrDefault(x => x.Key == profileName);
            if (profile.Value != null)
            {
                profile.Value.TranslationProfileState = TranslationProfileState.RemovalPending;
            }
        }

        public TranslationExpression<T> AddTranslation<T>(string profileName, TranslationSettings translationSettings) where T : new()
        {
            var translation = new Translation(typeof(T), GetProfile(profileName), translationSettings);
            this._translations.Add(translation.TranslationUniqueIdentifier, translation);
            return new TranslationExpression<T>(translation);
        }

        public IEnumerable<Translation> GetAllTranslations()
        {
            return this._translations.Values;
        }

        public IEnumerable<Translation> GetTranslations(Func<Translation, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return this._translations.Values.Where(predicate);
        }

        public IEnumerable<Translation> GetProfileTranslations<T>() where T : TranslationProfile, new()
        {
            return this._translations
                .Where(x => x.Key.IsMatch(new T().ProfileName))
                .Select(x => x.Value);
        }

        public IEnumerable<M> GetProfileTranslationsForType<T, K, M>() 
            where T : TranslationProfile, new()
            where M : TranslationBase
        {
            return GetTranslationDictionary<M>()
                .Where(x => x.Key.IsMatch(new T().ProfileName, typeof(K).Name)
                    && ValidateGenericArguments(x.Value.TraversedGenericArguments, typeof(K)))
                .Select(x => x.Value);
        }

        public M GetProfileTranslation<T, K, M>(string translationName) 
            where T : TranslationProfile, new() 
            where M : TranslationBase
        {
            return GetTranslationDictionary<M>()
                .Where(x => x.Key.IsMatch(new T().ProfileName, typeof(K).Name, translationName)
                    && ValidateGenericArguments(x.Value.TraversedGenericArguments, typeof(K)))
                .Select(x => x.Value).FirstOrDefault();
        }

        public InitializedTranslation SingleInitializedTranslation<T, K>(string translationName = "") where T : TranslationProfile, new()
        {
            InitializedTranslation translation;

            if (string.IsNullOrEmpty(translationName))
            {
                var translations = this.GetProfileTranslationsForType<T, K, InitializedTranslation>().ToList();
                if (translations.Count() > 1)
                {
                    throw new TableTranslatorException(string.Format("More than one translation was found in translation profile '{0}' for type of '{1}'.", new T().ProfileName, typeof(K).FullName));
                }
                if (!translations.Any())
                {
                    throw new TableTranslatorException(string.Format("No translation was found in translation profile '{0}' for type of '{1}'.", new T().ProfileName, typeof(K).FullName));
                }
                translation = translations.Single();
            }
            else
            {
                translation = this.GetProfileTranslation<T, K, InitializedTranslation>(translationName);
                if (translation == null)
                {
                    throw new TableTranslatorException(string.Format("No translation named '{0}' was found in translation profile '{1}' for type of '{2}'.", translationName, new T().ProfileName, typeof(K).FullName));
                }
            }
            return translation;
        }

        private static bool ValidateGenericArguments(IEnumerable<Type> initializedSettings, Type typeToTranslate)
        {
            return typeToTranslate.GetTraversedGenericTypes().SequenceEqual(initializedSettings, new TypeComparer());
        }

        private void CascadeProfileChanges(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                {
                    foreach (var p in args.OldItems.OfType<TranslationProfile>())
                    {
                        var profile = p;
                        this._translations.RemoveAll((k, v) => v.TranslationProfile.ProfileName == profile.ProfileName);
                    }
                }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this._translations.Clear();
                    break;
            }
        }

        private void CascadeInitializedProfileChanges(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var p in args.OldItems.OfType<TranslationProfile>())
                        {
                            var profile = p;
                            this._initializedTranslations.RemoveAll((k, v) => v.TranslationProfile.ProfileName == profile.ProfileName);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this._initializedTranslations.Clear();
                    break;
            }
        }

        private Dictionary<TranslationUniqueIdentifier, M> GetTranslationDictionary<M>() where M : TranslationBase
        {
            return typeof (M) == typeof (InitializedTranslation)
                ? this._initializedTranslations as Dictionary<TranslationUniqueIdentifier, M>
                : this._translations as Dictionary<TranslationUniqueIdentifier, M>;
        }
    }
}