using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKDgtlOfferingMgr")]
    public class IMFDBKDgtlOfferingMgr
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? DgtlOfferingId { get; set; }
        public bool IsDeleted { get; set; }
        public int RoleId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
