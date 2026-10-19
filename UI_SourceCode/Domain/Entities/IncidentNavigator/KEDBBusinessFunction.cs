using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.IncidentNavigator
{
    [Table("KEDB_BusinessFunction")]
    public class KEDBBusinessFunction
    {
        [Key]
        public int Id { get; set; }
        public string BusinessFunction { get; set; }
        public int ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
