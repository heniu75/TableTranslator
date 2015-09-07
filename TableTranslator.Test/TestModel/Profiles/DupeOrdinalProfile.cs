using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class DupeOrdinalProfile : TranslationProfile
    {
        protected override void Configure()
        {
            this.AddTranslation<TestPerson>()
                .AddColumnConfiguration("Hello")
                .AddExplicitColumnConfiguration(new SimpleValueColumnConfiguration(100, typeof (int), 0, "DupeOrdinal", 99));
        }
    }
}