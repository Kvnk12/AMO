using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("VendorProductVersion")]
   public class VendorProductVersion
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductVersionName { get; set; }
    }
}
