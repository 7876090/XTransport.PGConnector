namespace XTransport.PGConnector.Models
{
    public interface ICatalogModel
    {
        public string AddRecordQueryText<T>() where T : class, new();
        public string UpdateRecordQueryText<T>() where T : class, new();
        public string RecordsQueryText<T>() where T : class, new();
        public string RecordsQueryText<T>(int id) where T : class, new();
        public string RecordsQueryText<T>(List<int> id) where T : class, new();

    }
}
