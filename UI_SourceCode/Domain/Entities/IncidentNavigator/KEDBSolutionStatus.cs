using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.IncidentNavigator
{
    [Table("KEDBSolutionStatus")]
    public class KEDBSolutionStatus
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
        //public int ClientId { get; set; }
        public int Weightage { get; set; }
    }
}
