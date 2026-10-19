using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class ScheduleJob
    {
        public Guid JobId { get; set; }
        public string ApplicationName { get; set; }
        public int ClientId { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CurrentRunState { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime LastRunDate { get; set; }
        public DateTime NextRrunDate { get; set; }
        //public string Command { get; set; }
        public string FrequencyType { get; set; }
        public DateTime ActiveStarteDate { get; set; }
        public TimeSpan ActiveStartTime { get; set; }
    }
}