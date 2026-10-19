using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
    {
    public class ClientAccountContract : DelEntity
        {
               

                public string ClientName { get; set; }

                public string ClientAccountName { get; set; }

                public int ClientId { get; set; }

                public int AccountId { get; set; }

                public string ContractName { get; set; }

                public string ContractStatus { get; set; }

                public string ContractType { get; set; }

                public string HoursOfService { get; set; }

                public string NTTDataClientManager { get; set; }

                public string NTTDataDeliveryManager { get; set; }

                public string ClientContractSponspor { get; set; }

                public DateTime  ContractOpenDate { get; set; }

                public DateTime  ContractCloseDate { get; set; }

                public Decimal FYActiveContractValue { get; set; }

                public bool RollUpFYActiveContractValue { get; set; }

                public Decimal FYPlannedContractValue { get; set; }

                public bool RollUpFYPlannedContractValue { get; set; }

                public Decimal FYPipeLineContractValue { get; set; }

                public bool RollUpFYPipeLineContractValue { get; set; }

                public Decimal FYExecutedContractValue { get; set; }

                public bool RollUpFYExecutedContractValue { get; set; }

                public Decimal AverageAnnualRevenue { get; set; }

                public bool RollUpAverageAnnualRevenue { get; set; }

                public decimal RevenueToDate { get; set; }

                public bool RollUpRevenueToDate { get; set; }

                public decimal AverageAnnualFTE { get; set; }

                public bool RollUpAverageAnnualFTE { get; set; }

                public bool  SumOfFTEOnShoreOffShore { get; set; }

           
                public decimal AverageAnnualFTEOnshore { get; set; }

                public bool RollUpAverageAnnualFTEOnshore { get; set; }

                public decimal AverageAnnualFTEoffShore { get; set; }

                public bool RollUpAverageAnnualFTEoffShore { get; set; }


        }
    }
