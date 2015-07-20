using System;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Model.ColumnConfigurations;

namespace TableTranslator.Test.SimpleValueColumnConfiguration_Test.SadPath
{
    [TestFixture]
    public class Bad_parameter_values
    {
        [TestCase(null)]
        [TestCase("")]
        [Test]
        public void Null_or_empty_column_name_should_get_set_to_ColumnX_where_X_is_ordinal(string columnName)
        {
            var colConfig = new SimpleValueColumnConfiguration("Hello", typeof(string), 0, columnName, DBNull.Value);
            Assert.AreEqual("Column0", colConfig.ColumnName);
        }

        [Test]
        public void Null_output_type_throws_TableTranslationConfigurationException()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleValueColumnConfiguration("Hello", null, 0, "ColumnName", DBNull.Value));
        }

        [TestCase("A String", typeof(string), 1)]
        [TestCase("A String", typeof(int), "Another String")]
        [TestCase(1, typeof(int), "A string")]
        [TestCase(1, typeof(bool), "A string")]
        [Test]
        public void Objects_and_output_types_must_match_otherwise_throw_TableTranslationConfigurationException(object value, Type outputType, object nullReplacement)
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new SimpleValueColumnConfiguration(value, outputType, 0, "ColumnName", nullReplacement));
        }

        [Test]
        public void Value_and_output_type_must_match_when_null_replacement_is_not_populated_otherwise_throw_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new SimpleValueColumnConfiguration("A string", typeof(int), 0, "ColumnName", null));
            Assert.Throws<TableTranslatorConfigurationException>(() => new SimpleValueColumnConfiguration("A string", typeof(int), 0, "ColumnName", DBNull.Value));
        }
    }
}
