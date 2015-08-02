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

        [Test]
        public void Value_type_and_output_types_must_match_otherwise_throw_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new SimpleValueColumnConfiguration("A String", typeof(int), 0, "ColumnName", null));
        }

        [Test]
        public void Value_and_output_type_must_match_otherwise_throw_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new SimpleValueColumnConfiguration("A string", typeof(int), 0, "ColumnName", null));
        }
    }
}
