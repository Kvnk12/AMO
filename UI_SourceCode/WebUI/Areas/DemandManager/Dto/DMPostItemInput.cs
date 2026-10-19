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


namespace NTTData.AMOPortal.WebUI.Dto
{
    public class DMPostItemInput : Input
    {
        public int ClientId { get; set; }
        public string TabName { get; set; }

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