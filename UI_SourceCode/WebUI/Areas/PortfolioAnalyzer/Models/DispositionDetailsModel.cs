using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.PortfolioAnalyzer.Models;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class DispositionDetailsModel
    {
        public int Id { get; set; }
        //DespositionDetails
        public int? AppId { get; set; }
        public string ApplicationName { get; set; }
        public int? OriginalDispositionId { get; set; }     //dropdown
        public int? PlannedDispositionId { get; set; }      //dropdown
      //  public string RoadmapTargetApp { get; set; }        //dropdown
        public int? RedundancyId { get; set; }              //dropdown
        public DateTime? OriginalRetirementDate { get; set; }
        public int? RoadmapTargetYear { get; set; }
        public string TargetedPlatformCapability { get; set; }      //dropdown  
        public int? DispositionActionId { get; set; }    //dropdown
        public int? DispositionRationaleId { get; set; }    //dropdown
        // from check box
       // public string DispositionDrivers { get; set; }
        public bool Value { get; set; }
        public bool TCO { get; set; }
        public bool Risk { get; set; }
       
        public string Analysis { get; set; }

        public string Recommendation { get; set; }
        //Benifits and Estimates

        public int? FTEImpact { get; set; }
        //IT Disposition Benegit - Annual Support Savings(FTEs)
        public int? ITDispostionSupportFTEs { get; set; }
        //IT Disposition Benegit - Annual Dev Savings (FTEs)
        public int? ITDispostionDevFTEs { get; set;}
        // Vendor Disposition Benefit - Annual Commercial Licence Savings($)
        public decimal? VendorDisCommercialLicenceSavings { get; set;}
        //Vendor Disposition Benefit - Annual support Savings($)
        public decimal? VendorDisSupportSavings { get; set;}
        // Business Disposition Benefit - Annual Support Savings(FTEs)
        public int? BusinessDisSupportFTEs { get; set;} 
        //Hardware Dispositions Benefit -  Annual Support Savings($)
        public decimal? HardwareDisSupportSavings { get; set;}
        // Hardware Disposition Benefits - Annual Amount of CPU Freed 
        public int? HardwareDisAmountofCPUFreed { get; set;} 
        // Operational Efficency Benefits - Efficiency Units
        public string OperationalEfficiencyUnits { get; set;} 
        // Operational Efficency Benefits - Annual Efficency Amount
        public int? OperationalEfficiencyAmount { get; set;}
        // Planned Disposition Date 
        public DateTime? PlannedDispositionDate { get; set;} 
        // Disposition Duration in days
        public int? DispositionDuration { get; set;} 
        // Disposition Hours in Hours
        public int? DispositionEfforts { get; set;} 

        public int? ITDispositionEffort { get; set; }
        public int? VendorDispositionEffort { get; set; }
        public int? BusinessDispositionEffort { get; set; }
        public decimal? ITDispositionRate { get; set; }  
        public decimal? VendorDispositionRate { get; set; } 
        public decimal? BusinessDispositionRate { get; set; }
        //public int ITDispositionCost { get; set; }
        public decimal? ITDispositionCost { get; set; }
        public decimal? VendorDispositionCost { get; set; }
        public decimal? BusinessDispositionCost { get; set; }
       // public string Analysis { get; set; }

        
       
        /// Taking List for Desposition details in terms of Hours, Rate and Cost
        /// 
        //public List<DespositionEffortRate_ForList> IT { get; set; }
        //public List<DispositionEffortRate_ForList> Vendor { get; set; }
        //public List<DespositionEffortRate_ForList> Business { get; set; }
        public string ReplacementAlternatives { get; set; }
        public int? PlannedDispSolutionId { get; set; }
         public string AutomatedDispSolution { get; set; }
       public string AutomatedDisp { get; set; }
        public string[] MultipleRoadmap { get; set; }
        public string[] RoadmapTargetApp { get; set; }
        public string PlannedDispSolution { get; set; }

    }
}