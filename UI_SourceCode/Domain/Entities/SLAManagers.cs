using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("SLAManager")]
    public class SLAManager : DelEntity
    {
        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //public string MetricId { get; set; }
        public string MetricName { get; set; }
        public string Objective { get; set; }
        public string ObjectiveType { get; set; }
        public string Calculation { get; set; }
        public string Calculationdataelement { get; set; }
        public bool inUse { get; set; }
        public string Contextualelement { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceComponent { get; set; }
        public string MetricPerspective { get; set; }
        public string MetricType { get; set; }
        //public string ChartIndicators { get; set; }
        public string MetricUnits { get; set; }
        public string Rules { get; set; }
        public string PrimaryAudience { get; set; }
    }
}