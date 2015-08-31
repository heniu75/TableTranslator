using NUnit.Framework;

namespace TableTranslator.Test
{
    [Category("PreInit")]
    public class UnloadedTranslatorTestBase
    {
        [SetUp]
        public virtual void Setup()
        {
            Translator.Uninitialize();
        }
    }
}