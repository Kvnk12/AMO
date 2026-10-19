using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.IncidentNavigator
{
    [Table("KEDBStatusAudit")]
    public  class KEDBStatusAudit
    {
        [Key] 
        public int Id { get; set; }
        public int KEDBID { get; set; }
        public int StatusId { get; set; }
        public int? PreviousStatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
