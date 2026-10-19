using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("Personnel")]
   public class Personnel : Entity
    {
        public string PersonnelName { get; set; }
        public string EmailAddress  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
