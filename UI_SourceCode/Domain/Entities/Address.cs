using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("Address")]
    public class Address : DelEntity
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }

    }
}
