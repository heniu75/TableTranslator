using System;
using System.Linq.Expressions;
using System.Reflection;
using TableTranslator.Abstract;
using TableTranslator.Helpers;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator
{
    internal class ColumnConfigurationBuilder : IColumnConfigurationBuilder
    {
        public SimpleValueColumnConfiguration BuildSimpleValueColumnConfiguration<T>(T value, ColumnSettings<T> columnSettings)
        {
            return new SimpleValueColumnConfiguration(value, typeof (T), columnSettings.Ordinal, columnSettings.ColumnName, columnSettings.NullReplacement);
        }

        public DelegateColumnConfiguration BuildDelegateColumnConfiguration<T, K>(Func<T, K> func, ColumnSettings<K> columnSettings)
        {
            var delegateSettings = new DelegateSettings(func);
            return new DelegateColumnConfiguration(delegateSettings, columnSettings.Ordinal, columnSettings.ColumnName, columnSettings.NullReplacement);
        }

        public ColumnConfigurationBase BuildColumnConfiguration<T, K>(Expression<Func<T, K>> func, ColumnSettings<K> columnSettings) where T : new()
        {
            // convert const members to become a simple configuration
            if (func.Body.NodeType == ExpressionType.Constant)
            {
                return CreateColumnConfigurationFromConstant<T, K>(func.Body as ConstantExpression, columnSettings);
            }

            var memberInfo = ReflectionHelper.GetMemberInfoFromLambda(func);
            var fullPropertyPath = ReflectionHelper.GetMemberRelativePathNameFromLambda(func);
            return new MemberColumnConfiguration(memberInfo, columnSettings.Ordinal, columnSettings.ColumnName, columnSettings.NullReplacement, fullPropertyPath);
        }

        public ColumnConfigurationBase BuildMemberColumnConfiguration<T>(MemberInfo memberInfo, int ordinal) where T : new()
        {
            return new MemberColumnConfiguration(memberInfo, ordinal, memberInfo.GetMemberType().GetDefaultValue());
        }

        private ColumnConfigurationBase CreateColumnConfigurationFromConstant<T, K>(ConstantExpression constExpression, ColumnSettings<K> columnSettings) where T : new()
        {
            if (constExpression == null) throw new ArgumentNullException("constExpression");

            var value = (K)constExpression.Value;

            // if we can get the constant member by value, use ForMember otherwise use ForSimpleValue
            MemberInfo memberInfo;
            if (!ReflectionHelper.TryGetConstantMemberInfoByValue<T, K>(value, out memberInfo))
                return BuildSimpleValueColumnConfiguration(value, columnSettings);

            columnSettings.ColumnName = string.IsNullOrEmpty(columnSettings.ColumnName)
                ? memberInfo.Name
                : columnSettings.ColumnName;

            return new MemberColumnConfiguration(memberInfo, columnSettings.Ordinal, columnSettings.ColumnName, columnSettings.NullReplacement, memberInfo.Name);
        }
    }
}