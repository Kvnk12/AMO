using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("INAssigneeGroup")]
    public class INAssigneeGroup
    {
        public int Id { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int TowerId { get; set; }
        public string AssignGroup { get; set; }
    }
}
