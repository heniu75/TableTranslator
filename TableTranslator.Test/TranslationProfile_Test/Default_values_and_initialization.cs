using NUnit.Framework;
using TableTranslator.Test.TestModels.Profiles;

namespace TableTranslator.Test.TranslationProfile_Test
{
    [TestFixture]
    public class Default_values_and_initialization
    {
        [SetUp]
        public void Setup()
        {
            Translator.RemoveAllProfiles();
        }

        [Test]
        public void Profile_name_is_the_name_of_the_class_by_default()
        {
            var profile = new DefaultNameProfile();
            Assert.AreEqual(typeof(DefaultNameProfile).Name, profile.ProfileName);
        }

        [Test]
        public void Configure_is_called_during_initialization()
        {
            var spyProfile = new SpyProfile();
            Translator.AddProfile(spyProfile);
            Translator.Initialize();
            Assert.IsTrue(spyProfile.ConfigureWasCalled);
        }
    }
}
