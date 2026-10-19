using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.Dto;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class TicketInfoViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime Todate { get; set; }
        public int TicketInfoTypeId { get; set; }
        public string TicketInfoType { get; set; }
        public decimal Incidents { get; set; }
        public decimal Problems { get; set; }
        public decimal UserSupportSR { get; set; }
        public decimal Enhancements { get; set; }
        public string Comments { get; set; }
        //public IList<TicketInfoViewModel> ClientTicketInfoList { get; set; }

        
    }
}