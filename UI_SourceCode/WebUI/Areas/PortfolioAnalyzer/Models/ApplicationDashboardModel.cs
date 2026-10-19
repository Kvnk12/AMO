using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class ApplicationDashboardModel
    {
        public int? ClientId { get; set; }
        public string AppId { get; set; }
                                           //    public _applicationDetails ApplicationDetails { get; set; }
                                           //    public _portfolioDetails PortfolioDetails { get; set; }
                                           //    public _demographics Demographics { get; set; }

        //    public _despositionDetails DespositionDetails { get; set; }
        //    public _technicalArchitecture TechnicalArchitecture { get; set; }
        public List<_costSummary> CostSummary { get; set; }        
            public List<_ratingSummary> RatingSummary { get; set; }
            public List<_labor> Labor { get; set; }
        public List<_ownershipType> OwnerShipType { get; set; }
        //}


        public List<_surveyRating> surveyRating { get; set; }


        //public class _applicationDetails
        //{
        public string ClientAppId { get; set; }
        public string AppDescription { get; set; }
        public string ApplicationName { get; set; }
        public string AliasName { get; set; }
        public int AvgTickets { get; set; }
        public string Analysis { get; set; }
        public string Reccommendation { get; set; }
        //}
        //public class _portfolioDetails
        //{
        public string portfolioName { get; set; }
        public string TechownerName { get; set; }
        public string BusinessOwnerName { get; set; }
        //} 
        //public class _demographics
        //{
        public string Criticality { get; set; }
        public int NoOfUsers { get; set; }
        public string RevenueImpact { get; set; }
        public string CodeType { get; set; }
        public string AppDomainName { get; set; }
        public string SecurityLevel { get; set; }

        public string ApplicationClassification { get; set; }
        public string ExternalHosting { get; set; }
        //}
        //public class _costSummary
        //{
        //public string CostType { get; set; }
        //public decimal amount { get; set; }
        //}
        //public class _despositionDetails
        //{
        public string PlannedDisposition { get; set; }
        public string DispositionImmediacy { get; set; }
        public string RetirementDate { get; set; }
        //}
        //public class _technicalArchitecture
        // {
        public string Architecture { get; set; }
        public string ApplicationSize { get; set; }
        //}
        //public class _ratingSummary
        //{
        //public decimal BusinessRisk { get; set; }
        //public decimal FunctionalQuality { get; set; }
        //public decimal StrategicAlignment { get; set; }
        //public decimal Maintainability { get; set; }
        //public decimal Operability { get; set; }
        //public decimal TechnicalCapability { get; set; }
        //public decimal TechnicalRisk { get; set; }
        //}
        //public class _labor
        //{
        //public int? ExternalLabor { get; set; }
        //public int? InternalLabor { get; set; }
        //public string WorkType { get; set; }

        //}
        ////Alternative Dashboard

        public string Level1Cat { get; set; }
        public string Level2Cat { get; set; }
        public string Level3Cat { get; set; }

        public string UserRange { get; set; }
        public string StandardStatus { get; set; }
        public string Differentiation { get; set; }
        public string SystemPriority { get; set; }

        public string DeploymentModel { get; set; }
        public string InterfacesIn { get; set; }
        public string InterfacesOut { get; set; }
        public string InterfacesEntangled { get; set; }

        public string UniqieVal { get; set; }
        public string HighVal { get; set; }
        public string LevelOfUse { get; set; }
        public string ChangeVolatality { get; set; }
        public string AppHealth { get; set; }
        //private string _ApplicationStandard="";
        public string ApplicationStandard {
            get
            {
                if (string.IsNullOrEmpty(this.StandardStatus))
                    return "No";
                else
                    return "Yes";
            }
 
        }
        public string Redundunt { get; set; }
        public string COTS { get; set; }
        public string AffectedBy { get; set; }
        public string TCO { get; set; }

        public string InitialDisposition { get; set; }
        public string APRDisposition { get; set; }
        public string DispositionSolution { get; set; }
        public string ReplacementTarget { get; set; }
        public string ReplacementAlternatives { get; set; }
        public int IncludeAlternativeDashboard { get; set; }





    }
    public class _ratingSummary
    {
        public int PerspectiveId { get; set; }
        public string PerspectiveName { get; set; }
        public decimal Rating { get; set; }
    }

    public class PerspectiveTypeRatingSummary
    {
        public int PerspectiveId { get; set; }
        public string Hint { get; set; }
        public string OriginalRating { get; set; }
        public string FinalRating { get; set; }
        public string Comments { get; set; }
    }

    public class _labor
    {
    public decimal ExternalLabor { get; set; }
    public decimal InternalLabor { get; set; }
    public string WorkType { get; set; }

    }
    public class _costSummary
    {
    public string CostType { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        public decimal? amount { get; set; }

        public string color { get; set; }
    }
    public class _ownershipType
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string IsSurveyParticipant { get; set; }

    }

    public class _surveyRating
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int answerValue { get; set; }
    }

}