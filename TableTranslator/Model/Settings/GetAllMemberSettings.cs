using System;
using System.Collections.Generic;
using System.Reflection;
using TableTranslator.Helpers;

namespace TableTranslator.Model.Settings
{
    public class GetAllMemberSettings
    {
        private BindingFlags _bindingFlags = ReflectionHelper.DEFAULT_BINDING_FLAGS;

        public Func<MemberInfo, bool> Predicate { get; set; }
        public IComparer<MemberInfo> Orderer { get; set; }
        public BindingFlags BindingFlags
        {
            get { return this._bindingFlags; }
            set { this._bindingFlags = value; }
        }
    }
}