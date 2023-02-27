namespace XTransport.PGConnector.Collections
{
    public class QueryResult<T>
    {
        public bool Success { get; }
        public string? ErrorDescription { get; }
        public string? InnerException { get; }

        public List<T>? Collection { get; set; }

        public QueryResult()
        {
            Success = true;
            ErrorDescription = string.Empty;
            InnerException = string.Empty;
        }

        public QueryResult(string errorDescription, string? innerExeption = null)
        {
            Success = false;
            ErrorDescription = errorDescription;
            InnerException = innerExeption;

        }
    }
}
