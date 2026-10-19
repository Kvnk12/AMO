using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("SlaFieldCol")]
    public class SlaFieldCol : Entity
    {
       // [Key]
        public string IFiledColValue { get; set; }
    }
}
