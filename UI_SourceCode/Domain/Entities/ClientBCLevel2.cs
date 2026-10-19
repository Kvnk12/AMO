using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class ClientBCLevel2 : DelEntity
    {
        public string ClientLevel2Name { get; set; }
        public int ClientLevel1Id { get; set; }
        [ForeignKey("ClientLevel1Id")]
        public ClientBCLevel1 CBCLevel1 { get; set; }
        public string ClientLevel2Des { get; set; }
        public string ClientLevel2Importance { get; set; }
        
    }
}
