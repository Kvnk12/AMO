using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("IMIdeaApprover")]
    public class IMIdeaApprover
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public int ApproverId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? Createdby { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
