using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class VerticalAndSubVertical : DelEntity
    {
        public int VerticalId { get; set; }
        public int SubVerticalId { get; set; }
        public string VerticalName { get; set; }
        public string SubVerticalName { get; set; }
    }
}
