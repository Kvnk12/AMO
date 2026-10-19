using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.IncidentNavigator
{
    [Table("RPTSubscription")]

    public class RPTSubscription
    {
        [Key]
        public int Id { get; set; }
        public int? ClientID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ReportName { get; set; }
        public string TelerikReportName { get; set; }

        public string ReportParameters { get; set; }
        public string ScheduleType { get; set; }
        public TimeSpan? ScheduleTime { get; set; }
        public int? WeekDay { get; set; }
        public int? MonthDay { get; set; }

        public string ReportFor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Recipients { get; set; }
        public string EmailSubject { get; set; }

        public string Status { get; set; }
        public string ReportFormat { get; set; }

        public string Timezone { get; set; }
        public int? AppID { get; set; }
    }
}
