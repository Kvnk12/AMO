using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("IMChallengeApprover")]
    public class IMChallengeApprover
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public int ApproverId { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
