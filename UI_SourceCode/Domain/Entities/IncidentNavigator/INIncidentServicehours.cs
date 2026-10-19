using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("INIncidentServicehours")]
    public class INIncidentServicehours
    {
        [Key]
        public int Id { get; set; }

        public int ClientId { get; set; }

        public Boolean? HasSLAManager { get; set; }

        public string Priority { get; set; }
        public int? PriorityServicehours { get; set; }
        public int? WeekendHours { get; set; }
        public Decimal? BlendedRate { get; set; }

    }
}
