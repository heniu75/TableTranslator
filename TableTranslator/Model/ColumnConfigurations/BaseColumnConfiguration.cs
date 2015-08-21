namespace TableTranslator.Model.ColumnConfigurations
{
    public abstract class BaseColumnConfiguration
    {
        public int Ordinal { get; internal set; }
        protected internal string OrdinalColumnName => $"Column{this.Ordinal}";
    }
}