using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
//using WebUI.Areas.AlignmentManager.Objects;
using WebUI.Areas.DemandManager.Object;
using PagedList.Mvc;
using PagedList;

namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class DMPostDemandInput : Input
        {

       

        public int ClientId { get; set; }
        public string TabName { get; set; }

        public PostDemandInput PostDemandInput
        {
            get;
            set;
            }


        public IPagedList<DMPostDemandModel> SortedPostedDemands { get; set; }
      

        //public IEnumerable<SelectListItem> DMItemPriorityList
        //    {
        //    get
        //        {
        //        return DemandManagerHelper.DMItemPriorityList;
        //        }
        //    }


        //public IEnumerable<SelectListItem> DMItemStatusList
        //    {
        //    get
        //        {
        //        return DemandManagerHelper.DMItemStatusList;
        //        }
        //    }



        //public IList<DMPostDemand> GetPostItems
        //    {
        //    get
        //        {
        //        return DemandManagerHelper.GetPostItems(this.ClientId);
        //        }

        //    }

        public IEnumerable<SelectListItem> DMDemandTypes
        {

        get
        {
        return DemandManagerHelper._getDemandTypes(this.ClientId);
            }
         }

        //public IEnumerable<SelectListItem> DMDemandSources
        //{
        //    get
        //        {
        //        return DemandManagerHelper._getDemandSources(this.ClientId);
        //        }
        //    }

        public IEnumerable<SelectListItem> DMLOBSourceList
            {
            get
                {
                    return DemandManagerHelper._getLOBSources(this.ClientId);
                }
            }

        public IList<DMItemITBenefits> GetITBenefits
        {
        get
        {
        return DemandManagerHelper.GetItemITBenefits(this.Id);
        }
        }

        public IList<DMDemandType> GetDemandTypes
        {
            get
                {
                return DemandManagerHelper.GetDemandTypes(this.ClientId);
                }
            }

        //public IList<DMDemandSource> GetDemandSources
        //{
        //    get
        //        {
        //        return DemandManagerHelper.GetDemandSources1(this.ClientId);
        //        }
        //    }

        public IList<DMItemComments> GetItemComments
            {
            get
                {
                return DemandManagerHelper.GetItemComments(this.Id);
                }
            }

        public IList<DMItemBusinessBenefits> GetBusinessBenefits
            {
            get
                {
                return DemandManagerHelper.GetItemBusinessBenefits(this.Id);
                }
            }

        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }

        public IList<DMPostDemand> GetDemandItems
        {
            get
                {
                return DemandManagerHelper.GetPostDemandItems(this.ClientId);
                }

            }

        public string PostItemType { get; set; }


        public IList<DMItemComments> GetDemandComments
        {
            get
                {
                return DemandManagerHelper.GetItemComments(this.Id);
                }
            }

        //public IList<vwDemandPriorityHistory> GetIdeaPriorityHistoryDetails
        //    {
        //    get
        //        {
        //        return DemandManagerHelper.GetDemandPriorityHistoryDetails(this.Id);
        //        }
        //    }


        //public IList<vwDemandStatusHistory> GetDemandStatusHistoryDetails
        //    {
        //    get
        //        {
        //        return DemandManagerHelper.GetDemandStatusHistoryDetails(this.Id);
        //        }
        //    }

    }
}