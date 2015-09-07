using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.ColumnConfiguration_Test.SeededIdentityColumnConfiguration_Test.SadPath
{
    [TestFixture]
    public class Sad_Path : InitializedTranslatorTestBase
    {
        [Test]
        public void Seeded_with_custom_increment_of_zero_throws_TableTranslatorConfigurationException()
        {
            Translator.AddProfile<ZeroIncrementSeededIdentityProfile>();
            Assert.Throws<TableTranslatorConfigurationException>(() => Translator.ApplyUpdates());
        }
    }
}