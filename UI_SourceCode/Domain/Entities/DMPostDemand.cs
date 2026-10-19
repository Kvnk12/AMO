using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("DMPostDemand")]
    public class DMPostDemand : DelEntity
    {
        public int ClientId { get; set; }
        public string ItemName { get; set; }
        public int? CreatedBy { get; set; }
        public decimal? QtyFication { get; set; }
        public int DemandTypeId { get; set; }
        public string DemandSource { get; set; }
        public int? LOBId { get; set; }
        public string Description { get; set; }
        public int ItemTypeId { get; set; }
        public int? PriorityId { get; set; }
        public int StatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string CreateDateString
        {
            get
            {
                return (string.Format("{0:dd-MMM-yyyy hh:mm}", CreatedDate));
            }
        }
        public int? UpdatedBy { get; set; }
        public bool InternalInitiative { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int? ProgressStatus { get; set; }
        public DateTime? StatusDt { get; set; }
        public int? ImpStatusId { get; set; }
        public DateTime? ImpStatusDt { get; set; }
        public DateTime? LastReviewDt { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDt { get; set; }
        public int? ImplementationManager { get; set; }
        public DateTime? CompletedDt { get; set; }
        public int? ImpPriorityId { get; set; }
        public string DemandNumber { get; set; }
        public decimal? ImplementationProgress { get; set; }
        public string ImprovementTheme { get; set; }
        public bool MultipleApprovals { get; set; }
        public string StatusComment { get; set; }
        public int? ComplexityId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string DocumentNo { get; set; }
        public string EstimatedCost { get; set; }
        public string DemandPlatformLead { get; set; }
        public string Approved_Arch { get; set; }
    }
}