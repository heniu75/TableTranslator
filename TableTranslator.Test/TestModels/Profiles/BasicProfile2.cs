using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModels.Profiles
{
    public class BasicProfile2 : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<TestPerson>(new TranslationSettings("Translation4"))
                .ForMember(x => x.PublicProperty, new ColumnSettings<string> { ColumnName = "ColOne" })
                .ForSimpleValue(27, new ColumnSettings<int> {ColumnName = "ColTwo"})
                .ForDelegate(x => x.PublicField + 8, new ColumnSettings<int> { ColumnName = "ColThree" });

            AddTranslation<TestPerson>(new TranslationSettings("Translation5"))
                .ForMember(x => x.PublicProperty, new ColumnSettings<string> { ColumnName = "ColOne" })
                .ForSimpleValue(27, new ColumnSettings<int> { ColumnName = "ColTwo" })
                .ForDelegate(x => x.PublicField + 8, new ColumnSettings<int> { ColumnName = "ColThree" });
        }
    }
}