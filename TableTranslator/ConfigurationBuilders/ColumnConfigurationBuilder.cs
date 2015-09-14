using System;
using System.Linq.Expressions;
using System.Reflection;
using TableTranslator.Abstract;
using TableTranslator.Helpers;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.ConfigurationBuilders
{
    internal class ColumnConfigurationBuilder : IColumnConfigurationBuilder
    {
        public NonIdentityColumnConfiguration BuildColumnConfiguration<T>(T value, ColumnConfigurationSettings<T> columnConfigurationSettings)
        {
            return new SimpleValueColumnConfiguration(value, typeof (T), columnConfigurationSettings.Ordinal, columnConfigurationSettings.ColumnName, columnConfigurationSettings.NullReplacement);
        }

        public NonIdentityColumnConfiguration BuildColumnConfiguration<T, K>(Expression<Func<T, K>> func, ColumnConfigurationSettings<K> columnConfigurationSettings)
        {
            if (func.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberInfo = ReflectionHelper.GetMemberInfoFromLambda(func);
                var fullPropertyPath = ReflectionHelper.GetMemberRelativePathNameFromLambda(func);
                return new MemberColumnConfiguration(memberInfo, columnConfigurationSettings.Ordinal, columnConfigurationSettings.ColumnName, columnConfigurationSettings.NullReplacement, fullPropertyPath);
            }

            var delegateSettings = new DelegateSettings(func.Compile());
            return new DelegateColumnConfiguration(delegateSettings, columnConfigurationSettings.Ordinal, columnConfigurationSettings.ColumnName, columnConfigurationSettings.NullReplacement);
        }

        public NonIdentityColumnConfiguration BuildColumnConfiguration<T>(MemberInfo memberInfo, int ordinal) where T : new()
        {
            return new MemberColumnConfiguration(memberInfo, ordinal, memberInfo.GetMemberType().GetDefaultValue());
        }
    }
}