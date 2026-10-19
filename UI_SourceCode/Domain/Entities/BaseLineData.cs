using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("BaseLineData")]

    public class BaseLineData
    {
        [Key]
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public string EngBaseLinePeriod { get; set; }
        public int MonthlyINCount { get; set; }
        public int MonthlyEventCount { get; set; }
        public int MonthlySRCount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
