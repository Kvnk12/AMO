using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IMAutoApprovers")]
   public class IMAutoApprovers
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
