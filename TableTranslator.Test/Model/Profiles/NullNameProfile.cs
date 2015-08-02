using TableTranslator.Model;

namespace TableTranslator.Test.Model.Profiles
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