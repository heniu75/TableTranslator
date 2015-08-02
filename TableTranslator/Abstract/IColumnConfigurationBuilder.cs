using System;
using System.Linq.Expressions;
using System.Reflection;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Abstract
{
    public interface IColumnConfigurationBuilder
    {
        SimpleValueColumnConfiguration BuildSimpleValueColumnConfiguration<T>(T value, ColumnSettings<T> columnSettings);
        DelegateColumnConfiguration BuildDelegateColumnConfiguration<T, K>(Func<T, K> func, ColumnSettings<K> columnSettings);
        NonIdentityColumnConfiguration BuildMemberColumnConfiguration<T>(MemberInfo memberInfo, int ordinal) where T : new();
        NonIdentityColumnConfiguration BuildColumnConfiguration<T, K>(Expression<Func<T, K>> func, ColumnSettings<K> columnSettings) where T : new();
    }
}