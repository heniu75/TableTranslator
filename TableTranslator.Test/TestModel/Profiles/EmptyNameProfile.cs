using TableTranslator.Model;

namespace TableTranslator.Test.TestModel.Profiles
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