namespace XTransport.PGConnector.Collections
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataTableColumn : Attribute
    {
        public string Name { get; }
        public string DataType { get; }
        public DataTableColumn(string name, string dataType)
        {
            Name = name;
            DataType = dataType;
        }
    }
}
