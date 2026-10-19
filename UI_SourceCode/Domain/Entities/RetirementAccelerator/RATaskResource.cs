using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RATaskResource")]
    public class RATaskResource : Entity
    {
        public int RAApplicationId { get; set; }
        public int TaskId { get; set; }
        public int ResourceId { get; set; }
        public decimal Units { get; set; }
    }
}
