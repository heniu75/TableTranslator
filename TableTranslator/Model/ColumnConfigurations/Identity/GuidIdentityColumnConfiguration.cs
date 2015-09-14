using System;

namespace TableTranslator.Model.ColumnConfigurations.Identity
{
    /// <summary>
    /// Configuration for an identity column that assings a unique Guid for every row
    /// </summary>
    public sealed class GuidIdentityColumnConfiguration : ProviderIdentityColumnConfiguration
    {
        /// <summary>
        /// Creates an instance of GuidIdentityColumnConfiguration
        /// </summary>
        public GuidIdentityColumnConfiguration() : base(typeof(Guid), null)
        {
        }

        /// <summary>
        /// Creates an instance of GuidIdentityColumnConfiguration
        /// </summary>
        /// <param name="columnName">Name for the identity column</param>
        public GuidIdentityColumnConfiguration(string columnName) : base(typeof(Guid), columnName)
        {
        }

        protected internal override object GetNextValue(object previousValue) => Guid.NewGuid();
    }
}