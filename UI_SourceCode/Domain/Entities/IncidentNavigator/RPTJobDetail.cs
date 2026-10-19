using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("RPTJobDetail")]
    public class RPTJobDetail
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
        public string ReportUID { get; set; }
        public string RPTJobUrl { get; set; }
        public DateTime? RPTStartDate { get; set; }
        public DateTime? RPTEndDate { get; set; }
    }
}
