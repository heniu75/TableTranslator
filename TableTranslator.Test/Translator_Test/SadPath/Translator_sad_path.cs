using NUnit.Framework;
using TableTranslator.Exceptions;

namespace TableTranslator.Test.Translator_Test.SadPath
{
    [TestFixture]
    public class Translator_sad_path
    {
        [SetUp]
        public void Setup()
        {
            Translator.RemoveAllProfiles();
        }

        [Test]
        public void Calling_ApplyUpdates_before_Initialize_throws_TableTranslationException()
        {
            if (!Translator.IsInitialized)
            {
                Assert.Throws<TableTranslatorException>(() => Translator.ApplyUpdates());
            }
        }
    }
}