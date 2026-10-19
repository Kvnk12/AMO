using Domain.Entities.ClientProfileManager;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class APRFinancialModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int Year { get; set; }
        //public DateTime? yearDatetime { get
        //    {
               
        //            return DateTime.ParseExact(Year.ToString() + "-01-01", "yyyy-mm-dd", CultureInfo.InvariantCulture);
               
        //    }
           
           
        //}
        public decimal? DiscretionaryCostInternal { get; set; }
        public decimal? DiscretionaryCostExternal { get; set; }
        public double? DiscretionaryFTEInternal { get; set; }
        public double? DiscretionaryFTEExternal { get; set; }
        public decimal? EnhancementCostInternal { get; set; }
        public decimal? EnhancementCostExternal { get; set; }
        public double? EnhancementFTEInternal { get; set; }
        public double? EnhancementFTEExternal { get; set; }
        public decimal? SupportCostInternal { get; set; }
        public decimal? SupportCostExternal { get; set; }
        public double? SupportFTEInternal { get; set; }
        public double? SupportFTEExternal { get; set; }
        public decimal? OtherCostInternal { get; set; }
        public decimal? OtherCostExternal { get; set; }
        public double? OtherFTEInternal { get; set; }
        public double? OtherFTEExternal { get; set; }
        public decimal? InfrastructureCostInternal { get; set; }
        public decimal? InfrastructureCostExternal { get; set; }
        public decimal? LicensingCostInternal { get; set; }
        public decimal? LicensingCostExternal { get; set; }
        public double? BusinessFTEInternal { get; set; }
        public double? BusinessFTEExternal { get; set; }
        public string FinancialNotes { get; set; }

        public decimal? BusinessCostInternal { get; set; }
        public decimal? BusinessCostExternal { get; set; }
        public decimal? TotalDiscretionaryCost
        {
            get
            {
                return (DiscretionaryCostExternal ?? 0) + (DiscretionaryCostInternal ?? 0) ;
            }
        }

        public decimal? TotalOtherCost
        {
            get
            {
                return (OtherCostExternal ?? 0) + (OtherCostInternal ?? 0);
            }
        }

        public decimal? TotalInfrastructureCost
        {
            get
            {
                return (InfrastructureCostExternal ?? 0) + (InfrastructureCostInternal ?? 0);
            }
        }

        public decimal? TotalLicensingCost
        {
            get
            {
                return (LicensingCostExternal ?? 0) + (LicensingCostInternal ?? 0);
            }
        }

        public decimal? TotalEnhancementCost
        {
            get
            {
                return (EnhancementCostExternal ?? 0) + (EnhancementCostInternal ?? 0);
            }
        }


        public decimal? TotalBusinessCost
        {
            get
            {
                return (BusinessCostExternal ?? 0) + (BusinessCostInternal ?? 0);
            }
        }
        public decimal? TotalSupportCost
        {
            get
            {
                return (SupportCostExternal ?? 0) + (SupportCostInternal ?? 0);
            }
        }
        public decimal? TotalExternalCost
        {
            get
            {
                return (DiscretionaryCostExternal ?? 0) + (OtherCostExternal ?? 0) + (InfrastructureCostExternal ?? 0) + (LicensingCostExternal ?? 0) + (EnhancementCostExternal ?? 0) + (SupportCostExternal ?? 0) + (BusinessCostExternal ?? 0);
            }
        }
        public decimal? TotalInternalCost
        {
            get
            {
                return (DiscretionaryCostInternal ?? 0) + (OtherCostInternal ?? 0) + (InfrastructureCostInternal ?? 0) + (LicensingCostInternal ?? 0) + (EnhancementCostInternal ?? 0)+(SupportCostInternal ?? 0) + (BusinessCostInternal ?? 0);
            }
        }
        public double? TotalExternalFTEs
        {
            get
            {
                return (DiscretionaryFTEExternal ?? 0.00)  + (OtherFTEExternal ?? 0.00)  + (BusinessFTEExternal ?? 0.00)  + (EnhancementFTEExternal ?? 0.00) +(SupportFTEExternal ?? 0.00) ;
            }
        }

        public double? TotalInternalFTEs
        {
            get
            {
                return (DiscretionaryFTEInternal ?? 0.00) +(OtherFTEInternal ?? 0.00)  + (BusinessFTEInternal ?? 0.00)  + (EnhancementFTEInternal ?? 0.00)  + (SupportFTEInternal ?? 0.00) ;
            }
        }
    }
}