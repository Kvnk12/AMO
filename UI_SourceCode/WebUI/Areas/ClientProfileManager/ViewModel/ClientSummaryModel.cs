using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class ClientSummaryModel
    {
        public int ClientId { get; set;}
        public string ClientName { get; set; }
        public int TotalPortfolios { get; set; }
        public int TotalApplications { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalMetric { get; set; }
        public int TotalIncidents { get; set; }
        public int TotalServiceRequest { get; set; }
        public int TotalTicketsClosed_Resolved { get; set; }
        public int TotalTicketsRejected { get; set; }
        public int TotalTicketSuspended { get; set; }
        //public decimal AvgEffortPerTicket { get; set; }
        public string AvgEffortPerTicket { get; set; }
        public decimal AvgCostPerTicket { get; set; }
        public int TotalSev1 { get; set; }
        public int TotalSev2 { get; set; }
        public int TotalSev3 { get; set; }
        public int TotalSev4 { get; set; }
    }
    [Serializable]
    public class ServiceDeliveryStatsModel
    {
        //L1 Tickets
        public string IN_L1Tickets { get; set; }
        public string SR_L1Tickets { get; set; }
        public string Prob_L1Tickets { get; set; }
        public string CM_L1Tickets { get; set; }//Enhancements
        //L2-L4 tickets
        public string IN_L1L4Tickets { get; set; }
        public string SR_L1L4Tickets { get; set; }
        public string Prob_L1L4Tickets { get; set; }
        public string CM_L1L4Tickets { get; set; }
        //AvgTicketsPerMonth
        public string IN_AvgTicketsPerMonth { get; set; }
        public string SR_AvgTicketsPerMonth { get; set; }
        public string Prob_AvgTicketsPerMonth { get; set; }
        public string CM_AvgTicketsPerMonth { get; set; }
        //AvgCostPerTicket
        public string IN_AvgCostPerTicket { get; set; }
        public string SR_AvgCostPerTicket { get; set; }
        public string Prob_AvgCostPerTicket { get; set; }
        public string CM_AvgCostPerTicket { get; set; }
        //AvgResTime
        public string IN_AvgResTime { get; set; }
        public string SR_AvgResTime { get; set; }
        public string Prob_AvgResTime { get; set; }
        public string CM_AvgResTime { get; set; }
        //TotalClusters
        public string IN_Clusters { get; set; }
        public string SR_Clusters { get; set; }
        public string Prob_Clusters { get; set; }
        public string CM_Clusters { get; set; }

        //Total SLA
        public string Mon_TotalSLA { get; set; }
        public string Quar_TotalSLA { get; set; }
        public string Year_TotalSLA { get; set; }
        //MeetSLAs
        public string Mon_MeetSLA { get; set; }
        public string Quar_MeetSLA { get; set; }
        public string Year_MeetSLA { get; set; }
        //ExceededSLAs
        public string Mon_ExSLA { get; set; }
        public string Quar_ExSLA { get; set; }
        public string Year_ExSLA { get; set; }
        //BreachedSLA
        public string Mon_BrSLA { get; set; }
        public string Quar_BrSLA { get; set; }
        public string Year_BrSLA { get; set; }

        //Portfolios
        public string TotalPortfolios { get; set; }
        public string TotalApps { get; set; }
        public string HighestTickForApp { get; set; }
        public string OverallTCO { get; set; }

        //Idea details
        public string TotalIdeas { get; set; }
        public string IdeaApproved { get; set; }
        public string CSI_index { get; set; }
        public string IdeaTrend { get; set; }

    }
}