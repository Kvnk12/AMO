using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Survey
{
    [Table("SurveyCollection")]
    public class SurveyCollection
    {
     [Key]
     public int Id { get; set; }
     public  int? SurveyModelId { get; set; }
     public string Comments { get; set; }
	 public int? OriginalRating { get; set; }
     public int? FinalRating { get; set; }
     public DateTime? Createddt { get; set; }
	 public DateTime? updateddt { get; set; }
	 public int? SurveyUserId { get; set; }
        
    }
}
