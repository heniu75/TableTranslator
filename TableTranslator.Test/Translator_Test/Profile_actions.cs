using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TableTranslator.Model;
using TableTranslator.Test.TestModels;
using TableTranslator.Test.TestModels.Profiles;

namespace TableTranslator.Test.Translator_Test
{
    [TestFixture]
    public class Profile_actions : InitializedTranslatorTestBase
    {
        [Test]
        public void Add_profile_with_generic()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();

            Assert.AreEqual(1, Translator.GetProfiles(x => x.ProfileName == new BasicProfile().ProfileName).Count());
        }

        [Test]
        public void Add_profile_with_instance()
        {
            Translator.AddProfile(new BasicProfile());
            Translator.ApplyUpdates();

            Assert.AreEqual(1, Translator.GetProfiles(x => x.ProfileName == new BasicProfile().ProfileName).Count());
        }

        [Test]
        public void Add_multiple_profiles()
        {
            Translator.AddProfiles(new List<TranslationProfile> { new BasicProfile(), new BasicProfile2(), new SpyProfile() });
            Assert.AreEqual(3, Translator.GetAllProfiles().Count());
        }

        [Test]
        public void Get_all_profiles_returns_all_profiles()
        {
            var basicProfile = new BasicProfile();
            var basicProfile2 = new BasicProfile2();
            var spyProfile = new SpyProfile();

            Translator.AddProfile(basicProfile);
            Translator.AddProfile(basicProfile2);
            Translator.AddProfile(spyProfile);

            var profiles = Translator.GetAllProfiles().ToList();

            CollectionAssert.Contains(profiles, basicProfile);
            CollectionAssert.Contains(profiles, basicProfile2);
            CollectionAssert.Contains(profiles, spyProfile);
        }

        [Test]
        public void Get_all_profiles_returns_empty_collection_when_no_profiles_added()
        {
            CollectionAssert.IsEmpty(Translator.GetAllProfiles());
        }

        [Test]
        public void Get_profiles_using_predicate()
        {
            Translator.AddProfiles(new List<TranslationProfile> { new BasicProfile(), new BasicProfile2(), new SpyProfile() });
            Func<TranslationProfile, bool> pred = t => t.ProfileName.Contains("Basic");
            Assert.AreEqual(2, Translator.GetProfiles(pred).Count());
        }

        [Test]
        public void Get_profiles_using_predicate_with_no_matches_returns_empty_collection()
        {
            Translator.AddProfiles(new List<TranslationProfile> { new BasicProfile(), new BasicProfile2(), new SpyProfile() });
            Func<TranslationProfile, bool> pred = t => t.ProfileName.Contains("XYZ");
            Assert.AreEqual(0, Translator.GetProfiles(pred).Count());
        }

        [Test]
        public void Get_profile_using_name()
        {
            Translator.AddProfiles(new List<TranslationProfile> { new BasicProfile(), new BasicProfile2(), new SpyProfile() });
            var foundProfile = Translator.GetProfile("BasicProfile");
            Assert.IsNotNull(foundProfile);
            Assert.IsInstanceOf<BasicProfile>(foundProfile);
        }

        [Test]
        public void Get_profile_using_name_with_no_matches_returns_null()
        {
            Translator.AddProfiles(new List<TranslationProfile> { new BasicProfile(), new BasicProfile2(), new SpyProfile() });
            var foundProfile = Translator.GetProfile("ABC");
            Assert.IsNull(foundProfile);
        }

        [TestCase("")]
        [TestCase(null)]
        [Test]
        public void Get_profile_using_null_or_empty_name_returns_null(string profileName)
        {
            Translator.AddProfiles(new List<TranslationProfile> { new BasicProfile(), new BasicProfile2(), new SpyProfile() });
            var foundProfile = Translator.GetProfile(profileName);
            Assert.IsNull(foundProfile);
        }

