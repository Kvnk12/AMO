using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTTData.AMOPortal.Domain.Entities;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class ClientBCLevels : DelEntity
    {
        public int CLevel1Id { get; set; }
        public int CLevel2Id { get; set; }
        public int CLevel3Id { get; set; }
        public string CLevel1Name { get; set; }
        public string CLevel2Name { get; set; }
        public string CLevel3Name { get; set; }

    }
}
