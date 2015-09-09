using System;
using System.Linq.Expressions;
using TableTranslator.Abstract;
using TableTranslator.ConfigurationBuilders;
using TableTranslator.Helpers;
using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator
{
    public sealed class TranslationExpression<T> where T : new()
    {
        private readonly Translation Translation;
        private readonly IColumnConfigurationBuilder _colConfigBuilder = new ColumnConfigurationBuilder();

        private int _ordinal;
        private int NextOrdinal => this._ordinal++;

        internal TranslationExpression(Translation translation)
        {
            // if there is an identity setting defined, increment the ordinal so column configs will get increased as well
            if (translation.TranslationSettings.IdentityColumnConfiguration != null)
            {
                this._ordinal++;
            }
            this.Translation = translation;
        }

        /// <summary>
        /// Adds a column configuration to a translation
        /// </summary>
        /// <typeparam name="K">Data type for the column</typeparam>
        /// <param name="value">Value for the column</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<T> AddColumnConfiguration<K>(K value)
        {
            return AddColumnConfiguration(value, new ColumnSettings<K>());
        }

        /// <summary>
        /// Adds a column configuration to a translation
        /// </summary>
        /// <typeparam name="K">>Data type for the column</typeparam>
        /// <param name="value">Value for the column</param>
        /// <param name="settings">Additional configuration settings for the column</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<T> AddColumnConfiguration<K>(K value, ColumnSettings<K> settings)
        {
            settings.Ordinal = this.NextOrdinal;
            AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration(value, settings));
            return this;
        }

        /// <summary>
        /// Adds a column configuration to a translation
        /// </summary>
        /// <typeparam name="K">Data type for the column</typeparam>
        /// <param name="func">Expression that will be evaluated to get the value for the column</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<T> AddColumnConfiguration<K>(Expression<Func<T, K>> func)
        {
            return AddColumnConfiguration(func, new ColumnSettings<K>());
        }

        /// <summary>
        /// Adds a column configuration to a translation
        /// </summary>
        /// <typeparam name="K">>Data type for the column</typeparam>
        /// <param name="func">Expression that will be evaluated to get the value for the column</param>
        /// <param name="settings">Additional configuration settings for the column</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<T> AddColumnConfiguration<K>(Expression<Func<T, K>> func, ColumnSettings<K> settings)
        {
            settings.Ordinal = this.NextOrdinal;
            AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration(func, settings));
            return this;
        }

        /// <summary>
        /// Adds a column configuration for all members of T to a translation 
        /// </summary>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<T> AddColumnConfigurationForAllMembers()
        {
            return AddColumnConfigurationForAllMembers(new GetAllMemberSettings());
        }

        /// <summary>
        /// Adds a column configuration for all members of T to a translation 
        /// </summary>
        /// <param name="settings">Additional settings for determining which members to include</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<T> AddColumnConfigurationForAllMembers(GetAllMemberSettings settings)
        {
            var members = ReflectionHelper.GetAllMembers<T>(settings ?? new GetAllMemberSettings());
            foreach (var mi in members)
            {
                AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration<T>(mi, this.NextOrdinal));
            }
            return this;
        }

        /// <summary>
        /// Adds a column configuration to a translation using the most explicit level of detail (advanced, only use if AddColumnConfiguration() cannot)
        /// </summary>
        /// <param name="config">Explicit configuration of a non identity column to be added to a translation</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<T> AddExplicitColumnConfiguration(NonIdentityColumnConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            this.Translation.AddColumnConfiguration(config);
            return this;
        }
    }
}