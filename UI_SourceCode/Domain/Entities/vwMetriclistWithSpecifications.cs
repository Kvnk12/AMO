using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
     [Table("vwMetriclistWithSpecifications")]
   public class vwMetriclistWithSpecifications
    {

        //public virtual Int64 Id { get; set; }
         public int PrimaryID { get; set; }
         public int? ParentID { get; set; }
         public int ID { get; set; }
         public string MetricName { get; set; }
         public string ComplianceTarget { get; set; }
         public int? ClientId { get; set; }
       

    }
}
