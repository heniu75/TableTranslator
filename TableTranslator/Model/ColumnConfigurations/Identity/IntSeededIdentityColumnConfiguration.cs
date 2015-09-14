namespace TableTranslator.Model.ColumnConfigurations.Identity
{
    /// <summary>
    /// Configuration for an identity column that assings a seeded auto incrementing int value for every row
    /// </summary>
    public sealed class IntSeededIdentityColumnConfiguration : SeededIdentityColumnConfiguration<int>
    {
        /// <summary>
        /// Creates an instance of IntSeededIdentityColumnConfiguration
        /// </summary>
        /// <param name="identitySeed">Seed for the identity column</param>
        /// <param name="identityIncrement">Increment for the identity column</param>
        public IntSeededIdentityColumnConfiguration(int identitySeed = 1, int identityIncrement = 1) : base(identitySeed, identityIncrement)
        {

        }

        /// <summary>
        /// Creates an instance of IntSeededIdentityColumnConfiguration
        /// </summary>
        /// <param name="identitySeed">Seed for the identity column</param>
        /// <param name="identityIncrement">Increment for the identity column</param>
        /// <param name="columnName">Name of the identity column</param>
        public IntSeededIdentityColumnConfiguration(int identityIncrement, int identitySeed, string columnName)
            : base(identitySeed, identityIncrement, columnName)
        {

        }
    }
}