using TableTranslator.Model;

namespace TableTranslator.Test.TestModels.Profiles
{
    public class NullNameProfile : TranslationProfile
    {
        public override string ProfileName
        {
            get { return null; }
        }

        protected override void Configure()
        {
            //throw new NotImplementedException();
        }
    }
}