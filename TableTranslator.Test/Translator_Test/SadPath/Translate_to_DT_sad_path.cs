using System;
using System.Collections.Generic;
using NUnit.Framework;
using TableTranslator.Exceptions;
using TableTranslator.Test.TestModel;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.Translator_Test.SadPath
{
    [TestFixture]
    public class Translate_to_DT_sad_path : InitializedTranslatorTestBase
    {
        private readonly List<TestPerson> people = new List<TestPerson>
        {
            new TestPerson {PublicProperty = "Chris", PublicField = 100},
            new TestPerson {PublicProperty = "Aubrey", PublicField = 101}
        };

        [Test]
        public void Translate_to_DT_throws_TableTranslatorException_when_no_matching_translation_is_found()
        {
            Assert.Throws<TableTranslatorException>(() => Translator.Translate<BasicProfile, TestPerson>(this.people, "Bad Name"));
        }

        [Test]
        public void Translate_to_DT_throws_TableTranslatorException_when_more_than_one_matching_translation_is_found()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            Assert.Throws<TableTranslatorException>(() => Translator.Translate<BasicProfile, TestPerson>(this.people));
        }

        [Test]
        public void Translate_to_DT_throws_TableTranslatorException_when_generic_type_does_not_match()
        {
            Translator.AddProfile<GenericsProfile>();
            Translator.ApplyUpdates();
            Assert.Throws<TableTranslatorException>(() => 
                Translator.Translate<GenericsProfile, Generics.OneGeneric<bool>>(new List<Generics.OneGeneric<bool>>(), "IntGeneric"));
        }

        [Test]
        public void Translate_to_DT_throws_TableTranslatorException_when_nested_generic_type_does_not_match()
        {
            Translator.AddProfile<GenericsProfile>();
            Translator.ApplyUpdates();
            Assert.Throws<TableTranslatorException>(() =>
                Translator.Translate<GenericsProfile, Generics.OneGeneric<Generics.OneGeneric<DateTime>>>(new List<Generics.OneGeneric<Generics.OneGeneric<DateTime>>>(), "NestedGeneric"));
        }
    }
}