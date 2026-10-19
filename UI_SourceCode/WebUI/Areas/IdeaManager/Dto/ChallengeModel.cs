using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class ChallengeModel
    {
        public int ChallengeId { get; set; }
        public int ClientId { get; set; }
        public string ChallengeName { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public decimal? QtyFication { get; set; }
        public int LOBId { get; set; }
        public string LOB { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string CreatedByUser { get; set; }
        [AllowHtml]
        public string StatusComments { get; set; }
        [AllowHtml]
        public string PriorityComments { get; set; }
        public int? ConfigItem { get; set; }
        public string ConfigItemName { get; set; }
        public int? BusinessBenefits { get; set; }
        public int? ITSavings { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? MyProperty { get; set; }

        public bool IsChecked { get; set; }
        public int PriorityId { get; set; }


        public enum IMStatus
        {
            Created = 1,
            Posted = 7,
            Submitted = 2,
            Approved = 3,
            Implemented = 4,
            Declined = 6,
            Closed = 5

        }

        //public IPagedList<ChallengeModel> Challengelist { get; set; }
    }
}