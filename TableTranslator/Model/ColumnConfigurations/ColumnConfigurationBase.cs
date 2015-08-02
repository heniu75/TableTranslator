using System;
using TableTranslator.Abstract;
using TableTranslator.Exceptions;

namespace TableTranslator.Model.ColumnConfigurations
{
    public abstract class ColumnConfigurationBase : ICloneable<ColumnConfigurationBase>
    {
        public int Ordinal { get; private set; }
        public object NullReplacement { get; private set; }

        protected ColumnConfigurationBase(int ordinal, object nullReplacement)
        {
            if (ordinal < 0)
            {
                throw new TableTranslatorConfigurationException("Column ordinals must be zero or greater.");
            }

            this.Ordinal = ordinal;
            this.NullReplacement = nullReplacement;
        }

        public abstract string ColumnName { get; }
        public abstract Type OutputType { get; }
        public abstract object GetValueFromObject(object obj);
        internal abstract void ValidateInput();

        public ColumnConfigurationBase ShallowClone()
        {
            return this.MemberwiseClone() as ColumnConfigurationBase;
        }

        public ColumnConfigurationBase DeepClone()
        {
            // currently shallow clone and deep clone do the same thing
            return ShallowClone();
        }
        
        protected internal string DefaultColumnName 
        {
            get { return string.Format("Column{0}", this.Ordinal); }
        }
    }
}