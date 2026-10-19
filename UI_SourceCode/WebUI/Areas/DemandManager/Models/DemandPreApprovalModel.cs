using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.DemandManager.Controllers;

namespace WebUI.Areas.DemandManager.Models
{
    public class DemandPreApprovalModel
    {
        public int DemandId { get; set; }
        public string DemandNumber { get; set; }  
        public string DemandName { get; set; }
        public bool InternalInitiative { get; set; }
        public string Team { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? StatusChangeDt { get; set; }
        public DateTime? LastModifiedDt { get; set; }
    }

    public class SubmitForReviewModel
    {
        public int DemandId { get; set; }
        public string DemandNumber { get; set; }
        public string DemandName { get; set; }
        public string DemandSource { get; set; }
        public bool InternalInitiative { get; set; }
        public string Team { get; set; }
        public string LOB { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string PrevPriority { get; set; }
        public string Complexity { get; set; }
        public string Value { get; set; }
        public string AssignedApprover { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? StatusChangeDt { get; set; }
        public DateTime? LastModifiedDt { get; set; }
    }

    public class DemandApprovedModel
    {
        public int DemandId { get; set; }
        public string DemandNumber { get; set; }
        public string DemandName { get; set; }
        public string DemandSource { get; set; }
        public bool InternalInitiative { get; set; }
        public string Team { get; set; }
        public string LOB { get; set; }
        public string Status { get; set; }
        public string ImpStatus { get; set; }   
        public string Priority { get; set; }
        public string Complexity { get; set; }
        public string Value { get; set; }
        public string ApproverBy { get; set; }
        public string ImplementationManager { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? StatusChangeDt { get; set; }
    }

    public class DemandImplementationModel
    {

        public int DemandId { get; set; }
        public string DemandNumber { get; set; }
        public string DemandName { get; set; }
        public bool InternalInitiative { get; set; }
        public string Team { get; set; }
        public string ImpStatus { get; set; }
        public string Priority { get; set; }
        public string ImpPriority { get; set; }
        public string PrevImpPriority { get; set; }
        public string ImplementationManager { get; set; }
        public string Complexity { get; set; }
        
        public DateTime? LastReviewDt { get; set; }
        public DateTime? SubmitForPlanningDt { get; set; }

        public DateTime? PlannedStartDt { get; set; }
        public DateTime? PlannedEndDt { get; set; }
        public DateTime? ActualStartDt { get; set; }
        public DateTime? ActualEndDt { get; set; }
        public DateTime? CreatedDt { get; set; }

        //public string ImplementationHealth { get; set; }
        //public string TargetProgress { get; set; }
        //public string ActualProgress { get; set; }
        public DateTime? LastModifiedDt { get; set; }

        public string ImplementationHealth
        {
            get
            {
                decimal actualminustarget = Convert.ToDecimal(ActualProgress) - Convert.ToDecimal(TargetProgress);
                if (actualminustarget > 0)
                    return "Ahead";
                else if (actualminustarget == 0)
                {
                    if (Convert.ToDecimal(ActualProgress) == 0)
                    {
                        return " ";
                    }
                    else
                    {
                        return "On Track";
                    }
                }                
                else
                    return "Behind";
            }
        }
        public string TargetProgress
        {
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
                datediff = ((DateTime.Now - PlannedStartDt).Value.TotalDays / planedhrs) * 100;
                if (datediff <= 0)
                    return "0";
                else if (datediff > 100)
                    return "100";
                else
                    return datediff.ToString("###.##");
            }
        }
        public string ActualProgress
        {
            get;set;
            
        }

    }

    public class DemandCompletionModel
    {

        public int DemandId { get; set; }
        public string DemandNumber { get; set; }
        public string DemandSource { get; set; }
        public string DemandName { get; set; }
        public bool InternalInitiative { get; set; }
        public string Team { get; set; }
        public string Priority { get; set; }

        public string ApprovedBy { get; set; }
        public string ImplementationManager { get; set; }
        public string Complexity { get; set; }
        public string Value { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string CompletedOnSchedule { get; set; }

        public DateTime? PlannedStartDt { get; set; }
        public DateTime? PlannedEndDt { get; set; }
        public DateTime? ActualStartDt { get; set; }
        public DateTime? ActualEndDt { get; set; }
        
        public DateTime? LastModifiedDt { get; set; }

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
        }
        public string TargetProgress
        {
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
                datediff = ((DateTime.Now - PlannedStartDt).Value.TotalDays / planedhrs) * 100;
                if (datediff <= 0)
                    return "0";
                else if (datediff > 100)
                    return "100";
                else
                    return datediff.ToString("###.##");
            }
        }
        public string ActualProgress
        {
            get; set;
            
        }

    }
     
    public enum DMViewDemand
    {
        ViewMyDemands = 1,
        ViewAllDemands = 2
    }

}