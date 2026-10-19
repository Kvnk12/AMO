using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("TicketInfoType")]
    public class TicketInfoType
    {

        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        
    }
}
