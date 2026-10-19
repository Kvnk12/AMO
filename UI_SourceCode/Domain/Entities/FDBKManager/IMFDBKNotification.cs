using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKNotification")]
    public class IMFDBKNotification
    {
        public int Id { get; set; }
        public int IMFDBKId { get; set; }
        public int NotificationUser { get; set; }
        public int NotificationTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
