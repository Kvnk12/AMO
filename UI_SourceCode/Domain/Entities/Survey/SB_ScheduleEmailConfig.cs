using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Survey
{
    [Table("SB_ScheduleEmailConfig")]
    public class  SB_ScheduleEmailConfig
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int Survey_ID { get; set; }      
        public string Ack_Message { get; set; }
        public string FirstReminder_Message { get; set; }
        public string SecondReminder_Message { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public int? updatedBy { get; set; }
    }
}
