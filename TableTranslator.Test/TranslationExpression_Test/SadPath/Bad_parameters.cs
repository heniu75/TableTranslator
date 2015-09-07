using System;
using NUnit.Framework;
using TableTranslator.Model;
using TableTranslator.Test.Translator_Test;

namespace TableTranslator.Test.TranslationExpression_Test.SadPath
{
    [TestFixture]
    public class Bad_parameters : InitializedTranslatorTestBase
    {
        [Test]
        public void Null_passed_to_AddColumnConfiguration_throws_ArgumentNullException()
        {
            Translator.AddProfile<AddColumnConfigurationNullProfile>();
            Assert.Throws<ArgumentNullException>(() => Translator.ApplyUpdates());
        }

        private class AddColumnConfigurationNullProfile : TranslationProfile
        {
            protected override void Configure()
            {
                AddTranslation<DateTime>()
                    .AddExplicitColumnConfiguration(null);
            }
        }
    }
}
