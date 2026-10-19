using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;

using WebUI.Areas.ClientProfileManager.ViewModel;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class ticketInfoInput
    {
        public int? ClientId { get; set; }
        public DateTime fromDt { get; set; }
        public DateTime toDt { get; set; }
        public int ToQuat { get; set; }
        public int FromQuat { get; set; }
        public IList<TicketInfoViewModel> ticketInfoInputList { get; set; }
       // public IEnumerable<TicketInfoViewModel> ticketInfoList { get; set; }
        public IPagedList<TicketInfoViewModel> ticketInfoList { get; set; }
    }
}