using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("AMSCustomAuditLog")]
    public class AMSCustomAuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int Uniqueid { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string PreviousValue { get; set; }
        public int ClientId { get; set; }

    }
}
