using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Test.Model.Profiles;

namespace TableTranslator.Test.ColumnConfiguration_Test.IdentityColumnConfiguration_Test
{
    [TestFixture]
    public class Sad_path : InitializedTranslatorTestBase
    {
        [Test]
        public void DataType_and_return_type_of_get_value_throws_exception_when_not_the_same()
        {
            
        }

        public class WrongTypeIdentity : IdentityColumnConfiguration
        {
            public WrongTypeIdentity()
            {
            }

            public WrongTypeIdentity(string columnName) : base(columnName)
            {
            }

            protected override Type DataType => typeof(decimal);

            protected override object GetValue(object previousValue) => Guid.NewGuid();
        }
    }

    [TestFixture]
    public class Defaults_set_properly : InitializedTranslatorTestBase
    {
        [Test]
        public void Identity_column_set_to_correct_name_when_provided()
        {
            Translator.AddProfile<GuidIdProfile>();
            Translator.ApplyUpdates();
            var columnNames = Translator.Translate<GuidIdProfile, int>(new List<int> { 1, 2, 3 }, "ColumnNameProvided").GetColumnNames();
            Assert.AreEqual("MyGuidId", columnNames[0]);
        }

        [Test]
        public void Identity_column_set_to_correct_name_when_not_provided()
        {
            Translator.AddProfile<GuidIdProfile>();
            Translator.ApplyUpdates();
            var columnNames = Translator.Translate<GuidIdProfile, int>(new List<int> { 1, 2, 3 }, "NoColumnNameProvided").GetColumnNames();
            Assert.AreEqual("Column0", columnNames[0]);
        }

        [Test]
        public void Identity_column_set_as_primary_key()
        {
            Translator.AddProfile<GuidIdProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<GuidIdProfile, int>(new List<int> { 1, 2, 3 }, "NoColumnNameProvided");
            Assert.AreEqual(1, dataTable.PrimaryKey.Count());
            Assert.AreEqual("Column0", dataTable.PrimaryKey[0].ColumnName);
        }
    }
}
