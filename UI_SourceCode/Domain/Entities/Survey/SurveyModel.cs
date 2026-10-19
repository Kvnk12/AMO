using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Survey
{
    [Table("SurveyModel")]
    public class SurveyModel
    {
     [Key]
     public int Id { get; set; }
     public  int SurveyId { get; set; }
     public string ModelType { get; set; }
     public string Question { get; set; }
     public string Hint { get; set; }
     public decimal? Weighting { get; set; }
     public string DefaultComment { get; set; }
     public int? PerspectiveTypeId { get; set; }
     public DateTime? Createddt { get; set; }
	 public DateTime? updateddt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Include { get; set; }
        public string QuestionIdentifier { get; set; }
        
    }
}
