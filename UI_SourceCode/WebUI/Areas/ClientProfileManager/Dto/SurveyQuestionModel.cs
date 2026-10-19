using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class SurveyQuestionModel
    {
        public int QId { get; set; }
        public int SurveyId { get; set; }
        public string ModelType { get; set; }
        public string Question { get; set; }
        public string Hint { get; set; }
        [Required]
        [RegularExpression(@"[0-9]{1,4}(\.[0-9]{0,2})?", ErrorMessage = "*Weighting must be Numeric")]
        public decimal? Weighting { get; set; }
        public string DefaultComment { get; set; }
        public int? PerspectiveTypeId { get; set; }
        public DateTime? Createddt { get; set; }
        public DateTime? updateddt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Include { get; set; }
        public string createdbyName { get; set; }
        [Required]
        //[Remote("IsQuesIdExist", "Survey", HttpMethod = "POST", ErrorMessage = "Question Identifier already exist in database. Please select unique Question Identifier.")]
        public string QuestionIdentifier { get; set; }
        public bool IsSurveyLinkSent { get; set; }
    }
}