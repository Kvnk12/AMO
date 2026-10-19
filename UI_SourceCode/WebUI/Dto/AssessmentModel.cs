using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;


namespace NTTData.AMOPortal.WebUI.Dto
{
    public class AssessmentModel1 : Input
    {

        public int Id { get; set; }
        public int ClientId { get; set; }
        public string EngagementName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string QuestionnaireType { get; set; }
        public string Status { get; set; }

    }
}