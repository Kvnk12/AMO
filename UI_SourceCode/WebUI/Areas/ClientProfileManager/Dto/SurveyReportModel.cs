using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class SurveyReportModel
    {
        public int surveyId { get; set; }
        public string surveyName { get; set; }
        public int? SurAppId { get; set; }
        public string SurveyFor { get;set;}
        public int QuestionId { get; set; }
        public int RatingFactor { get; set; }
        public int clientId { get; set; }
        public string modelType { get; set; }
        public int perspectiveId { get; set; }
        public decimal weightage { get; set; }
        public DateTime? surveyEndDate { get; set; }
        public DateTime? SyrveyCompletedDate { get; set; }
        public int personnelId { get; set; }
        public string PersonnelName { get; set; }
        public string Question { get; set; }
        public string SurveyStatus { get; set; }
        
    }
}