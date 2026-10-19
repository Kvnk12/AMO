using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.AssessmentManager.Models
{
    public class ActionItemTrackerModel
    {
        public int Id { get; set; }
        public int AssessmentID { get; set; }
        public int AssessmentTypeID { get; set; }
        public string AssessmentName { get; set; }
        public int Discipline_ID { get; set; }
        public string DisciplineName { get; set; }
        public int Component_ID { get; set; }
        public string ComponentName { get; set; }
        public int Aspect_ID { get; set; }
        public string AspectName { get; set; }
        public string ActionItem { get; set; }
        public int? OwnerID { get; set; }
        public string OwnerName { get; set; }

        public DateTime? PlannedCompletionDate { get; set; }
        public DateTime? LogupdateDate { get; set; }

        public string Status { get; set; }
        public string Comments { get; set; }

        public string Benefits { get; set; }
        public int BenefitId { get; set; }
        public string AssessmentStatus { get; set; }

        public int? ClientID { get; set; }


        public int? LogID { get; set; }
        public int? ActionItemID { get; set; }

        public string Updates { get; set; }

        public int AccountID { get; set; }
        public int ContractID { get; set; }

        public IEnumerable<SelectListItem> DisciplineList { get; set; }
        public IEnumerable<SelectListItem> ComponentList { get; set; }
        public IEnumerable<SelectListItem> AspectList { get; set; }
        public IEnumerable<SelectListItem> DataSourceList { get; set; }

        public IEnumerable<SelectListItem> AccountNamesList { get; set; }
        public IEnumerable<SelectListItem> ContractList { get; set; }
        public IEnumerable<SelectListItem> EngagementPOCList { get; set; }
        public IEnumerable<SelectListItem> EngagementTeamToCommunicateList { get; set; }
        public IEnumerable<SelectListItem> AssessmentNameList { get; set; }
        public IEnumerable<SelectListItem> QuestionSetNoList { get; set; }
        public IEnumerable<SelectListItem> AssessmentList { get; set; }

        public int ImplementationPlanId { get; set; }
        public string ImplementationPlan { get; set; }
        public int BusinessDriverId { get; set; }
        public string BusinessDriver { get; set; }
        public int AMSCatalogId { get; set; }
        public string AMSCatalog { get; set; }

        public int? Priority { get; set; }

    }
}