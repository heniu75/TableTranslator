using System;
using System.Linq;
using NUnit.Framework;
using TableTranslator.Model;
using TableTranslator.Test.TestModel;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.Translator_Test
{
    [TestFixture]
    public class Get_translations : InitializedTranslatorTestBase
    {
        [Test]
        public void Calling_apply_updates_more_than_once_does_not_duplicate_translations()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();

            Translator.ApplyUpdates();
            var translationsCount1 = Translator.GetAllTranslations().Count();
            Translator.ApplyUpdates();
            var translationsCount2 = Translator.GetAllTranslations().Count();

            Assert.AreEqual(translationsCount1, translationsCount2);
        }

        [Test]
        public void Get_all_translations_before_initialization_returns_empty_collection()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            var collections = Translator.GetAllTranslations();
            CollectionAssert.IsEmpty(collections);
        }

        [Test]
        public void Get_all_translations_returns_correct_count()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();
            var collections = Translator.GetAllTranslations();
            Assert.AreEqual(6, collections.Count());
        }

        [Test]
        public void Get_profile_translations_returns_translations_only_for_that_profile()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();
            var collections = Translator.GetProfileTranslations<BasicProfile2>();
            Assert.AreEqual(2, collections.Count());
        }

        [Test]
        public void Get_profile_translations_for_type_returns_translations_only_for_that_profile_and_type()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();

            var personTranslations = Translator.GetProfileTranslationsForType<BasicProfile, TestPerson>();
            var parentTranslations = Translator.GetProfileTranslationsForType<BasicProfile, TestParent>();

            Assert.AreEqual(2, personTranslations.Count());
            Assert.AreEqual(1, parentTranslations.Count());
        }

        [Test]
        public void Get_specific_profile_translation_returns_the_correct_translation_from_the_profile()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();

            var translation = Translator.GetProfileTranslation<BasicProfile, TestPerson>("Translation2");
            Assert.AreEqual("Translation2", translation.TranslationSettings.TranslationName);
        }

        [Test]
        public void Get_translations_using_predicate()
        {
            Translator.AddProfile(new BasicProfile());
            Translator.ApplyUpdates();
            Func<Translation, bool> pred = t => t.TranslationSettings.TranslationName.IndexOf("2", StringComparison.Ordinal) < 0;
            Assert.AreEqual(3, Translator.GetTranslations(pred).Count());
        }

        [Test]
        public void Get_translations_using_predicate_with_no_matches_returns_empty_collection()
        {
            Translator.AddProfile(new BasicProfile());
            Translator.ApplyUpdates();
            Func<Translation, bool> pred = t => t.TranslationSettings.TranslationName == "XYZ";
            Assert.AreEqual(0, Translator.GetTranslations(pred).Count());
        }
    }
}