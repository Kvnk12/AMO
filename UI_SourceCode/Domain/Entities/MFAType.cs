using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("MFAType")]
    public class MFAType
    {
        public int Id { get; set; }
        public string AuthType { get; set; }
    }
}
