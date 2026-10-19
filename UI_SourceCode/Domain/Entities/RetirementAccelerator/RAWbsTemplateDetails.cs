using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RAWbsTemplateDetails")]
    public class RAWbsTemplateDetails
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int? ParentId { get; set; }
        public int WBSTemplateId { get; set; }
        public string Task { get; set; }
        public int? WorkProductId { get; set; }
        public int? RoleId { get; set; }
        public int? ResourceGroupId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
