using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("DMTaskActivitiesHistory")]
    public class DMTaskActivitiesHistory
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        public int? ClientId { get; set; }
        public int TaskId { get; set; }
        public int StageId { get; set; }
        public string Comments { get; set; }
        public int? PostedBy { get; set; }
        public DateTime? PostedOn { get; set; }
        public string AssignedTo { get; set; }
    }
}
