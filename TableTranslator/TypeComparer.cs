using System;
using System.Collections.Generic;

namespace TableTranslator
{
    internal class TypeComparer : IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            return x.FullName == y.FullName;
        }

        public int GetHashCode(Type obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}