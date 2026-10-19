using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class ScheduleJobsViewModel
    {
        public Guid JobId { get; set; }
        public string ApplicationName { get; set; }
        public int ClientId { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CurrentRunState { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string LastRunDate { get; set; }
        public string NextRrunDate { get; set; }
        //public string Command { get; set; }
        public string FrequencyType { get; set; }
        public string JobEnabled { get; set; }
        public TimeSpan ActiveStartTime { get; set; }

    }
}
