namespace TableTranslator.Model.Settings
{
    public sealed class ColumnSettings<K>
    {
        public string ColumnName { get; set; }
        public K NullReplacement { get; set; }
        internal int Ordinal { get; set; }
    }
}