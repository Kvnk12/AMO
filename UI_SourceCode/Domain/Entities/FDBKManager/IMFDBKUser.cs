using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKUser")]
    public class IMFDBKUser
    {
        public int Id { get; set; }
        public int IMFDBKId { get; set; }
        public int UserRoleId { get; set; }
        public int? UserAction { get; set; }
        public int UserId { get; set; }
        public string ActionComment { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}