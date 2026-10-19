using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    public interface ITypeClass
    {
        int TypeId { get; }
        string TypeName { get; }
    }
}
