using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class DashboardModel
    {

        public int Id { get; set; }
        public string ClientApplicationNumber { get; set; }
        public string Description { get; set; }
        public string Criticality { get; set; }
        public string NoOfUsers { get; set; }
        public string RevenueImpact { get; set; }
        public string CodeType { get; set; }
        public string AppDomain { get; set; }
        public string SecurityLevel { get; set; }
        public string PortfolioName { get; set; }
        public string TechnicalOwnerName { get; set; }
        public string BusinessOwnerName { get; set; }
        public string Architecture { get; set; }
        public string AppSize { get; set; }
        public string Analysis { get; set; }
        public string Recommendation { get; set; }
       
        public int AppCount { get; set; }
        public string PieChartColumnName { get; set; }

        public string color { get; set; }

        public string Percentage { get; set; }
        
    }
    public class RatingHeatMapModel
    {
        public int Id { get; set; }
        public string LOBName { get; set; }
        public decimal NoOfApplications { get; set; }
        public decimal BusinessRisk { get; set; }
        public decimal FunctionalQuality { get; set; }
        public decimal Maintainability { get; set; }
        public decimal Operatibility { get; set; }
        public decimal StretagicAlignment { get; set; }
        public decimal TechnicalCapability { get; set; }
        public decimal TechnicalRisk { get; set; }
        public decimal CloudReadiness { get; set; }
    }

    public class RatingBySubperspective
    {
        public string ApplicationName { get; set; }
        public int AppId { get; set; }
        public List<string> PerspectiveTypelist { get; set; }
        public string PortfolioIdlist { get; set; }
        public bool check { get; set; }
        public int surveyId { get; set; }
        public List<RatingPerspReport> RatingPerspReport { get; set; }
        public string Xvalue { get; set; }
        public string YperspectiveTypes { get; set; }
    }

    public class RatingPerspReport
    {
        public string ApplicationName { get; set; }

        public string X { get; set; }
        public string Y { get; set; }

        public decimal Xrating { get; set; }

        public decimal Yrating { get; set; }
    }


    public class DispositionRoadMapModel
    {
        public string ApplicationName { get; set; }

        public string DispositionName { get; set; }
        public string DispositionRationaleType { get; set; }
        public string DispositionActionType { get; set; }

        public int RoadmapTargetYear { get; set; }

        public DateTime? OriginalRetirementDate { get; set; }
        public DateTime? PlannedDispositionDate { get; set; }
        public bool? InScope { get; set; }

        public string Color { get; set; }

        public string ApplicationClassificationName { get; set; }
        public string ClientApplicationNumber { get; set; }

        public int? Year { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        public decimal? Totalcost { get; set; }
    }

}