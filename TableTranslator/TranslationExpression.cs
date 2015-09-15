using System;
using System.Linq.Expressions;
using TableTranslator.Abstract;
using TableTranslator.ConfigurationBuilders;
using TableTranslator.Helpers;
using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations.NonIdentity;
using TableTranslator.Model.Settings;

namespace TableTranslator
{
    /// <summary>
    /// Expression generator used to add column configurations to a translation
    /// </summary>
    /// <typeparam name="TTranslationDataType">Data type that the translation is for</typeparam>
    public sealed class TranslationExpression<TTranslationDataType> where TTranslationDataType : new()
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
        /// <typeparam name="KColumnDataType">Data type for the column</typeparam>
        /// <param name="value">Value for the column</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<TTranslationDataType> AddColumnConfiguration<KColumnDataType>(KColumnDataType value)
        {
            return AddColumnConfiguration(value, new ColumnConfigurationSettings<KColumnDataType>());
        }

        /// <summary>
        /// Adds a column configuration to a translation
        /// </summary>
        /// <typeparam name="KColumnDataType">>Data type for the column</typeparam>
        /// <param name="value">Value for the column</param>
        /// <param name="configurationSettings">Additional configuration settings for the column</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<TTranslationDataType> AddColumnConfiguration<KColumnDataType>(KColumnDataType value, ColumnConfigurationSettings<KColumnDataType> configurationSettings)
        {
            configurationSettings.Ordinal = this.NextOrdinal;
            AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration(value, configurationSettings));
            return this;
        }

        /// <summary>
        /// Adds a column configuration to a translation
        /// </summary>
        /// <typeparam name="KColumnDataType">Data type for the column</typeparam>
        /// <param name="func">Expression that will be evaluated to get the value for the column</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<TTranslationDataType> AddColumnConfiguration<KColumnDataType>(Expression<Func<TTranslationDataType, KColumnDataType>> func)
        {
            return AddColumnConfiguration(func, new ColumnConfigurationSettings<KColumnDataType>());
        }

        /// <summary>
        /// Adds a column configuration to a translation
        /// </summary>
        /// <typeparam name="KColumnDataType">>Data type for the column</typeparam>
        /// <param name="func">Expression that will be evaluated to get the value for the column</param>
        /// <param name="configurationSettings">Additional configuration settings for the column</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<TTranslationDataType> AddColumnConfiguration<KColumnDataType>(Expression<Func<TTranslationDataType, KColumnDataType>> func, ColumnConfigurationSettings<KColumnDataType> configurationSettings)
        {
            configurationSettings.Ordinal = this.NextOrdinal;
            AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration(func, configurationSettings));
            return this;
        }

        /// <summary>
        /// Adds a column configuration for all members of TTranslationType to a translation 
        /// </summary>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<TTranslationDataType> AddColumnConfigurationForAllMembers()
        {
            return AddColumnConfigurationForAllMembers(new GetAllMemberSettings());
        }

        /// <summary>
        /// Adds a column configuration for all members of TTranslationType to a translation 
        /// </summary>
        /// <param name="settings">Additional settings for determining which members to include</param>
        /// <returns>Translation expression used to add column configurations to a translation</returns>
        public TranslationExpression<TTranslationDataType> AddColumnConfigurationForAllMembers(GetAllMemberSettings settings)
        {
            var members = ReflectionHelper.GetAllMembers<TTranslationDataType>(settings ?? new GetAllMemberSettings());
            foreach (var mi in members)
            {
                AddExplicitColumnConfiguration(this._colConfigBuilder.BuildColumnConfiguration<TTranslationDataType>(mi, this.NextOrdinal));
            }
            return this;
        }

        private void AddExplicitColumnConfiguration(NonIdentityColumnConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            this.Translation.AddColumnConfiguration(config);
        }
    }
}