using System;
using System.Linq.Expressions;
using System.Reflection;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Abstract
{
    public interface IColumnConfigurationBuilder
    {
        NonIdentityColumnConfiguration BuildColumnConfiguration<T>(T value, ColumnSettings<T> columnSettings);
        NonIdentityColumnConfiguration BuildColumnConfiguration<T, K>(Expression<Func<T, K>> func, ColumnSettings<K> columnSettings);
        NonIdentityColumnConfiguration BuildColumnConfiguration<T>(MemberInfo memberInfo, int ordinal) where T : new();
    }
}