using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class ExAppInventoryDataModel
    {

        public string ClientApplicationNumber { get; set; }
        public string ApplicationName { get; set; }
        public string AliasName { get; set; }
        public string BusinessFunction { get; set; }
        public string TechnicalOwner { get; set; }
        public string Description { get; set; }
        //public int? AppTypeId { get; set; }
        //public int? AppDomainId { get; set; }
        //public int? AppLifeCycleId { get; set; }
        public string IsTechnicalDocAvailable { get; set; }
        public string IsApplicationDocAvailable { get; set; }
        //public int? AppCriticalityId { get; set; }
        //public int? AppSecurityLevelId { get; set; }
        public string IsDisasterRecoveryPlanAvailable { get; set; }
        public string AppCriticality { get; set; }
        public string AppType { get; set; }
        public string TransactionVolumn { get; set; }
        public string TransactionVolumnUnit { get; set; }
        public string DocumentationNotes { get; set; }
        public int? AvgConcurrentUsers { get; set; }
        public int? MaxConcurrentUsers { get; set; }
        public string OutageFrequency { get; set; }
        public string OutageDuration { get; set; }
        public bool? RevenueImpactIfDown { get; set; }
        public string PerformanceNotes { get; set; }
        public string GeneralNotes { get; set; }
        //public int? AppArchitectureId { get; set; }
        //public int? AppProcessingId { get; set; }
        //public int? AppSourceCodeId { get; set; }
        public string BusinessLogicInClientTier { get; set; }
        public string MOTSLevel { get; set; }
        public string NumberOfInstances { get; set; }
        public string AppSize { get; set; }
        public string SizeUnits { get; set; }
        public string FinancialNotes { get; set; }
        //public int? AppEnvironmentId { get; set; }
        public string RegionNames { get; set; }
        //public int? CountryId { get; set; }
        public DateTime? MigrationDate { get; set; }
        public int? AvgIncidentTickets { get; set; }
        public int? AvgProblemTickets { get; set; }
        public int? AvgServiceRequestTickets { get; set; }
        //public int? ClientId { get; set; }
        public DateTime? PlannedRetirementDate { get; set; }
        //public bool? IsServer { get; set; }
        //public bool? IsHostedExternally { get; set; }
        public string ApplicationClassification { get; set; }
        public string ExternalHosting { get; set; }
        public string LegacyReasonExists { get; set; }
        //public int? PrimaryRegionId { get; set; }

        public string TechnicalOwnerEmail { get; set; }
        public int? OwningCompanies { get; set; }
        //  public string CreatedBy { get; set; }
        //public DateTime? CreatedDatetime { get; set; }
        //public string LastModifiedBy { get; set; }
        //public DateTime? LastModifiedDatetime { get; set; }
        // public bool? IsDeleted { get; set; }
        // public bool? AppIsActive { get; set; }
        public string CodeComplexity { get; set; }
        public string NumberOfBugs { get; set; }
        public string NumberOfLinesOfCode { get; set; }

        public int? AppId { get; set; }
        //public int? OriginalDispositionId { get; set; }
        //public int? PlannedDispositionId { get; set; }
        //public int? DispositionRationaleId { get; set; }
        //public int? DispositionActionId { get; set; }
        //public string RoadmapTargetApp { get; set; }
        public int? APRRoadmapTargetAppId { get; set; }
        public DateTime? OriginalRetirementDate { get; set; }
        public bool? Value { get; set; }
        public bool? TCO { get; set; }
        public bool? Risk { get; set; }
        public int? RoadmapTargetYear { get; set; }
        public string TargetedPlatformCapability { get; set; }
       // public int? RedundancyId { get; set; }
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
        public string Domain { get; set; }
        public string ArchitectureType { get; set; }
        public string Processing { get; set; }
        public string SourceType { get; set; }
        public string UserRange { get; set; }
        public string DeploymentModel { get; set; }
        public string StandardStatus { get; set; }
        public string DifferentiationLevel { get; set; }
        public string CountInboundInterfaces { get; set; }
        public string CountOutboundInterfaces { get; set; }
        public bool? EntangledInterface { get; set; }
        public string DispositionName { get; set; }
        public string LOBName { get; set; }
        public int Id { get; set; }


    }
}