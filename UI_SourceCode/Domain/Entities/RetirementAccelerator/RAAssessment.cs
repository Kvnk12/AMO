using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RAAssessment")]
    public class RAAssessment
    {
        public int Id { get; set; }
        public int SurveyModelId { get; set; }
        public int? OriginalRating { get; set; }
        public int? FinalRating { get; set; }
        public string  Comments { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int RAApplicationid { get; set; }
        public decimal? EstAdjHrs { get; set; }
        public string AdjRationale { get; set; }
    }
}
