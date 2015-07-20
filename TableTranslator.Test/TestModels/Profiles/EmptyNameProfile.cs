using TableTranslator.Model;

namespace TableTranslator.Test.TestModels.Profiles
{
    public class EmptyNameProfile : TranslationProfile
    {
        public override string ProfileName
        {
            get { return string.Empty; }
        }

        protected override void Configure()
        {
        }
    }
}