using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RATask")]
    public class RATask
    {
        public int Id { get; set; }
        public int RAApplicationId { get; set; }
        public string TaskName { get; set; }
        public int? ParentId { get; set; }
        public int OrderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Completion { get; set; }
        public string Comments { get; set; }
        //public int? PriorityId { get; set; }
        public decimal? EstimatedEfforts { get; set; }
        public decimal? ActualEfforts { get; set; }
        public int? ProgressStatus { get; set; }
        public int? WorkProductId { get; set; }
        public int? RoleId { get; set; }
        //public int? ResourceId { get; set; }
        public int? ResourceGroupId { get; set; }
        public bool? Milestone { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }


    }
    
}
