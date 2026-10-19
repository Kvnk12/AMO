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
    public class INReportObservationModel
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int userId { get; set; }
        public string UserName { get; set; }
        public string ClientName { get; set; }
        public string Observation { get; set; }
        public string ObservationType { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public DateTime? FromYear { get; set; }
        public DateTime? ToYear { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateRange { get; set; }


        public string ClusterPath { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }


        public string ReportName { get; set; }
        public string CreatedByName { get; set; }
        public string DefTicketType { get; set; }

        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> GroupList { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        public IEnumerable<SelectListItem> TowerList { get; set; }
        public IEnumerable<SelectListItem> DateColumnsList { get; set; }
        public IEnumerable<SelectListItem> Maintnenance_TypeList { get; set; }
        public IEnumerable<SelectListItem> Maintnenance_StatusList { get; set; }
        public IEnumerable<SelectListItem> Maintnenance_PriorityList { get; set; }
        public IEnumerable<SelectListItem> WaivedFilterList { get; set; }
        public IEnumerable<SelectListItem> AccountNamesList { get; set; }
        public IEnumerable<SelectListItem> ContractList { get; set; }
        public IEnumerable<SelectListItem> Maintnenance_ContractList { get; set; }
        public IEnumerable<SelectListItem> ReportList { get; set; }
        public string[] Contract { get; set; }
      
        public int SelectedReportGrp;
        public int[] SelectedTypeList;
        public int[] SelectedStatusList;
        public int[] SelectedPriorityList;
        public int[] SelectedGroupList;
        public int[] SelectedTowerList;
        public int SelectedWaiveOption;


    }
}