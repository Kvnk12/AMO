using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("DMCompletedAndNextSteps")]
    public class DMCompletedAndNextSteps
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        public int? ClientId { get; set; }
        public string CompletedSteps { get; set; }
        public string NextSteps { get; set; }
        public string Comments { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? ImpManagers { get; set; }
    }
}
