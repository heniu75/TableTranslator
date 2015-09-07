using System;
using System.Linq.Expressions;
using TableTranslator.Abstract;
using TableTranslator.Helpers;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Model
{
    public sealed class TranslationExpression<T> where T : new()
    {
        private readonly Translation Translation;
        private readonly IColumnConfigurationBuilder _colConfigBuilder = new ColumnConfigurationBuilder();

        private int _ordinal;
        private int Ordinal => this._ordinal++;

        internal TranslationExpression(Translation translation)
        {
            if (translation.TranslationSettings.IdentityColumnConfiguration != null)
            {
                // if there is an identity setting defined, increment the ordinal so column configs will get increased as well
                this._ordinal++;
            }
            this.Translation = translation;
        }

        public TranslationExpression<T> AddColumnConfiguration<K>(K value)
        {
            return AddColumnConfiguration(value, new ColumnSettings<K>());
        }

        public TranslationExpression<T> AddColumnConfiguration<K>(K value, ColumnSettings<K> settings)
        {
            settings.Ordinal = this.Ordinal;
            AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration(value, settings));
            return this;
        }

        public TranslationExpression<T> AddColumnConfiguration<K>(Expression<Func<T, K>> func)
        {
            return AddColumnConfiguration(func, new ColumnSettings<K>());
        }

        public TranslationExpression<T> AddColumnConfiguration<K>(Expression<Func<T, K>> func, ColumnSettings<K> settings)
        {
            settings.Ordinal = this.Ordinal;
            AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration(func, settings));
            return this;
        }

        public TranslationExpression<T> ForAllMembers()
        {
            return ForAllMembers(new GetAllMemberSettings());
        }

        public TranslationExpression<T> ForAllMembers(GetAllMemberSettings settings)
        {
            var members = ReflectionHelper.GetAllMembers<T>(settings ?? new GetAllMemberSettings());
            foreach (var mi in members)
            {
                AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration<T>(mi, this.Ordinal));
            }
            return this;
        }

        public TranslationExpression<T> AddExplicitColumnConfiguration(NonIdentityColumnConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            this.Translation.AddColumnConfiguration(config);
            return this;
        }
    }
}