using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.IncidentNavigator
{
    [Table("INColumnValue")]
    public class INColumnValue
    {
        [Key]
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public string Value { get; set; }

        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }

        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        public int ClientId { get; set; }
    }
}
