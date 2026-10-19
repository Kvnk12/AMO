using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
   public class ApplicationLOBJunctions:DelEntity
    {
       public int AppId { get; set; }
       [ForeignKey("AppId")]
       public virtual Application Applications { get; set; }
       public int LOBId { get; set; }
       [ForeignKey("LOBId")]
       public virtual LineOfBusinesses lineOfBusiness { get; set; }
    }
}
