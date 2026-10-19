using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class ClientBCLevel3 : DelEntity
    {
        public string ClientLevel3Name { get; set; }
        public int ClientLevel2Id { get; set; }
        [ForeignKey("ClientLevel2Id")]
        public ClientBCLevel2 CBCLevel2 { get; set; }
        public string ClientLevel3Des { get; set; }
        public string ClientLevel3Importance { get; set; }
    }
}
