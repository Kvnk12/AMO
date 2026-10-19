using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
     [Table("UserApplications")]
    public class UserApplications : DelEntity
    {
         //[Key]
         //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int  UserId { get; set; }
        public int ClientId { get; set; }
        public int RoleId { get; set; }
        public int  ApplicationId { get; set; }
      //  public bool isDeleted;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }


    }
}
