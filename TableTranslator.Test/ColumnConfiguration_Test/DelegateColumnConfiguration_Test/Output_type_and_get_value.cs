using System;
using NUnit.Framework;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;
using TableTranslator.Test.Model;

namespace TableTranslator.Test.ColumnConfiguration_Test.DelegateColumnConfiguration_Test
{
    public class Output_type_and_get_value
    {
        private TestPerson _personModel;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            this._personModel = Mother.GetTestPersonModel();
        }

        [Test]
        public void Output_type_gets_assigned_as_the_return_type_of_delegate()
        {
            var config = new DelegateColumnConfiguration(new DelegateSettings(Mother.IntInAndStringOutDelegate()),
                0, "ColName", DBNull.Value);
            Assert.AreEqual(typeof(string), config.OutputType);
        }

        [Test]
        public void Get_value_works_with_lambda_expression_delegate()
        {
            var config = new DelegateColumnConfiguration(new DelegateSettings(
                new IntInAndStringOutDelegate(x => string.Format("{0} x {1} = {2}", x, 2, x * 2))),
                0, "ColName", DBNull.Value);
            Assert.AreEqual("5 x 2 = 10", config.GetValueFromObject(5));
        }

        [Test]
        public void Get_value_works_with_existing_instance_method_delegate()
        {
            var config = new DelegateColumnConfiguration(new DelegateSettings(
                new PersonInAndStringOutDelegate(MyInstanceMethod)), 0, "ColName", DBNull.Value);
            Assert.AreEqual(this._personModel.PublicProperty.ToUpper(), config.GetValueFromObject(this._personModel));
        }

        [Test]
        public void Get_value_works_with_existing_static_method_delegate()
        {
            var config = new DelegateColumnConfiguration(new DelegateSettings(
                new PersonInAndStringOutDelegate(MyStaticMethod)), 0, "ColName", DBNull.Value);
            Assert.AreEqual(this._personModel.PublicProperty.ToUpper(), config.GetValueFromObject(this._personModel));
        }

        private string MyInstanceMethod(TestPerson person)
        {
            return string.Format("{0}", person.PublicProperty.ToUpper());
        }

        private static string MyStaticMethod(TestPerson person)
        {
            return string.Format("{0}", person.PublicProperty.ToUpper());
        }
    }
}