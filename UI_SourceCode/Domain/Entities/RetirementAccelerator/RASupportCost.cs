using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RASupportCost")]
    public class RASupportCost
    {
        public int Id { get; set; }
        public int RAApplicationId { get; set; }
        public int SupportActivityId { get; set; }
        public int? AnnualFTE { get; set; }
        public int? BlendedRate { get; set; }
        public int? AnnualSupportCost { get; set; }
        public int? FutureAnnualSupportFTE { get; set; }
        public int? FutureBlendedRate { get; set; }
        public int? FutureAnnualSupportCost { get; set; }
        //public int? AnnualFTESavings { get; set; }
        //public int? AnnualCostSavings { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? CreatedBy { get; set; }
    }
}
