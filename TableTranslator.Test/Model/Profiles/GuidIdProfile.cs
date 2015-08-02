using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.Model.Profiles
{
    public class GuidIdProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<int>(new TranslationSettings(new GuidIdentityColumnConfiguration("MyGuidId"), "ColumnNameProvided"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new GuidIdentityColumnConfiguration(), "NoColumnNameProvided"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });
        }
    }
}