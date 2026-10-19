using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities.A3MM
{
    [Table("A3MM_Catalog")]
    public class AMSCatalog
    {
        public int ID { get; set; }
        public string Catalog { get; set; }
    }
}
