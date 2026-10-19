using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("INDictionary")]
    public class INDictionary 
    {
        public int Id { get; set; }
        public string Word { get; set; }
        
        public int? ClientId { get; set; }
        public int? ExcludedBy { get; set; }
        public DateTime? ExcludedDate { get; set; }
    }
}
