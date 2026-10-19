using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Models
{
    public class PreApprovalModel
    {
        public int IdeaId { get; set; }
        public string IdeaNumber { get; set; }  
        public string IdeaName { get; set; }
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
        public int IdeaId { get; set; }
        public string IdeaNumber { get; set; }
        public string IdeaName { get; set; }
        public string IdeaSource { get; set; }
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

    public class ApprovedModel
    {
        public int IdeaId { get; set; }
        public string IdeaNumber { get; set; }
        public string IdeaName { get; set; }
        public string IdeaSource { get; set; }
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

    public class ImplementationModel
    {

        public int IdeaId { get; set; }
        public string IdeaNumber { get; set; }
        public string IdeaName { get; set; }
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
            //{
            //    double actualhrs = 0;
            //    if (ActualEndDt != null && ActualStartDt != null)
            //        actualhrs = (ActualEndDt - ActualStartDt).Value.TotalDays;
            //    else if (ActualEndDt == null && ActualStartDt != null)
            //        return "100.00";
            //    else if (ActualEndDt != null && ActualStartDt == null)
            //        return "00.00";
            //    else if (ActualEndDt == null && ActualStartDt == null)
            //        return "00.00";

            //    double datediff = 0;
            //    if (actualhrs == 0)
            //        actualhrs = 1;
            //    datediff = ((DateTime.Now - ActualStartDt).Value.TotalDays / actualhrs) * 100;
            //    if (datediff <= 0)
            //        return "00.00";
            //    else if (datediff > 100)
            //        return "100.00";
            //    else
            //        return datediff.ToString("###.##");
            //}
        }

    }

    public class CompletionModel
    {

        public int IdeaId { get; set; }
        public string IdeaNumber { get; set; }
        public string IdeaSource { get; set; }
        public string IdeaName { get; set; }
        public bool InternalInitiative { get; set; }
        public string Team { get; set; }
        public string Priority { get; set; }

        public string ApprovedBy { get; set; }
        public string ImplementationManager { get; set; }
        public string Complexity { get; set; }
        public string Value { get; set; }

        public string CreatedBy { get; set; }
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
            //{
            //    double actualhrs = 0;
            //    if (ActualEndDt != null && ActualStartDt != null)
            //        actualhrs = (ActualEndDt - ActualStartDt).Value.TotalDays;
            //    else if (ActualEndDt == null && ActualStartDt != null)
            //        return "100.00";
            //    else if (ActualEndDt != null && ActualStartDt == null)
            //        return "00.00";
            //    else if (ActualEndDt == null && ActualStartDt == null)
            //        return "00.00";

            //    double datediff = 0;
            //    if (actualhrs == 0)
            //        actualhrs = 1;
            //    datediff = ((DateTime.Now - ActualStartDt).Value.TotalDays / actualhrs) * 100;
            //    if (datediff <= 0)
            //        return "00.00";
            //    else if (datediff > 100)
            //        return "100.00";
            //    else
            //        return datediff.ToString("###.##");
            //}
        }

    }
     
    public enum IMViewIdea
    {
        ViewMyIdeas=1,
        ViewAllIdeas=2
    }

}