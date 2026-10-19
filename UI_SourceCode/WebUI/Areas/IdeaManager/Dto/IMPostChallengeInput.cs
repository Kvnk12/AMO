using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using WebUI.Areas.IdeaManager.Objects;
using PagedList;
using WebUI.Areas.IdeaManager.Dto;

namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class IMPostChallengeInput : Input
        {

       

        public int ClientId { get; set; }
        public string TabName { get; set; }
        public int ChallengeId { get; set; }

        public IPagedList<IMPostChallengeModel> SortedPostedChallenges { get; set; }

        public PostChallengeInput PostChallengeInput
            {
            get;
            set;
            }
        public IEnumerable<SelectListItem> IMLOBSourceList
        {

            get
            {
                return IdeaManagerHelper._getLOBSources(this.ClientId);
            }
        }

        public IList<IMPostIdea> GetIdeaItems
            {
            get
                {
                return IdeaManagerHelper.GetPostIdeaItems(this.ClientId);
                }

            }
        //for Challenges(Suggest Ideas) Filter: Only Ideas which are Approved
        public IList<IMPostIdea> Get_Approved_IdeaItems
        {
            get
            {
                return IdeaManagerHelper._get_Approved_IdeaItems(this.ClientId);
            }

        }


        public IList<IMPostIdea> GetSuggestIdeas
        {
            get
                {
                return IdeaManagerHelper.GetSuggestIdeas(this.ClientId,this.ChallengeId);
                }

            
 
        }

        public IList<vwIMSuggestIdeas> GetSuggestIdeasForChallengeId
            {
            get
                {
                return IdeaManagerHelper.GetSuggestIdeasForChallengeId(this.ClientId, this.ChallengeId);
                }



            }


        public IList<IMPostChallenge> GetChallengeItems
            {
            get
                {
                return IdeaManagerHelper.GetPostChallengeItems(this.ClientId);
                }

            }

        //public IList<IMChallengeITBenefits> GetChallengeITBenefits
        //    {
        //    get
        //        {
        //        return IdeaManagerHelper.GetChallengeITBenefits(this.Id);
        //        }
        //    }


        //public IList<IMChallengePriorityHistory> GetPriorityHistoryDetails
        //    {
        //    get
        //        {
        //        return IdeaManagerHelper.GetPriorityHistoryDetails(this.Id);
        //        }
        //    }


        //public IList<vwChallengeStatusHistory> GetStatusHistoryDetails
        //    {
        //    get
        //        {
        //        return IdeaManagerHelper.GetStatusHistoryDetails(this.Id);
        //        }
        //    }



        public IList<IMItemITBenefits> GetITBenefits
        {
        get
        {
        return IdeaManagerHelper.GetItemITBenefits(this.Id);
        }
        }

        //public IList<IMChallengeComments> GetChallengeComments
        //    {
        //    get
        //        {
        //        return IdeaManagerHelper.GetChallengeComments(this.Id);
        //        }
        //    }


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

        //public IList<IMChallengeBusinessBenefits> GetChallengeBusinessBenefits
        //    {
        //    get
        //        {
        //        return IdeaManagerHelper.GetChallengeBusinessBenefits(this.Id);
        //        }
        //    }

        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }

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

      



        public string PostItemType { get; set; }



        }
    }