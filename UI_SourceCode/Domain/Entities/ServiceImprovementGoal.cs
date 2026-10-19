using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Domain.Entities
{
    [Table("ServiceImprovementGoal")]
    public class ServiceImprovementGoal
    {
        public int Id { get; set; }
        public int? ImprovementTypeId { get; set; }
        public decimal Goal { get; set; }
        public string ImprovementNotes { get; set; }
        public int? ClientId { get; set; }
    }
}
