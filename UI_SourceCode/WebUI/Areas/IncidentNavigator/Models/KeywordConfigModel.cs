using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KeywordConfigModel
    {
        public int ConfigId { get; set; }
        //public string ConfigId { get; set; }
        public string WordValueID { get; set; }
        public string ByConfigItem { get; set; }
        public int CiWordIncidentCount { get; set; }
        public decimal CiWordIncidentPer { get; set; }
       
    }
}