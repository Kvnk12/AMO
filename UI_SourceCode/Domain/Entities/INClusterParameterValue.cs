using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("INClusterParameterValue")]
    public class INClusterParameterValue
    {
        public int ID { get; set; }
        public int KeyId { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public int SeqNo { get; set; }
        public string Description { get; set; }
        public int ActiveFlag { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
