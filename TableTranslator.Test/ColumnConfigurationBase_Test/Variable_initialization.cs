using NUnit.Framework;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.ColumnConfigurationBase_Test
{
    [TestFixture]
    public class Variable_initialization
    {
        [Test]
        public void Null_replacement_is_set_correctly_when_not_null()
        {
            const string nullReplacement = "Null replacement";
            var config = new SimpleValueColumnConfiguration("Hello", typeof (string), 0, "ColName", nullReplacement);
            Assert.AreEqual(nullReplacement, config.NullReplacement);
        }

        [Test]
        public void Null_replacement_parameter_of_null_should_be_not_be_updated_to_DBNull()
        {
            var delegateSettings = new DelegateSettings(Mother.IntInAndOutDelegate());
            var colConfig = new DelegateColumnConfiguration(delegateSettings, 0, "ColumnName", null);
            Assert.AreEqual(null, colConfig.NullReplacement);
        }
    }
}