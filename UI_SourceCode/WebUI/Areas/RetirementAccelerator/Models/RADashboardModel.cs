using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class RADashboardModel
    {
        public int Id { get; set; }
        public string ApplicationName { get; set; }
        public int RAApplicationId { get; set; }
        public int ProjectHealth { get; set; }
        public string ProjectManage { get; set; }
        public string BusinessFunction { get; set; }
        public string TechnicalOwner { get; set; }
        public int? RetirementPhaseId { get; set; }
        public int? RetirementStageId { get; set; }
        public string RetirementPhase { get; set; }
        public string RetirementStage { get; set; }
        public int? ReplacementScopeId { get; set; }
        public string GeneralNotes { get; set; }
        public bool? ArchiveData { get; set; }
        public bool? IsDeCommissioned { get; set; }
        public int RecommendationType { get; set; }
        public string Recommendation { get; set; }
        public string Action { get; set; }
        public int? Estimate { get; set; }

        //Action
        public string ReplaceAction { get; set; }
        public string ArchivalAction { get; set; }
        public string DecommAction { get; set; }
        public IEnumerable<Rating> FinalRates { get; set; }
    }

    public class Rating
    {
        public string PerspectiveCategory { get; set; }
        public string AvgRating { get; set; }
        public string PerspectiveType { get; set; }
        public decimal Rate { get; set; }
        public List<Rating> pRating { get; set; }
    }



}
public enum ReplacementScope
{
    Full = 1,
    Partial = 2,
    Obsolete = 3,
    Replaced = 4
}
public enum RecommendationType
{
    Replace = 1,
    Archival = 2,
    Decommission = 3
}






