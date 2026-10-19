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


namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class IMPostItemInput : Input
        {

       

        public int ClientId { get; set; }
        public string TabName { get; set; }

        //public PostChallengeInput postChallengeInput
        //    {
        //    get;
        //    set;
        //    }


        //public IList<IMPostItem> GetPostItems
        //    {
        //    get
        //        {
        //        return IdeaManagerHelper.GetPostItems(this.ClientId);
        //        }

        //    }




        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }


        public string PostItemType { get; set; }



        }
    }