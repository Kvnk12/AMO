using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_Component")]
    public class Component
    {
        public int Id { get; set; }
       // public int ClientId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDefinition { get; set; }
        public int DisciplineId { get; set; }
        public string ComponentAbbrevation { get; set; }
    }
}
