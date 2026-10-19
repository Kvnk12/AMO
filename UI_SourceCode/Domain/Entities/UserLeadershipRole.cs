using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("UserLeadershipRole")]
   public class UserLeadershipRole
    {        
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public int LeadershipRoleId { get; set; }
        public int ShoreId { get; set; }
        public string ReportingManager { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
