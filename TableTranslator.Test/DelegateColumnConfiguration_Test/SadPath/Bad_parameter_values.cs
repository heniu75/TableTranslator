using System;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.DelegateColumnConfiguration_Test.SadPath
{
    [TestFixture]
    public class Bad_parameter_values
    {
        private DelegateSettings _delegateSettings;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            this._delegateSettings = new DelegateSettings(Mother.IntInAndOutDelegate());
        }

        [TestCase(null)]
        [TestCase("")]
        [Test]
        public void Null_or_empty_column_name_should_get_set_to_name_of_ColumnX_where_X_is_ordinal(string columnName)
        {
            var colConfig = new DelegateColumnConfiguration(this._delegateSettings, 0, columnName, DBNull.Value);
            Assert.AreEqual("Column0", colConfig.ColumnName);
        }

        [Test]
        public void Null_delegate_throws_TableTranslationConfigurationException()
        {
            Assert.Throws<ArgumentNullException>(() => new DelegateColumnConfiguration(null, 0, "ColumnName", DBNull.Value));
        }

        [Test]
        public void Delegate_return_type_and_nonnull_null_replacement_type_must_match_otherwise_throw_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(
                () => new DelegateColumnConfiguration(this._delegateSettings, 0, "ColumnName", new DateTime()),
                "Null replacement for delegate must be either of the same type as the delegate's return type, null, or DBNull.");
        }

        [Test]
        public void Incorrect_object_type_to_get_value_throws_ArgumentException()
        {
            var colConfig = new DelegateColumnConfiguration(this._delegateSettings, 0, "ColName", DBNull.Value);
            Assert.Throws<ArgumentException>(() => colConfig.GetValueFromObject("A string"));
        }
    }
}
