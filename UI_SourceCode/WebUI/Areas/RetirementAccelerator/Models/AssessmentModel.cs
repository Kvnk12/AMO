using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class AssessmentModel
    {
        public int Id { get; set; }
        public string subPerspective { get; set; }
        public int SurveyModelId { get; set; }
        public string Question { get; set; }
        public int? OriginalRating { get; set; }
        public int? FinalRating { get; set; }
        public int? OR { get; set; }
        public int? FR { get; set; }
        public string Answer { get; set; }
        public string Comments { get; set; }
        public int RAApplicationid { get; set; }
        public string ApplicationName { get; set; }

        [UIHint("FinRating")]
        public ratingMod answers { get; set; }
        public IEnumerable<ratingMod> QuestionOptions { get; set; }
        public string Hint { get; set; }
        public string PersonnelName { get; set; }
        public int SurveyUserId { get; set; }
        //Temp
        public int CreatedBy { get; set; }
    }

    public class ratingMod
    {
        public int? Id { get; set; }
        public int QuestionId { get; set; }
        public string Rating { get; set; }
        public string RatingContext{get; set;}
    }

    public class ArchivalDecommissionModel
    {
        public int Id { get; set; }
        public string subPerspective { get; set; }
        public int SurveyModelId { get; set; }
        public string Question { get; set; }
        public string Hint { get; set; }
        public int? OriginalRating { get; set; }
        public int? FinalRating { get; set; }
        public string FinalRatingFactorValue { get; set; }
        public decimal? FinalRatingROMEstimate { get; set; }
        public decimal? AdjROMEstimate { get; set; }
        public decimal? TotalROMEstimate { get { return FinalRatingROMEstimate + AdjROMEstimate; } set {  } }
        public string EstimateType { get; set; }

        public string EstimateAssumption { get; set; }
        public string AdjRationale { get; set; }
        public int RAApplicationid { get; set; }
       
    }


    //new objects for Update ratings through external Ajax post.
    public class RatingJSObject
    {
        public int ApplicationId { get; set; }
        public IEnumerable<AnswersCollection> RatingsList { get; set; }
        public string ButtonType { get; set; }
        public int SurUserId { get; set; }
    }

    public class AnswersCollection
    {
        public int? RowId { get; set; }
        public int? OriginalRating { get; set; }
        public int? FinalRating { get; set; }
        public string Comments { get; set; }
        public int SurveyUserId { get; set; }
        public int SurveyModelId { get; set; }
    }
}