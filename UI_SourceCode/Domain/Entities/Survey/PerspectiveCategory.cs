using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Survey
{
    [Table("PerspectiveCategory")]
    public class PerspectiveCategory
    {
     public int Id { get; set; }
     public  int? ClientId { get; set; }
        public int? ToolId { get; set; }
        public string PerspectiveCategoryName { get; set; }
    }
}
