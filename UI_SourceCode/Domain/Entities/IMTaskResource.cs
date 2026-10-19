using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMTaskResource")]
    public class IMTaskResource : Entity
    {
        public int IdeaPostId { get; set; }
        public int TaskId { get; set; }
        public int ResourceId { get; set; }
        public decimal Units { get; set; }
    }
}
