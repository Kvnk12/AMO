using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Models
{
    public class DMChartDashboardModel
    {

        public int DemandCount { get; set; }

        // [Display(Name = "Trend line")]
        public long TrendVal { get; set; }

        // [Display(Name = "Resolved Trend line")]
        public long ResolvedTrend { get; set; }

        // [Display(Name = "No. of  Resolved Tickets")]
        public int ResolvedCount { get; set; }

        //    [RegularExpression(@"^\d+(\.\d{1,2})$")]
        public decimal Percent { get; set; }
        public decimal AverageVal { get; set; }
        public string DrpCategory { get; set; }
        public string LongCategory { get; set; }
        public string Dim { get; set; }
    }
   
}