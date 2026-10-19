using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IMConfigItem")]
    public class IMConfigItem
    {
        public int Id { get; set; }
        public int ConfigTypeId { get; set; }
        public string ConfigItem { get; set; }
        public int? ClientId  { get;set;}
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
