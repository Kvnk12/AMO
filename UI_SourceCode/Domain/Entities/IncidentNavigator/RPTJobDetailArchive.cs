using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("RPTJobDetailArchive")]
    public class RPTJobDetailArchive
    {
        [Key]
        public int Id { get; set; }
        public int RPTSubscriptionId { get; set; }
        public byte[] ReportDocument { get; set; }
        public DateTime ScheduleDateTime { get; set; }
        public string Status { get; set; }
        public DateTime JobStartDateTime { get; set; }
        public DateTime? JobEndDateTime { get; set; }
        public int? FailureCount { get; set; }
        public string StatusDetail { get; set; }
        public DateTime AchivedDateTime { get; set; }
    }
}
