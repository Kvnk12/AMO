using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("v_DashboardOverallSummary")]

    public class v_DashboardOverallSummary 
    {
        public int ID { get; set; }
        public string MetricName { get; set; }
        public string CustomMetricName { get; set; }
        public string MetricSLAName { get; set; }
        public string CustomSLAName { get; set; }
        public string ComplianceTarget { get; set; }
        public int? Percentage { get; set; }
        public int? SLAID { get; set; }
        public bool? IsIncludeInDashboard { get; set; }
        public bool? IsSLA { get; set; }
    }
}
