using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities.A3MM
{
    [Table("A3MM_ActionItemTrackerLog")]
    public class ActionItemTrackerLog
    {
        public int Id { get; set; }
        public int? ClientID { get; set; }
        public int ActionItemID { get; set; }
        public DateTime? updateDate { get; set; }

        
        public string Updates { get; set; }

    }
}
