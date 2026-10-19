using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRAppSecurity")]
    public class APRAppSecurity
    {
        [Key]
        public int Id { get; set; }
        public string SecurityLevelName { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
    }
}
