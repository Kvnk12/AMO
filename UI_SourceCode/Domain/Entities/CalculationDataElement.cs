using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("CalculationDataElement")]
    public class CalculationDataElement : DelEntity
    {
        public string ElementName { get; set; }
        public int CalculationId { get; set; }
        public string TableOfOrigin { get; set; }
        public string ColumnOfOrigin { get; set; }
        public int UniqueKeyId {get; set;}
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public DateTime RowVersion { get; set; }
    }
}
