using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("BAMDegreeofSupport")]
    public class BAMDegreeofSupport
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string DOS { get; set; }
    }
}
