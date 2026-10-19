using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PagedList;
using PagedList.Mvc;

using WebUI.Areas.ClientProfileManager.ViewModel;
namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class ClientDetailsModel

    {
        public string ClientName { get; set; }
        public string TotalContractVal { get; set; }
        public int VerticalId { get; set; }
        public DateTime WorkStartTime { get; set; }
        public DateTime WorkEndTime { get; set; }
        public DateTime ResolutionTime { get; set; }
        public string NttClientManager { get; set; }
        public string NttDeliveryManager { get; set; }
        public int ClientStatus { get; set; }

        //something "Retraceable" field is missing
        public DateTime CustSurveyDate { get; set; }
        public int CustRating { get; set; }
        public string OnshoreFTE { get; set; }
        public string OffshoreFTE { get; set;}
        public string NearShoreFTE { get; set; }
        public string Notes { get; set; }

       public IList<TicketInfoViewModel> ClientTicketInfoList { get; set; } 
      //public IPagedList<TicketInfoViewModel> ClientTicketInfoList { get; set; }


    }
}