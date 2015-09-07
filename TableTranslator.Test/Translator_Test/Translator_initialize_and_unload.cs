using NUnit.Framework;

namespace TableTranslator.Test.Translator_Test
{
    [TestFixture]
    public class Translator_initialize_and_unload : UnInitializedTranslatorTestBase
    {
        [Test]
        public void Calling_Initialize_sets_the_translator_as_being_initialized()
        {
            Translator.Initialize();
            Assert.IsTrue(Translator.IsInitialized);
        }

        [Test]
        public void Calling_UnloadAll_sets_the_translator_as_being_not_initialized()
        {
            Translator.Initialize();
            Translator.Uninitialize();
            Assert.IsFalse(Translator.IsInitialized);
        }
    }
}