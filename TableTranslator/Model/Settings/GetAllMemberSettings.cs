using System;
using System.Collections.Generic;
using System.Reflection;
using TableTranslator.Helpers;

namespace TableTranslator.Model.Settings
{
    public class GetAllMemberSettings
    {
        public Func<MemberInfo, bool> Predicate { get; set; }
        public IComparer<MemberInfo> Orderer { get; set; }
        public BindingFlags BindingFlags { get; set; } = ReflectionHelper.DEFAULT_BINDING_FLAGS;
    }
}