using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IdeaDataSet")]
    public class IdeaDataSet : DelEntity
    {
        public int ClientId { get; set; }
        public int ChallengeId { get; set; }
        public int IdeaId { get; set; }
        public int BenefitId { get; set; }
        public int ApplicationId { get; set; }
        public int LOBId { get; set; }

        public decimal IdeaMonthSubmitted { get; set; }
        public decimal IdeaYearSubmitted { get; set; }
        public decimal IdeaActualCompletionDate_Month { get; set; }
        public decimal IdeaActualCompletionDate_Year { get; set; }
        public decimal IdeaEstimatedCost { get; set; }
        public decimal IdeaActualCost { get; set; }
        public decimal EstimatedPaybackPeriod { get; set; }
        public decimal IncreasedRevenueEstimate_Low { get; set; }
        public decimal IncreasedRevenueEstimate_High { get; set; }
        public decimal CostofServiceReductionEstimate { get; set; }
        public decimal SupportCapacityImpactEstimate { get; set; }
        public decimal IncreasedRevenueActual { get; set; }
        public decimal CostofServiceReductionActual { get; set; }
        public decimal SupportCapacityImpactActual { get; set; }

        public string Include { get; set; }
        public string ChallengeDescription { get; set; }
        public string IdeaOrgin { get; set; }
        public string IdeaOwner { get; set; }
        public string IdeaDescription { get; set; }
        public string IdeaASIS { get; set; }
        public string IdeaTOBE { get; set; }
        public string IdeaCreatedBy { get; set; }
        public string IdeaSubmittedBy { get; set; }
        public string IdeaApprovedBy { get; set; }
        public string IncludeInRoadmap { get; set; }
        public string IdeaPriority { get; set; }
        public string IdeaRoadmapCycle { get; set; }
        public string IdeaRoadmapComments { get; set; }
        public string IdeaNTTDATAApproval { get; set; }
        public string IdeaClientApproval { get; set; }
        public string IdeaStatus { get; set; }
        public string IdeaSubmittedToClient { get; set; }
        public string IdeaSponsor { get; set; }
        public string IdeaEstimatedReleaseName { get; set; }
        public string IdeaActualReleaseName { get; set; }
        public string IdeaCategory { get; set; }
        public string IdeaServiceCategory { get; set; }
        public string IdeaComment { get; set; }
        public string IdeaOpenActionItems { get; set; }
        public string EstimateSize { get; set; }
        public string IdeaCostType { get; set; }
        public string RevenueIncrease { get; set; }
        public string ProductivityGainNotes { get; set; }
        public string IdeaPotentialValueSize { get; set; }
        public string IdeaBenefitsRealized { get; set; }
        public string BusinessCapabilityL1 { get; set; }
        public string BusinessCapabilityL2 { get; set; }
        public string IdeaEstimatedBenefitDescription { get; set; }
        public string IdeaActualBenefitDescrition { get; set; }
        public string IdeaBenefitValueType { get; set; }
        public string IdeaApplicationImpacts { get; set; }
        public string IdeaLOBImpacts { get; set; }
        public string IncreaseClientsMarketShare { get; set; }
        public string IncreaseClientsRevenue { get; set; }
        public string Provided_access_to_new_client_VerticalsDomains { get; set; }
        public string Enabled_NTTDATA_to_offer_new_services_to_client { get; set; }
        public string Increase_in_NTTDATA_price_premium { get; set; }
        public string Increase_in_NTTDATA_Customer_Satisfaction { get; set; }
        public string Increase_in_NTTDATA_employee_satisfaction { get; set; }
        public string Increase_in_NTTDATA_talent_retention { get; set; }
        public string Improvement_in_NTTDATA_Brand_Perception { get; set; }
        public string LastUpdatedBy { get; set; }

        public DateTime IdeaDateCreated { get; set; }
        public DateTime IdeaDateSubmitted { get; set; }
        public DateTime IdeaDateApproved { get; set; }
        public DateTime IdeaRequestedCompletionDate { get; set; }
        public DateTime IdeaEstimatedImplementationDate { get; set; }
        public DateTime IdeaActualCompletionDate { get; set; }
        public DateTime IdeaEstBenefitRealizationDate { get; set; }
        public DateTime DateLastUpdated { get; set; }

    }
}

        