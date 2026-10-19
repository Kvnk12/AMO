using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("INClusterConfigurationKey")]
    public class INClusterConfigurationKey
    {
        public int ID { get; set; }
        public string ParameterName { get; set; }
        public string DisplayName { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public int MandatoryFlag { get; set; }
        public int DisplayFlag { get; set; }
        public int ActiveFlag { get; set; }
        public int MultiSelectFlag { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
