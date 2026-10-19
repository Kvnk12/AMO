using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DashboardDemandModel
    {
        public string Category { get; set; }
        public int Count { get; set; }
        public string Percent { get; set; }


        public List<DashboardDemandModel> DemandTypes { get; set; }
        public List<DashboardDemandModel> DemandStatus { get; set; }
        public List<DashboardDemandModel> DemandSource { get; set; }
        public List<DashboardDemandModel> DemandPosted { get; set; }
    }
}