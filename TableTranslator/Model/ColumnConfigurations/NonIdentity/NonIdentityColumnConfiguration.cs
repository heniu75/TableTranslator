using TableTranslator.Abstract;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.ColumnConfigurations.NonIdentity
{
    /// <summary>
    /// Base class for non-identity column configurations
    /// </summary>
    public abstract class NonIdentityColumnConfiguration : BaseColumnConfiguration, ICloneable<NonIdentityColumnConfiguration>
    {
        /// <summary>
        /// Replacement value to be used if provided value is null
        /// </summary>
        public object NullReplacement { get; private set; }

        internal abstract object GetValueFromObject(object obj);
        internal abstract void ValidateInput();

        internal NonIdentityColumnConfiguration(int ordinal, object nullReplacement)
        {
            if (ordinal < 0)
            {
                throw new TableTranslatorConfigurationException("Column ordinals must be zero or greater.");
            }

            base.Ordinal = ordinal;
            this.NullReplacement = nullReplacement;
        }

        public NonIdentityColumnConfiguration ShallowClone()
        {
            return this.MemberwiseClone() as NonIdentityColumnConfiguration;
        }

        public NonIdentityColumnConfiguration DeepClone()
        {
            // currently shallow clone and deep clone do the same thing
            return ShallowClone();
        }
    }
}