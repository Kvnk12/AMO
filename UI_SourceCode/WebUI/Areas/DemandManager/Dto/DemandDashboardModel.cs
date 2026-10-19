using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebUI.Areas.DemandManager.Dto
{
    public class DemandDashboardModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ItemName { get; set; }
        public string DemandNumber { get; set; }
        public DateTime? SubmittedForReviewDt { get; set; }
        public string CreatedBy { get; set; }
        public string DemandType { get; set; }
        public string DemandSource { get; set; }
        public string Complexity { get; set; }
        public string Team { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int Priority { get; set; }
        public int ImplementationPriority { get; set; }
        public string Status { get; set; }
        public string ImplementationStatus { get; set; }
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
        public string Solution { get; set; }
        public string ImpComments { get; set; }
        public string Comments { get; set; }
        public string Classification { get; set; }
        public string DocumentNo { get; set; }
        public string EstimatedCost { get; set; }
        public string DocumentType { get; set; }
        public string DemandPlatformLead { get; set; }
        public string Approved_Arch { get; set; }

    }

}