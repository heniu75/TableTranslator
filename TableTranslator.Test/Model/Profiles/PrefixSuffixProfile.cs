using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.Model.Profiles
{
    public class PrefixSuffixProfile : TranslationProfile
    {
        protected override string ColumnNamePrefix { get { return "PROFPRE_"; } }
        protected override string ColumnNameSuffix { get { return "_PROFSUF"; } }

        protected override void Configure()
        {
            AddTranslation<TestPerson>(new TranslationSettings("ProfileInherited1"))
                .ForAllMembers();

            AddTranslation<TestPerson>(new TranslationSettings("ProfileInherited2"))
                .ForAllMembers();

            AddTranslation<TestPerson>(new TranslationSettings("TranslationSpecific", "TRANPRE_", "_TRANSUF"))
                .ForAllMembers();

            AddTranslation<TestPerson>(new TranslationSettings("NullTranslationSpecific", null, null ))
                .ForAllMembers();
        }
    }
}