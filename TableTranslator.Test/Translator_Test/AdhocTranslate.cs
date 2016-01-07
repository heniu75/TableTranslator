using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using TableTranslator.Model.Settings;
using TableTranslator.Test.TestModel;

namespace TableTranslator.Test.Translator_Test
{
    [TestFixture]
    public class AdhocTranslate : UnInitializedTranslatorTestBase
    {
        private readonly List<TestPerson> people = new List<TestPerson>
        {
            new TestPerson {PublicProperty = "Chris", PublicField = 100},
            new TestPerson {PublicProperty = "Aubrey", PublicField = 101},
            new TestPerson {PublicProperty = "John", PublicField = 102}
        };

        [Test]
        public void Adhoc_Translate()
        {
            var transExp = Translator.CreateAdhocTranslation<TestPerson>()
                .AddColumnConfiguration(x => x.PublicProperty)
                .AddColumnConfiguration(x => x.PublicField);

            var rows = Translator.AdhocTranslate(transExp, people).Rows;

            Assert.AreEqual("Chris", rows[0][0]);
            Assert.AreEqual(100, (int)rows[0][1]);

            Assert.AreEqual("Aubrey", rows[1][0]);
            Assert.AreEqual(101, (int)rows[1][1]);

            Assert.AreEqual("John", rows[2][0]);
            Assert.AreEqual(102, (int)rows[2][1]);

            Assert.IsFalse(Translator.IsInitialized);
        }

        [Test]
        public void Adhoc_TranslateToDbParameter()
        {
            var transExp = Translator.CreateAdhocTranslation<TestPerson>()
                .AddColumnConfiguration(x => x.PublicProperty)
                .AddColumnConfiguration(x => x.PublicField);

            var dbParam = Translator.AdhocTranslateToDbParameter(
                transExp, people, new DbParameterSettings("myParam", "myDbObjectName"));

            Assert.AreEqual("myParam", dbParam.ParameterName);
            Assert.AreEqual("myDbObjectName", ((DataTable)dbParam.Value).TableName);
            Assert.AreEqual(DbType.Object, dbParam.DbType);
            Assert.AreEqual(SqlDbType.Structured, ((SqlParameter)dbParam).SqlDbType);

            Assert.IsFalse(Translator.IsInitialized);
        }
    }
}