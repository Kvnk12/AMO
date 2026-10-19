using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("ClientEngagementFTE_Q")]
    public class ClientEngagementFTE_Q
    {
        [Key]
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int? LocationId { get; set; }
        public int? Year { get; set; }
        public string Quarter { get; set; }
        public decimal FTE { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
