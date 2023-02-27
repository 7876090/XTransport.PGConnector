using System.Reflection;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using XTransport.PGConnector.Collections;
using XTransport.PGConnector.Models;

namespace XTransport.PGConnector
{
    public class Connector
    {
        public const string ConnectionStringParameterName = "NSIConnectionServerAuto";
        private static string? GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("pgconnectorsettings.json", true, false)
                .Build();
            return configuration.GetConnectionString(ConnectionStringParameterName);
        }

        private static QueryResult<T> ErrorNullableConnectionString<T>()
        {
            return new QueryResult<T>("Не задана строка подключения!(appsettings.json => NSIConnection");
        }

        private static ExecuteResult ErrorNullableConnectionString()
        {
            return new ExecuteResult("Не задана строка подключения!(appsettings.json => NSIConnection");
        }

        public async static Task<QueryResult<object>> UpdateDatabaseTables()
        {
            QueryResult<object> result = new QueryResult<object>();

            List<string> dataBaseTables = new List<string>();
            List<string> systemModels = new List<string>();
            Dictionary<string, bool> tables = new Dictionary<string, bool>();

            string? connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                bool hasNSISystemModelsTable = false;
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    var data = await connection.QueryAsync<string>("SELECT table_name FROM information_schema.tables WHERE table_schema NOT IN ('information_schema','pg_catalog');");
                    foreach (var tableName in data)
                    {
                        if (tableName.ToLower() == "nsi_systemmodels")
                        {
                            hasNSISystemModelsTable = true;
                        }
                        dataBaseTables.Add(tableName.ToLower());
                    }
                }
                if (hasNSISystemModelsTable)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        var data = await connection.QueryAsync<string>("SELECT \"name\" as name FROM public.nsi_systemmodels;");
                        foreach (var tableName in data)
                        {
                            systemModels.Add(tableName);
                        }
                    }
                }

                List<string> queryText = new List<string>();
                List<string> insertQueryText = new List<string>();
                foreach (var defType in Assembly.GetExecutingAssembly().DefinedTypes)
                {
                    if (defType.BaseType == typeof(CatalogModel) | defType.BaseType == typeof(BaseModel))
                    {
                        object[] attrs = defType.GetCustomAttributes(typeof(DataTable), true);

                        foreach (DataTable attr in attrs)
                        {
                            if (!dataBaseTables.Contains(attr.Name.ToLower()))
                            {
                                queryText.Add($"CREATE TABLE IF NOT EXISTS {attr.Name}(");
                                var properties = defType.GetProperties();
                                foreach (var prop in properties)
                                {
                                    object[] columns = prop.GetCustomAttributes(typeof(DataTableColumn), true);
                                    foreach (DataTableColumn dt_column in columns)
                                    {
                                        queryText.Add($"{dt_column.Name} {dt_column.DataType}");
                                        queryText.Add(",");
                                    }
                                }
                                queryText.RemoveAt(queryText.Count - 1);
                                queryText.Add($");");
                                if (queryText.Count > 0)
                                {
                                    queryText.Add("\t");
                                }
                            }
                            if (!systemModels.Contains(attr.Name.ToLower()))
                            {
                                insertQueryText.Add($"INSERT INTO public.nsi_systemmodels\r\n(\"name\", createddate, isdeleted)\r\nVALUES('{attr.Name.ToLower()}', now(), false);");
                            }

                        }
                    }
                }
                if (queryText.Count > 0)
                {
                    if (insertQueryText.Count > 0)
                    {
                        queryText.AddRange(insertQueryText);
                    }
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        try
                        {
                            var tmp = await connection.QueryAsync(string.Join("", queryText));
                        }
                        catch (Exception ex)
                        {
                            result = new QueryResult<object>(ex.Message ?? string.Empty, ex.InnerException?.Message ?? string.Empty);
                        }
                    };
                }
            }
            else
            {
                result = ErrorNullableConnectionString<object>();
            }

            return result;
        }

        public static async Task<ExecuteResult> AddRecordAsync<T>(string queryText, T item)
        {
            ExecuteResult result = new ExecuteResult();
            string? connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        var tmp = await connection.ExecuteAsync(queryText, item);
                    }
                    catch (Exception ex)
                    {
                        result = new ExecuteResult(ex.Message ?? string.Empty, ex.InnerException?.Message ?? string.Empty);
                    }
                }
            }
            else
            {
                result = ErrorNullableConnectionString();
            }
            return result;
        }

        public static async Task<QueryResult<T>> GetRecordsAsync<T>(string queryText)
        {
            QueryResult<T> result = new QueryResult<T>();
            string? connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        var data = await connection.QueryAsync<T>(queryText);
                        result.Collection = data.ToList();
                    }
                    catch (Exception ex)
                    {
                        result = new QueryResult<T>(ex.Message ?? string.Empty, ex.InnerException?.Message ?? string.Empty);
                    }
                }
            }
            else
            {
                result = ErrorNullableConnectionString<T>();
            }
            return result;
        }

        public static async Task<ExecuteResult> UpdateRecordAsync<T>(string queryText, T item)
        {
            ExecuteResult result = new ExecuteResult();
            string? connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        await connection.ExecuteAsync(queryText, item);
                    }
                    catch (Exception ex)
                    {
                        result = new ExecuteResult(ex.Message ?? string.Empty, ex.InnerException?.Message ?? string.Empty);
                    }
                }
            }
            else
            {
                result = ErrorNullableConnectionString();
            }
            return result;
        }

        public static async Task<ExecuteResult> ExecuteAsync(string queryText)
        {
            ExecuteResult result = new ExecuteResult();
            string? connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {

                    try
                    {
                        await connection.ExecuteAsync(queryText);
                    }
                    catch (Exception ex)
                    {
                        result = new ExecuteResult(ex.Message ?? string.Empty, ex.InnerException?.Message ?? string.Empty);
                    }
                }
            }
            else
            {
                result = new ExecuteResult("\"Не задана строка подключения!(appsettings.json => NSIConnection\"");
            }
            return result;
        }
    }

}
