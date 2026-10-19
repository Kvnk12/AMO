using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.IncidentNavigator
{
    [Table("ConfigAgingQuery")]
    public class ConfigAgingQuery
    {
        [Key]
        public int ID { get; set; }
        public int? SeqNo { get; set; }
        public string Titlevalue { get; set; }
        public string Datetype { get; set; }
        public string StartValue { get; set; }
        public int? EndValue { get; set; }
        public int? ClientID { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
      
    }
}
