using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class BusinessCapabilitiesLevel1 : DelEntity
    {
        public string Level1 { get; set; }
        public int SubVerticalId { get; set; }
        [ForeignKey("SubVerticalId")]
        public SubVerticals SubVertical { get; set; }
        public string Description1 { get; set; }
        public string InternalExternal { get; set; }
    }
}
