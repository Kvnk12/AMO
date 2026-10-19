using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Survey
{
    [Table("SurveyUser")]
    public class SurveyUser
    {
     [Key]
     public int Id { get; set; }
     public  int SurveyId { get; set; }
     public int? SurveyForRefId { get; set; }
     public int PersonnelId { get; set; }
     public DateTime? Completedt { get; set; }
     public DateTime? Createddt { get; set; }
	 public DateTime? updateddt { get; set; }
     public int? CreatedBy { get; set; }
     public int? UpdatedBy { get; set; }
     public string UserSurveyToken { get; set; }
     public string SurveyStatus { get; set; }
        public DateTime? SurveySentDate { get; set; }
    }
}
