using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("SRFilters")]
    public class SRFilters : DelEntity
    {
        public string Name { get; set; }
        public int? Year {get; set;}
        public int? Month {get; set;}
        public int ClientId {get; set;}
        public string Description { get; set; }
        public string SRCategory { get; set; }
        public string Priority { get; set; }
        public string DivisionImpacted { get; set; }
        public string BUImpacted { get; set; }
        public string PYSysAffected { get; set; }
        public string RegImpacted { get; set; }
        public string LocImpacted { get; set; }
        public string ServiceArea { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceSubCategory { get; set; }
        public string IncludeRecord { get; set; }
    }
}