using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;

namespace WebUI.Models
{
    public class ClientAccountList
    {
        public string AccountCreate { get; set; }
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Enter Account Name")]
        [StringLength(200)]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Enter NTT Data Delivery Manager")]
        [StringLength(200)]
        public string GlobalDeliveryManager { get; set; }
        [Required(ErrorMessage = "Enter NTT Data Client Manager")]
        [StringLength(200)]
        public string GlobalClientManager { get; set; }
        [Required(ErrorMessage = "Enter Client Account Sponsor")]
        [StringLength(200)]
        public string ClientAccountSponsor { get; set; }
        [Required(ErrorMessage = "Select Account Status")]
        public string AccountStatus { get; set; }
        [Required(ErrorMessage = "Select  Open Date")]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; }
        [Required(ErrorMessage = "Select  Close Date")]
        [DataType(DataType.Date)]
        public DateTime CloseDate { get; set; }
        public decimal FiscalYearActiveValue { get; set; }
        public decimal FiscalYearPlannedValue { get; set; }
        public decimal FiscalYearPipelineValue { get; set; }
        public decimal FiscalYearExecutedValue { get; set; }
        public decimal AverageAnnualRevenue { get; set; }
        public decimal RevenueToDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###,###0}")]
        public decimal AverageAnnualFTEs { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###,###0}")]
        public decimal AverageAnnualFTEsOnShore { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###,###0}")]
        public decimal AverageAnnualFTEsOffShore { get; set; }
        public int AccountStatuses { get; set; }
        public int ClientId { get; set; }

        [Display(Name = "Calculate base on Contract rollup")]
        public bool FiscalYearContractRollupCheck { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool FiscalYearPlannedRollupCheck { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool FiscalYearPipelineRollupCheck { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool FiscalYearExecutedRollupCheck { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool AverageAnnualFTEsOnOffShoreCheck { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool AverageAnnualFTEsRollupCheck { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool AverageAnnualFTEsOnShoreRollupCheck { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool AverageAnnualFTEsOffSchoreCheck { get; set; }
        [Display(Name = "Sum of FTE's Onshore and Offshore")]
        public bool SumOfFTEOnShoreOffShore { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool AverageAnnualRevenueCheck { get; set; }
        [Display(Name = "Calculate base on Contract rollup")]
        public bool RevenueToDateCheck { get; set; }
        public IEnumerable<SelectListItem> AccountStatusList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientStatus> AccountStatus = new Repo<ClientStatus>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ClientStatus> AccountStatusList = AccountStatus.GetAll().ToList();
                var Items = AccountStatusList.Select(CList => new SelectListItem
                {
                    Value = CList.Status.ToString(),
                    Text = CList.Status.ToString()
                });
                return Items;
            }
        }
 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CloseDate <= this.OpenDate)
            {
                yield return new ValidationResult(string.Format("Close Date Should be greater than {0:MM/dd/yyyy}", this.OpenDate));
            }
            if (this.AccountStatus == "Active")
            {
                if (!(DateTime.Now >= this.OpenDate && this.CloseDate >= DateTime.Now))
                {
                    yield return new ValidationResult(string.Format("Account Status can not be set {0} since Current Date does not fall between  Open & Close Dates", this.AccountStatus));
                }
            }
            if (this.AccountStatus == "Inactive")
            {
                if ((DateTime.Now >= this.OpenDate && this.CloseDate >= DateTime.Now))
                {
                    yield return new ValidationResult(string.Format("Account Status can not be set {0} since Current Date  falls between  Open & Close Dates", this.AccountStatus));
                }
            }
        }
    }
}