using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class NullPrefixSuffixProfile : TranslationProfile
    {
        protected override string ColumnNamePrefix { get { return null; } }
        protected override string ColumnNameSuffix { get { return null; } }

        protected override void Configure()
        {
            AddTranslation<TestPerson>(new TranslationSettings("NullPrefixSuffix"))
                .AddColumnConfigurationForAllMembers();

            AddTranslation<TestPerson>(new TranslationSettings("TranslationLevelNullPrefixSuffix", null, null))
                .AddColumnConfigurationForAllMembers();
        }
    }
}