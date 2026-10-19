using Domain.Entities.ClientProfileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class DashboardTrendModel
    {
        public string yearMonth { get; set; }
        [Display(Name = "Ticket Count")]
        public int IncidentCount { get; set; }
        public int ResolvedIncidentCount { get; set; }

        public string Assignedtosupportgroup { get; set; }
        [Display(Name = "Ticket Count")]
        public int WorkgroupIncidentCount { get; set; }

        public int Holdtime { get; set; }
        public string ClusterLabel { get; set; }
        public int TotalTickets { get; set; }

        public string Severity { get; set; }
        [Display(Name = "Ticket Count")]
        public int SeverityIncidentCount { get; set; }

      
        public int DefaultYearMonth { get; set; }
        public string ddlyearMonth { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public decimal? AvgResolutionP1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public decimal? AvgResolutionP2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public decimal? AvgResolutionP3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public decimal? AvgResolutionP4 { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public decimal? AvgResolutionP5 { get; set; }
        public decimal? AvgCostP1 { get; set; }
        public decimal? AvgCostP2 { get; set; }
        public decimal? AvgCostP3 { get; set; }
        public decimal? AvgCostP4 { get; set; }
        public decimal? AvgCostP5 { get; set; }

        public string IncidentId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Configitemname { get; set; }
        public string Assignedtoperson { get; set; }
        public string Status { get; set; }

        public string Priority { get; set; }
        public string TopWords { get; set; }
        public DateTime? Createddatetime { get; set; }
        public DateTime? Resolutiondatetime { get; set; }

        public DateTime? Lastupdateddatetime { get; set; }
        public int Totalsuspenddurationertoresolution { get; set; }
        public DashboardTrendModel()
        {
        }

        public DashboardTrendModel(string YearMonth, int incidentCount)
        {
            yearMonth = YearMonth;
            IncidentCount = incidentCount;
           
        }
        public string Percent { get; set; }
        public string DrpCategory { get; set; }
    }
    public class INChartDashboardModel
    {
        
     //   [Display(Name = "No. of Tickets")]
        public int IncidentCount { get; set; }

      // [Display(Name = "Trend line")]
        public long TrendVal { get; set; }

       // [Display(Name = "Resolved Trend line")]
        public long ResolvedTrend { get; set; }

       // [Display(Name = "No. of  Resolved Tickets")]
        public int ResolvedCount { get; set; }

        //    [RegularExpression(@"^\d+(\.\d{1,2})$")]
        public decimal Percent { get; set; }
        public decimal AverageVal { get; set; }
        public string DrpCategory { get; set; }
        public string LongCategory { get; set; }
        public string Dim { get; set; }
    }
    public class INChartDashboard2MultiSeries
    {
        public List< IEnumerable<INChartDashboardModel>> Series { get; set; }
    }
    
    public class INNewDashboardModel
    {
        public string Accid { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
      //  public IEnumerable<SelectListItem> INMappedColumn { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        public IEnumerable<SelectListItem> TowerList { get; set; }
        public IEnumerable<SelectListItem> ContractList { get; set; }
        public IEnumerable<SelectListItem> AccountNamesList { get; set; }
        public IEnumerable<SelectListItem> AccContractList { get; set; }

    }
}