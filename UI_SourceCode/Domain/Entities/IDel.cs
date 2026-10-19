using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    public interface IDel
    {
        bool IsDeleted { get; set; }
    }
}
