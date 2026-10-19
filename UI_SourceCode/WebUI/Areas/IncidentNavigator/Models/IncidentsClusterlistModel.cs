using Domain.Entities.ClientProfileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class IncidentsClusterlistModel
    {
        [Key]
        public int PrimaryKeyIncidentId { get; set; }
        public string Incidentid { get; set; }
        [Required]
        public int ClusterIDfromCluster { get; set; }

        //  [Required]
        [MaxLength(256)]
        public string ClusterLablefromCluster { get; set; }

        public string DescriptionfromIncident { get; set; }

        public string Priority { get; set; }

        public int? Resolutiontime { get; set; }

        public string elapsedtime { get; set; }

        public string ClosureCategory { get; set; }

        public string Severity { get; set; }

        public string AffectedItemName { get; set; }

        public string Assignedtosupportgroup { get; set; }
        public string Type { get; set; }
        public string ConfigItemName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ResolutionDateTime { get; set; }
        public DateTime ClosedDateTime { get; set; }
        public string ResolutionDescription { get; set; }
    }
}