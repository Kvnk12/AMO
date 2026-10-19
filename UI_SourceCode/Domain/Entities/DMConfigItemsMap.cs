using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("DMConfigItemsMap")]
    public class DMConfigItemsMap
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        public int? ClientId { get; set; }
        public int ConfigItemId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
