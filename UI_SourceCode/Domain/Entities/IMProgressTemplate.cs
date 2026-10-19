using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IMProgressTemplate")]
    public class IMProgressTemplate
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Progress { get; set; }
        public int Phase { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Updateddate { get; set; }
    }
}
