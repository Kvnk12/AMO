using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Models
{
    public class DMTaskActivitiesModel
    {
        public string DemandName { get; set; }
        public string TaskName { get; set; }
        public string opLead { get; set; }
        public string stDate { get; set; }
        public string taskDesc { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string imgurl { get; set; }
        public string Status { get; set; }
        public string uniqueid { get; set; }
    }
}