using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_Discipline")]
    public class Discipline
    {
        public int Id { get; set; }
       // public int ClientId { get; set; }
        public string DisciplineName { get; set; }
        public string DiscplineAbbrevation { get; set; }
       
    }
}
