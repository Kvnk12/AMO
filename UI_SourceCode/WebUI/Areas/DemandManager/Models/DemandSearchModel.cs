using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Models
{
    public class DemandSearchModel
    {
        public int DemandId { get; set; }
        public string DemandNumber { get; set; }
        public string DemandName { get; set; }
        public string Status { get; set; }
    }
}