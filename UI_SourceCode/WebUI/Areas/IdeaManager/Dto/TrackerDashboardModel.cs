using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class TrackerDashboardModel
    {
        public int Clientid { get; set; }
        public int Id { get; set; }
        public string IdeaName { get; set; }
        public string LOBManager { get; set; }
        //public DateTime? IdeaDate { get; set; }
        public string IdeaDate { get; set; }

        public string ProgressStatusColor { get; set; }
        public string ProgressStatus { get; set; }
        //public DateTime? ActualStart { get; set; }
        //public DateTime? ActualEnd { get; set; }
        //public DateTime? PlannedStart { get; set; }
        //public DateTime? PlannedEnd { get; set; }
        public string ActualStart { get; set; }
        public string ActualEnd { get; set; }
        public string PlannedStart { get; set; }
        public string PlannedEnd { get; set; }

        public string Complexity { get; set; }
        public string Responsible { get; set; }
        public string Risk { get; set; }
        public string RiskMitigation { get; set; }
        //public int? CurrentPriority { get; set; }
        //public int? LastUpdatedPriority { get; set; }
        public string CurrentPriority { get; set; }
        public string LastUpdatedPriority { get; set; }


    }

    public class TasksDetails
    {
        public string T_Phase { get; set; }
        public string T_Status { get; set; }
        public DateTime? T_ActualStart { get; set; }
        public DateTime? T_ActualEnd { get; set; }
        public DateTime? T_PlannedStart { get; set; }
        public DateTime? T_PlannedEnd { get; set; }
        public decimal T_PercentCompleted { get; set; }
    }
    
}
