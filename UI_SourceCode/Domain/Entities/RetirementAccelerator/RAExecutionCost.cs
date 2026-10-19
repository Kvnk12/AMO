using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RAExecutionCost")]
    public class RAExecutionCost
    {
        public int Id { get; set; }
        public int RAApplicationId { get; set; }
        public int ExecutionActivityId { get; set; }
        public int? EstimatedHours { get; set; }
        public int? BlendedRate { get; set; }
        public int? EstimatedCost { get; set; }
        public int? EstimatedDuration { get; set; }
        public int? EstimatedFTE { get; set; }
        public Byte[] Attachment { get; set; }
        public string AttachmentName { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? CreatedBy { get; set; }
    }   
}
