using System;
using NUnit.Framework;
using TableTranslator.Model;

namespace TableTranslator.Test.TranslationExpression_Test.SadPath
{
    [TestFixture]
    public class Bad_parameters
    {
        [SetUp]
        public void Setup()
        {
            if (!Translator.IsInitialized)
            {
                Translator.Initialize();
            }
            Translator.RemoveAllProfiles();
            Translator.ApplyUpdates();
        }

        [Test]
        public void Null_passed_to_AddColumnConfiguration_throws_ArgumentNullException()
        {
            Translator.AddProfile<AddColumnConfigurationNullProfile>();
            Assert.Throws<ArgumentNullException>(() => Translator.Initialize());
        }

        private class AddColumnConfigurationNullProfile : TranslationProfile
        {
            protected override void Configure()
            {
                AddTranslation<DateTime>()
                    .AddColumnConfiguration(null);
            }
        }
    }
}
