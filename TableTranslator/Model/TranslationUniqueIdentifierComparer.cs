using System.Collections.Generic;

namespace TableTranslator.Model
{
    internal class TranslationUniqueIdentifierComparer : IEqualityComparer<TranslationUniqueIdentifier>
    {
        public bool Equals(TranslationUniqueIdentifier x, TranslationUniqueIdentifier y)
        {
            return x.ToString() == y.ToString();
        }

        public int GetHashCode(TranslationUniqueIdentifier obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}