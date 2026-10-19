using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class JunctionLOBClientBCs : DelEntity
    {
        public int LOBId { get; set; }
        [ForeignKey("LOBId")]
        public virtual LineOfBusinesses lineOfBusiness { get; set; }
        public int? CBCLevel1Id { get; set; }
        [ForeignKey("CBCLevel1Id")]
        public virtual ClientBCLevel1 clientBCLevel1 { get; set; }
        public int? CBCLevel2Id { get; set; }
        [ForeignKey("CBCLevel2Id")]
        public virtual ClientBCLevel2 clientBCLevel2 { get; set; }
        public int? CBCLevel3Id { get; set; }
        [ForeignKey("CBCLevel3Id")]
        public virtual ClientBCLevel3 clientBCLevel3 { get; set; }
        public bool CBCLevel1_AppSupport { get; set; }
        public bool CBCLevel1_ManualSupport { get; set; }
        public bool CBCLevel1_ExternalSupport { get; set; }
        public string CBCLevel1_SLEDesc { get; set; }
        public string CBCLevel1_AppSLEDesc { get; set; }
        public string CBCLevel1_ManualSLEDesc { get; set; }
        public string CBCLevel1_ExternalSLEDesc { get; set; }
        public bool CBCLevel2_AppSupport { get; set; }
        public bool CBCLevel2_ManualSupport { get; set; }
        public bool CBCLevel2_ExternalSupport { get; set; }
        public string CBCLevel2_AppSLEDesc { get; set; }
        public string CBCLevel2_ManualSLEDesc { get; set; }
        public string CBCLevel2_ExternalSLEDesc { get; set; }
        public bool CBCLevel3_AppSupport { get; set; }
        public bool CBCLevel3_ManualSupport { get; set; }
        public bool CBCLevel3_ExternalSupport { get; set; }
        public string CBCLevel3_AppSLEDesc { get; set; }
        public string CBCLevel3_ManualSLEDesc { get; set; }
        public string CBCLevel3_ExternalSLEDesc { get; set; }

    }
}
