using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.PortfolioAnalyzer;

namespace NTTData.AMOPortal.Domain.Entities
{
   public class ApplicationEcoSystemJunctions : DelEntity
    {
        public int LOBId { get; set; }
        [ForeignKey("LOBId")]
        public virtual LineOfBusinesses lineOfBusiness { get; set; }
        public int ESId { get; set; }
       [ForeignKey("ESId")]
       public virtual APREcoSystem APREcoSystems { get; set; }
    }
}
