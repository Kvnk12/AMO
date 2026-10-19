using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class CompReportModel
    {
        public string IncidentID { get; set; }
        public string Priority { get; set; }
        public DateTime? Createddate { get; set; }

      
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedGroup { get; set; }
        public string Tower { get; set; } 
        public string SLATarget { get; set; }
        public string PendingTime { get; set; }
        public string Age { get; set; }
        public string SLAStaus { get; set; }
        public DateTime? ResStartTime { get; set; }
        public DateTime? ResDueOn { get; set; }
        public string statusicon { get; set; }

        public string strCreateddate
        {
            get
            {
                if(Createddate!=null)
                return Createddate.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                else
                    return "";
            }
        }
        public string strResDueOn
        {
            get
            {
                if (ResDueOn != null)
                    return ResDueOn.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                else
                    return "";
            }
        }
        public string strResStartTime
        {
            get
            {
                if (ResStartTime!=null)
                return ResStartTime.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                else
                    return "";
            }
        }
    }
}