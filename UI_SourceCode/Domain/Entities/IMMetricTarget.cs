using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMMetricTarget")]
    public class IMMetricTarget
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string MetricTarget { get; set; }
        public DateTime? CreatedDate { get; set; }       
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
