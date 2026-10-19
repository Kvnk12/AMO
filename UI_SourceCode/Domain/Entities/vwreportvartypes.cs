using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{

   [Table("vwreportvartypes")]
    public class vwreportvartypes : Entity
    {
       public int ClientId { get; set; }
       public string VariableType { get; set; }
    }
}
