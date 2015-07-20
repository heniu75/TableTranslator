using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableTranslator.Helpers;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Model
{
    public abstract class TranslationBase
    {
        internal readonly TypeInfo TypeInfo;
        internal TranslationUniqueIdentifier TranslationUniqueIdentifier { get; set; }
        public TranslationProfile TranslationProfile { get; internal set; }
        public TranslationSettings TranslationSettings { get; internal set; }
        internal List<ColumnConfigurationBase> ColumnConfigurations { get; set; }
        internal List<Type> TraversedGenericArguments { get; set; }

        protected TranslationBase(Type type, TranslationProfile translationProfile, TranslationSettings translationSettings)
        {
            if (string.IsNullOrEmpty(translationSettings.TranslationName))
            {
                translationSettings.TranslationName = type.GetFormattedName();
            }

            this.TranslationProfile = translationProfile;
            this.TranslationSettings = translationSettings;
            this.TraversedGenericArguments = type.GetTraversedGenericTypes().ToList();
            this.ColumnConfigurations = new List<ColumnConfigurationBase>();
            this.TypeInfo = type.GetTypeInfo();
            this.TranslationUniqueIdentifier = TranslationUniqueIdentifier.GetInstance(this);
        }

        protected TranslationBase()
        {

        }
    }
}