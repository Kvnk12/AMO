using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class RTPSubcriptionModel
    {
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
        public string ScheduleTime { get; set; }
        public int? WeekDay { get; set; }
        public int? MonthDay { get; set; }

        public string ReportFor { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Recipients { get; set; }
        public string EmailSubject { get; set; }

        public string Status { get; set; }
        public string ReportFormat { get; set; }

        public string selectHours { get; set; }
        public string selectMinutes { get; set; }

        public string selectPeriod { get; set; }
        public string CreatedByUser { get; set; }
        public string UpdatedByUser { get; set; }
        public DateTime JobScheduledDate { get; set; }
        public byte[] ReportDocument { get; set; }
        public string Timezone { get; set; }
        public int? AppID { get; set; }

    }

    public class RTPJobDetailModel
    {
        public int Id { get; set; }

        public string ReportName { get; set; }
        public string TelerikReportName { get; set; }

        public string ReportParameters { get; set; }
        public string ScheduleType { get; set; }

        public string ReportFor { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Recipients { get; set; }
        public string EmailSubject { get; set; }

        public string Status { get; set; }
        public string ReportFormat { get; set; }

        public DateTime JobScheduledDate { get; set; }
        public byte[] ReportDocument { get; set; }
        public string RPTJobUrl { get; set; }
       

    }
}