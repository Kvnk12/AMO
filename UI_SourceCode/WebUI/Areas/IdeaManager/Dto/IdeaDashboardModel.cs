using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebUI.Areas.IdeaManager.Dto
{
    public class IdeaDashboardModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ItemName { get; set; }
        public string IdeaNumber { get { return String.IsNullOrEmpty(_ideaNum) ? "--NV--" : _ideaNum; } set { _ideaNum = value; } }
        private string _ideaNum;
        public decimal Quantification { get; set; }
        public DateTime? SubmittedForReviewDt { get; set; }
        public string CreatedBy { get; set; }
        public string IdeaType { get; set; }
        public string IdeaSource { get; set; }
        public string Complexity { get; set; }
        public string Team { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int Priority { get; set; }
        public int ImplementationPriority { get; set; }
        public string Status { get; set; }
        public string ImpManagers { get; set; }
        public string ImplementationStatus { get; set; }
        public decimal ImplementationProgress { get; set; }
        public bool InternalInitiative { get; set; }
        public string ApprovedBy { get; set; }
        public string ImplementationManager { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? CompletedDt { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        
        public string LOBDepartment { get; set; }

        /// <summary>
        /// aka Desciption
        /// </summary>
        public string Problem { get; set; }
        public string Goal{ get; set; }
        /// <summary>
        /// Analysis/Root Cause and Solution
        /// </summary>
        public string Analysis { get; set; }
        public string Solution { get; set; }
        public string ImpComments { get; set; }
        public string BusinessBenefits { get; set; }
        public string ITSavings { get; set; }
        public string OfflineApprover { get; set; }
        public string RiskMitigation { get; set; }
        public string Metrics { get; set; }
        public string Comments { get; set; }
        public string CompletedNextSteps { get; set; }
        public string Classification { get; set; }
        public string ProposedBy { get; set; }
        public string CreatedByName { get; set; }
        public string SystemImpactedService { get; set; }
        public string UpdatedBy { get; set; }
        public string ImplementationHealth
        {
            get
            {
                decimal actualminustarget = Convert.ToDecimal(ImplementationProgress) - Convert.ToDecimal(TargetProgress);
                if (actualminustarget > 0)
                    return "Ahead";
                else if (actualminustarget == 0)
                    if (Convert.ToDecimal(ImplementationProgress) == 0)
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
        public string TargetProgress
        {
            get
            {
                double planedhrs = 0;
                if (PlannedEndDate != null && PlannedStartDate != null)
                    planedhrs = (PlannedEndDate - PlannedStartDate).Value.TotalDays;
                if (PlannedEndDate == null && PlannedStartDate != null)
                    return "100.00";
                if (PlannedEndDate != null && PlannedStartDate == null)
                    return "0.00";
                if (PlannedEndDate == null && PlannedStartDate == null)
                    return "0.00";
                if (planedhrs == 0)
                    planedhrs = 1;
                double datediff = 0;
                datediff = ((DateTime.Now - PlannedStartDate).Value.TotalDays / planedhrs) * 100;
                if (datediff <= 0)
                    return "0.00";
                else if (datediff > 100)
                    return "100.00";
                else
                    return datediff.ToString("###.##");
            }
            //  set { TargetProgress = value; }
        }

    }

  

}