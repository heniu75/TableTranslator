using System;
using System.Reflection;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Model.ColumnConfigurations;

namespace TableTranslator.Test.MemberColumnConfiguration_Test.SadPath
{
    [TestFixture]
    public class Bad_parameter_values
    {
        private MemberInfo _memberInfo;

        [TestFixtureSetUp]
        public void Setup()
        {
            this._memberInfo = typeof (string).GetProperty("Length");
        }

        [TestCase(null)]
        [TestCase("")]
        [Test]
        public void Null_or_empty_column_name_should_get_set_to_name_of_member(string columnName)
        {
            var colConfig = new MemberColumnConfiguration(this._memberInfo, 0, columnName, DBNull.Value, this._memberInfo.Name);
            Assert.AreEqual(this._memberInfo.Name, colConfig.ColumnName);
        }

        [Test]
        public void Null_member_info_throws_TableTranslationConfigurationException()
        {
            Assert.Throws<ArgumentNullException>(() => new MemberColumnConfiguration(null, 0, "ColumnName", DBNull.Value, "Length"));
        }

        [TestCase(null)]
        [TestCase("")]
        [Test]
        public void Null_or_empty_relative_property_defaults_to_name_of_member(string relativePropertyPath)
        {
            var config = new MemberColumnConfiguration(this._memberInfo, 0, "ColumnName", DBNull.Value, relativePropertyPath);
            Assert.AreEqual(this._memberInfo.Name, config.RelativePropertyPath);
        }

        [Test]
        public void Relative_property_path_must_contain_member_info_name_otherwise_throw_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new MemberColumnConfiguration(this._memberInfo, 0, "ColumnName", DBNull.Value, "NotAMatch"));
        }

        [Test]
        public void Member_info_type_and_nonnull_null_replacement_type_must_match_otherwise_throw_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(
                () => new MemberColumnConfiguration(this._memberInfo, 0, "ColumnName", new DateTime(), this._memberInfo.Name));
        }

        [Test]
        public void Null_obj_passed_to_get_value_from_object_returns_null()
        {
            var config = new MemberColumnConfiguration(this._memberInfo, 0, "ColumnName", DBNull.Value, this._memberInfo.Name);
            Assert.AreEqual(null, config.GetValueFromObject(null));
        }

        [Test]
        public void Wrong_object_type_passed_to_get_value_from_object_returns_null()
        {
            var config = new MemberColumnConfiguration(this._memberInfo, 0, "ColumnName", DBNull.Value, this._memberInfo.Name);
            Assert.AreEqual(null, config.GetValueFromObject(new DateTime()));
        }
    }
}
