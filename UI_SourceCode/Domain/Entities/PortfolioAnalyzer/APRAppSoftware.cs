using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRAppSoftware")]
   public class APRAppSoftware
    {
        [Key]
        public int Id { get; set; }
        public int? ApplicationId { get; set; }

        public int VendorId { get; set; }
        public int VendorProductId { get; set; }
        public int VendorProductVersionId { get; set; }
        public string LifeCycleStage { get; set; }
        public decimal? MaxNoLicenseUsed { get; set; }

        public int? LatestVersionInMarket { get; set; }
        public int? LastVersionSupported { get; set; }

        public decimal? TotalLicenseCost { get; set; }
        public string Comments { get; set; }

    }
}
