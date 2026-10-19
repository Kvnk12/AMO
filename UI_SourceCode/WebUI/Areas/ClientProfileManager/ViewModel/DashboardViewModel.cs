using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.Dto;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class DashboardViewModel
    {
        public string ClientName { get; set; }
        public string IncidentPercentage { get; set; }
        public string SRPercentage { get; set; }
        public string FtePercentage { get; set; }

        public String ApplicationPercentage { get; set; }
        public string MTTRPercentage { get; set; }

        public string ProblemVsIncident { get; set; }

        public decimal EffortPerIncident { get; set; }
        public decimal TicketsPerFTE { get; set; }

        public decimal CostPerTicket { get; set; }
        public decimal CostPerFTE { get; set; }

        public string OnsiteRatio { get; set; }
        //public DataTable Table { get; set; }
        //public int Id { get; set; }
        //public int ClientId { get; set; }
        //public DateTime FromDate { get; set; }
        //public DateTime Todate { get; set; }
        //public int TicketInfoTypeId { get; set; }
        //public string TicketInfoType { get; set; }
        //public string ColumnTitle { get; set; }
        //public string RowTitle { get; set; }
        //public decimal Incidents1 { get; set; }
        //public decimal Incidents2 { get; set; }

        //public decimal Problems { get; set; }
        //public decimal UserSupportSR { get; set; }
        //public decimal Enhancements { get; set; }
        //public string Comments { get; set; }
    }
}