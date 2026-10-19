using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRRoadmapTargetApp")]
   public class APRRoadmapTargetApp
    {
        [Key]
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public string RoadmapTargetApp { get; set; }
        public int? APRRoadmapTargetAppId { get; set; }
        public int? APRDisId { get; set; }
    }
}
