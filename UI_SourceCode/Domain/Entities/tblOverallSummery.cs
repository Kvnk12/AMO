using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
      [Table("tblOverallSummery")]
  public  class tblOverallSummery
    {
public int ID { get; set; }
public string OriginalMetricName { get; set; }
public string CustomMetricName { get; set; }
public string OriginalSLAName { get; set; }
public string CustomSLAName { get; set; }
public string Target { get; set; }
public string CurrentPercentage { get; set; }
public int SLAID { get; set; }
public bool? IsIncludeInDashboard { get; set; }
public bool? IsSLA { get; set; }
    }
}
