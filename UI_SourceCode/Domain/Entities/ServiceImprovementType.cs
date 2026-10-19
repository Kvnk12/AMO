using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Domain.Entities
{
    [Table("ServiceImprovementType")]
    public class ServiceImprovementType
    {
        public int Id { get; set; }
        public string ImprovementType { get; set; }
        public string Description { get; set; }
    }
}

