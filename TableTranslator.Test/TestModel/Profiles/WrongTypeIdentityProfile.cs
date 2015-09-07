using System;
using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations.Identity;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class WrongTypeIdentityProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<int>(new TranslationSettings(new WrongTypeIdentity(), "WrongTypeIdentity"))
                .AddColumnConfiguration(x => x * 10, new ColumnSettings<int> { ColumnName = "Times10" });
        }

        public class WrongTypeIdentity : ProviderIdentityColumnConfiguration
        {
            public WrongTypeIdentity() : base(typeof(decimal))
            {
            }

            protected override object GetValue(object previousValue)
            {
                return Guid.NewGuid();
            }
        }
    }
}