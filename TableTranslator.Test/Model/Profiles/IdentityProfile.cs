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
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(10), "SeededStartingAt10"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(0), "SeededStartingAt0"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(identityIncrement: 5), "SeededIncrementOf5"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(10, 5), "SeededStartingAt10AndIncrementOf5"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new SeededIdentityColumnConfiguration(-10, -5), "SeededStartingAtNeg10AndIncrementOfNeg5"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new GuidIdentityColumnConfiguration("MyGuidId"), "GuidColumnNameProvided"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });

            AddTranslation<int>(new TranslationSettings(new GuidIdentityColumnConfiguration(), "GuidNoColumnNameProvided"))
                .ForDelegate(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });
        }
    }
}