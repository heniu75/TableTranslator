using NUnit.Framework;

namespace TableTranslator.Test
{
    public class InitializedTranslatorTestBase
    {
        [SetUp]
        public virtual void Setup()
        {
            TestHelper.ResetTranslator();
        }
    }
}