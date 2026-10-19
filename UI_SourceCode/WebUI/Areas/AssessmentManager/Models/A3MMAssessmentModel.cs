using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebUI.Areas.AssessmentManager.Models
{
    public class A3MMAssessmentModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string EngagementName { get; set; }
        public string ContractName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Planned_CompletionDate { get; set; }
        public DateTime AnalysisDateEnd { get; set; }
        public DateTime ReviewDateEnd { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateString { get; set; }
        public string StartDateString { get; set; }
        public string QuestionnaireType { get; set; }
        public string Status { get; set; }
        public int AccountID { get; set; }
        public int ContractID { get; set; }
        public int EngagementTeamPointOfContact { get; set; }
        public string EngagementTeamPointOfContactName { get; set; }
        public int EngagementTeamtoCommunicate { get; set; }
        public string EngagementTeamtoCommunicateName { get; set; }
        public string AssessmentTeam { get; set; }
        public string AssessmentTeamName { get; set; }
        public string AssessmentName { get; set; }
        public string QuestionaireVersion { get; set; }
        public int SurveyTypeID { get; set; }
        public string SurveyType { get; set; }
        public string startDateIgnore { get; set; }
        public string plannedCompletionDateIgnore { get; set; }
        public string analysisDateIgnore { get; set; }
        public string reviewDateIgnore { get; set; }
        public string userRole { get; set; }
    }
}