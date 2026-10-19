using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebUI.Areas.IdeaManager.Dto
{
    public class IdeaModel
    {
        public int IdeaId { get; set; }
        public int ClientId { get; set; }
        public string IdeaName { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int IdeaTypeId { get; set; }
        public string IdeaType { get; set; }
        public int IdeaSourceId { get; set; }
        public string IdeaSource { get; set; }
        public decimal? QtyFication { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }
        public int? LOBId { get; set; }
        public string LOB { get; set; }
        public int CategoryId { get; set; }
          public int PriorityId { get; set; }

        //public string PriorityId { get; set; }

        public int StatusId { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [AllowHtml]
        public string StatusComments { get; set; }
        [AllowHtml]
        public string PriorityComments { get; set; }
        public int? ConfigItem { get; set; }
        public string ConfigItemName { get; set; }
        public bool IsDeleted { get; set; }
        // for check box
        public bool IsChecked { get; set; }
        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime _appDate = DateTime.Now;

        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        //private string[] _SelectedApproverIds;
        public string[] SelectedApproverIds { get; set; }
        //public string[] SelectedApproverIds {
        //    get
        //    { if (_SelectedApproverIds != null)
        //            return _SelectedApproverIds;
        //      else
        //        return IdeaManagerHelper.GetIdeaApprovers(this.IdeaId);
        //    }
        //set
        //    {
        //        _SelectedApproverIds = value;
        //    }

            //}

        public string Ideasnumber { get; set; }

        public bool IsInternalInitiative { set; get; }

        public string Team { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }

    public enum IMStatus
    {
        Created = 1,
        Posted =7,

        Submitted = 2,
        Approved = 3,
        /// <summary>
        /// Completed Idea
        /// </summary>
        Implemented = 4,

        Declined = 6,
        /// <summary>
        /// Cancelled Idea
        /// </summary>
        Cancelled = 5, 

        //-------------//

        SubmitForReview = 11,
        Revised = 12,
        
        SubmitForRevision = 13,
        SubmitForApproval = 14,

        SubmitForPlanning = 15,
        Planned = 16,
        Initiated = 17,

        Hold = 18,
        
    }

}