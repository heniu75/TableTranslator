using System;
using TableTranslator.Abstract;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.ColumnConfigurations
{
    public abstract class NonIdentityColumnConfiguration : BaseColumnConfiguration, ICloneable<NonIdentityColumnConfiguration>
    {
        public object NullReplacement { get; private set; }

        protected NonIdentityColumnConfiguration(int ordinal, object nullReplacement)
        {
            if (ordinal < 0)
            {
                throw new TableTranslatorConfigurationException("Column ordinals must be zero or greater.");
            }

            base.Ordinal = ordinal;
            this.NullReplacement = nullReplacement;
        }

        public abstract string ColumnName { get; }
        public abstract Type OutputType { get; }
        public abstract object GetValueFromObject(object obj);
        internal abstract void ValidateInput();

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