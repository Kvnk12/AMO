using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("Incident")]
    public class Incident : DelEntity
    {
        //public string IncidentId { get; set; }
        //public string RecordIncluded { get; set; }
        //public DateTime? CreatedDateTime { get; set; }
        //public DateTime? AcknowledgedDateTime { get; set; }
        //public DateTime? ResolutionDateTime { get; set; }
        //public DateTime? ServiceRestoredDateTime { get; set; }
        //public DateTime? LastUpdatedDateTime { get; set; }
        //public DateTime? UploadDate { get; set; }
        //public string Status { get; set; }
        //public string Description { get; set; }
        //public string Title { get; set; }
        //public string Priority { get; set; }
        //public string ImpactScope { get; set; }
        //public string Urgency { get; set; }
        //public string Type { get; set; }
        //public string OutageType { get; set; }
        //public string DivisionImpacted { get; set; }
        //public string BusinessUnitImpacted { get; set; }
        //public string ServiceCategory { get; set; }
        //public string ServiceSubcategory { get; set; }
        //public string ServiceArea { get; set; }
        //public string RegionImpacted { get; set; }
        //public string LocationsImpacted { get; set; }
        //public string CustomerDept { get; set; }
        //public string CustomerLastName { get; set; }
        //public string CustomerFirstName { get; set; }
        //public string CustomerTitle { get; set; }
        //public string CustomerSatisfaction { get; set; }
        //public string ImpactType { get; set; }
        //public string ImpactDescription { get; set; }
        //public string ConfigItemID { get; set; }
        //public string ConfigType { get; set; }
        //public string ConfigItemName { get; set; }
        //public string EnvironmentImpacted { get; set; }
        //public string Module { get; set; }
        //public string NodeName { get; set; }
        //public string RootCauseType { get; set; }
        //public string RootCauseDescription { get; set; }
        //public string ResolutionType { get; set; }
        //public string ResolutionDescription { get; set; }
        //public string SolutionID { get; set; }
        //public string DeploymentRelease { get; set; }
        //public string RelatedChangeIDs { get; set; }
        //public string RelatedIDs { get; set; }
        //public string RelatedProblemIDs { get; set; }
        //public string Helpdesk { get; set; }
        //public string AssignedToPerson { get; set; }
        //public string AssignedToSupportGroup { get; set; }
        //public string CreatedByGroup { get; set; }
        //public string CreatedByPerson { get; set; }
        //public string CreatedTimeZone { get; set; }
        //public string CreatedTime { get; set; }
        //public string CreatedWeekDay { get; set; }
        //public string CreatedWeekNum { get; set; }
        //public string CreatedMonth { get; set; }
        //public string CreatedYear { get; set; }
        //public string AcknowlededByGroup { get; set; }
        //public string AcknowlededByPerson { get; set; }
        //public string AcknowledgedTimeZone { get; set; }
        //public string AcknowledgedTime { get; set; }
        //public string AcknowledgedWeekDay { get; set; }
        //public string AcknowledgedWeekNum { get; set; }
        //public string AcknowledgedMonth { get; set; }
        //public string AcknowledgedYear { get; set; }
        //public string ResolvedByGroup { get; set; }
        //public string ResolvedByPerson { get; set; }
        //public string ResolvedByVendor { get; set; }
        //public string ResolutionEffortHours { get; set; }
        //public string ResolvedTimeZone { get; set; }
        //public string ResolutionTime { get; set; }
        //public string ResolutionWeekDay { get; set; }
        //public string ResolutionWeekNum { get; set; }
        //public string ResolutionMonth { get; set; }
        //public string ResolutionYear { get; set; }
        //public string ServiceRestoredByGroup { get; set; }
        //public string ServiceRestoredByPerson { get; set; }
        //public string ServiceRestoredTimeZone { get; set; }
        //public string ServiceRestoredTime { get; set; }
        //public string ServiceRestoredWeekDay { get; set; }
        //public string ServiceRestoredWeekNum { get; set; }
        //public string ServiceRestoredMonth { get; set; }
        //public string ServiceRestoredYear { get; set; }
        //public string LastUpdatedByPerson { get; set; }
        //public string ModificationNotes { get; set; }
        //public string DataSource { get; set; }
        //public string DataSourceNotes { get; set; }
        //public string UploadedBy { get; set; }
        //public int ClientId { get; set; }

        //public string Resolvedindicator { get; set; }
        //public string Waivedindicator { get; set; }

        //public string Assignedbygroup { get; set; }
        //public string Assignedbyperson { get; set; }
        //public DateTime Assigneddatetime { get; set; }
        //public DateTime Esitmatedresponsetimebygroup { get; set; }
        //public DateTime Esitmatedresponsedatetime { get; set; }
        //public DateTime Esitmatedresolutiondate { get; set; }
        //public int Totalsuspenddurationassignedtoack { get; set; }
        //public int Totalsuspenddurationacktoer { get; set; }
        //public int Totalsuspenddurationertoresolution { get; set; }

        //public string IncidentDescription { get; set; }
        //public string IncidentType { get; set; }
        //public string IncidentCreatedbyGroup { get; set; }
        //public string IncidentCustomerDepartment { get; set; }
        //public int ConfiguredItemId { get; set; }
        //public string ImpectType { get; set; }
        //public string ImpectScope { get; set; }
        //public string ImpectDescription { get; set; }



        public int ClientId { get; set; }

        public string IncidentId { get; set; }

        public string RecordIncluded { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Priority { get; set; }

        public string clientpriority { get; set; }

        public string Impactscope { get; set; }

        public string Urgency { get; set; }

        public string Type { get; set; }

        public string OutageType { get; set; }

        public string DivisionImpacted { get; set; }

        public string BusinessUnitImpacted { get; set; }

        public string ServiceCategory { get; set; }

        public string ServiceSubCategory { get; set; }

        public string ServiceArea { get; set; }

        public string RegionImpacted { get; set; }

        public string LocationsImpacted { get; set; }

        public string CustomerDept { get; set; }

        public string CustomerLastname { get; set; }

        public string CustomerFirstname { get; set; }

        public string CustomerTitle { get; set; }

        public string CustomerSatisfaction { get; set; }

        public string ImpactType { get; set; }

        public string ImpactDescription { get; set; }

        public string ConfigItemId { get; set; }

        public string ConfigType { get; set; }

        public string ConfigItemName { get; set; }

        public string EnvironmentImpacted { get; set; }

        public string Module { get; set; }

        public string NodeName { get; set; }

        public string RootCauseType { get; set; }

        public string RootcauseDescription { get; set; }

        public string ResolutionType { get; set; }

        public string ResolutionDescription { get; set; }

        public string SolutionId { get; set; }

        public string DeploymentRelease { get; set; }

        public string RelatedChangeIds { get; set; }

        public string RelatedIds { get; set; }

        public string RelatedProblemids { get; set; }

        public string Helpdesk { get; set; }

        public string AssignedToPerson { get; set; }

        public string AssignedToSupportGroup { get; set; }

        public string CreatedByGroup { get; set; }

        public string CreatedByPerson { get; set; }

        public string CreatedTimezone { get; set; }

        public string CreatedTime { get; set; }

        public string CreatedWeekday { get; set; }

        public string CreatedWeeknum { get; set; }

        public string CreatedMonth { get; set; }

        public string CreatedYear { get; set; }

        public string AcknowlededByGroup { get; set; }

        public string AcknowlededByPerson { get; set; }

        public DateTime? AcknowledgedDateTime { get; set; }

        public string AcknowledgedTimeZone { get; set; }

        public string AcknowledgedTime { get; set; }

        public string Acknowledgedweekday { get; set; }

        public string Acknowledgedweeknum { get; set; }

        public string Acknowledgedmonth { get; set; }

        public string Acknowledgedyear { get; set; }

        public string ResolvedByGroup { get; set; }

        public string Resolvedbyperson { get; set; }

        public string ResolvedByVendor { get; set; }

        public string ResolutionEfforthours { get; set; }

        public DateTime? ResolutionDateTime { get; set; }

        public string ResolvedTimezone { get; set; }

        public int? ResolutionTime { get; set; }

        public string ResolutionWeekday { get; set; }

        public string ResolutionWeeknum { get; set; }

        public string ServiceRestoredByGroup { get; set; }

        public string ServiceRestoredbyperson { get; set; }

        public DateTime? ServiceRestoredDateTime { get; set; }

        public string ServiceRestoredTimeZone { get; set; }

        public string ServiceRestoredTime { get; set; }

        public string ServiceRestoredWeekday { get; set; }

        public string ServiceRestoredWeeknum { get; set; }

        public string ServiceRestoredMonth { get; set; }

        public string ServiceRestoredYear { get; set; }

        public string LastUpdatedByPerson { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        public string ModificationNotes { get; set; }

        public string DataSource { get; set; }

        public string DataSourceNotes { get; set; }

        public DateTime? UploadDate { get; set; }

        public string UploadedBy { get; set; }

        public string AssignedByGroup { get; set; }

        public string AssignedByPerson { get; set; }

        public DateTime? AssignedDateTime { get; set; }

        public DateTime? EsitmatedResponseTimeByGroup { get; set; }

        public DateTime? EsitmatedResponseDateTime { get; set; }

        public string ResolutionYear { get; set; }

        public string ResolutionMonth { get; set; }

        public string Waivedindicator { get; set; }

        public DateTime? EsitmatedResolutionDate { get; set; }

        public int? TotalSuspendDurationAssignedtoack { get; set; }

        public int? TotalSuspendDurationacktoer { get; set; }

        public int? TotalSuspendDurationertoreSolution { get; set; }

        public string Resolvedindicator { get; set; }

      //  public int? JobID { get; set; }

        public string IsNTTGroup { get; set; }

        public string SupportGroupType { get; set; }

        public DateTime? InProgressDateTime { get; set; }

        public DateTime? ClosedDateTime { get; set; }

        public DateTime? ScheduledStartDateTime { get; set; }

        public DateTime? ScheduledEndDateTime { get; set; }

        public int? ScheduledChangeCount { get; set; }

        public string Company { get; set; }

        public string Site { get; set; }

        public string AssigneeCompany { get; set; }

        public string AssigneeOrganization { get; set; }

        public int? EstimatedWorkEffort { get; set; }

        public int? ActualWorkEffort { get; set; }

        public DateTime? ActualStartDateTime { get; set; }

        public int? Recyclecount { get; set; }

        public string StatusReason { get; set; }

        public string ConfigItemCategory { get; set; }

        public int? ResponseTime { get; set; }

        public string ResponseTimeMetSLA { get; set; }

        public string ResolutionTimeMetSLA { get; set; }

        public int? EstimateResponseTime { get; set; }

        public string ClosureCategory { get; set; }

        public string EstimatedResponseTimeMetSLA { get; set; }

        public string EstResptarget { get; set; }

        public string ReslntargetTime { get; set; }

        public DateTime? ResolutionCalcX { get; set; }

        public DateTime? ResolutionCalcY { get; set; }

        public string ResolutionExceptionIndicator { get; set; }

        public string ResolutionExceptionReason { get; set; }

        public DateTime? ResponseCalcX { get; set; }

        public DateTime? ResponseCalcY { get; set; }

        public string ResponseExceptionIndicator { get; set; }

        public string ResponseExceptionReason { get; set; }

        public string ResptargetTime { get; set; }

        public string Severity { get; set; }

        public string WorkGroupLevel { get; set; }

        public string AffectedItemName { get; set; }

        public decimal? SLACostPerTicket { get; set; }

        public int? INResolutionTime { get; set; }

        public decimal? INCostPerTicket { get; set; }

        public string ShortDescription { get; set; }

        public int AccountId { get; set; }

        public int ContractId { get; set; }

        ////public DateTime? NttCreatedDate { get; set; }

        ////public DateTime? NttUpdatedDate { get; set; }

        //newly added Properties by DP
        public string Active { get; set; }

        public DateTime? Activityduedatetime { get; set; }

        public DateTime? ActualenddateTime { get; set; }

        public string Additionalassigneelist { get; set; }

        public string Approval { get; set; }

        public string Approvalhistory { get; set; }

        public DateTime? Approvalsetdatetime { get; set; }

        public string Businessduration { get; set; }

        public string Closenotes { get; set; }

        public string Closedby { get; set; }

        public string Commentsworknotes { get; set; }

        public string Correlationdisplay { get; set; }

        public string CorrelationID { get; set; }

        public string Deliveryplan { get; set; }

        public string Deliverytask { get; set; }

        public string Domain { get; set; }

        public string Domainpath { get; set; }

        public string Escalation { get; set; }

        public DateTime? Followupdatetime { get; set; }

        public string Grouplist { get; set; }

        public string Knowledge { get; set; }

        public string Location { get; set; }

        public string MadeSLA { get; set; }

        public DateTime? Openeddatetime { get; set; }

        public string Openedby { get; set; }

        public int? Order { get; set; }

        public string ParentticketID { get; set; }

        public int? Reassignmentcount { get; set; }

        public DateTime? SLAduedatetime { get; set; }

        public string Tags { get; set; }

        public string Timeworked { get; set; }

        public int? Updatescount { get; set; }

        public string Uponapprovalstep { get; set; }

        public string Uponrejectstep { get; set; }

        public string Userinput { get; set; }

        public string Watchlist { get; set; }

        public string Worknotes { get; set; }

        public string Worknoteslist { get; set; }

        public string Businessresolvetime { get; set; }

        public string Caller { get; set; }

        public string Causedbychange { get; set; }

        public int? ChildIncidentscount { get; set; }

        public string Incidentstate { get; set; }

        public DateTime? Lastreopenedatdatetime { get; set; }
        
        public string Lastreopenedby { get; set; }

        public string Notify { get; set; }

        public string ParentIncidentID { get; set; }

        public string TotalResolvetime { get; set; }

        public string Knownerror { get; set; }

        public string Majorproblem { get; set; }

        public string Problemstate { get; set; }

        public int? RelatedIncidentscount { get; set; }

        public string Workaround { get; set; }

        public string Backoutplan { get; set; }

        public DateTime? CABdatetime { get; set; }

        public string CABdelegate { get; set; }

        public string CABrecommendation { get; set; }

        public string CABrequired { get; set; }

        public string Changeplan { get; set; }

        public string Closecode { get; set; }

        public DateTime? Conflictlastrundatetime { get; set; }

        public string Conflictstatus { get; set; }

        public string Implementationplan { get; set; }

        public string Onhold { get; set; }

        public string OnHoldChangeTasks { get; set; }

        public string Outsidemaintenanceschedule { get; set; }

        public string Phase { get; set; }

        public string Phasestate { get; set; }

        public string Productionsystem { get; set; }

        public string Reason { get; set; }

        public string Requestedby { get; set; }

        public DateTime? Requestedbydatetime { get; set; }

        public string Reviewcomments { get; set; }

        public DateTime? Reviewdatetime { get; set; }

        public int? Reviewstatus { get; set; }

        public string Risk { get; set; }

        public string Changerequestscope { get; set; }
        
        public string Changerequesttype { get; set; }

        public string Tower { get; set; }

        public string WaiveReason { get; set; }

    }
}