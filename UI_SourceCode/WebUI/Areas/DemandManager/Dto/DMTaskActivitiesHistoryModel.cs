using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DMTaskActivitiesHistoryModel
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        public int? ClientId { get; set; }
        public string Task { get; set; }
        public string Stage { get; set; }
        public string Comments { get; set; }
        public string PostedBy { get; set; }
        public DateTime? PostedOn { get; set; }
        public string AssignedTo { get; set; }
    }
}

 