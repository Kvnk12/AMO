using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;


namespace NTTData.AMOPortal.WebUI.Dto
{
    public class ClientAccountContractInput : Input, IValidatableObject
    {



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
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ContractCloseDate
        {
            get;
            set;

        }

        [Display(Name = "Fiscal Year Active Contract Value")]
        //[Range(0, decimal.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Decimal FYActiveContractValue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpFYActiveContractValue { get; set; }

        [Display(Name = "Fiscal Year Planned Contract Value")]
        //[Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Decimal FYPlannedContractValue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpFYPlannedContractValue { get; set; }

        [Display(Name = "Fiscal Year Pipeline Contract Value")]
        //[Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Decimal FYPipeLineContractValue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpFYPipeLineContractValue { get; set; }

        [Display(Name = "Fiscal Year Contract Revenue to Date")]
        //[Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Decimal FYExecutedContractValue { get; set; }

        [Display(Name = "Calculate based on Service rollup")]
        public bool RollUpFYExecutedContractValue { get; set; }

        [Display(Name = "Average Annaul Revenue")]
        //[Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
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
                return GetDropDownListItems.AccountStatusList;
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