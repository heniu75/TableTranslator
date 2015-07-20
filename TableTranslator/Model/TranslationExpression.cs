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
        private int Ordinal
        {
            get { return this._ordinal++; }
        }

        internal TranslationExpression(Translation translation)
        {
            this.Translation = translation;
        }

        public TranslationExpression<T> ForSimpleValue<K>(K value)
        {
            return ForSimpleValue(value, new ColumnSettings<K>());
        }

        public TranslationExpression<T> ForSimpleValue<K>(K value, ColumnSettings<K> settings)
        {
            settings.Ordinal = this.Ordinal;
            AddColumnConfiguration(this._colConfigBuilder.BuildSimpleValueColumnConfiguration(value, settings));
            return this;
        }


        public TranslationExpression<T> ForMember<K>(Expression<Func<T, K>> func)
        {
            return ForMember(func, new ColumnSettings<K>());
        }

        public TranslationExpression<T> ForMember<K>(Expression<Func<T, K>> func, ColumnSettings<K> settings)
        {
            settings.Ordinal = this.Ordinal;
            AddColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration(func, settings));
            return this;
        }


        public TranslationExpression<T> ForDelegate<K>(Func<T, K> func)
        {
            return ForDelegate(func, new ColumnSettings<K>());
        }

        public TranslationExpression<T> ForDelegate<K>(Func<T, K> func, ColumnSettings<K> settings)
        {
            settings.Ordinal = this.Ordinal;
            AddColumnConfiguration(this._colConfigBuilder.BuildDelegateColumnConfiguration(func, settings));
            return this;
        }


        public TranslationExpression<T> ForAllMembers()
        {
            return ForAllMembers(new GetAllMemberSettings());
        }

        public TranslationExpression<T> ForAllMembers(GetAllMemberSettings getAllMemberSettings)
        {
            var members = ReflectionHelper.GetAllMembers<T>(getAllMemberSettings ?? new GetAllMemberSettings());
            foreach (var mi in members)
            {
                AddColumnConfiguration(this._colConfigBuilder.BuildMemberColumnConfiguration<T>(mi, this.Ordinal));
            }
            return this;
        }

        public TranslationExpression<T> AddColumnConfiguration(ColumnConfigurationBase config)
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