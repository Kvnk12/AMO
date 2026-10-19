using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IMCompletedAndNextSteps")]
    public class IMCompletedAndNextSteps
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public string CompletedSteps { get; set; }
        public string NextSteps { get; set; }
        public string Comments { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public int? ImpManagers { get; set; }    
    }
}
