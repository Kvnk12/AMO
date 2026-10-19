using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class SubVerticals : DelEntity
    {
        public string SubVerticalName { get; set; }
        public int VerticalId { get; set; }
       // [ForeignKey("VerticalId")]
        public Verticals Vertical { get; set; }
        public string Description { get; set; }
    }
}