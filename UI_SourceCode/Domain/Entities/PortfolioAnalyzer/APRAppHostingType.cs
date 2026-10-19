using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Domain.Entities
{

    [Table("APRAppHostingType")]
    public class ExternalHosting
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string HostingTypeName { get; set; }
    }
}
