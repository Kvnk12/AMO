using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
    {
    public class ClassAndCategory : DelEntity
        {
                public string ClientName { get; set; }

                public string Classification { get; set; }

                public int ClientId { get; set; }

                public int ClassifiationId { get; set; }

                public int CategoryId { get; set; }

                public string Category { get; set; }
                


        }
    }
