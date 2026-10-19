using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRAppFinancial")]
    public class APRAppFinancial
    {
        [Key]
        public int Id { get; set; }
        public int AppId { get; set; }
        public int Year { get; set; }
        public decimal? DiscretionaryCostInternal { get; set; }
        public decimal? DiscretionaryCostExternal { get; set; }
        public double? DiscretionaryFTEInternal { get; set; }
        public double? DiscretionaryFTEExternal { get; set; }
        public decimal? EnhancementCostInternal { get; set; }
        public decimal? EnhancementCostExternal { get; set; }
        public double? EnhancementFTEInternal { get; set; }
        public double? EnhancementFTEExternal { get; set; }
        public decimal? SupportCostInternal { get; set; }
        public decimal? SupportCostExternal { get; set; }
        public double? SupportFTEInternal { get; set; }
        public double? SupportFTEExternal { get; set; }
        public decimal? OtherCostInternal { get; set; }
        public decimal? OtherCostExternal { get; set; }
        public double? OtherFTEInternal { get; set; }
        public double? OtherFTEExternal { get; set; }
        public decimal? InfrastructureCostInternal { get; set; }
        public decimal? InfrastructureCostExternal { get; set; }
        public decimal? LicensingCostInternal { get; set; }
        public decimal? LicensingCostExternal { get; set; }
        public decimal? BusinessCostInternal { get; set; }
        public decimal? BusinessCostExternal { get; set; }
        public double? BusinessFTEInternal { get; set; }
        public double? BusinessFTEExternal { get; set; }
        public string FinancialNotes { get; set; }

    }
}
