using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRInterfaceMethod")]
    public  class APRInterfaceMethod
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        
        public string InterfaceMethod { get; set; }


   
    }
}
