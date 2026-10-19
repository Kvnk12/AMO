using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class SurveyUserModel
    {
        public int Id { get; set; }
        public int? SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string surveyType { get; set; }
        public string Description { get; set; }
        public int? SurveyForRefId { get; set; }
        public int PersonnelId { get; set; }
        public string PersonnelName { get; set; }
        public string Email { get; set; }
        public DateTime? Completedt { get; set; }
        public DateTime? surveyenddate { get; set; }
        public DateTime? Createddt { get; set; }
        public DateTime? updateddt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string UserSurveyToken { get; set; }
        public string SurveyFor { get; set; }
        //public string idforgridrow { get; set; }
        public string idforgridrow
        {
            get
            {
                if (SurveyForRefId==null && SurveyForRefId > 0)
                    return (PersonnelId.ToString() + "-" + "");
                else return (PersonnelId.ToString() + "-" + SurveyForRefId.ToString());
            }
        }
        public string clientname { get; set; }
        public string SurveyStatus { get; set; }
        public DateTime? SurveySentDate {get;set;}

    }
}