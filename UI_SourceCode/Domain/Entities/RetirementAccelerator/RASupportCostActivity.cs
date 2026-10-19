using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RASupportCostActivity")]
    public class RASupportCostActivity
    {
        public int Id { get; set; }
        public string Activity { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