        [Test]
        public void Remove_profile_removes_the_correct_profile()
        {
            var basicProfile = new BasicProfile();
            var basicProfile2 = new BasicProfile2();
            var spyProfile = new SpyProfile();
            var defaultNameProfile = new DefaultNameProfile();

            Translator.AddProfile(basicProfile);
            Translator.AddProfile(basicProfile2);
            Translator.AddProfile(spyProfile);
            Translator.AddProfile(defaultNameProfile);

            // by generic
            Translator.RemoveProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var profiles = Translator.GetAllProfiles().ToList();
            CollectionAssert.DoesNotContain(profiles, basicProfile);
            Assert.AreEqual(3, profiles.Count());

            // by instance
            Translator.RemoveProfile(basicProfile2);
            Translator.ApplyUpdates();
            profiles = Translator.GetAllProfiles().ToList();
            CollectionAssert.DoesNotContain(profiles, basicProfile2);
            Assert.AreEqual(2, profiles.Count());

            // by profile name
            Translator.RemoveProfile(spyProfile.ProfileName);
            Translator.ApplyUpdates();
            profiles = Translator.GetAllProfiles().ToList();
            CollectionAssert.DoesNotContain(profiles, spyProfile);
            Assert.AreEqual(1, profiles.Count());
        }

        [Test]
        public void Remove_all_profiles_empties_all_the_profiles()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.AddProfile<SpyProfile>();

            Translator.RemoveAllProfiles();
            Translator.ApplyUpdates();

            CollectionAssert.IsEmpty(Translator.GetAllProfiles());
        }

        [Test]
        public void Remove_profile_cascades_deletes_and_empties_the_translations_for_only_that_profile()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();

            Translator.RemoveProfile<BasicProfile>();
            Translator.ApplyUpdates();

            CollectionAssert.IsEmpty(Translator.GetProfileTranslations<BasicProfile>());
            CollectionAssert.IsNotEmpty(Translator.GetProfileTranslations<BasicProfile2>());
        }

        [Test]
        public void Remove_all_profiles_cascades_deletes_and_empties_all_the_translations()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();

            Translator.RemoveAllProfiles();
            Translator.ApplyUpdates();

            CollectionAssert.IsEmpty(Translator.GetAllTranslations());
        }

        [Test]
        public void Remove_profile_with_null_or_empty_name_does_not_remove_any_profiles()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();

            Assert.DoesNotThrow(() => Translator.RemoveProfile<NullNameProfile>());
            Assert.DoesNotThrow(() => Translator.RemoveProfile<EmptyNameProfile>());

            Assert.AreEqual(2, Translator.GetAllProfiles().Count());
        }

        [Test]
        public void Remove_profile_with_name_does_not_remove_profile_when_name_not_found()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();

            Translator.RemoveProfile("FakeName");
            Translator.ApplyUpdates();

            Assert.AreEqual(2, Translator.GetAllProfiles().Count());
        }

        [TestCase("")]
        [TestCase(null)]
        [Test]
        public void Remove_profile_with_empty_or_null_parameter_does_not_remove_any_profiles(string profileName)
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();

            Translator.RemoveProfile(profileName);
            Translator.ApplyUpdates();

            Assert.AreEqual(2, Translator.GetAllProfiles().Count());
        }

        [Test]
        public void Remove_all_does_not_throw_exception_when_no_profiles_have_been_added()
        {
            Assert.DoesNotThrow(() => Translator.RemoveAllProfiles());
        }

        [Test]
        public void Null_profile_instance_passed_to_remove_profile_throws_ArgumentNullException()
        {
            TranslationProfile profile = null;
            Assert.DoesNotThrow(() => Translator.RemoveProfile(profile));
        }

        [Test]
        public void Remove_profiles_using_predicate()
        {
            Translator.AddProfiles(new List<TranslationProfile> { new BasicProfile(), new BasicProfile2(), new SpyProfile() });
            Func<TranslationProfile, bool> pred = t => t.ProfileName.Contains("Basic");
            Translator.RemoveProfiles(pred);
            Translator.ApplyUpdates();
            Assert.AreEqual(1, Translator.GetAllProfiles().Count());
        }

        [Test]
        public void Remove_profiles_using_predicate_with_no_matches_removes_no_profiles()
        {
            Translator.AddProfiles(new List<TranslationProfile> { new BasicProfile(), new BasicProfile2(), new SpyProfile() });
            Func<TranslationProfile, bool> pred = t => t.ProfileName.Contains("XYZ");
            Assert.AreEqual(0, Translator.GetProfiles(pred).Count());
        }
    }
}