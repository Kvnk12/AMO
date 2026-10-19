using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKActivityLogs")]
    public class IMFDBKActivityLogs
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ActionDetail { get; set; }
        public int clientId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
