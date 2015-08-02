using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.Model.Profiles
{
    public class BasicProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<TestPerson>(new TranslationSettings("Translation1"))
                .ForMember(x => x.PublicProperty, new ColumnSettings<string> { ColumnName = "ColOne" })
                .ForSimpleValue(27, new ColumnSettings<int> { ColumnName = "ColTwo" })
                .ForDelegate(x => x.PublicField + 8, new ColumnSettings<int> { ColumnName = "ColThree" });

            AddTranslation<TestPerson>(new TranslationSettings("Translation2"))
                .ForMember(x => x.PublicProperty, new ColumnSettings<string> { ColumnName = "ColOne" })
                .ForSimpleValue(27, new ColumnSettings<int> { ColumnName = "ColTwo" })
                .ForDelegate(x => x.PublicField + 8, new ColumnSettings<int> { ColumnName = "ColThree" });

            AddTranslation<TestParent>(new TranslationSettings("Translation3"))
                .ForMember(x => x.TestPerson.PublicField, new ColumnSettings<int> { ColumnName = "ColOne" })
                .ForSimpleValue(27, new ColumnSettings<int> { ColumnName = "ColTwo" });

            AddTranslation<bool>(new TranslationSettings())
                .ForDelegate(x => x.ToString());
        }
    }
}