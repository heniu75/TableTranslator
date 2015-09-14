using System;
namespace TableTranslator.Model.ColumnConfigurations
{
    /// <summary>
    /// Base class for column configurations
    /// </summary>
    public abstract class BaseColumnConfiguration
    {
        /// <summary>
        /// Ordinal of the column in the translation
        /// </summary>
        public int Ordinal { get; internal set; }

        /// <summary>
        /// Name of the column
        /// </summary>
        public abstract string ColumnName { get; }

        /// <summary>
        /// Data type of the column
        /// </summary>
        public abstract Type ColumnDataType { get; }

        internal string OrdinalColumnName => $"Column{this.Ordinal}";
    }
}