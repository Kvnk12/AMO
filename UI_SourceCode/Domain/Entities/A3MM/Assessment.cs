using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_Assessment")]
    public class Assessment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string EngagementName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime AnalysisDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime EndDate { get; set; }
        public string QuestionnaireType { get; set; }
        public string Status { get; set; }
        public int AccountID { get; set; }
        public int ContractID { get; set; }
        public int EngagementTeamPointOfContact { get; set; }
        public int EngagementTeamtoCommunicate { get; set; }
        public string AssessmentTeam { get; set; }
        public string AssessmentName { get; set; }
        public string QuestionaireVersion { get; set; }
        public int SurveyTypeID { get; set; }

        public DateTime? Planned_CompletionDate { get; set; }

    }
}
