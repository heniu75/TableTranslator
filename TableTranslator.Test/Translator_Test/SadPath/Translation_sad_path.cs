using System;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Model;
using TableTranslator.Model.Settings;
using TableTranslator.Test.TestModels;
using TableTranslator.Test.TestModels.Profiles;

namespace TableTranslator.Test.Translator_Test.SadPath
{
    [TestFixture]
    public class Translation_sad_path : InitializedTranslatorTestBase
    {
        [Test]
        public void Get_all_translations_returns_empty_collection_if_not_initialized()
        {
            var translations = Translator.GetAllTranslations();
            CollectionAssert.IsEmpty(translations);
        }

        [Test]
        public void Get_profile_translations_returns_empty_collection_if_profile_not_added()
        {
            var translations = Translator.GetProfileTranslations<BasicProfile>();
            CollectionAssert.IsEmpty(translations);
        }

        [Test]
        public void Get_profile_translations_for_type_returns_empty_collection_if_profile_not_added()
        {
            var translations = Translator.GetProfileTranslationsForType<BasicProfile, TestPerson>();
            CollectionAssert.IsEmpty(translations);
        }

        [Test]
        public void Get_profile_translations_for_type_returns_empty_collection_if_profile_does_not_have_translation_for_the_type()
        {
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();
            var translations = Translator.GetProfileTranslationsForType<BasicProfile, TestParent>();
            CollectionAssert.IsEmpty(translations);
        }

        [Test]
        public void Get_specific_profile_translations_returns_null_if_profile_not_added()
        {
            var translation = Translator.GetProfileTranslation<BasicProfile, TestParent>("Translation3");
            Assert.IsNull(translation);
        }

        [Test]
        public void Get_specific_profile_translations_returns_null_if_profile_does_not_have_translation_for_the_type()
        {
            Translator.AddProfile<BasicProfile2>();
            Translator.ApplyUpdates();
            var translation = Translator.GetProfileTranslation<BasicProfile, TestParent>("Translation4");
            Assert.IsNull(translation);
        }

        [Test]
        public void Get_specific_profile_translations_returns_null_if_profile_does_not_have_translation_for_the_type_and_name()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var translation = Translator.GetProfileTranslation<BasicProfile, TestParent>("WrongName");
            Assert.IsNull(translation);
        }

        [Test]
        public void Duplicate_column_ordinal_throws_TableTranslatorConfigurationException()
        {
            Translator.AddProfile<DupeOrdinalProfile>();
            Assert.Throws<TableTranslatorConfigurationException>(() => Translator.ApplyUpdates());
        }

        [Test]
        public void Explicit_column_name_matching_default_column_name_for_member_config_throws_TableTranslatorConfigurationException()
        {
            Translator.AddProfile<DupeExplicitMemberColumnNameProfile>();
            Assert.Throws<TableTranslatorConfigurationException>(() => Translator.ApplyUpdates());
        }

        [Test]
        public void Explicit_column_name_matching_default_column_name_for_non_member_config_throws_TableTranslatorConfigurationException()
        {
            Translator.AddProfile<DupeExplicitNonMemberColumnNameProfile>();
            Assert.Throws<TableTranslatorConfigurationException>(() => Translator.ApplyUpdates());
        }

        [Test]
        public void Get_translations_using_null_predicate_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Translator.GetTranslations(null));
        }

        private class DupeExplicitMemberColumnNameProfile : TranslationProfile
        {
            protected override void Configure()
            {
                AddTranslation<TestPerson>(new TranslationSettings("Member"))
                    .ForMember(p => p.PublicProperty)
                    .ForMember(p => p.InternalProperty, new ColumnSettings<string> {ColumnName = "PublicProperty"});
            }
        }

        private class DupeExplicitNonMemberColumnNameProfile : TranslationProfile
        {
            protected override void Configure()
            {
                AddTranslation<TestPerson>(new TranslationSettings("NonMember"))
                    .ForSimpleValue(12)
                    .ForMember(p => p.InternalProperty, new ColumnSettings<string> { ColumnName = "Column0" });
            }
        }
    }
}