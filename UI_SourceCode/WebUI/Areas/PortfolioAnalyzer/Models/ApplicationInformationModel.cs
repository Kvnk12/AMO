using Domain.Entities.ClientProfileManager;
using Domain.Entities.PortfolioAnalyzer;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class ApplicationInformationModel
    {
        ////public int Application_ID { get; set; }
        ////public string Application_Number { get; set; }        
        ////public string ApplicationName { get; set; }
        ////public string ApplicationAliasName { get; set; }
        ////public string Business_Owner_Name { get; set; }
        ////public string Technical_Owner_name { get; set; }
        ////public string ParentApplicationName { get; set; }
        ////public bool In_Scope { get; set; }
        ////public bool IsRetirementCandidate { get; set; }
        ////public int? Portfolio_ID { get; set; }
        ////public string profileName { get; set; }
        ////public Boolean? IsBusinessRatingCompleted { get; set; }
        ////public Boolean? IsTechnicalRatingCompleted { get; set; }

        //////lifecycle
        ////public string Applifecycle { get; set; }
        ////public string techdoc { get; set; }
        ////public string appdoc { get; set; }
        ////public string codecomplex { get; set; }
        ////public int bug { get; set; }
        ////public int noofline { get; set; }
        ////public string Appcriticality { get; set; }
        ////public string appsecurity { get; set; }        
        ////public string disasterrecovery { get; set; }
        ////public string appenvironment { get; set; }        

        //////Arch
        ////public string archtype { get; set; }
        ////public string appprocess{ get; set; }
        ////public string sourcecode { get; set; }
        ////public string businesslogic { get; set; }
        ////public string mots { get; set; }
        ////public string country { get; set; }

        public int Id { get; set; }
        public string ClientApplicationNumber { get; set; }
        public string ApplicationName { get; set; }
        public string AliasName { get; set; }
        public string BusinessFunction { get; set; }
        public string TechnicalOwner { get; set; }

        //public string ParentApplicationName { get; set; }
        public bool InScope { get; set; }
        public string Description { get; set; }


        public int? AppTypeId { get; set; }
        public int? AppDomainId { get; set; }
        public int? AppLifeCycleId { get; set; }
        public string IsTechnicalDocAvailable { get; set; }
        public string IsApplicationDocAvailable { get; set; }
        public int? AppCriticalityId { get; set; }
        public int? AppSecurityLevelId { get; set; }
        public string IsDisasterRecoveryPlanAvailable { get; set; }
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
        public int? AppArchitectureId { get; set; }
        public int? AppProcessingId { get; set; }
        public int? AppSourceCodeId { get; set; }
        public string BusinessLogicInClientTier { get; set; }
        public string MOTSLevel { get; set; }
        public string NumberOfInstances { get; set; }
        public string AppSize { get; set; }
        public string SizeUnits { get; set; }
        public string FinancialNotes { get; set; }
        public int? AppEnvironmentId { get; set; }
        public string RegionNames { get; set; }
        public int? CountryId { get; set; }
        public DateTime? MigrationDate { get; set; }
        public int? AvgIncidentTickets { get; set; }
        public int? AvgProblemTickets { get; set; }
        public int? AvgServiceRequestTickets { get; set; }
        public int? ClientId { get; set; }
        public DateTime? PlannedRetirementDate { get; set; }
        //public bool? IsServer { get; set; }
        //public bool? IsHostedExternally { get; set; }
        public int? ApplicationClassificationId { get; set; }
        public int? HostingTypeId { get; set; }
        public string LegacyReasonExists { get; set; }
        public int? PrimaryRegionId { get; set; }

        public string TechnicalOwnerEmail { get; set; }
      //  public string OwningCompanies { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? AppIsActive { get; set; }
        public string CodeComplexity { get; set; }
        public string NumberOfBugs { get; set; }
        public string NumberOfLinesOfCode { get; set; }
        public bool? IsRetirementCandidate { get; set; }
        public string[] Regionlist { 
            get
            {
                string[] regionlist = null;
                if (!string.IsNullOrEmpty(RegionNames))
                {
                    return regionlist= RegionNames.Split(',');
                }
                else
                   return regionlist;
            }
                }

        public IEnumerable<Vendor> Vendor { get; set; }
        //public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<VendorProduct> VendorProduct { get; set; }

        public IEnumerable<VendorProductVersion> VendorProductVersion { get; set; }
        public string UserRange { get; set; }
        public int? APRDeploymentModelId { get; set; }
        public int? APRStandardStatusId { get; set; }
        public int? APRDifferentiationLevelId { get; set; }
        public int? APROwningCompanyId { get; set; }
        public string[] CountriesId { get; set; }
        public string[] MultipleCountries { get; set; }
    }
}