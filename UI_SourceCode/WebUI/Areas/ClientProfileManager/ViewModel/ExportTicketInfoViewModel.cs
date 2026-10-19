using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Data;


namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class ClientTicketinfoSearchModel
    {
        public int clientstatus { get; set; }
        public int quarter { get; set; }
        public int year { get; set; }
        public DateTime renewaldate { get; set; }
    }

    public class ExportTicketInfoViewModel
    {
        public int TicketId { get; set; }
        public int ClientId { get; set; }
        public int ClientStatus { get; set; }
        public string ClientStatusName { get; set; }


       // public DateTime ClientRenewalDate { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }

        public string ClientName { get; set; }
        public string TotalContractValue { get; set; }
        public string IndustryVertical { get; set; }
        public string SubVertical { get; set; }
        public string YearOfRelationship { get; set; }
        public string NTTGlobalClientManager { get; set; }
        public string NTTGlobalDeliveryManager { get; set; }       
        public string LastCustomerSurvey { get; set; }
        public decimal Rating { get; set; }

        public decimal FTEsOnShore { get; set; }
        public decimal OffShore { get; set; }
        public decimal FTEsNearShore { get; set; }
       

        public decimal IncidentsCount { get; set; }
        public decimal IncidentEffortHrs { get; set; }
        public decimal IncidentEffortPerUnit { get; set; }

        public decimal UserSupportSRCount { get; set; }
        public decimal UserSupportSREffortHrs { get; set; }
        public decimal UserSupportSREffortPerUnit { get; set; }

        public decimal EnhancementsCount { get; set; }
        public decimal EnhacementEffortHrs { get; set; }
        public decimal EnhancementEffortPerUnit { get; set; }

        public decimal ProblemsCount { get; set; }
        public decimal ProblemsEffortHrs { get; set; }
        public decimal ProblemsEffortPerUnit { get; set; }

        public decimal SRBacklog { get; set; }

        public string Comment { get; set; }

        public decimal InMTTRSev1 { get; set; }
        public decimal InMTTRSev2 { get; set; }
        public decimal InMTTRSev3 { get; set; }
        public decimal InMTTRSev4 { get; set; }

        public decimal SrMTTRSev1 { get; set; }
        public decimal SrMTTRSev2 { get; set; }
        public decimal SrMTTRSev3 { get; set; }
        public decimal SrMTTRSev4 { get; set; }

       
    }
}