using System.Linq;
using NUnit.Framework;
using TableTranslator.Model;
using TableTranslator.Test.TestModel;

namespace TableTranslator.Test.TranslationExpression_Test
{
    [TestFixture]
    public class CreateColumnConfiguration : InitializedTranslatorTestBase
    {
        [Test(Description = "https://github.com/cknightdevelopment/TableTranslator/issues/1")]
        public void String_reverse_non_expression_column_config_does_not_throw_exception()
        {
            Translator.AddProfile<StringReverseProfile>();
            Assert.DoesNotThrow(() => Translator.ApplyUpdates());
        }

        [Test]
        public void Null_non_expression_column_config_does_not_throw_exception()
        {
            Translator.AddProfile<NullNonExpressionProfile>();
            Assert.DoesNotThrow(() => Translator.ApplyUpdates());
        }

        #region specific issue profiles

        public class StringReverseProfile : TranslationProfile
        {
            protected override void Configure()
            {
                AddTranslation<int>()
                    .AddColumnConfiguration("Hello".Reverse());
            }
        }

        public class NullNonExpressionProfile : TranslationProfile
        {
            private readonly TestPerson person = null;

            protected override void Configure()
            {
                AddTranslation<int>()
                    .AddColumnConfiguration(person);
            }
        }

        #endregion
    }
}
