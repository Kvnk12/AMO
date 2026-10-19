using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
     [Table("UserActionTracking")]
    public class UserActionTracking //: DelEntity
    {
         //[Key]
         //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? userid { get; set; }
        public int? workingClient { get; set; }
        public string userrole { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Action { get; set; }
        public string IPAddress { get; set; }
        public string AdditionalDetail { get; set; }
        public DateTime ActionStartTime { get; set; }
        public DateTime ActionEndTime { get; set; }
        public string URLstring { get; set; }
    }
}
