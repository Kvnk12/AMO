using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBInstanceModel
    {
        public int Id { get; set; }
        public int KEDBID { get; set; }

        public int ClientId { get; set; }
        public string PointOfContact { get; set; }
        public string KnowledgeArticle { get; set; }
        private string _Description { get; set; }
        public DateTime? FromYear { get; set; }
        public DateTime? ToYear { get; set; }
        public string DateRange { get; set; }
        public string ClientName { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDt { get; set; }

        public string Description
        {
            get
            {
                //return Description.Length > 1000 ? Description.Substring(0, 999) : Description;
                return _Description;
            }

            set
            {
                if (value != null && value.Length >= 750)
                    _Description = value.Substring(0, 749);
                else
                    _Description = value;
            }
        }

        public int? IssueTypeId { get; set; }
        public int? SolutionCategoryId { get; set; }
        public int? SolutionStatusId { get; set; }
        public string KEDBNumber { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? BusinessFunctionId { get; set; }
        public string ConfigItemName { get; set; }
        public string Tower { get; set; }
        public string KedbIssueType { get; set; }
        public string SolutionCategory { get; set; }
        public string SolStatus { get; set; }
        public string BusinessFunction { get; set; }
        public string Cause { get; set; }
        public string CIName { get; set; }
        public string TicketId { get; set; }
        public string TicketDescription { get; set; }
        public bool IsDeleted { get; set; }
        public string UpdatedUser { get; set; }
        public string UpdatedDateTime { get; set; }
        public int IncidentId { get; set; }
        public IEnumerable<SelectListItem> IssueTypeList { get; set; }
        public IEnumerable<SelectListItem> BusinessFunctionList { get; set; }
        public IEnumerable<SelectListItem> SolutionCategoryList { get; set; }
        public IEnumerable<SelectListItem> SolutionStatusList { get; set; }

        public string Incid { get; set; }

        public IEnumerable<KEDBInstanceModel> klist { get; set; }

        //public HttpPostedFileBase KAFile { get; set; }
        public string httpPostedKAFile { get; set; }
        public byte[] KAFile
        {
            get; set;

        }

        public string FileName { get; set; }

        public string ResolutionSteps { get; set; }

        public string Clause { get; set; }

        public IEnumerable<SelectListItem> AccountNamesList { get; set; }
        public IEnumerable<SelectListItem> ContractList { get; set; }

        public IEnumerable<SelectListItem> TowerList { get; set; }


        public int[] SelectedTowerList;
        public int[] SelectedIssueTypeList;
        public int[] SelectedCategoryList;
        public int[] SelectedStatusList;

        public string AccountName { get; set; }

        public int ContractID { get; set; }

        public string LinkTextFile { get; set; }

        public bool isAutoIdentified { get; set; }
        public string isAutoIdentifiedVal { get; set; }
    }
}