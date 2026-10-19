using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KeywordIncidentModel
    {
        public int Id { get; set; }
        //public string Id { get; set; }
        public string Incidentid { get; set; }
        //public string incid { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string ConfigItem { get; set; }
        public string CreatedDate { get; set; }
        public string ResolvedDate { get; set; }
        public string ResolutionDescription { get; set; }
        
    }
}