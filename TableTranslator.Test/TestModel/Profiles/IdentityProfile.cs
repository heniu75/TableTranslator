using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations.Identity;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class IdentityProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<int>(new TranslationSettings(new LongSeededIdentityColumnConfiguration(), "SeededBasic"))
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new LongSeededIdentityColumnConfiguration(10), "SeededStartingAt10"))
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new LongSeededIdentityColumnConfiguration(0), "SeededStartingAt0"))
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new LongSeededIdentityColumnConfiguration(identityIncrement: 5), "SeededIncrementOf5"))
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new LongSeededIdentityColumnConfiguration(10, 5), "SeededStartingAt10AndIncrementOf5"))
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new LongSeededIdentityColumnConfiguration(-10, -5), "SeededStartingAtNeg10AndIncrementOfNeg5"))
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new GuidIdentityColumnConfiguration("MyGuidId"), "GuidColumnNameProvided"))
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new GuidIdentityColumnConfiguration(), "GuidNoColumnNameProvided"))
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> { ColumnName = "Times10" });
        }
    }
}