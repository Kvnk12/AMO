using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class SurveyINUserModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int SurveyId { get; set; }
        public int? IncidentId { get; set; }
        public string TicketId { get; set; }
        public DateTime? INCreatedDate { get; set; }
        public DateTime? INClosedDate { get; set; }
        public DateTime? Createddt { get; set; }
        public DateTime? Completedt { get; set; }
        public string SurveyUserEmail { get; set; }
        public string SurveyUserName { get; set; }
        public DateTime? updateddt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? CreatedBy { get; set; }
        public string SurveyStatus { get; set; }
        public DateTime? validtill { get; set; }
        public string UserSurveyToken { get; set; }
        public string INTitle { get; set; }
        public DateTime? SurveySentDate { get; set; }

        public string shortDescription { get; set; }

        public string ClientPriority { get; set; }

        public string Track { get; set; }
    }
}