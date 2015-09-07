using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations.Identity;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class ZeroIncrementSeededIdentityProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(identityIncrement: 0)))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });
        }
    }
}