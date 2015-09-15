using NUnit.Framework;
using TableTranslator.Model.ColumnConfigurations.Identity;

namespace TableTranslator.Test.ColumnConfiguration_Test.GuidIdentityColumnConfiguration_Test.SadPath
{
    [TestFixture]
    public class Bad_parameter_values
    {
        [TestCase(null)]
        [TestCase("")]
        [Test]
        public void Null_or_empty_column_name_should_get_set_to_name_of_ColumnX_where_X_is_ordinal(string columnName)
        {
            var colConfig = new GuidIdentityColumnConfiguration(columnName);
            Assert.AreEqual("Column0", colConfig.ColumnName);
        }
    }
}