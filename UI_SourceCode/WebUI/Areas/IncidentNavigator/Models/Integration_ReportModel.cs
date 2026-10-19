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
    public class Integration_ReportModel
    {
        [Display(Name = "Ticket Count")]
        public int IncidentCount { get; set; }
        public string Assignedtosupportgroup { get; set; }
        public int TotalTickets { get; set; }

        public string IncidentId { get; set; }
        public string seriesColor { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public string Priority { get; set; }
        public string TopWords { get; set; }
        public DateTime? Createddatetime { get; set; }
        public Integration_ReportModel()
        {
        }

        public Integration_ReportModel(string YearMonth, int incidentCount)
        {
            IncidentCount = incidentCount;
           
        }
        public string LongCategory { get; set; }
        public string Token { get; set; }
        public string Tower { get; set; }
        public string Lever { get; set; }
        public decimal Percent { get; set; }
        public string DrpCategory { get; set; }
    }
    public class Integration_ReportListModel
    {
        public int Accid { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        //  public IEnumerable<SelectListItem> INMappedColumn { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        public IEnumerable<SelectListItem> TowerList { get; set; }
        public IEnumerable<SelectListItem> ContractList { get; set; }
        public IEnumerable<SelectListItem> AccountNamesList { get; set; }
        public IEnumerable<SelectListItem> AccContractList { get; set; }
        public string ClientName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? FromYear { get; set; }
        public DateTime? ToYear { get; set; }
        public int ClientId { get; set; }
        public string DateRange { get; set; }
        public int[] SelectedTypeList;
        public int[] SelectedStatusList;
        public int[] SelectedPriorityList;
        public int[] SelectedGroupList;
        public int[] SelectedTowerList;
        public string[] Contract { get; set; }

    }
}