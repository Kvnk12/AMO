using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_EngagementQuestionnaire")]
    public class EngagementQuestionnaire
    {      
        public int Id { get; set; }
        public string VersionID { get; set; }
        public int? AspectID { get; set; }
        public int? Discipline_ID { get; set; }
        public int? Component_ID { get; set; }
        public string Question { get; set; }
        public string ExampleAns { get; set; }
        public string Status { get; set; }

        public int? DataSourceID { get; set; }

        public int? QNo { get; set; }
      
        public int SurveyTypeID { get; set; }
        public string DisciplineName { get; set; }
        public string ComponentName { get; set; }
        public string AspectName { get; set; }

        public string MaturityLevel { get; set; }
        public string Response { get; set; }
        public string AreaOfImprovement { get; set; }
        public string Feedback { get; set; }

        public DateTime? LastFeedBackOn { get; set; }

        public int? LastFeedBackBy { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string FollowUpQuestion { get; set; }

        public int? ParentID { get; set; }
        public string SQNo { get; set; }
        public int? SubQOrder { get; set; }
        public int? QuestionOrder { get; set; }

        public string QuestionIdentifier { get; set; }

    }
}
