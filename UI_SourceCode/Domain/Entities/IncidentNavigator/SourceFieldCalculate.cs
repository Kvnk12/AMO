using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("SourceFieldCalculate")]
    public class SourceFieldCalculate
    {
        public int Id { get; set; }
        public int SourceDataMappingId { get; set; }
        public string SourceField { get; set; }
        public string Operator { get; set; }
        public decimal? Constant { get; set; }


        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public int? CreatedBy{ get; set; }
        public int? UpdatedBy { get; set; }

    }
}
