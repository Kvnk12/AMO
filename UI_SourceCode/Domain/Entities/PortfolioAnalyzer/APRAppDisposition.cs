using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRAppDisposition")]
    public class APRAppDisposition
    {
        [Key]
        public int Id { get; set; }
       // [ForeignKey]
        public int? AppId { get; set; }
        public int? OriginalDispositionId { get; set; }
        public int? PlannedDispositionId { get; set; }
        public int? DispositionRationaleId {get; set;}
        public int? DispositionActionId { get; set; }
        //public string RoadmapTargetApp { get; set; }
        public int? APRRoadmapTargetAppId { get; set; }
        public DateTime? OriginalRetirementDate { get; set; }
        public bool Value { get; set; }
        public bool TCO { get; set; }
        public bool Risk { get; set; }
        public int? RoadmapTargetYear { get; set; }
        public string TargetedPlatformCapability { get; set; }
        public int? RedundancyId { get; set; }
        public string Analysis { get; set; }
        public string Recommendation { get; set; }
        public int? FTEImpact { get; set; }
        public int? ITDispostionSupportFTEs { get; set; }
        public int? ITDispostionDevFTEs { get; set; }
        public decimal? VendorDisCommercialLicenceSavings { get; set; }
        public decimal? VendorDisSupportSavings { get; set; }
        public int? BusinessDisSupportFTEs { get; set; }
        public decimal? HardwareDisSupportSavings { get; set; }
        public int? HardwareDisAmountofCPUFreed { get; set; }
        public string OperationalEfficiencyUnits { get; set; }
        public int? OperationalEfficiencyAmount { get; set; }
        public DateTime? PlannedDispositionDate { get; set; }
        public int? DispositionEfforts { get; set; }
        public int? DispositionDuration { get; set; }
        public int? ITDispositionEffort { get; set; }
        public int? VendorDispositionEffort { get; set; }
        public int? BusinessDispositionEffort { get; set; }
        public decimal? ITDispositionRate { get; set; } 
        public decimal? VendorDispositionRate { get; set; }
        public decimal? BusinessDispositionRate { get; set; }

        public decimal? ITDispositionCost { get; set; }
        public decimal? VendorDispositionCost { get; set; }
        public decimal? BusinessDispositionCost { get; set; }

        public int? AutomatedDispId { get; set; }
        public int? AutomatedDispSolutionId { get; set; }
        public int? PlannedDispSolutionId { get; set; }
        //public int? APRRoadmapTargetAppId { get; set; }
        public string ReplacementAlternatives { get; set; }
    }
}
