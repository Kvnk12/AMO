using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("INAuditTable")]
    public class INAuditTable
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public string IPAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string IncidentId { get; set; }
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Comments { get; set; }

        
    }
}
