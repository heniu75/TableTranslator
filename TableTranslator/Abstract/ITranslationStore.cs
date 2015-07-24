using System;
using System.Collections.Generic;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Abstract
{
    internal interface ITranslationStore
    {
        IEnumerable<InitializedTranslation> Initialize(bool fullReset);
        void UnloadAll();

        void AddProfile(TranslationProfile translationProfile);

        IEnumerable<TranslationProfile> GetAllProfiles();
        TranslationProfile GetProfile(string profileName);
        IEnumerable<TranslationProfile> GetProfiles(Func<TranslationProfile, bool> predicate);

        void RemoveAllProfiles();
        void RemoveProfile(string profileName);
        void RemoveProfiles(Func<TranslationProfile, bool> predicate);

        TranslationExpression<T> AddTranslation<T>(string profileName, TranslationSettings translationSettings) where T : new();

        IEnumerable<Translation> GetAllTranslations();
        IEnumerable<Translation> GetTranslations(Func<Translation, bool> predicate);
        IEnumerable<Translation> GetProfileTranslations<T>() where T : TranslationProfile, new();
        IEnumerable<M> GetProfileTranslationsForType<T, K, M>()
            where T : TranslationProfile, new()
            where M : TranslationBase;
        M GetProfileTranslation<T, K, M>(string translationName)
            where T : TranslationProfile, new()
            where M : TranslationBase;
        InitializedTranslation SingleInitializedTranslation<T, K>(string translationName = "") where T : TranslationProfile, new();
    }
}