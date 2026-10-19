using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRAppHardware")]
   public class APRAppHardware
    {
        [Key]
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int HostId { get; set; }
        public string Environment { get; set; }

        public DateTime? AppDeployedDate { get; set; }
        public decimal? AppAge { get; set; }

        public string AppAllocatedStorage { get; set; }

        public string CPUConsumptionVolume { get; set; }
        public string CPUConsumptionUnit { get; set; }
        public decimal? AppAllocatedHardwareCost { get; set; }
        public string Comments { get; set; }

       
    }
}
