using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("ServiceRequest")]
    public class ServiceReqeustDataSet : DelEntity
    {
        public int ClientId { get; set; }
        public string IncludeRecord { get; set; }
        public string ServiceRequestId { get; set; }
        public string ServiceRequestCategory { get; set; }
        public string ServiceRequestType { get; set; }
        public string ServiceRequestStatus { get; set; }
        public string ServiceRequestStage { get; set; }
        public string Urgency { get; set; }
        public string ImpactScope { get; set; }
        public string Priority { get; set; }
        public string ServiceRequestTitle { get; set; }
        public string ServiceRequestDescription { get; set; }
        public string Benefit { get; set; }
        public int? AnnualBenefitAmount { get; set; }
        public string Impactofnotdoing { get; set; }
        public string Manualworkaroundavailable { get; set; }
        public string RevenueImpact { get; set; }
        public string RevenueImpactSize { get; set; }
        public string RegualtoryImpact { get; set; }
        public string RegualtoryImpactSize { get; set; }
        public string PeopleAffectedCount { get; set; }
        public string DivisionImpacted { get; set; }
        public string BusinessUnitImpacted { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceSubcategory { get; set; }
        public string ServiceArea { get; set; }
        public string RegionsImpacted { get; set; }
        public string LocationsImpacted { get; set; }
        public string RelatedProjectId { get; set; }
        public string RelatedIncIdentId { get; set; }
        public string RelatedProblemId { get; set; }
        public string RelatedServiceRequestId { get; set; }
        public string RelatedAssetId { get; set; }
        public string EnvironmentImpact { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string CreatedByGroup { get; set; }
        public string AssignedToSupportGroup { get; set; }
        public DateTime? RequestedCompletionDate { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }
        public string MandatedCompletionDate { get; set; }
        public string RequestedByGroup { get; set; }
        public string RequestedForGroup { get; set; }
        public DateTime? AcknowledgedDateTime { get; set; }
        public string AcknowlededByGroup { get; set; }
        public DateTime? ROMEstimateActualCompletionDate { get; set; }
        public string ServiceRequestEstimateSize { get; set; }
        public int? ServiceRequestEstimatedHour { get; set; }
        public int? EACActualHours { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? ResolutionDateTime { get; set; }
        public string ResolvedByGroup { get; set; }
        public string RootCauseType { get; set; }
        public string RootCauseDescription { get; set; }
        public string ResolutionType { get; set; }
        public string ResolutionDescription { get; set; }
        public string SolutionId { get; set; }
        public string DeploymentReleaseName { get; set; }
        public string RelatedChangeId { get; set; }
        public string RelatedErrorCode { get; set; }
        public string PrimarySystemAffected { get; set; }
        public string ConfigItemId { get; set; }
        public string ConfigItemName { get; set; }
        public string ConfigType { get; set; }
        public int? DefectReleasedtoProductionCount { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public string CustomerSatisfaction { get; set; }
        public DateTime? ClosedDateTime { get; set; }
        public string ClosedByGroup { get; set; }
        public string DataSource { get; set; }
        public DateTime? UploadDate { get; set; }
        public string UploadedBy { get; set; }
        public string ModificationNotes { get; set; }
        public int? ECD_ACDYear { get; set; }
        public int? ECD_ACDMonth { get; set; }
    }
}

