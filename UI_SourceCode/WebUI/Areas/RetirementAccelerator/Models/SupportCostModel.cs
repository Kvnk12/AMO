using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class SupportCostModel
    {
        public int Id { get; set; }
        public int RAApplicationId { get; set; }
        public int SupportActivityId { get; set; }
        public string SupportActivity { get; set; }
        public int? AnnualFTE { get; set; }
        public int? BlendedRate { get; set; }
        public int? AnnualSupportCost { get; set; }
        public int? FutureAnnualSupportFTE { get; set; }
        public int? FutureBlendedRate { get; set; }
        public int? FutureAnnualSupportCost { get; set; }
        public int? AnnualFTESavings { get; set; }
        public int? AnnualCostSavings { get; set; }
    }
}