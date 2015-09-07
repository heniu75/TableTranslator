using TableTranslator.Model;

namespace TableTranslator.Test.TestModel.Profiles
{
    public class SpyProfile : TranslationProfile
    {
        public bool ConfigureWasCalled { get; private set; }
        protected override void Configure()
        {
            this.ConfigureWasCalled = true;
        }
    }
}