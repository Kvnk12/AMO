using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class JunctionAppLOBClientBCs : DelEntity
    {
        public int AppId { get; set; }
        [ForeignKey("AppId")]
        public virtual Application application { get; set; }
        public int LOBClientBCId { get; set; }
        [ForeignKey("LOBClientBCId")]
        public virtual JunctionLOBClientBCs junctionLOBClientBCs { get; set; }
    }
}
