using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.ColumnConfiguration_Test.IdentityColumnConfiguration_Test
{
    [TestFixture]
    public class Defaults_set_properly : InitializedTranslatorTestBase
    {
        [Test]
        public void Identity_column_set_to_correct_name_when_provided()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var columnNames = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3 }, "GuidColumnNameProvided").GetColumnNames();
            Assert.AreEqual("MyGuidId", columnNames[0]);
        }

        [Test]
        public void Identity_column_set_to_correct_name_when_not_provided()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var columnNames = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3 }, "GuidNoColumnNameProvided").GetColumnNames();
            Assert.AreEqual("Column0", columnNames[0]);
        }

        [Test]
        public void Identity_column_set_as_primary_key()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3 }, "GuidNoColumnNameProvided");
            Assert.AreEqual(1, dataTable.PrimaryKey.Count());
            Assert.AreEqual("Column0", dataTable.PrimaryKey[0].ColumnName);
        }
    }
}
