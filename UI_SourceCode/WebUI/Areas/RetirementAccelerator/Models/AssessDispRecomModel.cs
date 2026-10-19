using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class AssessDispRecomModel
    {
        public int  Id { get; set; }
        public int RAApplicationId { get; set; }
        
        
        //Common Fields
        public int? ReplacementScopeId { get; set; }
        public bool? ArchiveData { get; set; }
        public bool? IsDeCommissioned { get; set; }

        //Recommendations
        public string ReplaceRecommendation { get; set; }
        public string ArchivalRecommendation { get; set; }
        public string DecommRecommendation { get; set; }

        //Action
        public string ReplaceAction { get; set; }
        public string ArchivalAction { get; set; }
        public string DecommAction { get; set; }

        //Estimate hours
        public int? ReplaceEstimate { get; set; }
        public int? ArchivalEstimate { get; set; }
        public int? DecommEstimate { get; set; }

    }

    public enum RecommendationType
    {
        Replace=1,
        Archival=2,
        Decommission =3
    }
    public enum ActionType
    {
        Execute,
        Investigate,
        Monitor
    }

    public enum ReplacementScope
    {
        Full=1,
        Partial =2,
        Obsolete =3,
        Replaced =4
    }
}