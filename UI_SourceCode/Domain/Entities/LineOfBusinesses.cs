using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
  public  class LineOfBusinesses:DelEntity
    {
      public string LOBName { get; set; }
      public int ClientId { get; set; }
      [ForeignKey("ClientId")]
      public Client Clients { get; set; }
    }
}
