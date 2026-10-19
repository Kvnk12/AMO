using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class Survey_Model
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }

        public string SurveyType { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? Createddt { get; set; }
        public DateTime? updateddt { get; set; }
        public DateTime? Launchdt { get; set; }
        public int? LaunchedBy { get; set; }
        public DateTime? enddate { get; set; }
        public string CreatedByName { get; set; }

        public bool IsLinkSent { get; set; }

        public IEnumerable<SelectListItem> EstimateTypes { get; set; }
        public IEnumerable<SelectListItem> EstimateCategories { get; set; }

        //public IEnumerable<SelectListItem> EstimateTypes
        //{
        //    get
        //    {
        //        return SurveyHelper._getEstimateTypes(this.ClientId.Value);
        //    }
        //}
        //public IEnumerable<SelectListItem> EstimateCategories
        //{
        //    get
        //    {
        //        return SurveyHelper._getEstimateCategories(this.ClientId.Value);
        //    }
        //}

        // public int? ToolId { get; set; }
        public int? EndDay { get; set; }
        public int? ToolId { get; set; }

    }

    public class Survey_ScheduleEmailConfigModel
    {

        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public int? Survey_ID { get; set; }      
        public string Ack_Message { get; set; }
        public string FirstReminder_Message { get; set; }
        public string SecondReminder_Message { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public int? updatedBy { get; set; }       

    }
}