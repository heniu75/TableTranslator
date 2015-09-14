using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class BasicProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<TestPerson>(new TranslationSettings("Translation1"))
                .AddColumnConfiguration(x => x.PublicProperty, new ColumnConfigurationSettings<string> { ColumnName = "ColOne" })
                .AddColumnConfiguration(27, new ColumnConfigurationSettings<int> { ColumnName = "ColTwo" })
                .AddColumnConfiguration(x => x.PublicField + 8, new ColumnConfigurationSettings<int> { ColumnName = "ColThree" })
                .AddColumnConfiguration(x => TestPerson.ConstantField, new ColumnConfigurationSettings<int> { ColumnName = "ColFour" });

            AddTranslation<TestPerson>(new TranslationSettings("Translation2"))
                .AddColumnConfiguration(x => x.PublicProperty, new ColumnConfigurationSettings<string> { ColumnName = "ColOne" })
                .AddColumnConfiguration(27, new ColumnConfigurationSettings<int> { ColumnName = "ColTwo" })
                .AddColumnConfiguration(x => x.PublicField + 8, new ColumnConfigurationSettings<int> { ColumnName = "ColThree" });

            AddTranslation<TestParent>(new TranslationSettings("Translation3"))
                .AddColumnConfiguration(x => x.TestPerson.PublicField, new ColumnConfigurationSettings<int> { ColumnName = "ColOne" })
                .AddColumnConfiguration(27, new ColumnConfigurationSettings<int> { ColumnName = "ColTwo" });

            AddTranslation<bool>(new TranslationSettings())
                .AddColumnConfiguration(x => x.ToString());
        }
    }
}