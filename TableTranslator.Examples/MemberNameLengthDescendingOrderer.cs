using System.Collections.Generic;
using System.Reflection;

namespace TableTranslator.Examples

{
    class MemberNameLengthDescendingOrderer : IComparer<MemberInfo>
    {
        public int Compare(MemberInfo x, MemberInfo y)
        {
            if (x.Name.Length > y.Name.Length)
            {
                return -1;
            }
            if (x.Name.Length < y.Name.Length)
            {
                return 1;
            }
            return 0;
        }
    }
}