using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKStatus")]
    public class IMFDBKStatus
    {
        public int Id { get; set; }
        public int IMFDBKId { get; set; }
        public int StatusId { get; set; }
        public int? PrevStatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
