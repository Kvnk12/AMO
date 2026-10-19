using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRStandardStatus")]
    public class APRStandardStatus
    {

        [Key]
        public int Id { get; set; }
        public string StandardStatus { get; set; }
        public int ClientId { get; set; }
    }
}
