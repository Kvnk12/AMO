using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IdeaManager.Dto;

namespace WebUI.Areas.IdeaManager.Models
{
    public class IdeaViewModel
    {
        public bool IsDeleted { get; set; }

        public bool IsIdeaMine { get; set; }
        public bool EditIdea { get; set; }
        public bool IsSysAdmin { get; set; }
        public bool IsCusUser { get; set; }
        public bool PostNewIdea { get; set; }
        public bool ViewIdea { get; set; }//ReadOnly Mode
        public bool IsImplementationManager { get; set; }
        public int ClientId { get; set; }

        //Tab
        public string TabName { get; set; }

        //idea Details to display.
        public string IdeaName { get; set; }
        public int IdeaId { get; set; }
        public string IdeaNumber { get; set; }
        
        //Idea Status
        public string IdeaStatus { get; set; }
        public int IdeaStatusID { get; set; }
        //Idea Implementation status
        public string  ImpIdeaStatus { get; set; }
        public int? ImpIdeaStatusID { get; set; }

        //--Extra Details
        //public decimal? Value { get; set; }
        public decimal? QtyFication { get; set; }
        public int? IdeatypeId { get; set; }
        /// <summary>
        /// Not an Id, Changed to String (NTT / Client)
        /// </summary>
        public string  IdeaSourceId { get; set; }
        public int? LOBId { get; set; }
        public int? Complexity { get; set; }
        public bool InternalInit { get; set; }
        public int? TeamId { get; set; }
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
        public string CreatedByName { get; set; }
        public int? ApprovedBy { get; set; }

        //Implementation Details
        public string StatusComment { get; set; }
        public int? ImpPriority { get; set; }
        public DateTime? LastReviewDt { get; set; }
        [Required]
        public int? ImpManager { get; set; }


        public string[] ExistingApproversIds { get; set; }
        public bool MultipleApprovals { get; set; }

        public string ImplementationHealth
        {
            get
            {
                decimal actualminustarget = Convert.ToDecimal(ActualProgress) - Convert.ToDecimal(TargetProgress);
                if (actualminustarget > 0)
                    return "Ahead";
                else if (actualminustarget == 0)
                    if (Convert.ToDecimal(ActualProgress) == 0)
                    {
                        return " ";
                    }
                    else
                    {
                        return "On Track";
                    }
                else
                    return "Behind";
            }
            //set
            //{
            //    ImplementationHealth = value;
            //}
        }
        public string TargetProgress {
            get
            {
                double planedhrs = 0;
                if (PlannedEndDt != null && PlannedStartDt != null)
                    planedhrs = (PlannedEndDt - PlannedStartDt).Value.TotalDays;
                if (PlannedEndDt == null && PlannedStartDt != null)
                    return "100.00";
                if (PlannedEndDt != null && PlannedStartDt == null)
                    return "0.00";
                if (PlannedEndDt == null && PlannedStartDt == null)
                    return "0.00";
                if (planedhrs == 0)
                    planedhrs = 1;
                double datediff = 0;
                    datediff = ((DateTime.Now - PlannedStartDt).Value.TotalDays / planedhrs) *100;
                if (datediff <= 0)
                    return "0.00";
                else if (datediff > 100)
                    return "100.00";
                else
                    return datediff.ToString("###.##");
            }
          //  set { TargetProgress = value; }
        }
        public decimal? ActualProgress
        {
            get; set;
            
            ////{
            ////    double actualhrs = 0;
            ////    if (ActualEndDt != null && ActualStartDt != null)
            ////        actualhrs = (ActualEndDt - ActualStartDt).Value.TotalDays;
            ////    else if (ActualEndDt == null && ActualStartDt != null)
            ////        return "100.00";
            ////    else if (ActualEndDt != null && ActualStartDt == null)
            ////        return "00.00";
            ////    else if (ActualEndDt == null && ActualStartDt == null)
            ////        return "00.00";

            ////    double datediff = 0;
            ////    if (actualhrs == 0)
            ////        actualhrs = 1;
            ////    datediff = ((DateTime.Now - ActualStartDt).Value.TotalDays / actualhrs) * 100;
            ////    if (datediff <= 0)
            ////        return "00.00";
            ////    else if (datediff > 100)
            ////        return "100.00";
            ////    else
            ////        return datediff.ToString("###.##");
            ////}
          //  set { ActualProgress = value; }
        }

        public DateTime? PlannedStartDt { get; set; }
        public DateTime? PlannedEndDt { get; set; }
        public DateTime? ActualStartDt { get; set; }
        public DateTime? ActualEndDt { get; set; }
       // public string IMPStatusComment { get; set; }

        public string ImprovementTheme { get; set; }

        public IList<IMClassification> GetClassification
        {
            get
            {
                return IMHelper._getClassification(this.ClientId);
            }
        }
        public IList<vwIMclasswithcat> GetCatAndClassification
        {
            get
            {
                return IMHelper.GetCatAndClassification(this.ClientId);
            }
        }
        public IList<IdeaViewModel> DetailClassification { get; set; }
        public IList<IdeaViewModel> DetailConfigItem { get; set; }
        public IList<IMImpPriorityHistoryModel> ImpPriorityHistoryList { get; set; }
        public IList<IMCompletedAndNextStepsModel> CompletedAndNextStepList { get; set; }

        //public IList<IdeaViewModel> DetailClassification
        //{
        //    get
        //    {
        //        return IMHelper.DetailClassification(this.IdeaId);
        //    }
        //    set
        //    {

        //    }
        //}
        public IList<IMItemCategoryMap> GetMapItems
        {
            get
            {
                return IMHelper.GetMapItemForItemId(this.IdeaId);
            }
        }


        public IList<IMItemBusinessBenefitsInput> BusinessBenefitsList { get; set; }
        public IList<IMItemITBenefitsInput> ITSavingsList { get; set; }
        public IList<IMImpPriorityHistoryModel> ImpPriorityHistory { get; set; }
        public IList<IMIdeaPriorityHistoryModel> PriorityHistory { get; set; }
        public IList<IMIdeaStatusHistoryModel> StatusHistory { get; set; }
        public IList<ApproverModel> Approvers { get; set; }
        public IList<IMItemCommentsInput> Comments { get; set; }
        public IList<IMRiskAndMitigationModel> RiskMitigationList { get; set; }
        public IList<MetricTargetModel> MetricTargetList{ get; set; }
        public IList<IMClassificationInput> ClassificationList { get; set; }
        public IList<IMClassificationInput> ConfigurationList { get; set; }
        
        public IEnumerable<SelectListItem> AllLOB
        {
            get
            {
                return IMHelper.getLOB(this.ClientId);
            }
        }
        public bool IMAutoApprovalSelection { get; set; }
        public string ProposedBy { get; set; }
        public int? SystemImpactedId { get; set; }

    }
}