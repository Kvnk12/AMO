using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRAppProcessingType")]
    public class APRAppProcessingType
    {
        [Key]
        public int Id { get; set; }
        public string ProcessingType { get; set; }
        public int ClientId { get; set; }
    }
}
