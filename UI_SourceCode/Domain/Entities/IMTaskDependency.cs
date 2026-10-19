using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMTaskDependency")]
    public class IMTaskDependency : Entity
    {
        public int IdeaPostId { get; set; }
        public int PredecessorId { get; set; }
        public int SuccessorId { get; set; }
        public int type { get; set; }
    }
}
