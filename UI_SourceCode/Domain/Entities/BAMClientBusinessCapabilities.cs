using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("BAMClientBusinessCapabilities")]
    public  class BAMClientBusinessCapabilities
    {
        //public BAMClientBusinessCapabilities()
        //{
        //    this.BAMClientBusinessCapabilities1 = new HashSet<BAMClientBusinessCapabilities>();
           
        //}
        public int Id { get; set; }
        public string BusinessCapName { get; set; }

        public int? SubVerticalId { get; set; }
        public int? ParentId { get; set; }

        public int? ClientId { get; set; }

        public int? CreatedBy { get; set; }

        public int? OrderId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string InternalExternal { get; set; }

        public int? ProcessImportanceId { get; set; }

        public int? ProcessHealthId { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }

        //public  ICollection<BAMClientBusinessCapabilities> BAMClientBusinessCapabilities1 { get; set; }
        //public  BAMClientBusinessCapabilities BAMClientBusinessCapability1 { get; set; }
    }
}
