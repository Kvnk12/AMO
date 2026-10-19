using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Survey
{
    [Table("Survey")]
    public class Survey
    {
     [Key]
     public int Id { get; set; }
     public  int? ClientId { get; set; }
	 public string SurveyType{ get; set; }
     public string Name{ get; set; }
     public string Description{ get; set; }
	 public int? CreatedBy{ get; set; }
     public int? UpdatedBy{ get; set; }
     public DateTime? Createddt { get; set; }
	 public DateTime? updateddt { get; set; }
	 public DateTime? Launchdt { get; set; }
	 public int? LaunchedBy{ get; set; }
	 public DateTime? enddate{ get; set; }
        public int? ToolId { get; set; }
        public int? EndDay{ get; set; }

    }
}
