using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRAppInterface")]
    public class APRAppInterface
    {
        [Key]
        public int Id { get; set; }
        public int AppId { get; set; }
      public int ApplicationInterfaced {get; set;}
      public string InterfaceType { get; set; }
        

    }
}
