namespace TableTranslator.Model.Settings
{
    /// <summary>
    /// Additional settings used to define a column configuration
    /// </summary>
    /// <typeparam name="KColumnDataType">Data type of the column configuration</typeparam>
    public sealed class ColumnConfigurationSettings<KColumnDataType>
    {
        /// <summary>
        /// Name of the column
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Replacement value to be used if provided value is null
        /// </summary>
        public KColumnDataType NullReplacement { get; set; }

        internal int Ordinal { get; set; }
    }
}