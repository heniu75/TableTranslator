using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations.Identity;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.Model.Profiles
{
    public class IdentityProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(), "SeededBasic"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(10), "SeededStartingAt10"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(0), "SeededStartingAt0"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(identityIncrement: 5), "SeededIncrementOf5"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(10, 5), "SeededStartingAt10AndIncrementOf5"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(-10, -5), "SeededStartingAtNeg10AndIncrementOfNeg5"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new GuidIdentityColumnConfiguration("MyGuidId"), "GuidColumnNameProvided"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new GuidIdentityColumnConfiguration(), "GuidNoColumnNameProvided"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });
        }
    }
}