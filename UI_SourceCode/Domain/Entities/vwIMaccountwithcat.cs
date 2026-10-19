using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{

    [Table("vwIMaccountwithcat")]
    public class vwIMaccountwithcat : Entity
    {

        public int IdeaId { get; set; }
        public string Idea { get; set; }
        public string Category2 { get; set; }
    }
}
