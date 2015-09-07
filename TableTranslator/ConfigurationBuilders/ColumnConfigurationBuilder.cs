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
        public NonIdentityColumnConfiguration BuildColumnConfiguration<T>(T value, ColumnSettings<T> columnSettings)
        {
            return new SimpleValueColumnConfiguration(value, typeof (T), columnSettings.Ordinal, columnSettings.ColumnName, columnSettings.NullReplacement);
        }

        public NonIdentityColumnConfiguration BuildColumnConfiguration<T, K>(Expression<Func<T, K>> func, ColumnSettings<K> columnSettings)
        {
            if (func.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberInfo = ReflectionHelper.GetMemberInfoFromLambda(func);
                var fullPropertyPath = ReflectionHelper.GetMemberRelativePathNameFromLambda(func);
                return new MemberColumnConfiguration(memberInfo, columnSettings.Ordinal, columnSettings.ColumnName, columnSettings.NullReplacement, fullPropertyPath);
            }

            var delegateSettings = new DelegateSettings(func.Compile());
            return new DelegateColumnConfiguration(delegateSettings, columnSettings.Ordinal, columnSettings.ColumnName, columnSettings.NullReplacement);
        }

        public NonIdentityColumnConfiguration BuildColumnConfiguration<T>(MemberInfo memberInfo, int ordinal) where T : new()
        {
            return new MemberColumnConfiguration(memberInfo, ordinal, memberInfo.GetMemberType().GetDefaultValue());
        }
    }
}