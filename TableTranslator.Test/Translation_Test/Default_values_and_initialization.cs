using System.Linq;
using NUnit.Framework;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.Translation_Test
{
    [TestFixture]
    public class Default_values_and_initialization : InitializedTranslatorTestBase
    {
        [Test]
        public void Get_column_configurations_returns_correct_count_when_no_identity_column_present()
        {
            Translator.AddProfile<ColumnConfigurationTestProfile>();
            Translator.ApplyUpdates();
            var translation = Translator.GetTranslations(x => 
                x.TranslationProfile.GetType() == typeof (ColumnConfigurationTestProfile) && x.TranslationSettings.TranslationName == "WithoutIdentityColumn")
                .FirstOrDefault();
            Assert.AreEqual(3, translation.GetColumnConfigurations().Count);
        }

        [Test]
        public void Get_column_configurations_returns_correct_count_when_identity_column_present()
        {
            Translator.AddProfile<ColumnConfigurationTestProfile>();
            Translator.ApplyUpdates();
            var translation = Translator.GetTranslations(x =>
                x.TranslationProfile.GetType() == typeof(ColumnConfigurationTestProfile) && x.TranslationSettings.TranslationName == "WithIdentityColumn")
                .FirstOrDefault();
            Assert.AreEqual(4, translation.GetColumnConfigurations().Count);
        }
    }
}
