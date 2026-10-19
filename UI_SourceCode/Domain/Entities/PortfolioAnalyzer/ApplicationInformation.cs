using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("Application_1")]
    public class ApplicationInformation1
    {
        [Key]
        public int ID { get; set; }
        public string ClientApplicationNumber { get; set; }
        public string ApplicationName { get; set; }             
        public string AliasName { get; set; }
        public string BusinessFunction { get; set; }
        public string TechnicalOwner { get; set; }
       
        //public string ParentApplicationName { get; set; }
        public bool InScope { get; set; }
        public string Description { get; set; }
        
    
        public int AppTypeId{ get; set; }
        public int AppDomainId { get; set; }
        public int AppLifeCycleId { get; set; }
        public string IsTechnicalDocAvailable { get; set; }
        public string IsApplicationDocAvailable { get; set; }
        public int AppCriticalityId { get; set; }
        public int AppSecurityLevelId { get; set; }
        public string IsDisasterRecoveryPlanAvailable { get; set; }
        public string TransactionVolumn { get; set; }
        public string TransactionVolumnUnit { get; set; }
        public string DocumentationNotes { get; set; }
        public int AvgConcurrentUsers { get; set; }
        public int MaxConcurrentUsers { get; set; }
        public string OutageFrequency { get; set; }
        public string OutageDuration { get; set; }
        public bool RevenueImpactIfDown { get; set; }
        public string PerformanceNotes{ get; set; }
        public string GeneralNotes { get; set; }
        public int AppArchitectureId { get; set; }
        public int AppProcessingId { get; set; }
        public int AppSourceCodeId { get; set; }
        public string BusinessLogicInClientTier { get; set; }
        public string MOTSLevel { get; set; }
        public string NumberOfInstances { get; set; }
        public string AppSize { get; set; }
        public string SizeUnits { get; set; }
        public string FinancialNotes { get; set; }
        public int AppEnvironmentId { get; set; }
        public string RegionNames { get; set; }
        public int CountryId { get; set; }
        public DateTime MigrationDate { get; set; }
        public int AvgIncidentTickets { get; set; }
        public int AvgProblemTickets { get; set; }
        public int AvgServiceRequestTickets { get; set; }
        public int ClientId{ get; set; }
        public DateTime PlannedRetirementDate { get; set; }
        //public bool IsServer { get; set; }
        //public bool IsHostedExternally { get; set; }
        public int? ApplicationClassification { get; set; }
        public int? HostingTypeId { get; set; }
        public string LegacyReasonExists { get; set; }
        public int PrimaryRegionId { get; set; }
        
        public string TechnicalOwnerEmail { get; set; }
        public string OwningCompanies { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDatetime { get; set; }
        public bool IsDeleted { get; set; }
        public bool AppIsActive { get; set; }
        public string CodeComplexity { get; set; }
        public string NumberOfBugs { get; set; }
        public string NumberOfLinesOfCode { get; set; }
    }
}
