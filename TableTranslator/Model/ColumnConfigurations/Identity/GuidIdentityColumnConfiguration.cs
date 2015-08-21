using System;

namespace TableTranslator.Model.ColumnConfigurations.Identity
{
    public sealed class GuidIdentityColumnConfiguration : ProviderIdentityColumnConfiguration
    {
        public GuidIdentityColumnConfiguration() : base(typeof(Guid), null)
        {
        }

        public GuidIdentityColumnConfiguration(string columnName) : base(typeof(Guid), columnName)
        {
        }

        protected internal override object GetValue(object previousValue) => Guid.NewGuid();
    }
}