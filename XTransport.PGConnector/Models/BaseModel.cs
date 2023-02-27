using XTransport.PGConnector.Collections;

namespace XTransport.PGConnector.Models
{
    public class BaseModel
    {
        [DataTableColumn("Id", WellknownDataTypes.SERIAL_PRIMARY_KEY)]
        public int Id { get; set; }

        [DataTableColumn("CreatedDate", WellknownDataTypes.TIMESTAMP_NOT_NULL_DEF_NOW)]
        public DateTime CreatedDate { get; set; }

        [DataTableColumn("IsDeleted", WellknownDataTypes.BOOL_NOT_NULL_DEF_F)]
        public bool IsDeleted { get; set; }
    }
}
