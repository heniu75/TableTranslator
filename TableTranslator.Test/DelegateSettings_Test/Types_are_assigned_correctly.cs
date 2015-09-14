using NUnit.Framework;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.DelegateSettings_Test
{
    [TestFixture]
    public class Types_are_assigned_correctly
    {
        private DelegateSettings _delegateSettings;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            this._delegateSettings = new DelegateSettings(Mother.IntInAndStringOutDelegate());
        }

        [Test]
        public void Input_type_same_as_delegate_parameter_type()
        {
            Assert.AreEqual(typeof (int), this._delegateSettings.InputType);
        }

        [Test]
        public void Output_type_same_as_delegate_return_type()
        {
            Assert.AreEqual(typeof(string), this._delegateSettings.ReturnType);
        }
    }
}
