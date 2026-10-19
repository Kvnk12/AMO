using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{

   [Table("vwIMclasswithcat")]
    public class vwIMclasswithcat : Entity 
    {
        
        public int classificationid { get; set; }
        public string Classification { get; set; }
        public string category { get; set; }
        public int? clientid { get; set; }
    }
}
