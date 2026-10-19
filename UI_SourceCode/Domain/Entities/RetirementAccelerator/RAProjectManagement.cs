using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RAProjectManagement")]
    public class RAProjectManagement
    {
        public int Id { get; set; }
        public int RAApplicationId { get; set; }
        public int ProjectManagementActivityId { get; set; }
        public int? EstimatedHours { get; set; }
        public int? EstimatedCost { get; set; }
        public string EstimateAssumptions { get; set; }
        public byte[] Attachment { get; set; }
        public string AttachmentName { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? CreatedBy { get; set; }
    }
}
