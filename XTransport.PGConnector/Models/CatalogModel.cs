using XTransport.PGConnector.Collections;

namespace XTransport.PGConnector.Models
{
    public class CatalogModel : BaseModel, ICatalogModel
    {

        [DataTableColumn("Name", WellknownDataTypes.VARCHAR_100_NOT_NULL)]
        public string? Name { get; set; }

        [DataTableColumn("Description", WellknownDataTypes.VARCHAR_255)]
        public string? Description { get; set; }


        public string AddRecordQueryText<T>() where T : class, new()
        {
            List<string> queryText = new List<string>();
            List<string> valuesText = new List<string>();
            valuesText.Add("VALUES(");
            T model = new T();
            var type = model.GetType();
            foreach (DataTable item in type.GetCustomAttributes(typeof(DataTable), true))
            {
                queryText.Add($"INSERT INTO {item.Name}(");
            }
            foreach (var prop in type.GetProperties())
            {
                object[] columns = prop.GetCustomAttributes(typeof(DataTableColumn), true);
                foreach (DataTableColumn column in columns)
                {
                    if (prop.Name == "Id")
                    {
                        continue;
                    }
                    queryText.Add($"{prop.Name}");
                    valuesText.Add($"@{prop.Name}");
                    queryText.Add(",");
                    valuesText.Add(",");
                }
            }
            queryText.RemoveAt(queryText.Count - 1);
            valuesText.RemoveAt(valuesText.Count - 1);
            queryText.Add(") ");
            valuesText.Add(");");
            queryText.AddRange(valuesText);

            return string.Join("", queryText);
        }

        public string RecordsQueryText<T>() where T : class, new()
        {
            List<string> queryText = new List<string>();

            T model = new T();
            var type = model.GetType();
            foreach (DataTable item in type.GetCustomAttributes(typeof(DataTable), true))
            {
                queryText.Add($"SELECT * FROM {item.Name} ORDER BY id ASC;");
            }
            return string.Join("", queryText);
        }

        public string RecordsQueryText<T>(int id) where T : class, new()
        {
            List<string> queryText = new List<string>();

            T model = new T();
            var type = model.GetType();
            foreach (DataTable item in type.GetCustomAttributes(typeof(DataTable), true))
            {
                queryText.Add($"SELECT * FROM {item.Name} WHERE id={id};");
            }
            return string.Join("", queryText);
        }

        public string RecordsQueryText<T>(List<int> items) where T : class, new()
        {
            List<string> queryText = new List<string>();

            T model = new T();
            var type = model.GetType();
            foreach (DataTable item in type.GetCustomAttributes(typeof(DataTable), true))
            {
                queryText.Add($"SELECT * FROM {item.Name} WHERE id in ({string.Join(",", items)}) ORDER BY id ASC;");
            }
            return string.Join("", queryText);
        }

        public string UpdateRecordQueryText<T>() where T : class, new()
        {
            List<string> queryText = new List<string>();
            T model = new T();
            var type = model.GetType();
            foreach (DataTable item in type.GetCustomAttributes(typeof(DataTable), true))
            {
                queryText.Add($"UPDATE {item.Name} SET ");
            }
            foreach (var prop in type.GetProperties())
            {
                object[] columns = prop.GetCustomAttributes(typeof(DataTableColumn), true);
                foreach (DataTableColumn column in columns)
                {
                    if (prop.Name == "Id")
                    {
                        continue;
                    }
                    queryText.Add($"{prop.Name}=@{prop.Name}");
                    queryText.Add(",");
                }
            }
            queryText.RemoveAt(queryText.Count - 1);
            queryText.Add(" WHERE id=@Id;"); ;

            return string.Join("", queryText);
        }
    }
}
