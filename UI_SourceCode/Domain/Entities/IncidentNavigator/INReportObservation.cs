using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("INReportObservation")]
    public class INReportObservation
    {
        [Key]
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int ClientId { get; set; }
        public int userId { get; set; }
        public string Observation { get; set; }
        public string ObservationType { get; set; }
        public string DateRange { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}