using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
    {
    [Table("Account")]
    public class ClientAccounSetup : DelEntity
        {
        public string AccountName { get; set; }
        public int AccountStatusId { get; set; }
        public string Status { get; set; }
        public string GlobalDeliveryManager { get; set; }
        public string GlobalClientManager { get; set; }
        public string ClientAccountSponsor { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal FiscalYearActiveValue { get; set; }
        public decimal FiscalYearPlannedValue { get; set; }
        public decimal FiscalYearPipelineValue { get; set; }
        public decimal FiscalYearExecutedValue { get; set; }
        public decimal AverageAnnualRevenue { get; set; }
        public decimal RevenueToDate { get; set; }
        public decimal AverageAnnualFTEs { get; set; }
        public decimal AverageAnnualFTEsOnShore { get; set; }
        public decimal AverageAnnualFTEsOffShore { get; set; }
        public int ClientId { get; set; }
        public bool FiscalYearContractRollupCheck { get; set; }
        public bool FiscalYearPlannedRollupCheck { get; set; }
        public bool FiscalYearPipelineRollupCheck { get; set; }
        public bool FiscalYearExecutedRollupCheck { get; set; }
        public bool AverageAnnualFTEsOnOffShoreCheck { get; set; }
        public bool AverageAnnualFTEsRollupCheck { get; set; }
        public bool AverageAnnualFTEsOnShoreRollupCheck { get; set; }
        public bool AverageAnnualFTEsOffSchoreCheck { get; set; }
        public bool SumOfFTEOnShoreOffShore { get; set; }
        public bool AverageAnnualRevenueCheck { get; set; }
        public bool RevenueToDateCheck { get; set; }
        
        



        }
    }
