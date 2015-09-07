using System.Collections.Generic;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Test.TestModel;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.Translator_Test.SadPath
{
    [TestFixture]
    public class Translator_sad_path : UnInitializedTranslatorTestBase
    {
        [Test]
        public void Calling_ApplyUpdates_before_Initialize_throws_TableTranslationException()
        {
            Assert.Throws<TableTranslatorException>(() => Translator.ApplyUpdates());
        }

        [Test]
        public void Translate_to_DT_throws_TableTranslatorException_when_attempting_to_translate_before_initialization()
        {
            Assert.Throws<TableTranslatorException>(() => Translator.Translate<BasicProfile, TestPerson>(new List<TestPerson>(), "Bad Name"));
        }
    }
}