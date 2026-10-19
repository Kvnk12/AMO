using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("PersonnelRole")]
    public class PersonnelRole
    {
        [Key]
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int? AppId { get; set; }
        public int? ClientId { get; set; }
        public string Title { get; set; }
        public string IsSurveyParticipant { get; set; }
      
        public Boolean? ReceiveEmailNotification { get; set; }
        

    }
}
