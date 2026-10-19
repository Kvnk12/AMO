using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IMIdeaSourceMaster")]
    public class IMIdeaSourceMaster : DelEntity
    {

        public string IdeaSource { get; set; }
        public bool? isDefault { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ClientId { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
