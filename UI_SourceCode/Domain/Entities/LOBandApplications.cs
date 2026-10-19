using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTTData.AMOPortal.Domain.Entities;

namespace NTTData.AMOPortal.Domain.Entities
{
   public class LOBandApplications: DelEntity
    {
       public int ClientId { get; set; }

       public int LOBId { get; set; }
       public int AppId { get; set; }
       public string LOBName { get; set; }
       public string AppName { get; set; }
    }
}
