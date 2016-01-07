using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class BasicProfile3 : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<TestPerson>()
                .AddColumnConfiguration(x => x.PublicProperty)
                .AddColumnConfiguration(x => x.PublicField)
                .AddColumnConfiguration("Bag Item", new ColumnConfigurationSettings<string> { ColumnName = "BagItem1"})
                .AddColumnConfiguration(12345, new ColumnConfigurationSettings<int> { ColumnName = "BagItem2" });
        }
    }
}