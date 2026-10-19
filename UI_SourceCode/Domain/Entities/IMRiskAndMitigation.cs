using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IMRiskAndMitigation")]
    public class IMRiskAndMitigation
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public string Risk { get; set; }
        public string MitigationPlan { get; set; }
        public string Comments { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
