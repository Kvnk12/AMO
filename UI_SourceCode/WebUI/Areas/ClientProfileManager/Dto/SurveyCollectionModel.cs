using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class SurveyCollectionModel
    {
        public int Id { get; set; }
        public int SurveyModelId { get; set; }
        public string Question { get; set; }
        public string Hint { get; set; }
        public string Comments { get; set; }
        public int? OriginalRating { get; set; }
        public int? FinalRating { get; set; }
        public DateTime? Createddt { get; set; }
        public QA qa { get; set; }
        public int SurveyUserId { get; set; }
        public string SurveyForName { get; set; }
        public string Answer { get; set; }
        public string OriginalRatingAnswer { get; set; }
        public string DefaultComment { get; set; }


    }

    public class QA
    {
        public int qid { get; set; }
        public int? answerid { get; set; }
    }


    public class ratingModel
    {
        public int? Id { get; set; }
        public int QuestionId { get; set; }
        public string Rating { get; set; }
        public string RatingContext { get; set; }
    }

    public class SurveyCollectiondto
    {
        public int Id { get; set; }
        public int SurveyModelId { get; set; }
        public IEnumerable<ratingModel> QuestionOptions { get; set; }
        public IList<SelectListItem> SurveyModelAnswers { get; set; }
       // public IList<SelectListItem> SurveyModelAnswerContext { get; set; }
        public string Question { get; set; }
        public string Hint { get; set; }
        public string ModelType { get; set; }
        public bool CanShow { get; set; }
        public string DefaultComment { get; set; }

        public string Comments { get; set; }
        public int? OriginalRating { get; set; }
        
        public int? FinalRating { get; set; }
        public DateTime? Createddt { get; set; }
        public int SurveyUserId { get; set; }
        public int? FR { get; set; }
    }

    public class UserSurvey
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string Description { get; set; }
        public IList<SurveyCollectiondto> surveyCollection { get; set; }
        public IList<SelectListItem> userSurveyApplications { get; set; }
        public int SurveyUserId { get; set; }
        public int PersonnelId { get; set; }
        public DateTime? surveyenddate { get; set; }
        public string UserSurveyToken { get; set; }
        public string SurveyType { get; set; }
        public int? SurveyForRefId { get; set; }
        public string SurveyFor { get; set; }
        public string ClientName { get; set; }
        public DateTime? completedDt { get; set; }

    }

    public class UserINSurvey
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string Description { get; set; }
        public IList<SurveyCollectiondto> surveyCollection { get; set; }
        public int SurveyUserId { get; set; }
        public string emailid { get; set; }
        public DateTime? surveyenddate { get; set; }
        public string UserSurveyToken { get; set; }
        public string SurveyType { get; set; }
        public int IncidentId { get; set; }
        public string TicketId { get; set; }
        public string SurveyFor { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public DateTime? completedDt { get; set; }
        public string SurveyComments { get; set; }
        public string SurveyStatus { get; set; }
        public int? SurveyValidDays { get; set; }
        public DateTime SurveyCreatedDate { get; set; }
        public string SurveyUserEmail { get; set; }
        public DateTime? SurveySentDate { get; set; }
        public string SurveyUserName { get; set; }
    }


}