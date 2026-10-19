using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{

   [Table("vwclientinfo")]
    public class vwclientinfo : Entity
    {
       public int verticalid { get; set; }
       public int subverticalid { get; set; }
       public string clientName { get; set; }
       public string verticalname { get; set; }
       public string subverticalname { get; set; }
    }
}
