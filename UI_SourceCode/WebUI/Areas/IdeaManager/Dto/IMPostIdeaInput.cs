using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
//using WebUI.Areas.AlignmentManager.Objects;
using WebUI.Areas.IdeaManager.Objects;
using PagedList.Mvc;
using PagedList;

namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class IMPostIdeaInput : Input
        {

       

        public int ClientId { get; set; }
        public string TabName { get; set; }

        public PostIdeaInput PostIdeaInput
            {
            get;
            set;
            }


        public IPagedList<IMPostIdeaModel> SortedPostedIdeas { get; set; }
      

        public IEnumerable<SelectListItem> IMItemPriorityList
            {
            get
                {
                return IdeaManagerHelper.IMItemPriorityList;
                }
            }


        public IEnumerable<SelectListItem> IMItemStatusList
            {
            get
                {
                return IdeaManagerHelper.IMItemStatusList;
                }
            }



        //public IList<IMPostIdea> GetPostItems
        //    {
        //    get
        //        {
        //        return IdeaManagerHelper.GetPostItems(this.ClientId);
        //        }

        //    }

        public IEnumerable<SelectListItem> IMIdeaTypes
        {

        get
        {
        return IdeaManagerHelper._getIdeaTypes(this.ClientId);
            }
         }


        public IEnumerable<SelectListItem> IMIdeaSources
            {

            get
                {
                return IdeaManagerHelper._getIdeaSources(this.ClientId);
                }
            }

        public IEnumerable<SelectListItem> IMLOBSourceList
            {

            get
                {
                    return IdeaManagerHelper._getLOBSources(this.ClientId);
                }
            }

        public IList<IMItemITBenefits> GetITBenefits
        {
        get
        {
        return IdeaManagerHelper.GetItemITBenefits(this.Id);
        }
        }

        public IList<IMIdeaType> GetIdeaTypes
            {
            get
                {
                return IdeaManagerHelper.GetIdeaTypes(this.ClientId);
                }
            }


        public IList<IMIdeaSource> GetIdeaSources
            {
            get
                {
                return IdeaManagerHelper.GetIdeaSources1(this.ClientId);
                }
            }

        public IList<IMItemComments> GetItemComments
            {
            get
                {
                return IdeaManagerHelper.GetItemComments(this.Id);
                }
            }



        public IList<IMItemBusinessBenefits> GetBusinessBenefits
            {
            get
                {
                return IdeaManagerHelper.GetItemBusinessBenefits(this.Id);
                }
            }



        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }


        public IList<IMPostIdea> GetIdeaItems
            {
            get
                {
                return IdeaManagerHelper.GetPostIdeaItems(this.ClientId);
                }

            }


        public string PostItemType { get; set; }


        public IList<IMItemComments> GetIdeaComments
            {
            get
                {
                return IdeaManagerHelper.GetItemComments(this.Id);
                }
            }



        public IList<vwIdeaPriorityHistory> GetIdeaPriorityHistoryDetails
            {
            get
                {
                return IdeaManagerHelper.GetIdeaPriorityHistoryDetails(this.Id);
                }
            }


        public IList<vwIdeaStatusHistory> GetIdeaStatusHistoryDetails
            {
            get
                {
                return IdeaManagerHelper.GetIdeaStatusHistoryDetails(this.Id);
                }
            }

        //GetPostIdeas
        //public IList<IMPostIdea> GetPostIdeas
        //{
        //    get
        //    {
        //        return IdeaManagerHelper.GetPostIdeas(this.Id);
        //    }
        //}



    }
    }