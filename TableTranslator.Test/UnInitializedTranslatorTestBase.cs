using NUnit.Framework;

namespace TableTranslator.Test
{
    [Category("PreInit")]
    public class UnInitializedTranslatorTestBase
    {
        [SetUp]
        public virtual void Setup()
        {
            Translator.Uninitialize();
        }
    }
}