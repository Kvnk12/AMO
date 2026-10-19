using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using WebUI.Areas.IdeaManager.Objects;


namespace NTTData.AMOPortal.WebUI.Dto
    {

    

    public class PostChallengeInput : Input
        {

        public int ClientId { get; set; }

        [Required(ErrorMessage = "Enter Challenge Name")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set; }

        
        //[Required(ErrorMessage = "Enter Quantity($)")]
        public decimal? QtyFication { get; set; }

        public int ItemTypeId { get; set; }


        public int CategoryId { get; set; }

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

        public int? CreatedBy { get; set; }

        public string StatusComments { get; set; }
        public string PriorityComments { get; set; }



        public IList<IMClassification> GetClassification
            {
            get
                {
                //return IdeaManagerHelper.GetClassification();
                return IdeaManagerHelper._getClassification(this.ClientId);
                }

            }

        public int LOBId { get; set; }

        //public IList<IMChallengeCategoryMap> GetMapItemsForChallenge
        //{
        //get
        //{
        //return IdeaManagerHelper.GetMapItemForChallengeItemId(this.Id);

        //}
        //}

        public IList<vwIMclasswithcat> GetCatAndClassification
            {
            get
                {
                return IdeaManagerHelper.GetCatAndClassification(this.ClientId);
                }

            }

        
        public bool IsDeleted { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now;

        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }

        public int? ConfigItem { get; set; } // APR application Id

        public int? UpdatedBy { get; set; }

        public string RootCause { get; set; }
       
    }
}