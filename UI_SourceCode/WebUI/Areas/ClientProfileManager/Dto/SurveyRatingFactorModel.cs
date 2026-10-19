using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class SurveyRatingFactorModel
    {
        public int? Id { get; set; }
        public int SurveyModelId { get; set; }
        [Required]
        public string RatingFactor { get; set; }      
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "*RatingFactorValue must be Numeric")]
        public int? RatingFactorValue { get; set; }
        public string createdbyName { get; set; }
        public int? CreatedBy { get; set; }
        public decimal? ROMEstimate { get; set; }
        public int? EstimateTypeId { get; set; }
        public int? EstimateCategoryId { get; set; }
        public string EstimateType { get; set; }
        public string EstimateCategory { get; set; }
        public string EstimateAssumption { get; set; }
        public bool IsSurveyLinkSent1 { get; set; }
        public string RatingContext { get; set; }
        
    }
}