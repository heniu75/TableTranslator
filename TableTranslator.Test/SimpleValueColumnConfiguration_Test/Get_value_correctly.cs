using System;
using NUnit.Framework;
using TableTranslator.Model.ColumnConfigurations;

namespace TableTranslator.Test.SimpleValueColumnConfiguration_Test
{
    [TestFixture]
    public class Get_value_correctly
    {
        [TestCase(null)]
        [TestCase("SomethingElse")]
        [TestCase(100)]
        [Test]
        public void Get_value_returns_value_passed_in_constructor_of_class_regardless_of_method_parameter(object paramForGetValue)
        {
            const string value = "Hello";
            var colConfig = new SimpleValueColumnConfiguration(value, typeof(string), 0, "ColumnName", DBNull.Value);
            Assert.AreEqual(value, colConfig.GetValueFromObject(paramForGetValue));
        }
    }
}