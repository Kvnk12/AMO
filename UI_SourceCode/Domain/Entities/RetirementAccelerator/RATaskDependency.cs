using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    
    [Table("RATaskDependency")]
    public class RATaskDependency : Entity
    {
        public int RAApplicationId { get; set; }
        public int? PredecessorId { get; set; }
        public int? SuccessorId { get; set; }
        public int? type { get; set; }
    }
}
