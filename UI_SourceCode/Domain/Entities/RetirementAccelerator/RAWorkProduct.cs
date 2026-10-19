using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RAWorkProduct")]
    public class RAWorkProduct
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string WorkProduct { get; set; }
        public string Url { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
