using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMIdeaTask")]
    public class IMIdeaTask : Entity
    {
        public int IdeaPostId { get; set; }
        public int OrderId { get; set; }
        public string TaskName { get; set; }
        public int? ParentId { get; set; }
        public int? status { get; set; }
        public int? ProgressStatus { get; set; }
        public decimal Completion { get; set; }
        public string Comments { get; set; }
        public int? PriorityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EstimatedStart { get; set; }
        public DateTime? EstimatedEnd { get; set; }
        public bool? Milestone { get; set; }
    }
}
