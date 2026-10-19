using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class Countries:Entity
    {        
        public string CountryName { get; set; }
        public int RegionId { get; set; }
        public int CurrencyId { get; set; }
        public int? CountryCode { get; set; }
    }
}
