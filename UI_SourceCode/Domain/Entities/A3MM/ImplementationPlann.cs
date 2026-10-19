using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities.A3MM
{
    [Table("A3MM_ImplementationPlan")]
    public class ImplementationPlann
    {
        public int ID { get; set; }
        public string ImplementationPlan { get; set; }

    }
}
