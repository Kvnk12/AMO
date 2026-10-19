using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace NTTData.AMOPortal.Domain.Entities
{
    
    public class ClientAccount : DelEntity    
    {

       
        public int ClientId { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }


    }
}
