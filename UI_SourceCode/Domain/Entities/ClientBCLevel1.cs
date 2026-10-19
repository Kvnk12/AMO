using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities

{
    public class ClientBCLevel1 : DelEntity
    {
        public string ClientLevel1Name { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string ClientLevel1Des { get; set; }
        public string ClientLevel1Type { get; set; }
        public string ClientLevel1Importance { get; set; }
        public string ClientInternalExternal { get; set; }
    }
}
