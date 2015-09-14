namespace TableTranslator.Model.ColumnConfigurations.Identity
{
    /// <summary>
    /// Configuration for an identity column that assings a seeded auto incrementing long value for every row
    /// </summary>
    public sealed class LongSeededIdentityColumnConfiguration : SeededIdentityColumnConfiguration<long>
    {
        /// <summary>
        /// Creates an instance of LongSeededIdentityColumnConfiguration
        /// </summary>
        /// <param name="identitySeed">Seed for the identity column</param>
        /// <param name="identityIncrement">Increment for the identity column</param>
        public LongSeededIdentityColumnConfiguration(long identitySeed = 1, long identityIncrement = 1) : base(identitySeed, identityIncrement)
        {
        }

        /// <summary>
        /// Creates an instance of LongSeededIdentityColumnConfiguration
        /// </summary>
        /// <param name="identitySeed">Seed for the identity column</param>
        /// <param name="identityIncrement">Increment for the identity column</param>
        /// <param name="columnName">Name of the identity column</param>
        public LongSeededIdentityColumnConfiguration(long identityIncrement, long identitySeed, string columnName)
            : base(identitySeed, identityIncrement, columnName)
        {
        }
    }
}