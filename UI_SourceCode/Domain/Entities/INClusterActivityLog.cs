using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    [Table("INClusterActivityLog")]
    public class INClusterActivityLog
    {
        public int ID { get; set; }
        public int RunId { get; set; }
        public string ActivityName { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
    }
}
