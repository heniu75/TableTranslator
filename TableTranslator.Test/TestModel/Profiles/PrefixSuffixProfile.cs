using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class PrefixSuffixProfile : TranslationProfile
    {
        protected override string ColumnNamePrefix { get { return "PROFPRE_"; } }
        protected override string ColumnNameSuffix { get { return "_PROFSUF"; } }

        protected override void Configure()
        {
            AddTranslation<TestPerson>(new TranslationSettings("ProfileInherited1"))
                .AddColumnConfigurationForAllMembers();

            AddTranslation<TestPerson>(new TranslationSettings("ProfileInherited2"))
                .AddColumnConfigurationForAllMembers();

            AddTranslation<TestPerson>(new TranslationSettings("TranslationSpecific", "TRANPRE_", "_TRANSUF"))
                .AddColumnConfigurationForAllMembers();

            AddTranslation<TestPerson>(new TranslationSettings("NullTranslationSpecific", null, null ))
                .AddColumnConfigurationForAllMembers();
        }
    }
}