using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class Verticals : DelEntity
    {

        public string VerticalName { get; set; }
       // public int? ClientId { get; set; }
        public string Description { get; set; }
    }
}
