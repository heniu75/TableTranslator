using System;
using TableTranslator.Abstract;

namespace TableTranslator.Model
{
    internal class TranslationUniqueIdentifier : ICloneable<TranslationUniqueIdentifier>
    {
        private readonly string _profileName;
        private readonly string _translationName;
        private readonly string _typeName;

        private TranslationUniqueIdentifier(string profileName, string translationName, string typeName)
        {
            this._profileName = profileName;
            this._translationName = translationName;
            this._typeName = typeName;
        }

        public override string ToString()
        {
            return $"[ProfileName:{this._profileName}],[TranslationName:{this._translationName}],[TypeName:{this._typeName}]";
        }

        internal static TranslationUniqueIdentifier GetInstance(TranslationBase translation)
        {
            return new TranslationUniqueIdentifier(translation.TranslationProfile.ProfileName, translation.TranslationSettings.TranslationName, translation.TypeInfo.Name);
        }

        internal bool IsMatch(string profileName)
        {
            return profileName.Equals(this._profileName, StringComparison.InvariantCulture);
        }

        internal bool IsMatch(string profileName, string typeName)
        {
            return IsMatch(profileName) && typeName.Equals(this._typeName, StringComparison.InvariantCulture);
        }

        internal bool IsMatch(string profileName, string typeName, string translationName)
        {
            return IsMatch(profileName, typeName) && translationName.Equals(this._translationName, StringComparison.InvariantCulture);
        }

        public TranslationUniqueIdentifier ShallowClone()
        {
            return this.MemberwiseClone() as TranslationUniqueIdentifier;
        }

        public TranslationUniqueIdentifier DeepClone()
        {
            return ShallowClone();
        }
    }
}