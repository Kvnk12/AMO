using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRDeploymentModel")]
    public class APRDeploymentModel
    {
        [Key]
        public int Id { get; set; }
        public string DeploymentModel { get; set; }
        public int ClientId { get; set; }
    }
}
