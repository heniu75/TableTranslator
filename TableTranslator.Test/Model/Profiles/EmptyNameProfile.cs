using TableTranslator.Model;

namespace TableTranslator.Test.Model.Profiles
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