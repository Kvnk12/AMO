using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class ClientAccContract
    {
        public int ContractId { get; set; }
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Display(Name = "Client Account Name")]
        public string ClientAccountName { get; set; }

        [Required(ErrorMessage = "Select Client Name")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Select Account Name")]
        public int AccountId { get; set; }

        [Display(Name = "Contract Name")]
        [Required(ErrorMessage = "Enter Contract Name")]
        [StringLength(200)]
        public string ContractName { get; set; }

        [Display(Name = "Contract Status")]
        [Required(ErrorMessage = "Select Contract Status")]
        public string ContractStatus { get; set; }
        public string ContractCreate { get; set; }

        [Display(Name = "Contract Type")]
        [Required(ErrorMessage = "Enter Contract Status")]
        public string ContractType { get; set; }

        [Display(Name = "Hours of Service")]
        [Required(ErrorMessage = "Select Hours of Service")]
        public string HoursOfService { get; set; }

        [Display(Name = "NTT Data Client Manager")]
        [Required(ErrorMessage = "Enter NTT Data Client Manager")]
        [StringLength(200)]
        public string NTTDataClientManager { get; set; }

        [StringLength(200)]
        [Display(Name = "NTT Data Delivery Manager")]
        [Required(ErrorMessage = "Enter NTT Data Delivery Manager")]
        public string NTTDataDeliveryManager { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Client Contract Sponsor")]
        public string ClientContractSponspor { get; set; }

        [Display(Name = "Contract Open Date")]
        [Required(ErrorMessage = "Select Contract OpenDate")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ContractOpenDate
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Select Contract CloseDate")]
        [Display(Name = "Contract Close Date")]
        [DataType(DataType.Date)]
        public DateTime ContractCloseDate
        {
            get;
            set;

        }

        [Display(Name = "Fiscal Year Active Contract Value")]
        public Decimal FYActiveContractValue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpFYActiveContractValue { get; set; }

        [Display(Name = "Fiscal Year Planned Contract Value")]
        public Decimal FYPlannedContractValue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpFYPlannedContractValue { get; set; }

        [Display(Name = "Fiscal Year Pipeline Contract Value")]
        public Decimal FYPipeLineContractValue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpFYPipeLineContractValue { get; set; }

        [Display(Name = "Fiscal Year Contract Revenue to Date")]
        public Decimal FYExecutedContractValue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpFYExecutedContractValue { get; set; }

        [Display(Name = "Average Annaul Revenue")]
        public Decimal AverageAnnualRevenue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpAverageAnnualRevenue { get; set; }

        [Display(Name = "Revenue to Date")]
        public Decimal RevenueToDate { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpRevenueToDate { get; set; }

        [Display(Name = "Average Annual FTE's")]
        public Decimal AverageAnnualFTE { get; set; }

        [Display(Name = "Calculate based on Contract rollup")]
        public bool RollUpAverageAnnualFTE { get; set; }

        [Display(Name = "Sum of FTE's onshore and offshore")]
        public bool SumOfFTEOnShoreOffShore { get; set; }

        [Display(Name = "Average Annual FTE's (onshore)")]

        public decimal AverageAnnualFTEOnshore { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpAverageAnnualFTEOnshore { get; set; }

        [Display(Name = "Average Annual FTE's (offshore)")]
        public decimal AverageAnnualFTEoffShore { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpAverageAnnualFTEoffShore { get; set; }

        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                return GetDropDownListItems.ClientList;
            }
        }

        public IEnumerable<SelectListItem> HoursOfServiceTypes
        {
            get
            {
                return GetDropDownListItems.HoursOfServiceTypes;
            }
        }

        public IEnumerable<SelectListItem> AccountStatusList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.AccountStatus> clientStatus = new Repo<NTTData.AMOPortal.Domain.Entities.AccountStatus>(Context);
                List<NTTData.AMOPortal.Domain.Entities.AccountStatus> clientStatusList = clientStatus.GetAll().ToList();
                var Items = clientStatusList.Select(CList => new SelectListItem
                {
                    Value = CList.Status.ToString(),
                    Text = CList.Status.ToString()
                });
                return Items;
            }
           
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ContractCloseDate <= this.ContractOpenDate)
            {
                yield return new ValidationResult(string.Format("Close Date Should be greater than {0:MM/dd/yyyy}", this.ContractOpenDate), new[] { "ContractCloseDate" });
            }
            if (this.ContractStatus == "Active")
            {
                if (!(DateTime.Now >= this.ContractOpenDate && this.ContractCloseDate >= DateTime.Now))
                {
                    yield return new ValidationResult(string.Format("Contract Status can not be set {0} since Current Date does not fall between  Open & Close Dates", this.ContractStatus));
                }
            }
            if (this.ContractStatus == "Inactive")
            {
                if ((DateTime.Now >= this.ContractOpenDate && this.ContractCloseDate >= DateTime.Now))
                {
                    yield return new ValidationResult(string.Format("Contract  Status can not be set {0} since Current Date  falls between  Open & Close Dates", this.ContractStatus));
                }
            }
        }
    }
}