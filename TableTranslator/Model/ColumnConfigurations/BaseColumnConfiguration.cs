namespace TableTranslator.Model.ColumnConfigurations
{
    public abstract class BaseColumnConfiguration
    {
        public int Ordinal { get; protected internal set; }
        protected internal string OrdinalColumnName => string.Format("Column{0}", this.Ordinal);
    }
}