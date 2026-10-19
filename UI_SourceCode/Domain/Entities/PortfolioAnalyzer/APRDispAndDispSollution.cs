using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRDispAndDispSollution")]
    public class APRDispAndDispSollution
    {
        [Key]
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int DispId { get; set; }
        public int DispSolutionId { get; set; }
    }
}
