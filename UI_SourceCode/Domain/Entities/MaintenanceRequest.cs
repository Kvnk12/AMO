using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("MaintenanceRequest")]
    public class MaintenanceRequest : DelEntity
    {
        public string IncidentId { get; set; }
        public string RecordIncluded { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? AcknowledgedDateTime { get; set; }
        public DateTime? ResolutionDateTime { get; set; }
        public DateTime? ServiceRestoredDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public DateTime? UploadDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public string ImpactScope { get; set; }
        public string Urgency { get; set; }
        public string Type { get; set; }
        public string OutageType { get; set; }
        public string DivisionImpacted { get; set; }
        public string BusinessUnitImpacted { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceSubcategory { get; set; }
        public string ServiceArea { get; set; }
        public string RegionImpacted { get; set; }
        public string LocationsImpacted { get; set; }
        public string CustomerDept { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerTitle { get; set; }
        public string CustomerSatisfaction { get; set; }
        public string ImpactType { get; set; }
        public string ImpactDescription { get; set; }
        public string ConfigItemID { get; set; }
        public string ConfigType { get; set; }
        public string ConfigItemName { get; set; }
        public string EnvironmentImpacted { get; set; }
        public string Module { get; set; }
        public string NodeName { get; set; }
        public string RootCauseType { get; set; }
        public string RootCauseDescription { get; set; }
        public string ResolutionType { get; set; }
        public string ResolutionDescription { get; set; }
        public string SolutionID { get; set; }
        public string DeploymentRelease { get; set; }
        public string RelatedChangeIDs { get; set; }
        public string RelatedIDs { get; set; }
        public string RelatedProblemIDs { get; set; }
        public string Helpdesk { get; set; }
        public string AssignedToPerson { get; set; }
        public string AssignedToSupportGroup { get; set; }
        public string CreatedByGroup { get; set; }
        public string CreatedByPerson { get; set; }
        public string CreatedTimeZone { get; set; }
        public string CreatedTime { get; set; }
        public string CreatedWeekDay { get; set; }
        public string CreatedWeekNum { get; set; }
        public string CreatedMonth { get; set; }
        public string CreatedYear { get; set; }
        public string AcknowlededByGroup { get; set; }
        public string AcknowlededByPerson { get; set; }
        public string AcknowledgedTimeZone { get; set; }
        public string AcknowledgedTime { get; set; }
        public string AcknowledgedWeekDay { get; set; }
        public string AcknowledgedWeekNum { get; set; }
        public string AcknowledgedMonth { get; set; }
        public string AcknowledgedYear { get; set; }
        public string ResolvedByGroup { get; set; }
        public string ResolvedByPerson { get; set; }
        public string ResolvedByVendor { get; set; }
        public string ResolutionEffortHours { get; set; }
        public string ResolvedTimeZone { get; set; }
        public string ResolutionTime { get; set; }
        public string ResolutionWeekDay { get; set; }
        public string ResolutionWeekNum { get; set; }
        public string ResolutionMonth { get; set; }
        public string ResolutionYear { get; set; }
        public string ServiceRestoredByGroup { get; set; }
        public string ServiceRestoredByPerson { get; set; }
        public string ServiceRestoredTimeZone { get; set; }
        public string ServiceRestoredTime { get; set; }
        public string ServiceRestoredWeekDay { get; set; }
        public string ServiceRestoredWeekNum { get; set; }
        public string ServiceRestoredMonth { get; set; }
        public string ServiceRestoredYear { get; set; }
        public string LastUpdatedByPerson { get; set; }
        public string ModificationNotes { get; set; }
        public string DataSource { get; set; }
        public string DataSourceNotes { get; set; }
        public string UploadedBy { get; set; }
        public int ClientId { get; set; }
        public string Waivedindicator { get; set; }
        public string Resolvedindicator { get; set; }

        public string Assignedbygroup { get; set; }
        public string Assignedbyperson { get; set; }
        public DateTime? Assigneddatetime { get; set; }
        public DateTime? Esitmatedresponsetimebygroup { get; set; }
        public DateTime? Esitmatedresponsedatetime { get; set; }
        public DateTime? Esitmatedresolutiondate { get; set; }
        public int? Totalsuspenddurationassignedtoack { get; set; }
        public int? Totalsuspenddurationacktoer { get; set; }
        public int? Totalsuspenddurationertoresolution { get; set; }


    }
}
