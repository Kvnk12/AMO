using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("TicketInfo")]
    public class TicketInfo
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime Todate { get; set; }
        public int TicketInfoTypeId { get; set; }
        public decimal Incidents { get; set; }
        public decimal Problems { get; set; }
        public decimal UserSupportSR { get; set; }
        public decimal Enhancements { get; set; }
        public string Comments { get; set; }
    }
}