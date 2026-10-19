using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{
    [Table("RADispRecommendation")]
    public class RADispRecommendation
    {
        public int Id { get; set; }
        public int RADispCandidateId { get; set; }
        public int RecommendationType { get; set; }
        public string Recommendation { get; set; }
        public string Action { get; set; }
        public int? Estimate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
