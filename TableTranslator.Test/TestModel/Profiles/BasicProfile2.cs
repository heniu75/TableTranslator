using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class BasicProfile2 : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<TestPerson>(new TranslationSettings("Translation4"))
                .AddColumnConfiguration(x => x.PublicProperty, new ColumnSettings<string> { ColumnName = "ColOne" })
                .AddColumnConfiguration(27, new ColumnSettings<int> {ColumnName = "ColTwo"})
                .AddColumnConfiguration(x => x.PublicField + 8, new ColumnSettings<int> { ColumnName = "ColThree" });

            AddTranslation<TestPerson>(new TranslationSettings("Translation5"))
                .AddColumnConfiguration(x => x.PublicProperty, new ColumnSettings<string> { ColumnName = "ColOne" })
                .AddColumnConfiguration(27, new ColumnSettings<int> { ColumnName = "ColTwo" })
                .AddColumnConfiguration(x => x.PublicField + 8, new ColumnSettings<int> { ColumnName = "ColThree" });
        }
    }
}