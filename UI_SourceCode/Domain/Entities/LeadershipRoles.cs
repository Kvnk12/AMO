using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("LeadershipRoles")]
    public class LeadershipRoles
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int weightage { get; set; }
    }
}
