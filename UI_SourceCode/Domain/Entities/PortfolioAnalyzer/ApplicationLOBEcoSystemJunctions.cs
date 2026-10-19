using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using NTTData.AMOPortal.Domain.Entities;

namespace Domain.Entities.PortfolioAnalyzer
{
   public class ApplicationLOBEcoSystemJunctions : DelEntity
    {
       public int AppId { get; set; }
       [ForeignKey("AppId")]
       public virtual Application Applications { get; set; }
       public int LobEcoId { get; set; }
       [ForeignKey("LobEcoId")]
       public virtual ApplicationEcoSystemJunctions ApplicationEcoSystemJunctions { get; set; }
       
    }
}
