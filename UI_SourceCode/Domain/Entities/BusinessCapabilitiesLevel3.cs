using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
   public class BusinessCapabilitiesLevel3 : DelEntity
    {
        public string Level3 { get; set; }
        public int Level2Id { get; set; }
        [ForeignKey("Level2Id")]
        public BusinessCapabilitiesLevel2 BCLevel2 { get; set; }
        public string Description3 { get; set; }

    }
}
