using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.TransformationAccelerator.Model
{
    public class EngagementType
    {
        [Key]
        public int Id { get; set; }
        public string EngagementTypes { get; set; }
        public int EngagementTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public enum EngType
    {
        ClientContract = 1,
        Transition = 2,
        Stabilization = 3,
        Baseline = 4,
        SteadyState=5,
            Renewal=6
    }
    public enum Location
    {
        Onshore = 1,
        Nearshore = 2,
        Offshore = 3      
    }
}