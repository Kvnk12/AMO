using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
   public class BusinessCapabilitiesLevel2 : DelEntity
    {
        public string Level2 { get; set; }
        public int Level1Id { get; set; }
        [ForeignKey("Level1Id")]
        public BusinessCapabilitiesLevel1 BCLevel1 { get; set; }
        public string Description2 { get; set; }
        
    }
}