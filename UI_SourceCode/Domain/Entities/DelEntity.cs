using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class DelEntity : Entity, IDel
    {
        public bool IsDeleted { get; set; }
    }
}
