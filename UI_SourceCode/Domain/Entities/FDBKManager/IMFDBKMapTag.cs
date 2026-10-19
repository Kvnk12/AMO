using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKMapTag")]
    public class IMFDBKMapTag
    {
        public int Id { get; set; }
        public int? FdbkId { get; set; }
        public int? Tagid { get; set; }
        public int ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
