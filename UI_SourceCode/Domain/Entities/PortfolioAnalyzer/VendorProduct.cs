using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("VendorProduct")]
   public class VendorProduct
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string ProductName { get; set; }
    }
}
