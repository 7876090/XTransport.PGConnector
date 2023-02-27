namespace XTransport.PGConnector.Collections
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataTable : Attribute
    {
        public string Name { get; }
        public DataTable(string name)
        {
            Name = name;
        }
    }
}
