using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
//using WebUI.Areas.AlignmentManager.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using WebUI.Areas.DemandManager.Object;


namespace NTTData.AMOPortal.WebUI.Dto
{
    public class PostDemandInput : Input
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Enter Demand Name")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Select Demand Type from the list")]
        public int DemandTypeId { get; set; }
        [Required(ErrorMessage = "Select Demand Source from the list")]
        public int DemandSourceId { get; set; }
        //[Required(ErrorMessage = "Enter Quantity($)")]
        public decimal? QtyFication { get; set; }

        public int ItemTypeId { get; set; }

        public int LOBId { get; set; }


        public int CategoryId { get; set; }

        public int? PriorityId { get; set; }

        public int StatusId { get; set; }

        public int? CreatedBy { get; set; }


        public string StatusComments { get; set; }
        public string PriorityComments { get; set; }

        public int? ConfigItem { get; set; }

        public IList<DMClassification> GetClassification
            {
            get
                {
                return DemandManagerHelper._getClassification(this.ClientId);
            }

        }

        public IList<DMItemCategoryMap> GetMapItems
        {
        get
        {
        return DemandManagerHelper.GetMapItemForItemId(this.Id);

        }
        } 

        //public IList<vwDMclasswithcat> GetCatAndClassification
        //    {
        //    get
        //        {
        //        return DemandManagerHelper.GetCatAndClassification(this.ClientId);
        //        }

        //    }
        
        public bool IsDeleted { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now;
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }
        public int? UpdatedBy { get; set; }

        public string Complexity { get; set; }
        public string Risk { get; set; }
        public string RiskMitigation { get; set; }
        public bool InternalInitiative { get; set; }

    }
    }