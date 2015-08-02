using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations;

namespace TableTranslator.Test.Model.Profiles
{
    public class DupeOrdinalProfile : TranslationProfile
    {
        protected override void Configure()
        {
            this.AddTranslation<TestPerson>()
                .ForSimpleValue("Hello")
                .AddColumnConfiguration(new SimpleValueColumnConfiguration(100, typeof (int), 0, "DupeOrdinal", 99));
        }
    }
}