using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Survey
{
    [Table("SurveyModelRatingFactor")]
    public class SurveyModelRatingFactor
    {
     [Key]
     public int? Id { get; set; }
     public  int SurveyModelId { get; set; }
     public string RatingFactor { get; set; }
     public int? RatingFactorValue { get; set; }
     public decimal? ROMEstimate { get; set; }
     public int? EstimateTypeId { get; set; }
     public int? EstimateCategoryId { get; set; }
     public string EstimateAssumption { get; set; }
        //   public DateTime? Createddt { get; set; }
        //public DateTime? updateddt { get; set; }
        public int? CreatedBy { get; set; }
        //  public int? UpdatedBy { get; set; }
        public string RatingContext { get; set; }

    }
}
