using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_QuestionnaireResponse")]
    public class QuestionnaireResponse
    {
        public int Id { get; set; }
        public int AssessmentID { get; set; }
        public int ClientID { get; set; }
        public int? QNo { get; set; }
        //public string SQNo { get; set; }
        public int? SurveyTypeId { get; set; }
        public string MaturityLevel { get; set; }
        public string Response { get; set; }
        public string AssessmentResponse { get; set; }
        public string AreaOfImprovement { get; set; }
        public string Feedback { get; set; }
        
        public DateTime? LastFeedBackOn { get; set; }
       
        public int? LastFeedBackBy { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string FollowUpQuestion { get; set; }
        public string Status { get; set; }
        public string VersionID { get; set; }
        public string SQNo { get; set; }

        public string EngagementRating { get; set; }
        public string EngagementComments { get; set; }
        public string responseStatus { get; set; }


    }
}
