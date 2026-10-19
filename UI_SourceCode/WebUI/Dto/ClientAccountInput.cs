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
    public class ClientAccountInput : Input, IValidatableObject
        {



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
        public string Status { get; set; }

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
        [DisplayFormat(DataFormatString="{0:###,###,###0}")] 
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
                Repo<NTTData.AMOPortal.Domain.Entities.ClientStatus> AccountStatus = new Data.Repo<Domain.Entities.ClientStatus>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ClientStatus> AccountStatusList = AccountStatus.GetAll().ToList();
                var Items = AccountStatusList.Select(CList => new SelectListItem
                {
                    Value = CList.Status.ToString(),
                    Text = CList.Status.ToString()
                });
                return Items;
                }
            }
        

        public class AcctWithStatus
            {
            public AcctWithStatus(int id, string name, string status)
                {
                ID = id;
                Name = name;
                Status = status;
                }

            public int ID { get; set; }
            public String Name { get; set; }
            public String Status { get; set; }
            }

        public IList<AcctWithStatus> ClientAccountFilterList
            {
            get
                {


                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientAccounSetup> Clients = new Data.Repo<Domain.Entities.ClientAccounSetup>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ClientAccounSetup> ClientsList = Clients.GetAll().Where(a => a.ClientId == ClientId).ToList();

                Repo<NTTData.AMOPortal.Domain.Entities.AccountStatus> Clientstatus = new Data.Repo<Domain.Entities.AccountStatus>(Context);
                List<NTTData.AMOPortal.Domain.Entities.AccountStatus> StatusList = Clientstatus.GetAll().ToList();



                var accountSet = from p in ClientsList
                                 join q in StatusList on p.AccountStatusId equals q.Id
                                 select new AcctWithStatus
                                   (
                                       p.Id,
                                       p.AccountName.ToString(),
                                       q.Status.ToString()
                                    );
                return accountSet.ToList();
                }
            }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {

            if (this.CloseDate <= this.OpenDate)
                {

                yield return new ValidationResult(string.Format("Close Date Should be greater than {0:MM/dd/yyyy}", this.OpenDate));

                }

            if (this.Status == "Active")
                {

                if (!(DateTime.Now >= this.OpenDate &&  this.CloseDate >= DateTime.Now))
                    {

                    yield return new ValidationResult(string.Format("Account Status can not be set {0} since Current Date does not fall between  Open & Close Dates", this.Status));

                    }
                }


            if (this.Status == "Inactive")
                {

                if ((DateTime.Now >= this.OpenDate &&  this.CloseDate >= DateTime.Now))
                    {

                    yield return new ValidationResult(string.Format("Account Status can not be set {0} since Current Date  falls between  Open & Close Dates", this.Status));

                    }
                }


            }

        }
    }