using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("BAMBusinessCapabilities")]
   public class BAMBusinessCapabilities 
    {

        public int Id { get; set; }
        public string BusinessCapName { get; set; }

        public int? SubVerticalId { get; set; }
        public int? ParentId { get; set; }
       
        public int? CreatedBy { get; set; }
      
        public DateTime? CreatedDate { get; set; }
        public string InternalExternal { get; set; }

    }
}
