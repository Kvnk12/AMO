using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRApplicationInterfaceMethod")]
    public class APRApplicationInterfaceMethod
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int? InterfaceMethodId { get; set; }
        public int? AppId { get; set; }
       
    }
}
