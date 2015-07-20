using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using TableTranslator.Abstract;
using TableTranslator.Exceptions;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Model
{
    public sealed class Translation : TranslationBase, ICloneable<Translation>
    {
        public Translation(Type type, TranslationProfile translationProfile, TranslationSettings translationSettings)
            : base(type, translationProfile, translationSettings)
        {
        }

        internal void AddColumnConfiguration(ColumnConfigurationBase config)
        {
            if (this.ColumnConfigurations.Any(cc => cc.Ordinal == config.Ordinal))
            {
                throw new TableTranslatorConfigurationException("Duplicate ordinal value.", this.TranslationProfile);
            }
            if (this.ColumnConfigurations.Any(cc => cc.ColumnName == config.ColumnName))
            {
                throw new TableTranslatorConfigurationException("Duplicate column name.", this.TranslationProfile);
            }
            this.ColumnConfigurations.Add(config);
        }

        public ReadOnlyCollection<ColumnConfigurationBase> GetColumnConfigurations()
        {
            return this.ColumnConfigurations.AsReadOnly();
        }

        public Translation ShallowClone()
        {
            return this.MemberwiseClone() as Translation;
        }

        public Translation DeepClone()
        {
            var translation = this.MemberwiseClone() as Translation;
            translation.TranslationProfile = this.TranslationProfile.ShallowClone();
            translation.TranslationSettings = this.TranslationSettings.ShallowClone();
            translation.TraversedGenericArguments = new List<Type>(this.TraversedGenericArguments.ConvertAll(x => new TypeDelegator(x)));
            translation.TranslationUniqueIdentifier = this.TranslationUniqueIdentifier.DeepClone();
            translation.ColumnConfigurations = this.ColumnConfigurations.Select(x => x.ShallowClone()).ToList();

            return translation;
        }
    }
}