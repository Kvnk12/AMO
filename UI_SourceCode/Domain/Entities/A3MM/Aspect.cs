using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_Aspect")]
    public class Aspect
    {
        public int Id { get; set; }
       // public int ClientId { get; set; }
        public string AspectName { get; set; }
        public int A3_DisciplineId { get; set; }
        public int ComponentId { get; set; }
    }
}
