using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IMSystemImpactedService")]
    public class IMSystemImpactedService
    {
        public int Id { get; set; }
        public string SystemImpactedService { get; set; }
        public int? ClientId { get; set; }
    }
}
