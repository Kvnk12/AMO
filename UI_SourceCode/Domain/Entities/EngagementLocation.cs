using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("EngagementLocation")]
    public class EngagementLocation
    {
        [Key]
        public int Id { get; set; }      
        public string Location { get; set; }
    }
}
