using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class Currencies:Entity
    {
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
