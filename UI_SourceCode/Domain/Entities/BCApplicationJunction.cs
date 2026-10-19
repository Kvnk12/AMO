using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("BCApplicationJunction")]
    public class BCApplicationJunction 
    {
       public int Id { get; set; }
       public int AppId { get; set; }
       public int BCId { get; set; }
       public int? DoSId { get; set; }
      //  public int? oldDoSId { get; set; }
        public int? PostDoSId { get; set; }
    }
}
