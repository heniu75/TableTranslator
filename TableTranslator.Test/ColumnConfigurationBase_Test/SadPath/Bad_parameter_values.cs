using System;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.ColumnConfigurationBase_Test.SadPath
{
    [TestFixture]
    public class Bad_parameter_values
    {
        private DelegateSettings _delegateSettings;

        [TestFixtureSetUp]
        public void Setup()
        {
            this._delegateSettings = new DelegateSettings(Mother.IntInAndOutDelegate());
        }

        [Test]
        public void Negative_ordinal_throws_TableTranslationConfigurationException()
        {
            Assert.Throws<TableTranslatorConfigurationException>(() => new DelegateColumnConfiguration(this._delegateSettings, -1, "ColumnName", null));
        }
    }
}
