using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMIdeaTaskPriority")]
    public class IMIdeaTaskPriority : Entity
    {
        public string TaskPriority { get; set; }
    }
}
