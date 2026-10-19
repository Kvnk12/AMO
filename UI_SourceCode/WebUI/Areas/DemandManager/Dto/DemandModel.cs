using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebUI.Areas.DemandManager.Dto
{
    public class DemandModel
    {
        public int DemandId { get; set; }
        public int ClientId { get; set; }
        public string DemandName { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int DemandTypeId { get; set; }
        public string DemandType { get; set; }
        public int DemandSourceId { get; set; }
        public string DemandSource { get; set; }
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


        public string Demandsnumber { get; set; }

        public bool IsInternalInitiative { set; get; }

        public string Team { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }

    public enum DMStatus
    {
        Created = 1,
        Posted =7,

        Submitted = 2,
        Approved = 3,
        /// <summary>
        /// Completed Demand
        /// </summary>
        Implemented = 4,

        Declined = 6,
        /// <summary>
        /// Cancelled Demand
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
        Completed=19,
    }
    public enum DMDefaultStage
    {
       // Unassigned=1,
        Cancelled=2,
        Completed=3,
        Closed=4,
    }
    
}