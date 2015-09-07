using System;
using System.Collections.Generic;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.Model.Profiles
{
    public class BasicProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<TestPerson>(new TranslationSettings("Translation1"))
                .AddColumnConfiguration(x => x.PublicProperty, new ColumnSettings<string> { ColumnName = "ColOne" })
                .AddColumnConfiguration(27, new ColumnSettings<int> { ColumnName = "ColTwo" })
                .AddColumnConfiguration(x => x.PublicField + 8, new ColumnSettings<int> { ColumnName = "ColThree" });

            AddTranslation<TestPerson>(new TranslationSettings("Translation2"))
                .AddColumnConfiguration(x => x.PublicProperty, new ColumnSettings<string> { ColumnName = "ColOne" })
                .AddColumnConfiguration(27, new ColumnSettings<int> { ColumnName = "ColTwo" })
                .AddColumnConfiguration(x => x.PublicField + 8, new ColumnSettings<int> { ColumnName = "ColThree" });

            AddTranslation<TestParent>(new TranslationSettings("Translation3"))
                .AddColumnConfiguration(x => x.TestPerson.PublicField, new ColumnSettings<int> { ColumnName = "ColOne" })
                .AddColumnConfiguration(27, new ColumnSettings<int> { ColumnName = "ColTwo" });

            AddTranslation<bool>(new TranslationSettings())
                .AddColumnConfiguration(x => x.ToString());
        }
    }
}