using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("SSIS_INUpdateQuery")]
    public class SSIS_INUpdateQuery : Entity
    {
       // [Key]
      //  public int Id { get; set; }
        public string Type { get; set; }
        public int ClientId { get; set; }
        public string Query { get; set; }
        public string TicketId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }

    }
}

