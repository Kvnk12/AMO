using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("EstimateCategory")]
    public class EstimateCategories
    {
        public int Id { get; set; }
        public string EstimateCategory { get;set;}
       // public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
       // public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
       // public DateTime? UpdatedDt { get; set; }
    }
}
