using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Domain.Entities
{
    [Table("ServiceImprovementPlanStatus")]
    public class ServiceImprovementPlanStatus
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public string Status { get; set; }

        public DateTime Modified_Date { get; set; }

        public int Modified_By { get; set; }

    }
}
