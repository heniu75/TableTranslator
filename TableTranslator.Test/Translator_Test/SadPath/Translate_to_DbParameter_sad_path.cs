using System;
using System.Collections.Generic;
using NUnit.Framework;
using TableTranslator.Model.Settings;
using TableTranslator.Test.Model;
using TableTranslator.Test.Model.Profiles;

namespace TableTranslator.Test.Translator_Test.SadPath
{
    [TestFixture]
    public class Translate_to_DbParameter_sad_path : InitializedTranslatorTestBase
    {
        [TestCase("", "value")]
        [TestCase(null, "value")]
        [TestCase("     ", "value")]
        [TestCase("value", "")]
        [TestCase("value", null)]
        [TestCase("value", "     ")]
        [Test]
        public void Param_name_and_db_object_name_must_have_a_value(string dbParam, string dbObjectName)
        {
            Assert.Throws<ArgumentNullException>(() => new DbParameterSettings(dbParam, dbObjectName));
        }

        [Test]
        public void Null_db_parameter_settings_throws_ArgumentNullException()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            Assert.Throws<ArgumentNullException>(() => Translator.TranslateToDbParameter<BasicProfile, TestPerson>(new List<TestPerson>(), "Translation1", null));
        }
    }
}