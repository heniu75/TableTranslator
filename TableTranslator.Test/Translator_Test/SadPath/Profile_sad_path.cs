﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Model;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.Translator_Test.SadPath
{
    [TestFixture]
    public class Profile_sad_path : InitializedTranslatorTestBase
    {
        [Test]
        public void Null_profile_passed_to_add_profile_throws_TableTranslationException()
        {
            Assert.Throws<ArgumentNullException>(() => Translator.AddProfile(null));
        }

        [Test]
        public void Null_or_empty_profile_name_throws_TableTranslationConfigurationException()
        {
            var nullNameProfile = new NullNameProfile();
            var emptyNameProfile = new EmptyNameProfile();

            Assert.Throws<TableTranslatorConfigurationException>(() => Translator.AddProfile(nullNameProfile));
            Assert.Throws<TableTranslatorConfigurationException>(() => Translator.AddProfile(emptyNameProfile));

            Assert.Throws<TableTranslatorConfigurationException>(() => Translator.AddProfile<NullNameProfile>());
            Assert.Throws<TableTranslatorConfigurationException>(() => Translator.AddProfile<EmptyNameProfile>());
        }

        [Test]
        public void Adding_duplicate_profiles_throws_TableTranslationConfigurationException()
        {
            Translator.AddProfile<DefaultNameProfile>();
            Assert.Throws<TableTranslatorConfigurationException>(Translator.AddProfile<DefaultNameProfile>);
        }

        [Test]
        public void Adding_profiles_with_same_name_throws_TableTranslationConfigurationException()
        {
            Translator.AddProfile<DupeProfile1>();
            Assert.Throws<TableTranslatorConfigurationException>(Translator.AddProfile<DupeProfile2>);
        }

        [Test]
        public void Add_multiple_profiles_with_null_parameter_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Translator.AddProfiles(null));
        }

        [Test]
        public void Add_multiple_profiles_with_an_enumerable_that_contains_a_null_throws_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Translator.AddProfiles(new List<TranslationProfile> {new BasicProfile(), null, new BasicProfile2()}));
        }

        [Test]
        public void Get_profiles_using_null_predicate_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Translator.GetProfiles(null));
        }

        [Test]
        public void Remove_profiles_using_null_predicate_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Translator.RemoveProfiles(null));
        }

        public class DupeProfile1 : TranslationProfile
        {
            public override string ProfileName
            {
                get { return "Duplicate Profile Name"; }
            }

            protected override void Configure()
            {
                
            }
        }
        public class DupeProfile2 : TranslationProfile
        {
            public override string ProfileName
            {
                get { return "Duplicate Profile Name"; }
            }

            protected override void Configure()
            {
                
            }
        }
    }
}
