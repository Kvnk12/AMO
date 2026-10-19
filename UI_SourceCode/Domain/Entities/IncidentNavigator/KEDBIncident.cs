using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.IncidentNavigator
{
    [Table("KEDBIncident")]
    public  class KEDBIncident
    {
        [Key] 
        public int Id { get; set; }
        public int KEDBID { get; set; }
        public int IncidentID { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool isAutoIdentified { get; set; }
    }
}
