using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKClient")]
    public class IMFDBKClient
    {
        public int Id { get; set; }
        public int IMFDBKId { get; set; }
        public int InternalClientId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool isDeleted { get; set; }
    }
}
