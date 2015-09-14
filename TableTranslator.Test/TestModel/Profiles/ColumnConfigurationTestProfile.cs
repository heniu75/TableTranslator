using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations.Identity;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class ColumnConfigurationTestProfile : TranslationProfile
    { 
        protected override void Configure()
        {
            AddTranslation<bool>(new TranslationSettings("WithoutIdentityColumn"))
                .AddColumnConfiguration(x => x)
                .AddColumnConfiguration(x => x.ToString())
                .AddColumnConfiguration(2 + 2);

            AddTranslation<bool>(new TranslationSettings(new GuidIdentityColumnConfiguration(), "WithIdentityColumn"))
                .AddColumnConfiguration(x => x)
                .AddColumnConfiguration(x => x.ToString())
                .AddColumnConfiguration(2 + 2);
        }
    }
}