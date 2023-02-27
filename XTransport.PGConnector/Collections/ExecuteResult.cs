namespace XTransport.PGConnector.Collections
{
    public class ExecuteResult
    {
        public bool Success { get; }
        public string? ErrorDescription { get; }
        public string? InnerException { get; }
        public int RecordId { get; }

        public ExecuteResult()
        {
            Success = true;
            ErrorDescription = string.Empty;
            InnerException = string.Empty;
        }

        public ExecuteResult(string errorDescription, string? innerExeption = null)
        {
            Success = false;
            ErrorDescription = errorDescription;
            InnerException = innerExeption;
        }
    }
}
