using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Dto
{ 
    public class AMSCustomAuditLogModel
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int Uniqueid { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }

    }
}
