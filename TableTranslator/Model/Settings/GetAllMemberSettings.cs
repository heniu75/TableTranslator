using System;
using System.Collections.Generic;
using System.Reflection;
using TableTranslator.Helpers;

namespace TableTranslator.Model.Settings
{
    /// <summary>
    /// Additional settings used to define the members to create column configurations for
    /// </summary>
    public class GetAllMemberSettings
    {
        /// <summary>
        /// A function to filter which members of the type to create a column configuration for
        /// </summary>
        public Func<MemberInfo, bool> Predicate { get; set; }

        /// <summary>
        /// A class used to determine the order of the column configurations returned
        /// </summary>
        public IComparer<MemberInfo> Orderer { get; set; }

        /// <summary>
        /// Specifies flags that control the way in which the search for members and types will be conducted.
        /// The default is: BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static
        /// </summary>
        public BindingFlags BindingFlags { get; set; } = ReflectionHelper.DEFAULT_BINDING_FLAGS;
    }
}