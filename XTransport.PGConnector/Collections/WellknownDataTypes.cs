namespace XTransport.PGConnector.Collections
{
    public static class WellknownDataTypes
    {
        public const string SERIAL_PRIMARY_KEY = "SERIAL PRIMARY KEY";
        public const string INTEGER = "integer";
        public const string BIGINT = "bigint";
        public const string VARCHAR_15 = "varchar(15)";
        public const string VARCHAR_50 = "varchar(50)";
        public const string VARCHAR_100 = "varchar(100)";
        public const string VARCHAR_255 = "varchar(255)";
        public const string VARCHAR_15_NOT_NULL = "varchar(15) NOT NULL";
        public const string VARCHAR_50_NOT_NULL = "varchar(50) NOT NULL";
        public const string VARCHAR_100_NOT_NULL = "varchar(100) NOT NULL";
        public const string VARCHAR_255_NOT_NULL = "varchar(255) NOT NULL";
        public const string DATE = "date";
        public const string TIMESTAMP_WITH_TIME_ZONE = "TIMESTAMP WITH TIME ZONE";
        public const string TIMESTAMP_WITHOUT_TIME_ZONE = "TIMESTAMP WITHOUT TIME ZONE";
        public const string TIMESTAMP_NOT_NULL_DEF_NOW = "TIMESTAMP NOT NULL DEFAULT NOW()";
        public const string BOOL_NOT_NULL_DEF_F = "bool NOT NULL DEFAULT false";
        public const string BOOL_NOT_NULL_DEF_T = "bool NOT NULL DEFAULT true";
        public const string TEXT = "text";
    }
}
