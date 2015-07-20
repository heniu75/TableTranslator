using System;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.DelegateSettings_Test.SadPath
{
    [TestFixture]
    public class Bad_parameter_values
    {
        delegate void VoidDelegate();
        delegate int NoParameterDelegate();
        delegate int MultParameterDelegate(string str, int number);

        [Test]
        public void Null_delegate_throws_TableTranslationConfigurationException()
        {
            Assert.Throws<ArgumentNullException>(() => new DelegateSettings(null));
        }

        [Test]
        public void Delegate_that_returns_void_throws_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new DelegateSettings(new VoidDelegate(() => { })));
        }

        [Test]
        public void Delegate_with_more_than_one_input_parameter_throws_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new DelegateSettings(new MultParameterDelegate((str, number) => 1)));
        }

        [Test]
        public void Delegate_with_zero_input_parameters_throws_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new DelegateSettings(new NoParameterDelegate(() => 1)));
        }

    }
}
