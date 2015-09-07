using System.Collections.Generic;
using NUnit.Framework;
using TableTranslator.Model;
using TableTranslator.Model.Settings;
using TableTranslator.Test.TestModel;

namespace TableTranslator.Test.Translator_Test
{
    [TestFixture]
    public class Translate_data_results : InitializedTranslatorTestBase
    {
        [Test]
        public void Delegate_funtion_that_updates_member_ordered_after_member_configs_does_not_change_values()
        {
            var people = new List<TestPerson> {new TestPerson {PublicProperty = "Hello"}};
            Translator.AddProfile<StrangeDelegatesProfile>();
            Translator.ApplyUpdates();
            var table = Translator.Translate<StrangeDelegatesProfile, TestPerson>(people, "After");
            Assert.AreEqual("Hello", table.Rows[0][0]);
        }

        [Test]
        public void Delegate_funtion_that_updates_member_ordered_before_member_configs_does_change_values()
        {
            var people = new List<TestPerson> { new TestPerson { PublicProperty = "Hello" } };
            Translator.AddProfile<StrangeDelegatesProfile>();
            Translator.ApplyUpdates();
            var table = Translator.Translate<StrangeDelegatesProfile, TestPerson>(people, "Before");
            Assert.AreEqual(StrangeDelegatesProfile.ValueUpdatedTo, table.Rows[0][1]);
        }


        public class StrangeDelegatesProfile : TranslationProfile
        {
            public const string ValueUpdatedTo = "CHANGED";

            protected override void Configure()
            {
                AddTranslation<TestPerson>(new TranslationSettings("After"))
                    .AddColumnConfiguration(p => p.PublicProperty)
                    .AddColumnConfiguration(p => UpdateProperties(p));

                AddTranslation<TestPerson>(new TranslationSettings("Before"))
                    .AddColumnConfiguration(p => UpdateProperties(p))
                    .AddColumnConfiguration(p => p.PublicProperty);
            }

            public string UpdateProperties(TestPerson person)
            {
                person.PublicProperty = ValueUpdatedTo;
                return "I Changed Stuff!";
            }
        }
    }
}