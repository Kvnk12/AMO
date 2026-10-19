using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("SlaCalSetup")]
    public class SlaCalSetup : Entity
    {
       // [Key]
        public string IFiledColValue { get; set; }
        public int IPriority { get; set; }
        public string InputFiled { get; set; }
        public int ClientId { get; set; }
    }
}
