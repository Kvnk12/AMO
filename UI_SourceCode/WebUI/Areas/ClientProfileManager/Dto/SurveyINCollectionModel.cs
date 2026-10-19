using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class SurveyINCollectionModel
    {
        public int Id { get; set; }
        public int SurveyModelId { get; set; }
        public int? OriginalRating { get; set; }
        public int? FinalRating { get; set; }
        public string Comments { get; set; }
        public DateTime? Createddt { get; set; }
        public int? SurveyINUserId { get; set; }
        public DateTime? updateddt { get; set; }
    }
}