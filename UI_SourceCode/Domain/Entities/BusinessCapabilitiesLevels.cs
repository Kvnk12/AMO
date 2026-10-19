using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTTData.AMOPortal.Domain.Entities;

namespace NTTData.AMOPortal.Domain.Entities
{
  public  class BusinessCapabilitiesLevels : DelEntity
    {
        public int Level1Id { get; set; }
        public int Level2Id { get; set; }
        public int Level3Id { get; set; }
        public string Level1Name { get; set; }
        public string Level2Name { get; set; }
        public string Level3Name { get; set; }

    }
}
