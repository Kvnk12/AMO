using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("MetricActual")]
  public  class MetricActual : DelEntity
    {
        public int MetricId { get; set; }
        public int ClientId { get; set; }
        public int ApplicationId { get; set; }
        public double Actual { get; set; }
        public string Period { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comments { get; set; }
        public string MetricName { get; set; }
        
    }
}
