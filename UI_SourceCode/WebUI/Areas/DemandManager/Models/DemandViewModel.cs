using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.DemandManager.Dto;

namespace WebUI.Areas.DemandManager.Models
{
    public class DemandViewModel
    {
        public bool IsDeleted { get; set; }

        public bool IsDemandMine { get; set; }
        public bool EditDemand { get; set; }
        public bool IsSysAdmin { get; set; }
        public bool IsCusUser { get; set; }
        public bool PostNewDemand { get; set; }
        public bool ViewDemand { get; set; }//ReadOnly Mode
        public bool IsImplementationManager { get; set; }
        public int ClientId { get; set; }

        //Tab
        public string TabName { get; set; }

        //idea Details to display.
        public string DemandName { get; set; }
        public int DemandId { get; set; }
        public string DemandNumber { get; set; }

        //Demand Status
        public string DemandStatus { get; set; }
        public int DemandStatusID { get; set; }
        //Demand Implementation status
        public string ImpDemandStatus { get; set; }
        public int? ImpDemandStatusID { get; set; }

        //--Extra Details
        //public decimal? Value { get; set; }
        public int DemandtypeId { get; set; }
        /// <summary>
        /// Not an Id, Changed to String (NTT / Client)
        /// </summary>
        public string DemandSourceId { get; set; }
        public int? LOBId { get; set; }
        public bool InternalInit { get; set; }
        public string Description { get; set; }

        public string Classification{ get; set; }
        public string category { get; set; }

        public string ConfigItem { get; set; }
        public string ConfigType { get; set; }

        //Analysis and Solution
        public string Analysis { get; set; }
        public string Solution { get; set; }



        //Persons
        public int? CreatedBy { get; set; }
        public int? ApprovedBy { get; set; }

        //Implementation Details
        public string StatusComment { get; set; }
        public int? ImpPriority { get; set; }
        public int? PriorityId { get; set; }

        public DateTime? LastReviewDt { get; set; }
        [Required]
        public int? ImpManager { get; set; }
        public int? ComplexityId { get; set; }

        public string[] ExistingApproversIds { get; set; }
        public bool MultipleApprovals { get; set; }

        public string EstimatedCost { get; set; }
        public int? DocumentTypeId { get; set; }
        public string DocumentNo { get; set; }
        public string DemandPlatformLead { get; set; }
        public decimal? ActualProgress
        {
            get; set;
           
        }
        public DateTime? CreatedDt { get; set; }

        public DateTime? PlannedStartDt { get; set; }
        public DateTime? PlannedEndDt { get; set; }
        public DateTime? ActualStartDt { get; set; }
        public DateTime? ActualEndDt { get; set; }
       // public string DMPStatusComment { get; set; }

        public string ImprovementTheme { get; set; }
        [Required(ErrorMessage = "required!")]
        public string Approved_Arch { get; set; }
        public IList<DMClassification> GetClassification
        {
            get
            {
                return DMHelper._getClassification(this.ClientId);
            }
        }
        //public IList<vwDMclasswithcat> GetCatAndClassification
        //{
        //    get
        //    {
        //        return DMHelper.GetCatAndClassification(this.ClientId);
        //    }
        //}
        public IList<DemandViewModel> DetailClassification { get; set; }
        public IList<DemandViewModel> DetailConfigItem { get; set; }
        public IList<DMImpPriorityHistoryModel> ImpPriorityHistoryList { get; set; }
        public IList<DMTaskAndActivitiesModel> TaskAndActivitiesList { get; set; }

        public IList<DMItemCategoryMap> GetMapItems
        {
            get
            {
                return DMHelper.GetMapItemForItemId(this.DemandId);
            }
        }
        public IList<DMItemBusinessBenefitsInput> BusinessBenefitsList { get; set; }
        public IList<DMItemITBenefitsInput> ITSavingsList { get; set; }
        public IList<DMImpPriorityHistoryModel> ImpPriorityHistory { get; set; }
        public IList<DemandPriorityHistoryModel> PriorityHistory { get; set; }
        public IList<DemandStatusHistoryModel> StatusHistory { get; set; }
        public IList<DMTaskActivitiesHistoryModel> TaskActivitiesHistory { get; set; }

        public IList<DemandApproverModel> Approvers { get; set; }
        public IList<DMItemCommentsInput> Comments { get; set; }
        public IList<DMRiskAndMitigationModel> RiskMitigationList { get; set; }
        public IList<DemandMetricTargetModel> MetricTargetList{ get; set; }
        public IList<DMClassificationInput> ClassificationList { get; set; }
        public IList<DMClassificationInput> ConfigurationList { get; set; }
        
        public IEnumerable<SelectListItem> AllLOB
        {
            get
            {
                return DMHelper.getLOB(this.ClientId);
            }
        }
        public IList<DMTaskAndActivitiesModel> DMTaskAndActivitiesList { get; set; }



    }
}